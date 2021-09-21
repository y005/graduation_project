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
    //원하는 유튜브 동영상 관련 정보를 URL로 조합해서 요청하는 방식 
    const string baseURL = "https://www.googleapis.com/youtube/v3";
    const string apiKey = "AIzaSyAGRkQrjnyX95_mPdJ_IJejXztgjPAVYWw"; 
    private string pageToken = "";//원하는 정보 선택과 관련된 키워드(영상의 조회수 정보랑 추천 정보 가져오는 키워드 찾아야됨)
    private string keywords = "카카오+주식";//검색할 키워드로 띄어쓰기는 + 표시
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
