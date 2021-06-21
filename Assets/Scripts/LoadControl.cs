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
    //api키를 저장한 객체
    public API api;

    //사용하는 주식종목코드 배열
    List<string> codeList = new List<string>() { "MSFT", "ORCL" }; //"AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS", "AMZN", "TSLA", "HD", "LOW", "V", "PYPL", "BAC" };
    //종목코드와 얻은 api 자료를 저장한 딕셔너리
    Dictionary<string, APIData> apiInfo = new Dictionary<string, APIData>();
    public class APIData //API에서 불러온 데이터 return 하기 위한 클래스
    {
        public float api_marketprice; //현재 시가
        public string api_divDate; //배당일
        public float api_divRate; //배당률
        public string api_sector; //관련 업종별 분류
        public float api_marketcap; //시가총액
        public float api_per; //PER
        public float api_52week; //시가 성장 변화(52 week change)

        public APIData(float api_marketprice, string api_divDate, float api_divRate, string api_sector, float api_marketcap, float api_per, float api_52week)
        {
            this.api_marketprice = api_marketprice;
            this.api_divDate = api_divDate;
            this.api_divRate = api_divRate;
            this.api_sector = api_sector;
            this.api_marketcap = api_marketcap;
            this.api_per = api_per;
            this.api_52week = api_52week;
        }
    }

    void Start()
    {
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
    //저장이 완료되면 인게임 씬을 로드하기

    IEnumerator gameSetting() 
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("InGame");
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            float progress1 = Mathf.Clamp01(op.progress / .9f * .5f);
            LoadSlider.value = progress1; 
            if (progress1 >= 0.5)
            {
                while (totalStockCnt < codeList.Count)
                {
                    float progress2 = Mathf.Clamp01(totalStockCnt/ 10 * .5f);
                    LoadSlider.value = 0.5f + progress2;
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
        { "x-rapidapi-key", api.key },
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
            ;
            //시가 성장 변화(52 week change)
            string send_tmp4 = (string)obj["defaultKeyStatistics"]["52WeekChange"]["raw"];
            float.TryParse(send_tmp4, out float send_52);
            
            //정보를 얻은 주식의 갯수 업데이트
            totalStockCnt++;
            
            //apiInfo 정보 업데이트
            if (apiInfo.ContainsKey(code))
            {
                apiInfo[code].api_marketprice = send_price;
                apiInfo[code].api_divDate = send_divdate;
                apiInfo[code].api_divRate = send_divrate;
                apiInfo[code].api_sector = send_sector;
                apiInfo[code].api_marketcap = send_marketcap;
                apiInfo[code].api_per = send_per;
                apiInfo[code].api_52week = send_52;
                Debug.Log("배당일:");
                Debug.Log(send_divdate);
            }
            else
            {
                apiInfo.Add(code, new APIData(send_price, send_divdate, send_divrate, send_sector, send_marketcap, send_per, send_52));
            }
        };
    }
}
