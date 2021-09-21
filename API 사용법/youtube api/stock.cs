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
    //원하는 유튜브 동영상 관련 정보를 URL로 조합해서 요청하는 방식 
    const string baseURL = "https://www.googleapis.com/youtube/v3";
    const string apiKey = "api ";
    string videoid = "3mxHOuIA2T8";
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

        //search 메소드
       if (pageToken == "")
        {
            query = baseURL + "/search?part=snippet&maxResults=1&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }
        else
        {
            query = baseURL + "/search?pageToken=" + pageToken + "&part=snippet&maxResults=1&order=relevance&q=" + keywords + "&type=video&videoDefinition=high&key=" + apiKey;
        }

        //videos 메소드 (조회수, 좋아요, 싫어요, favorite Count?!, 댓글수)
        //query = baseURL + "/videos?part=statistics&id=" + videoid + "&key=" + apiKey;

        WWW w = new WWW(query);
        yield return w;
        JObject result = JObject.Parse(w.text);
        Debug.Log(result);
     }
}
