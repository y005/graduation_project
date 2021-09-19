using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class mouseCtrl : MonoBehaviour
{
    private Camera cam; //게임 화면 카메라
    private GameObject mouseOn; //이벤트 조건별 발생하는 말풍선UI
    private GameObject sentiment; //이벤트 조건별 발생하는 게이지바UI
    private TextMeshProUGUI symbol;
    private portfolio myStocks;
    private StockList api;
    private GameObject upEffect; //상승 수치일 경우 이펙트UI
    private GameObject downEffect; //하락 수치일 경우 이펙트UI
    private GameObject lightEffect; //관심도가 많을 때 이펙트UI
    private GameObject sleepEffect; //관심도가 저조할 때 이펙트UI
    private Vector3 m_Size;

    void Start()
    {
        myStocks = GameObject.Find("InGameControl").GetComponent<InGameControl>().myPortfolio;
        api = GameObject.Find("InGameControl").GetComponent<InGameControl>().list;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_Size = GetComponent<Collider>().bounds.size;

        mouseOn = (GameObject)Instantiate(Resources.Load("Prefabs/etc/mouseOn"));
        symbol = mouseOn.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        sentiment = (GameObject)Instantiate(Resources.Load("Prefabs/etc/sentiment"));
        upEffect = (GameObject)Instantiate(Resources.Load("Prefabs/EFX/upEFX"));
        downEffect = (GameObject)Instantiate(Resources.Load("Prefabs/EFX/downEFX"));
        lightEffect = (GameObject)Instantiate(Resources.Load("Prefabs/EFX/lightEFX"));
        sleepEffect = (GameObject)Instantiate(Resources.Load("Prefabs/EFX/sleepEFX"));

        mouseOn.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);
        sentiment.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);
        upEffect.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);
        downEffect.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);
        lightEffect.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);
        sleepEffect.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);

        mouseOn.SetActive(false);
        sentiment.SetActive(false);
        upEffect.SetActive(false);
        downEffect.SetActive(false);
        lightEffect.SetActive(false);
        sleepEffect.SetActive(false);
    }

    void FixedUpdate()
    {
        //콜라이더 큐브의 폭을 이용해서 배치할 위치인 좌측 상단의 좌표를 찾아냄
        Vector3 tmp0 = new Vector3(transform.position.x - (m_Size.x / 2), transform.position.y + m_Size.y*1.6f, transform.position.z - (m_Size.z / 2));
        Vector3 tmp = new Vector3(transform.position.x + (m_Size.x), transform.position.y + m_Size.y * 1.6f, transform.position.z - (m_Size.z / 2));
        Vector3 tmp3 = new Vector3(transform.position.x , transform.position.y + m_Size.y * 1.6f, transform.position.z - (m_Size.z / 2));

        var wantedPos0 = cam.WorldToScreenPoint(tmp0);
        var wantedPos = cam.WorldToScreenPoint(tmp);
        var wantedPos3 = cam.WorldToScreenPoint(tmp3);

        //UI 위치를 조정
        mouseOn.transform.position = wantedPos0;
        sentiment.transform.position = wantedPos3;
        upEffect.transform.GetChild(0).gameObject.GetComponent<rising>().pos = wantedPos;
        downEffect.transform.GetChild(0).gameObject.GetComponent<falling>().pos = wantedPos;
        lightEffect.transform.GetChild(0).gameObject.GetComponent<shining>().pos = wantedPos;
        
        sleepEffect.transform.GetChild(0).gameObject.GetComponent<sleeping>().pos = wantedPos + new Vector3(-20f, -20f, 0f);
        sleepEffect.transform.GetChild(1).gameObject.GetComponent<sleeping>().pos = wantedPos;
        sleepEffect.transform.GetChild(2).gameObject.GetComponent<sleeping>().pos = wantedPos + new Vector3(20f, 20f, 0f);

        lightEffect.transform.GetChild(0).gameObject.GetComponent<shining>().pos = wantedPos + new Vector3(-100f,-100f,0f);
        lightEffect.transform.GetChild(1).gameObject.GetComponent<shining>().pos = wantedPos + new Vector3(-70f,0f,0f);
        lightEffect.transform.GetChild(2).gameObject.GetComponent<shining>().pos = wantedPos + new Vector3(-30f,-100f,0f);
        lightEffect.transform.GetChild(3).gameObject.GetComponent<shining>().pos = wantedPos + new Vector3(0f,0f,0f);

        mouseOn.SetActive(false);
        sentiment.SetActive(false);
        upEffect.SetActive(false);
        downEffect.SetActive(false);
        lightEffect.SetActive(false);
        sleepEffect.SetActive(false);
        
        symbol.text = transform.name + "\n";
        //조건1 확인:화면에 다른 창(설정창,포폴창,세부정보창)이 떠있는 경우에는 띄우지 X
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp) { return; }

        //조건 2 확인: 시티뷰일 경우 말풍선의 스케일 변경
        if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView){mouseOn.transform.localScale = new Vector3(0.2f,0.2f,1f);}
        else{mouseOn.transform.localScale = new Vector3(0.8f, 0.8f, 1f);}

        //조건3 확인: 선택한 레이어의 정보 제공이 가능한 경우에만 말풍선 정보를 띄움
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().priceFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!api.apiInfo.ContainsKey(transform.name)) { return; }
            //종목의 전날 종가 대비 현재 주가 비율 계산
            float percent = (api.apiInfo[transform.name].api_marketprice - api.apiInfo[transform.name].api_preclose) / api.apiInfo[transform.name].api_preclose * 100;
            symbol.text += percent.ToString("F2") + "%";
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
                if (percent < 0) { downEffect.SetActive(true); }
                else { upEffect.SetActive(true); }
                mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205 / 255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            mouseOn.SetActive(true);
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().gainFlag)
        {
            //보유 종목이 아닌 경우 정보 표시 제외
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { return; }
            if (myStocks.stockInfo[transform.name].shares == 0) { return; }
            //보유 종목의 평단가 대비 현재 주가 비율 계산
            float percent = (api.apiInfo[transform.name].api_marketprice - myStocks.stockInfo[transform.name].avgCostPerShare) / myStocks.stockInfo[transform.name].avgCostPerShare * 100;
            symbol.text += percent.ToString("F2") + "%";
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
                if (percent < 0) { downEffect.SetActive(true); }
                else { upEffect.SetActive(true); }
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
            if (!api.apiInfo.ContainsKey(transform.name)) { return; }
            //10일 평균 거래량 대비 현재 거래량 비율 계산
            float percent = (api.apiInfo[transform.name].api_volume - api.apiInfo[transform.name].api_avgVolume) / api.apiInfo[transform.name].api_avgVolume * 100;
            symbol.text += percent.ToString("F2") + "%";

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
                if (percent < 0) { downEffect.SetActive(true); }
                else { upEffect.SetActive(true); }
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
            if (!api.apiInfo.ContainsKey(transform.name)) { return; }
            string date1 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divDate(transform.name);
            if (date1.Length < 1) { return; }
            TimeSpan dateDiff = DateTime.Parse(date1) - DateTime.Now;
            int tmp1 = dateDiff.Days;
            symbol.text += "D-" + tmp1.ToString() + "(";
            //보유 종목인 경우에만 받을 배당액 정보 표시
            mouseOn.SetActive(true);
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { return; }
            if (myStocks.stockInfo[transform.name].shares == 0) { return; }
            float tmp2 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().dividend(transform.name);
            symbol.text += tmp2.ToString("F2") + "$)";
            mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
            Color color = mouseOn.GetComponent<Image>().color;
            color.a = 205 / 255f;
            mouseOn.GetComponent<Image>().color = color;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().vidCntFlag)
        {
            if (!api.youtubeInfo.ContainsKey(transform.name)) { return; }
            if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView) { return; }
            //종목의 순위를 확인
            int rank = api.getVidCntRank(transform.name);
            string message = rank.ToString() + "등";
            float percent = rank / (api.vidCntRank.Count) * 100;

            if (percent < 20)
            {
                lightEffect.SetActive(true);
                message += " <sprite=3>";
            }
            else if (percent < 40)
            {
                lightEffect.SetActive(true);
                message += " <sprite=0>";
            }
            else if (percent < 60) { message += " <sprite=7>"; }
            else if (percent < 80)
            {
                sleepEffect.SetActive(true);
                message += " <sprite=9>";
            }
            else
            {
                sleepEffect.SetActive(true);
                message += " <sprite=6>";
            }
            symbol.text += message;
            mouseOn.SetActive(true);
            mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
            Color color = mouseOn.GetComponent<Image>().color;
            color.a = 205 / 255f;
            mouseOn.GetComponent<Image>().color = color;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().viewCntFlag)
        {
            if (!api.youtubeInfo.ContainsKey(transform.name)) { return; }
            if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView) { return; }
            //종목의 순위를 확인
            int rank = api.getViewRank(transform.name);
            string message = rank.ToString() + "등";
            float percent = rank / (api.viewRank.Count) * 100;

            if (percent < 20)
            {
                lightEffect.SetActive(true);
                message += " <sprite=3>";
            }
            else if (percent < 40)
            {
                lightEffect.SetActive(true);
                message += " <sprite=0>";
            }
            else if (percent < 60) { message += " <sprite=7>"; }
            else if (percent < 80)
            {
                sleepEffect.SetActive(true);
                message += " <sprite=9>";
            }
            else
            {
                sleepEffect.SetActive(true);
                message += " <sprite=6>";
            }
            symbol.text += message;
            mouseOn.SetActive(true);
            mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
            Color color = mouseOn.GetComponent<Image>().color;
            color.a = 205 / 255f;
            mouseOn.GetComponent<Image>().color = color;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().newsSentiFlag)
        {
            int state;
            if (!api.sentimentInfo.ContainsKey(transform.name)) { return; }
            if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView) { return; }
            int percent = api.sentimentInfo[transform.name].api_positive;
            Sprite[] spr = Resources.LoadAll<Sprite>("Prefabs/etc/EmojiOne");
            if (percent>75){state = 8; }
            else if (percent>50){ state = 15; }
            else if (percent>25){ state = 14; }
            else{ state = 10; }
            sentiment.SetActive(true);
            sentiment.transform.GetChild(3).gameObject.GetComponent<Image>().sprite = spr[state];
            sentiment.GetComponent<Slider>().value = percent;
        }
        else { return; }//체크된 옵션이 없는 경우 표시 X
    }
}
