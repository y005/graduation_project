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
using TMPro;
public class MainMenuControl : MonoBehaviour
{
    public GameObject start;
    public GameObject apiSetting;
    public TextMeshProUGUI fgi;
    public TextMeshProUGUI yahoo;
    public TextMeshProUGUI youtube;
    public API api;
    bool flag = true;
    public void StartBtnClick()
    {
        //유효한 API인 경우에만 인게임으로 진입한다.
        if (true) { 
            SceneManager.LoadScene("Load");
        }
        //유효하지 않은 경우 상태메세지를 출력한다. 
        else
        {
            Debug.Log("유효한 API키를 입력하세요");
        }
    }
    public void apiSettingBtnClick()
    {
        if (flag)
        {
            start.SetActive(false);
            apiSetting.SetActive(true);
            flag = false;
        }
        else
        {
            start.SetActive(true);
            apiSetting.SetActive(false);
            flag = true;
        }
    }
    public void InsertBtnClick()
    {
        api.fgi = fgi.text;
        api.yahoo = yahoo.text;
        api.youtube = youtube.text;
    }
}
