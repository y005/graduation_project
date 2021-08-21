using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using TMPro;

public class LoadControl : MonoBehaviour
{
    IEnumerator coroutine1;
    float totalStockCnt=0;
    //프로그래스 바
    public Slider LoadSlider;
    //API키를 저장한 객체
    public API api;
    //주식 정보들을 저장한 객체
    public StockList stockList;

    public TextMeshProUGUI progress1; // 진행사항 문구 UI
    public TextMeshProUGUI progress2; // 진행사항 문구 UI

    //사용하는 주식의 종목코드 배열
    List<string> codeList = new List<string>() { "PEP","KO","SBUX","NKE"};
    /*List<string> codeList = new List<string>() { "MSFT", "ORCL", "AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS",
                                                 "AMZN", "TSLA", "SBUX", "NKE", "WMT", "COST", "KO", "PEP",
                                                  "V", "PYPL", "BAC", "C", "WFC",
                                                  "JNJ","PFE", "UNH", "AMGN", "LLY",
                                                  "HON", "UNP", "MMM", "TT", "LMT",
                                                  "AMT", "EQIX", "PLD", "O" };*/

    void Start()
    {
        //슬라이더 바 상태 초기화
        LoadSlider.value = 0;
        progress1.text = "0%";
        progress2.text = "게임 맵 세팅 중...";//"DOWNLODING...(0/10)";

        //API 호출로 주식정보들 저장
        apiCall();
        //코루틴 함수 호출을 통해 로딩을 슬라이더 바로 보여줌
        coroutine1 = gameSetting();
        StartCoroutine(coroutine1);
    }

    //API요청하여 정보를 스크립터블 오브젝트에 저장
    //전체 갯수에 대비 비례한 프로그레스 바를 표시
    //인게임 씬로드의 경우 50%까지 비율 표시
    //50~100%의 경우 주식정보 로드 진행비율로 반영 
    //로드가 완료되면 인게임 씬을 로드하기

    IEnumerator gameSetting() 
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("InGame");
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            float prog = Mathf.Clamp01(op.progress / .9f)*5f;
            LoadSlider.value = prog;
            progress1.text = (prog*10).ToString()+"%";
            progress2.text = "게임 맵 세팅 중...";//"DOWNLODING...("+ prog.ToString()+ "/10)";

            if (prog >= 5)
            {
                while (totalStockCnt < codeList.Count)
                {
                    float prog2 = Mathf.Clamp01(totalStockCnt / codeList.Count)*5f;
                    LoadSlider.value = 5f+prog2;
                    progress1.text = ((5f + prog2) * 10).ToString() + "%";
                    progress2.text = "주식 정보 다운로드 중...";//"DOWNLODING...(" + (5f + prog2).ToString() + "/10)";
                    yield return null;
                }
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
    async Task apiCall()
    {
        try
        {
            foreach (string i in codeList)
            {
                await BeginNetwork(i);
            }
        }catch (Exception)
        {
            Debug.Log("API를 사용 중 에러가 발생했습니다.");
        }
    }
    async Task BeginNetwork(string code)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/v2/get-summary?symbol=" + code + "&region=US"),
            Headers =
    {
        { "x-rapidapi-key", "3473a4204dmsh3c2017b9c74173bp17eebbjsn6cfd3058fce6"},
        { "x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            //Debug.Log(obj["financialData"]["currentPrice"]["raw"]);
            
            //현재 시가
            string tmp1 = (string)obj["financialData"]["currentPrice"]["raw"];
            float.TryParse(tmp1, out float send_price);

            //배당일
            string send_divdate = (string)obj["calendarEvents"]["exDividendDate"]["fmt"];

            //배당률
            string tmp2 = (string)obj["summaryDetail"]["dividendRate"]["raw"];
            float.TryParse(tmp2, out float send_divrate);

            //관련 업종별 분류(sector)
            string send_sector = (string)obj["summaryProfile"]["sector"];

            //시가총액
            string tmp3 = (string)obj["price"]["marketCap"]["raw"];
            float.TryParse(tmp3, out float send_marketcap);

            //PER
            string tmp4 = (string)obj["summaryDetail"]["forwardPE"]["raw"];
            float.TryParse(tmp4, out float send_per);
            
            //시가 성장 변화(52 week change)
            string send_tmp4 = (string)obj["defaultKeyStatistics"]["52WeekChange"]["raw"];
            float.TryParse(send_tmp4, out float send_52);

            //previous Close
            string send_tmp5 = (string)obj["price"]["regularMarketPreviousClose"]["raw"];
            float.TryParse(send_tmp5, out float send_preclose);
            
            //volume
            string send_tmp6 = (string)obj["summaryDetail"]["volume"]["raw"];
            float.TryParse(send_tmp6, out float send_volume);

            //avg Volume(10day)
            string send_tmp7 = (string)obj["price"]["averageDailyVolume10Day"]["raw"];
            float.TryParse(send_tmp7, out float send_avgVolume);

            //정보를 얻은 주식의 갯수 업데이트
            totalStockCnt++;
            
            //apiInfo 정보 업데이트
            if (stockList.apiInfo.ContainsKey(code))
            {
                stockList.apiInfo[code].api_marketprice = send_price;
                stockList.apiInfo[code].api_divDate = send_divdate;
                stockList.apiInfo[code].api_divRate = send_divrate;
                stockList.apiInfo[code].api_sector = send_sector;
                stockList.apiInfo[code].api_marketcap = send_marketcap;
                stockList.apiInfo[code].api_per = send_per;
                stockList.apiInfo[code].api_52week = send_52;
                stockList.apiInfo[code].api_preclose = send_preclose; 
                stockList.apiInfo[code].api_volume = send_volume;
                stockList.apiInfo[code].api_avgVolume = send_avgVolume;
            }
            else
            {
                stockList.add(code, send_price, send_divdate, send_divrate, send_sector, send_marketcap, send_per, send_52, send_preclose, send_volume, send_avgVolume);
            }
        };
    }
}
