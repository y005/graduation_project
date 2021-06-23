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
    public Text apikey;
    public API api;

    public async void StartBtnClick()
    {
        bool result = true; //await checkApi();
        //��ȿ�� API�� ��쿡�� �ΰ������� �����Ѵ�.
        if (result) {
            //api�� ��ũ���ͺ� ������Ʈ�� ����
            //api.key = apikey.text;
            SceneManager.LoadScene("Load");
        }
        //��ȿ���� ���� ��� ���¸޼����� ����Ѵ�. 
        else
        {
            Debug.Log("��ȿ�� APIŰ�� �Է��ϼ���");
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
        { "x-rapidapi-key", "bd2bc7360bmsha9dc79919d1c7e9p1641b7jsnb4bd1c60dbe6" },
        { "x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com" },
    },
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            //JSON������ ���� �ľ�
            //Debug.Log(obj);
        };
    }

}
