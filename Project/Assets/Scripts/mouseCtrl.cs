using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCtrl : MonoBehaviour
{
    //1. 계속 정보가 올라오는 형태
    private Camera cam; //게임 화면 카메라
    private GameObject mouseOn; //이벤트 조건별 발생하는 이펙트UI
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
        mouseOn.transform.position = new Vector3(wantedPos.x + 20f, wantedPos.y + 375f, wantedPos.z); //말풍선 위치 변경
    }
    void Update()
    {
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp) { mouseOn.SetActive(false);  return; }
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().priceFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            //보유 종목의 평단가 대비 가격 비율 계산
            float percent = (api.apiInfo[transform.name].api_marketprice - myStocks.stockInfo[transform.name].avgCostPerShare) / myStocks.stockInfo[transform.name].avgCostPerShare * 100;
            symbol.text = percent.ToString() + "%";
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().volumeFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!api.apiInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            //거래량 정보
            float tmp = api.apiInfo[transform.name].api_marketcap;
            string marketcap = "";
            if (tmp >= 1000000000000) { marketcap = (tmp / 1000000000000).ToString("F3") + " T"; } //trillion
            else if (tmp >= 100000000000) { marketcap = (tmp / 100000000000).ToString("F3") + " B"; } //billion
            else { marketcap = tmp + ""; } //T, B 외의 단위 있다면 추가하기
            symbol.text = marketcap;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().divFlag)
        {
            //남은 배당일 정보와 배당액 계산
            //보유 종목이 아닌 경우 정보 표시 제외
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
            //관심도 지수 정보
            symbol.text = "";
        }
        else { mouseOn.SetActive(false); return; }//체크된 옵션이 없는 경우 표시 X
        mouseOn.SetActive(true);
    }

    //2. 마우스를 올리면 정보가 올라오는 형태
    /*
    private Camera cam; //게임 화면 카메라
    private GameObject mouseOn; //이벤트 조건별 발생하는 이펙트UI
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
        mouseOn.SetActive(false); //말풍선 on
        OnMouse = false;
    }
    void FixedUpdate()//마우스 이동할 때 말풍선 위치를 갱신시킴
    {
        if (OnMouse)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition); //현재 마우스 커서 위치
            var wantedPos = cam.WorldToScreenPoint(transform.position);
            mouseOn.transform.position = new Vector3(wantedPos.x + 20f, wantedPos.y + 375f, wantedPos.z); //말풍선 위치 변경
        }
    }
    해당 오브젝트 위에 마우스가 올라가면 말풍선 on
    private void OnMouseEnter()
    {
        
        //if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp ||
        //    (Camera.main.GetComponent<DemoScript>().StockInfoMenuPopUp) { return; }

        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp) { return;  }

        symbol.text = gameObject.name; //말풍선 텍스트 종목 이름으로 변경
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().priceFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { return; }
            //보유 종목의 평단가 대비 가격 비율 계산
            float percent = (api.apiInfo[transform.name].api_marketprice - myStocks.stockInfo[transform.name].avgCostPerShare)/ myStocks.stockInfo[transform.name].avgCostPerShare*100;
            symbol.text = percent.ToString()+"%";
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().volumeFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!api.apiInfo.ContainsKey(transform.name)) { return; }
            //거래량 정보
            float tmp = api.apiInfo[transform.name].api_marketcap;
            string marketcap = "";
            if (tmp >= 1000000000000) { marketcap = (tmp / 1000000000000).ToString("F3") + " T"; } //trillion
            else if (tmp >= 100000000000) { marketcap = (tmp / 100000000000).ToString("F3") + " B"; } //billion
            else { marketcap = tmp + ""; } //T, B 외의 단위 있다면 추가하기
            symbol.text =  marketcap;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().divFlag)
        {
            //남은 배당일 정보와 배당액 계산
            //보유 종목이 아닌 경우 정보 표시 제외
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
            //관심도 지수 정보
            symbol.text = "";
        }
        else{ return; }//체크된 옵션이 없는 경우 표시 X
        mouseOn.SetActive(true);
        OnMouse = true;
    }

    //해당 오브젝트 위에 마우스가 올라가면 말풍선 off
    private void OnMouseExit()
    {
        mouseOn.SetActive(false);
        OnMouse = false;
    }
    */
}
