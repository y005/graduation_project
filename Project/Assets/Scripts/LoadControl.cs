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

    //사용하는 주식의 종목코드 배열
    List<string> codeList = new List<string>() { "MSFT", "GOOGL", "SBUX", "PYPL" };
    //List<string> codeList = new List<string>() { "MSFT", "ORCL", "AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS", "AMZN", "TSLA", "SBUX", "NKE", "V", "PYPL", "BAC" };

    void Start()
    {
        //슬라이더 바 상태 초기화
        LoadSlider.value = 0;
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
            float progress1 = Mathf.Clamp01(op.progress / .9f)*5f;
            LoadSlider.value = progress1;
            if (progress1 >= 5)
            {
                while (totalStockCnt < codeList.Count)
                {
                    float progress2 = Mathf.Clamp01(totalStockCnt / codeList.Count)*5f;
                    LoadSlider.value = 5f+progress2;
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
        { "x-rapidapi-key", "bd2bc7360bmsha9dc79919d1c7e9p1641b7jsnb4bd1c60dbe6"},
        { "x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);

            //현재 시가
            string tmp1 = (string)obj["financialData"]["currentPrice"]["raw"];
            float.TryParse(tmp1, out float send_price);

            //배당일
            string send_divdate = (string)obj["calendarEvents"]["dividendDate"]["fmt"];

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
            }
            else
            {
                stockList.add(code, send_price, send_divdate, send_divrate, send_sector, send_marketcap, send_per, send_52, send_preclose);
            }
        };
    }
}
