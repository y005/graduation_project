using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;

public class stock : MonoBehaviour
{
    //���ϴ� ��Ʃ�� ������ ���� ������ URL�� �����ؼ� ��û�ϴ� ��� 
    const string baseURL = "https://www.googleapis.com/youtube/v3";
    const string apiKey = "AIzaSyAuAYznsu0YJDB2Pbv3_ukCJwtGDZCHnNM";
    string videoid = "3mxHOuIA2T8";
    private string pageToken = "";//���ϴ� ���� ���ð� ���õ� Ű����(������ ��ȸ�� ������ ��õ ���� �������� Ű���� ã�ƾߵ�)
    private string keywords = "īī��+�ֽ�";//�˻��� Ű����� ����� + ǥ��
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Search(keywords));
    }

     IEnumerator Search(string keywords)
     {
        // Pull down the JSON from YouTube
        string query = "";

        //search �޼ҵ�
       if (pageToken == "")
        {
            query = baseURL + "/search?part=snippet&maxResults=1&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }
        else
        {
            query = baseURL + "/search?pageToken=" + pageToken + "&part=snippet&maxResults=1&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }

        //videos �޼ҵ� (��ȸ��, ���ƿ�, �Ⱦ��, favorite Count?!, ��ۼ�)
        //query = baseURL + "/videos?part=statistics&id=" + videoid + "&key=" + apiKey;

        WWW w = new WWW(query);
        yield return w;
        JObject result = JObject.Parse(w.text);
        Debug.Log(result);
     }
}
