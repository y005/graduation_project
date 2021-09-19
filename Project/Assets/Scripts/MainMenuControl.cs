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
    public TextMeshProUGUI yahoo;
    public TextMeshProUGUI youtube;
    public API api;
    bool flag = true;
    public void StartBtnClick()
    {
        //��ȿ�� API�� ��쿡�� �ΰ������� �����Ѵ�.
        if (true) { 
            SceneManager.LoadScene("Load");
        }
        //��ȿ���� ���� ��� ���¸޼����� ����Ѵ�. 
        else
        {
            Debug.Log("��ȿ�� APIŰ�� �Է��ϼ���");
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
        if (checkApiInput())
        {
            api.yahoo = yahoo.text;
            api.youtube = youtube.text;
        }
        inputsClear();
    }
    bool checkApiInput()
    {
        if ((yahoo.text == "") && (youtube.text == "")) { return false; }
        return true;
    }
    //���� ���� ������ �Ǹ� ��ǲ�ʵ� ���� ��������.
    void inputsClear()
    {
        yahoo.text = "���� apiŰ�� �Է��ϼ���";
        youtube.text = "��Ʃ�� apiŰ�� �Է��ϼ���";
    }
}
