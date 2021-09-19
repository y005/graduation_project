using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class youtube : MonoBehaviour
{
    //���ϴ� ��Ʃ�� ������ ���� ������ URL�� �����ؼ� ��û�ϴ� ��� 
    const string baseURL = "https://www.googleapis.com/youtube/v3";
    const string apiKey = "AIzaSyAuAYznsu0YJDB2Pbv3_ukCJwtGDZCHnNM";
    public List<string> keywords = new List<string>() { "īī��" };
    public List<string> idList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(youtubeStat());
    }
    
    IEnumerator youtubeStat()
    {   
        foreach(string keyword in keywords)
        {
            //���� �ڵ�� �˻��� �ֱ� 1���ϰ� ���� ���� ID�� ��û�ϴ� URL
            DateTime today = DateTime.Now;
            string query1 = baseURL + "/search?part=id&maxResults=2&order=viewCount&publishedAfter=" + today.AddDays(-7).ToString(("yyyy-MM-dd")) + "T00:00:00Z&publishedBefore=" + today.ToString(("yyyy-MM-dd")) + "T00:00:00Z&q=" + keyword + "&type=video&videoDefinition=high&key=" + apiKey;

            WWW w1 = new WWW(query1);
            yield return w1;
            JObject result = JObject.Parse(w1.text);
            //����� ���� ����Ʈ �߿��� ���� ID�� ���� ����Ʈ�� ��ȯ
            foreach (JObject item in result["items"])
            {
                idList.Add(item["id"]["videoId"].ToString());
            }
            foreach (var videoID in idList)
            {
                //������ �����ǥ�� ��û�ϴ� URL (��ȸ��, ���ƿ�, �Ⱦ��, ��ۼ�)
                string query = baseURL + "/videos?part=statistics&id=" + videoID + "&key=" + apiKey;

                WWW w2 = new WWW(query);
                yield return w2;

                JObject result1 = JObject.Parse(w2.text);
                foreach (JObject item in result1["items"])
                {
                    int value = int.Parse(item["statistics"]["viewCount"].ToString());
                    int value1 = int.Parse(item["statistics"]["likeCount"].ToString());
                    int value2 = int.Parse(item["statistics"]["dislikeCount"].ToString());
                    int value3 = int.Parse(item["statistics"]["commentCount"].ToString());
                    Debug.Log(keyword);
                    Debug.Log(value);
                    Debug.Log(value1); 
                    Debug.Log(value2);
                    Debug.Log(value3);
                }
            }
        }
    }
}
