using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCtrl : MonoBehaviour
{
    //1. ��� ������ �ö���� ����
    private Camera cam; //���� ȭ�� ī�޶�
    private GameObject mouseOn; //�̺�Ʈ ���Ǻ� �߻��ϴ� ����ƮUI
    private Text symbol;
    private portfolio myStocks;
    private StockList api;

    void Start()
    {
        myStocks = GameObject.Find("InGameControl").GetComponent<InGameControl>().myPortfolio;
        api = GameObject.Find("InGameControl").GetComponent<InGameControl>().list;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mouseOn = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform.GetChild(0).gameObject;
        symbol = mouseOn.transform.GetChild(0).GetComponent<Text>();
        mouseOn.SetActive(false);
    }
    void FixedUpdate()
    {
        var wantedPos = cam.WorldToScreenPoint(transform.position);
        mouseOn.transform.position = new Vector3(wantedPos.x + 20f, wantedPos.y + 375f, wantedPos.z); //��ǳ�� ��ġ ����
    }
    void Update()
    {
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp) { mouseOn.SetActive(false);  return; }
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().priceFlag)
        {
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            //���� ������ ��ܰ� ��� ���� ���� ���
            float percent = (api.apiInfo[transform.name].api_marketprice - myStocks.stockInfo[transform.name].avgCostPerShare) / myStocks.stockInfo[transform.name].avgCostPerShare * 100;
            symbol.text = percent.ToString() + "%";
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().volumeFlag)
        {
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!api.apiInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            //�ŷ��� ����
            float tmp = api.apiInfo[transform.name].api_marketcap;
            string marketcap = "";
            if (tmp >= 1000000000000) { marketcap = (tmp / 1000000000000).ToString("F3") + " T"; } //trillion
            else if (tmp >= 100000000000) { marketcap = (tmp / 100000000000).ToString("F3") + " B"; } //billion
            else { marketcap = tmp + ""; } //T, B ���� ���� �ִٸ� �߰��ϱ�
            symbol.text = marketcap;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().divFlag)
        {
            //���� ����� ������ ���� ���
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            string date1 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divDate(transform.name);
            if (date1.Length < 1) { mouseOn.SetActive(false); return; }
            TimeSpan dateDiff = DateTime.Parse(date1) - DateTime.Now;
            int tmp1 = dateDiff.Days;
            float tmp2 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().dividend(transform.name);
            symbol.text = "D-" + tmp1.ToString() + "\n";
            symbol.text += tmp2.ToString() + "$";
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().interestFlag)
        {
            //���ɵ� ���� ����
            symbol.text = "";
        }
        else { mouseOn.SetActive(false); return; }//üũ�� �ɼ��� ���� ��� ǥ�� X
        mouseOn.SetActive(true);
    }

    //2. ���콺�� �ø��� ������ �ö���� ����
    /*
    private Camera cam; //���� ȭ�� ī�޶�
    private GameObject mouseOn; //�̺�Ʈ ���Ǻ� �߻��ϴ� ����ƮUI
    private Text symbol;
    private bool OnMouse;
    private portfolio myStocks;
    private StockList api;

    void Start()
    {
        myStocks = GameObject.Find("InGameControl").GetComponent<InGameControl>().myPortfolio;
        api = GameObject.Find("InGameControl").GetComponent<InGameControl>().list;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mouseOn = GameObject.Find("Canvas").transform.Find("mouseOn").gameObject;
        symbol = mouseOn.transform.GetChild(0).GetComponent<Text>();
        mouseOn.SetActive(false); //��ǳ�� on
        OnMouse = false;
    }
    void FixedUpdate()//���콺 �̵��� �� ��ǳ�� ��ġ�� ���Ž�Ŵ
    {
        if (OnMouse)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition); //���� ���콺 Ŀ�� ��ġ
            var wantedPos = cam.WorldToScreenPoint(transform.position);
            mouseOn.transform.position = new Vector3(wantedPos.x + 20f, wantedPos.y + 375f, wantedPos.z); //��ǳ�� ��ġ ����
        }
    }
    �ش� ������Ʈ ���� ���콺�� �ö󰡸� ��ǳ�� on
    private void OnMouseEnter()
    {
        
        //if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp ||
        //    (Camera.main.GetComponent<DemoScript>().StockInfoMenuPopUp) { return; }

        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp) { return;  }

        symbol.text = gameObject.name; //��ǳ�� �ؽ�Ʈ ���� �̸����� ����
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().priceFlag)
        {
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { return; }
            //���� ������ ��ܰ� ��� ���� ���� ���
            float percent = (api.apiInfo[transform.name].api_marketprice - myStocks.stockInfo[transform.name].avgCostPerShare)/ myStocks.stockInfo[transform.name].avgCostPerShare*100;
            symbol.text = percent.ToString()+"%";
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().volumeFlag)
        {
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!api.apiInfo.ContainsKey(transform.name)) { return; }
            //�ŷ��� ����
            float tmp = api.apiInfo[transform.name].api_marketcap;
            string marketcap = "";
            if (tmp >= 1000000000000) { marketcap = (tmp / 1000000000000).ToString("F3") + " T"; } //trillion
            else if (tmp >= 100000000000) { marketcap = (tmp / 100000000000).ToString("F3") + " B"; } //billion
            else { marketcap = tmp + ""; } //T, B ���� ���� �ִٸ� �߰��ϱ�
            symbol.text =  marketcap;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().divFlag)
        {
            //���� ����� ������ ���� ���
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { return; }
            string date1 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divDate(transform.name);
            if(date1.Length < 1) { return; }
            TimeSpan dateDiff = DateTime.Parse(date1) - DateTime.Now;
            int tmp1 = dateDiff.Days;
            float tmp2 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().dividend(transform.name);
            symbol.text = "D-"+tmp1.ToString()+"\n";
            symbol.text +=  tmp2.ToString()+"$";
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().interestFlag)
        {
            //���ɵ� ���� ����
            symbol.text = "";
        }
        else{ return; }//üũ�� �ɼ��� ���� ��� ǥ�� X
        mouseOn.SetActive(true);
        OnMouse = true;
    }

    //�ش� ������Ʈ ���� ���콺�� �ö󰡸� ��ǳ�� off
    private void OnMouseExit()
    {
        mouseOn.SetActive(false);
        OnMouse = false;
    }
    */
}
