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
    Vector3 m_Size;

    void Start()
    {
        myStocks = GameObject.Find("InGameControl").GetComponent<InGameControl>().myPortfolio;
        api = GameObject.Find("InGameControl").GetComponent<InGameControl>().list;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_Size = GetComponent<Collider>().bounds.size;
        mouseOn = (GameObject)Instantiate(Resources.Load("Prefabs/etc/mouseOn"));
        mouseOn.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);
        symbol = mouseOn.transform.GetChild(0).GetComponent<Text>();
        mouseOn.SetActive(false);
    }

    void FixedUpdate()
    {
        //콜라이더 큐브의 폭을 이용해서 배치할 위치인 좌측 상단의 좌표를 찾아냄
        Vector3 tmp = new Vector3(transform.position.x - (m_Size.x / 2), transform.position.y + m_Size.y*1.6f, transform.position.z - (m_Size.z / 2));
        var wantedPos = cam.WorldToScreenPoint(tmp);
        mouseOn.transform.position = new Vector3(wantedPos.x, wantedPos.y, wantedPos.z); //말풍선 위치 변경

        //조건1 확인:화면에 다른 창(설정창,포폴창,세부정보창)이 떠있는 경우에는 띄우지 X
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp) { mouseOn.SetActive(false); return; }
        //말풍선 배치 디버깅 용 임시코드
        //if (true) { symbol.text = ""; mouseOn.SetActive(true); return; }

        //조건 2 확인: 시티뷰일 경우 말풍선의 스케일 변경
        if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView){mouseOn.transform.localScale = new Vector3(0.2f,0.2f,1f);}
        else{mouseOn.transform.localScale = new Vector3(0.8f, 0.8f, 1f);}
        //조건3 확인: 선택한 레이어의 정보 제공이 가능한 경우에만 말풍선 정보를 띄움
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().priceFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            if (myStocks.stockInfo[transform.name].shares == 0) { mouseOn.SetActive(false); return; }
            //보유 종목의 평단가 대비 현재 주가 비율 계산
            float percent = (api.apiInfo[transform.name].api_marketprice - myStocks.stockInfo[transform.name].avgCostPerShare) / myStocks.stockInfo[transform.name].avgCostPerShare * 100;
            symbol.text = percent.ToString("F2") + "%";
            //시티뷰 일 경우 색깔 정보로 표시
            if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView)
            {
                if (percent < 0) { mouseOn.GetComponent<Image>().color = new Color(0, 0, 1); }
                else { mouseOn.GetComponent<Image>().color = new Color(1, 0, 0); }
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205 / 255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            else
            {
                mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205/255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            mouseOn.SetActive(true);
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().volumeFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!api.apiInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            //10일 평균 거래량 대비 현재 거래량 비율 계산
            float percent = (api.apiInfo[transform.name].api_volume - api.apiInfo[transform.name].api_avgVolume) / api.apiInfo[transform.name].api_avgVolume * 100;
            symbol.text = percent.ToString("F2") + "%";

            //시티뷰 일 경우 색깔 정보로 표시
            if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView)
            {
                if (percent < 0) { mouseOn.GetComponent<Image>().color = new Color(0, 0, 1); }
                else { mouseOn.GetComponent<Image>().color = new Color(1, 0, 0); }
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205 / 255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            else
            {
                mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205 / 255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            mouseOn.SetActive(true);
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().divFlag)
        {
            //남은 배당일 정보와 배당액 계산
            if (!api.apiInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            string date1 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divDate(transform.name);
            if (date1.Length < 1) { mouseOn.SetActive(false); return; }
            TimeSpan dateDiff = DateTime.Parse(date1) - DateTime.Now;
            int tmp1 = dateDiff.Days;
            symbol.text = "D-" + tmp1.ToString() + "\n";
            //보유 종목인 경우에만 받을 배당액 정보 표시
            mouseOn.SetActive(true);
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { return; }
            if (myStocks.stockInfo[transform.name].shares == 0) { return; }
            float tmp2 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().dividend(transform.name);
            symbol.text += tmp2.ToString("F2") + "$";
            mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
            Color color = mouseOn.GetComponent<Image>().color;
            color.a = 205 / 255f;
            mouseOn.GetComponent<Image>().color = color;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().interestFlag)
        {
            //관심도 지수 정보
            symbol.text = "";
            mouseOn.SetActive(true);
            mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
            Color color = mouseOn.GetComponent<Image>().color;
            color.a = 205 / 255f;
            mouseOn.GetComponent<Image>().color = color;
        }
        else { mouseOn.SetActive(false); return; }//체크된 옵션이 없는 경우 표시 X
    }
}
