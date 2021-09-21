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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    //���ϴ� ��Ʃ�� ������ ���� ������ URL�� �����ؼ� ��û�ϴ� ��� 
    const string baseURL = "https://www.googleapis.com/youtube/v3";
    const string apiKey = "AIzaSyAGRkQrjnyX95_mPdJ_IJejXztgjPAVYWw"; 
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

        if (pageToken == "")
        {
            query = baseURL + "/search?part=snippet&maxResults=12&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }
        else
        {
            query = baseURL + "/search?pageToken=" + pageToken + "&part=snippet&maxResults=12&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }

        WWW w = new WWW(query);
        yield return w;
        JObject result = JObject.Parse(w.text);
        Debug.Log(result);
    }
}
