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
    //APIŰ�� ������ ��ü
    public API api;
    //�ֽ� �������� ������ ��ü
    public StockList stockList;

    //����ϴ� �ֽ��� �����ڵ� �迭
    List<string> codeList = new List<string>() { "MSFT", "GOOGL", "SBUX", "PYPL" };
    //List<string> codeList = new List<string>() { "MSFT", "ORCL", "AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS", "AMZN", "TSLA", "SBUX", "NKE", "V", "PYPL", "BAC" };

    void Start()
    {
        //�����̴� �� ���� �ʱ�ȭ
        LoadSlider.value = 0;
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
    //�ε尡 �Ϸ�Ǹ� �ΰ��� ���� �ε��ϱ�

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
        { "x-rapidapi-key", "bd2bc7360bmsha9dc79919d1c7e9p1641b7jsnb4bd1c60dbe6"},
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
            
            //�ð� ���� ��ȭ(52 week change)
            string send_tmp4 = (string)obj["defaultKeyStatistics"]["52WeekChange"]["raw"];
            float.TryParse(send_tmp4, out float send_52);

            //previous Close
            string send_tmp5 = (string)obj["price"]["regularMarketPreviousClose"]["raw"];
            float.TryParse(send_tmp5, out float send_preclose);

            //������ ���� �ֽ��� ���� ������Ʈ
            totalStockCnt++;
            
            //apiInfo ���� ������Ʈ
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
