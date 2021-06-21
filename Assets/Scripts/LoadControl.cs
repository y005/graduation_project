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
    //���α׷��� ��
    public Slider LoadSlider;
    //apiŰ�� ������ ��ü
    public API api;

    //����ϴ� �ֽ������ڵ� �迭
    List<string> codeList = new List<string>() { "MSFT", "ORCL" }; //"AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS", "AMZN", "TSLA", "HD", "LOW", "V", "PYPL", "BAC" };
    //�����ڵ�� ���� api �ڷḦ ������ ��ųʸ�
    Dictionary<string, APIData> apiInfo = new Dictionary<string, APIData>();
    public class APIData //API���� �ҷ��� ������ return �ϱ� ���� Ŭ����
    {
        public float api_marketprice; //���� �ð�
        public string api_divDate; //�����
        public float api_divRate; //����
        public string api_sector; //���� ������ �з�
        public float api_marketcap; //�ð��Ѿ�
        public float api_per; //PER
        public float api_52week; //�ð� ���� ��ȭ(52 week change)

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
        //API ȣ��� �ֽ������� ����
        apiCall();
        //�ڷ�ƾ �Լ� ȣ���� ���� �ε��� �����̴� �ٷ� ������
        coroutine1 = gameSetting();
        StartCoroutine(coroutine1);
    }

    //API��û�Ͽ� ������ ��ũ���ͺ� ������Ʈ�� ����
    //��ü ������ ��� ����� ���α׷��� �ٸ� ǥ��
    //�ΰ��� ���ε��� ��� 50%���� ���� ǥ��
    //50~100%�� ��� �ֽ����� �ε� ��������� �ݿ� 
    //������ �Ϸ�Ǹ� �ΰ��� ���� �ε��ϱ�

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
            Debug.Log("API�� ��� �� ������ �߻��߽��ϴ�.");
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

            //���� �ð�
            string tmp1 = (string)obj["financialData"]["currentPrice"]["raw"];
            float.TryParse(tmp1, out float send_price);

            //�����
            string send_divdate = (string)obj["calendarEvents"]["dividendDate"]["fmt"];

            //����
            string tmp2 = (string)obj["summaryDetail"]["dividendRate"]["raw"];
            float.TryParse(tmp2, out float send_divrate);

            //���� ������ �з�(sector)
            string send_sector = (string)obj["summaryProfile"]["sector"];

            //�ð��Ѿ�
            string tmp3 = (string)obj["price"]["marketCap"]["raw"];
            float.TryParse(tmp3, out float send_marketcap);

            //PER
            string tmp4 = (string)obj["summaryDetail"]["forwardPE"]["raw"];
            float.TryParse(tmp4, out float send_per);
            ;
            //�ð� ���� ��ȭ(52 week change)
            string send_tmp4 = (string)obj["defaultKeyStatistics"]["52WeekChange"]["raw"];
            float.TryParse(send_tmp4, out float send_52);
            
            //������ ���� �ֽ��� ���� ������Ʈ
            totalStockCnt++;
            
            //apiInfo ���� ������Ʈ
            if (apiInfo.ContainsKey(code))
            {
                apiInfo[code].api_marketprice = send_price;
                apiInfo[code].api_divDate = send_divdate;
                apiInfo[code].api_divRate = send_divrate;
                apiInfo[code].api_sector = send_sector;
                apiInfo[code].api_marketcap = send_marketcap;
                apiInfo[code].api_per = send_per;
                apiInfo[code].api_52week = send_52;
                Debug.Log("�����:");
                Debug.Log(send_divdate);
            }
            else
            {
                apiInfo.Add(code, new APIData(send_price, send_divdate, send_divrate, send_sector, send_marketcap, send_per, send_52));
            }
        };
    }
}
