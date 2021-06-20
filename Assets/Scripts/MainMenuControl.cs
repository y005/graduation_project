using System;
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

public class MainMenuControl : MonoBehaviour
{
    public Text API;

    public async void StartBtnClick()
    {
        bool result = await checkApi();
        //유효한 API인 경우에만 인게임으로 진입한다.
        if (result) {
            SceneManager.LoadScene("InGame");
        }
        //유효하지 않은 경우 상태메세지를 출력한다. 
        else
        {
            Debug.Log("유효한 API키를 입력하세요");
        }
    }
    async Task<bool> checkApi()
    {
        try {
            await BeginNetwork();
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    async Task BeginNetwork()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://apidojo-yahoo-finance-v1.p.rapidapi.com/stock/v2/get-summary?symbol=AMRN&region=US"),
            Headers =
    {
        { "x-rapidapi-key", API.text },
        { "x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            //JSON형태의 구조 파악
            //Debug.Log(obj);
        };
    }

}
