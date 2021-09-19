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
    float yahooApiCnt = 0;
    float youtubeApiCnt = 0;
    float sentimentApiCnt = 0;

    //���α׷��� ��
    public Slider LoadSlider;
    //APIŰ�� ������ ��ü
    public API api;
    //�ֽ� �������� ������ ��ü
    public StockList stockList;

    public TextMeshProUGUI progress1; // ������� ���� UI
    public TextMeshProUGUI progress2; // ������� ���� UI

    //����ϴ� �ֽ��� �����ڵ� �迭
    List<string> codeList = new List<string>() { "MSFT", "ORCL", "AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS",
                                                 "AMZN", "TSLA", "SBUX", "NKE", "WMT", "COST", "KO", "PEP",
                                                  "V", "PYPL", "BAC", "C", "WFC",
                                                  "JNJ","PFE", "UNH", "AMGN", "LLY",
                                                  "HON", "UNP", "MMM", "TT", "LMT",
                                                  "AMT", "EQIX", "PLD", "O" };

    //api Ű ������
    string yahooApiKey = "f977847401mshd7a2fe4cba6191fp1ae565jsnf4676589249d";
    string youtubeApiKey = "AIzaSyDbfW37L3 - gRuKPrvU2yZDVP_ySxr1wttU";//"AIzaSyAuAYznsu0YJDB2Pbv3_ukCJwtGDZCHnNM";

    void Start()
    {
        //�����̴� �� ���� �ʱ�ȭ
        LoadSlider.value = 0;
        progress1.text = "0%";
        progress2.text = "���� �� ���� ��...";
        
        //API ȣ��� api���� ����
        apiCall();
        //StartCoroutine(youtubeStat());
        //�ε��������ٷ� ���� �Լ� ������
        StartCoroutine(gameSetting());
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
            float prog = Mathf.Clamp01(op.progress / .9f) * 1f;
            LoadSlider.value = prog;
            progress1.text = (prog * 10).ToString() + "%";
            progress2.text = "���� �� ���� ��...";

            if (prog >= 1)
            {
                while (yahooApiCnt < codeList.Count)
                {
                    float prog2 = Mathf.Clamp01(yahooApiCnt / codeList.Count) * 3f;
                    LoadSlider.value = 1f + prog2;
                    progress1.text = ((1f + prog2) * 10).ToString("F0") + "%";
                    progress2.text = "�ֽ� ���� �ٿ�ε� ��...";
                    yield return null;
                }
                while (youtubeApiCnt < codeList.Count)
                {
                    float prog3 = Mathf.Clamp01(youtubeApiCnt / codeList.Count) * 3f;
                    LoadSlider.value = 4f + prog3;
                    progress1.text = ((4f + prog3) * 10).ToString("F0") + "%";
                    progress2.text = "��Ʃ�� ��� �ٿ�ε� ��...";
                    yield return null;
                }
                while(sentimentApiCnt < codeList.Count)
                {
                    float prog4 = Mathf.Clamp01(sentimentApiCnt / codeList.Count) * 3f;
                    LoadSlider.value = 7f + prog4;
                    progress1.text = ((7f + prog4) * 10).ToString("F0") + "%";
                    progress2.text = "���� �����м� ��...";
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
            await FearAndGreed();
            foreach (string code in codeList)
            {
                //await yahooFinanceApi(code);
                yahooApiCnt = codeList.Count;
                youtubeApiCnt = codeList.Count;
                await sentimentApi(code);
            }
        }
        catch (Exception)
        {
            Debug.Log("API�� ��� �� ������ �߻��߽��ϴ�.");
        }
    }
    async Task yahooFinanceApi(string code)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/v2/get-summary?symbol=" + code + "&region=US"),
            Headers =
    {
        { "x-rapidapi-key", yahooApiKey},
        { "x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            //Debug.Log(obj["financialData"]["currentPrice"]["raw"]);

            //���� �ð�
            string tmp1 = (string)obj["financialData"]["currentPrice"]["raw"];
            float.TryParse(tmp1, out float send_price);

            //�����
            string send_divdate = (string)obj["calendarEvents"]["exDividendDate"]["fmt"];

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

            //volume
            string send_tmp6 = (string)obj["summaryDetail"]["volume"]["raw"];
            float.TryParse(send_tmp6, out float send_volume);

            //avg Volume(10day)
            string send_tmp7 = (string)obj["price"]["averageDailyVolume10Day"]["raw"];
            float.TryParse(send_tmp7, out float send_avgVolume);

            //������ ���� �ֽ��� ���� ������Ʈ
            yahooApiCnt++;

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
                stockList.apiInfo[code].api_volume = send_volume;
                stockList.apiInfo[code].api_avgVolume = send_avgVolume;
            }
            else
            {
                stockList.add(code, send_price, send_divdate, send_divrate, send_sector, send_marketcap, send_per, send_52, send_preclose, send_volume, send_avgVolume);
            }
        };
    }
    async Task sentimentApi(string code)
    {
        string uri = "http://3.37.190.174:5000/sentiment/"+code;
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(uri),
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            stockList.addSenti(code, (int)obj["result"]["positive ratio"], (int)obj["result"]["negative ratio"]);
            sentimentApiCnt++;
        };
    }
    IEnumerator youtubeStat()
    {
        DateTime today = DateTime.Now;
        string tmp;
        string baseURL = "https://www.googleapis.com/youtube/v3";
        int val;
        foreach (string keyword in codeList)
        {
            List<string> idList = new List<string>();
            //���� �ڵ�� �˻��� �ֱ� �� �ް� ���� ���� ID�� ��û�ϴ� URL
            string query1 = baseURL + "/search?part=id&maxResults=100&order=viewCount&publishedAfter=" + today.AddDays(-30).ToString(("yyyy-MM-dd")) + "T00:00:00Z&publishedBefore=" + today.ToString(("yyyy-MM-dd")) + "T00:00:00Z&q=" + keyword + "+stock&type=video&videoDefinition=high&key=" + youtubeApiKey;

            WWW w1 = new WWW(query1);
            yield return w1;
            JObject result = JObject.Parse(w1.text);
            //����� ���� ����Ʈ �߿��� ���� ID�� ���� ����Ʈ�� ��ȯ
            try
            {
                foreach (JObject item in result["items"])
                {
                    idList.Add(item["id"]["videoId"].ToString());
                }
            }
            catch (Exception)
            {
                Debug.Log("API�� ��� �� ������ �߻��߽��ϴ�.");
                youtubeApiCnt++;
                continue;
            }
            int totalView = 0;
            int totalLike = 0;
            int totalDislike = 0;
            int totalComment = 0;
            int cnt = 0;
            foreach (var videoID in idList)
            {
                //������ �����ǥ�� ��û�ϴ� URL (��ȸ��, ���ƿ�, �Ⱦ��, ��ۼ�)
                string query = baseURL + "/videos?part=statistics&id=" + videoID + "&key=" + youtubeApiKey;

                WWW w2 = new WWW(query);
                yield return w2;

                JObject result1 = JObject.Parse(w2.text);
                foreach (JObject item in result1["items"])
                {
                    try
                    {
                        tmp = item["statistics"]["viewCount"].ToString();
                        val = int.Parse(tmp);
                        if (val < 1000) { break; }
                        totalView += val;
                        cnt++;

                        tmp = item["statistics"]["likeCount"].ToString();
                        val = int.Parse(tmp);
                        totalLike += val;

                        tmp = item["statistics"]["dislikeCount"].ToString();
                        val = int.Parse(tmp);
                        totalDislike += val;

                        tmp = item["statistics"]["commentCount"].ToString();
                        val = int.Parse(tmp);
                        totalComment += val;
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            stockList.addYoutube(keyword, cnt, totalView, totalLike, totalDislike, totalComment);
            youtubeApiCnt++;
        }
    }
    async Task FearAndGreed()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://fear-and-greed-index.p.rapidapi.com/v1/fgi"),
            Headers =
    {
        { "x-rapidapi-host", "fear-and-greed-index.p.rapidapi.com" },
        { "x-rapidapi-key", yahooApiKey },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            stockList.fearAndGreed = (int)obj["fgi"]["now"]["value"];
        }
    }
}
