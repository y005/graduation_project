using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using TMPro;
public class InGameControl : MonoBehaviour
{
    public StockList list; //종목API정보 저장자료
    public portfolio myPortfolio; //포트폴리오 정보 저장자료

    public GameObject SubMenu; //서브메뉴창
    public GameObject EditPage; //포트폴리오 정보 수정 페이지
    public GameObject InfoOptPage; //정보범위 수정 페이지
    public GameObject OptPage; //정보범위 수정 페이지

    public GameObject totalGainPage; //포트폴리오 정보 페이지
    public GameObject content; //포트폴리오 정보를 넣는 오브젝트
    public TextMeshProUGUI totalGainInfo; //자산 정보를 입력하는 텍스트 UI

    public Toggle priceOpt; //주가 정보 표시하는 옵션
    public Toggle volumeOpt; //거래량 정보 표시하는 옵션
    public Toggle divOpt; //배당 정보 표시하는 옵션
    public Toggle interestOpt; // 관심도 지수 표시하는 옵션

    public Toggle myStockOpt; //내 종목만 표시하는 옵션
    public Toggle layerOpt; //내 종목만 투명하게 표시하는 옵션
    public Toggle weatherOpt; //포트폴리오 이익과 연동되는 날씨 옵션
    public Toggle marketTimeOpt; //주식 시장 시간과 연동하는 시간 옵션 

    public bool priceFlag;
    public bool volumeFlag;
    public bool divFlag;
    public bool interestFlag;

    public bool myStockFlag;
    public bool layerFlag;
    public bool weatherFlag;
    public bool marketTimeFlag;
    public bool pagePopUp; //창이 현재 게임화면에 떠있는지 확인하는 bool변수
    public TextMeshProUGUI optInfo; //선택한 레이어 정보 표시창

    public GameObject building;

    private List<Vector3> scales = new List<Vector3>();//종목의 초기 디폴트 스케일 설정 저장 배열

    void Start()
    {
        optInfo.text = "";
        //종목의 스케일 정보 저장
        for (int i = 0; i < building.transform.childCount; i++)
        {
            Vector3 tmp = building.transform.GetChild(i).gameObject.transform.localScale;
            scales.Add(new Vector3(tmp.x, tmp.y, tmp.z));
        }
        Load();//저장된 데이터를 불러온 뒤 날씨 제어
        checkOpt();
        pagePopUp = false;
        totalBtnClick();
        myStockOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(myStockOpt); });
        priceOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(priceOpt);});
        volumeOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(volumeOpt); });
        divOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(divOpt); });
        interestOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(interestOpt); });
    }
    void Update()
    {
        checkOpt(); //켜진 옵션 확인
    }
    void checkOpt()//매번 옵션 선택 확인
    {
        myStockFlag = myStockOpt.isOn;
        layerFlag = layerOpt.isOn;
        weatherFlag = weatherOpt.isOn;
        marketTimeFlag = marketTimeOpt.isOn;

        priceFlag = priceOpt.isOn; 
        volumeFlag = volumeOpt.isOn; 
        divFlag = divOpt.isOn; 
        interestFlag = interestOpt.isOn;
    }
    void ToggleValueChanged(Toggle change)
    {
        if (change == myStockOpt)
        {
            if (change.isOn)//내 보유 종목만 보는 설정
            {
                portfolioBtnClick();
            }
            else//전체 주식 시장 종목들만 보는 설정 
            {
                totalBtnClick();
            }
            return;
        }
        if (change.isOn)
        {
            optInfo.text = "";
            offToggleExcept(change);
        }
    }
    void offToggleExcept(Toggle change)
    {
        if (change == priceOpt)
        {
            optInfo.text = "수익률";
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            interestOpt.isOn = false;
            priceOpt.isOn = true;
        }
        else if (change == volumeOpt)
        {
            optInfo.text = "거래량";
            priceOpt.isOn = false;
            divOpt.isOn = false;
            interestOpt.isOn = false;
            volumeOpt.isOn = true;
        }
        else if (change == divOpt)
        {
            optInfo.text = "배당일";
            volumeOpt.isOn = false;
            priceOpt.isOn = false;
            interestOpt.isOn = false;
            divOpt.isOn = true;
        }
        else
        {
            optInfo.text = "관심도";
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            priceOpt.isOn = false;
            interestOpt.isOn = true;
        }
    }
    public void SubMenuBtnClick()
    {
        if (EditPage.activeSelf || InfoOptPage.activeSelf || OptPage.activeSelf)
        {
            EditPage.SetActive(false);
            InfoOptPage.SetActive(false);
            OptPage.SetActive(false);
            pagePopUp = false;
            return;
        }
        //서브메뉴가 화면에 없으면 창 띄우기
        if (!SubMenu.activeSelf)
        {
            totalGainPage.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().StockInfo.SetActive(false);

            SubMenu.SetActive(true);
            pagePopUp = true;
        }
        //서브메뉴가 화면에 있으면 창내리기
        else
        {
            SubMenu.SetActive(false);
            pagePopUp = false;
        }
    }
    //total버튼을 누르면 인게임이 전체 주식시장 모드로 전환됨
    public void totalBtnClick()
    {
        GameObject stock;
        GameObject effect;
        string key;
        float tmp_cap = 0;
        float scale = 1f;

        //보유한 종목만 말풍선과 객체 활성화
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            stock.SetActive(true);
            effect.SetActive(true);
            //총 자산 대비 종목 비중 만큼 객체 스케일 조정하기
            if (list.apiInfo.ContainsKey(key))
            {
                tmp_cap = list.apiInfo[key].api_marketcap / 1000000000000;
                if (tmp_cap >= 0.9) { scale = 1.25f; }
                else if (tmp_cap < 0.9 && tmp_cap >= 0.3) { scale = 1f; }
                else if (tmp_cap < 0.3) { scale = 0.75f; }
                stock.transform.localScale = new Vector3(scale * scales[i].x,scale * scales[i].y,scale * scales[i].z);
            }
            else
            {
                stock.transform.localScale = new Vector3(scales[i].x, scales[i].y, scales[i].z);
            }
        }
    }

    //portfolio모드인 경우 인게임이 포트폴리오 모드로 전환됨
    public void portfolioBtnClick()
    {
        GameObject stock;
        GameObject effect;
        float total = 0;//전체 주식평가금액 저장변수
        string key;

        //전체 평가금액 계산
        foreach (var key1 in myPortfolio.stockInfo.Keys.ToList())
        {
            //개별 종목의 보유수량이 0개인 경우 카운트에서 제외
            if (myPortfolio.stockInfo[key1].shares == 0) { continue; }
            total += myPortfolio.updateGain(key1);
        }
        //보유한 종목만 말풍선과 객체 활성화
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            //보유하지 않는 경우 비활성화
            if(!myPortfolio.stockInfo.ContainsKey(key)){
                stock.SetActive(false);
                effect.SetActive(false);
            }
            else
            {
                if (myPortfolio.stockInfo[key].shares == 0) {
                    stock.SetActive(false);
                    effect.SetActive(false); 
                    continue; 
                }
                stock.SetActive(true);
                effect.SetActive(true);
                //총 자산 대비 종목 비중 만큼 객체 스케일 조정하기
                float ratio = myPortfolio.updateGain(key) / total * 100;
                float scale = 1f;
                if (ratio < 30) { scale = 0.75f; }
                else if (ratio >= 30 && ratio < 70) { scale = 1f; }
                else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }
                stock.transform.localScale = new Vector3(scale * scales[i].x, scale * scales[i].y, scale * scales[i].z);
            }
        }
    }

    //종목 추가버튼을 누르면 인게임이 보유종목 수정 페이지로 전환됨
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //정보 설정버튼을 누르면 인게임내 정보표시 설정 페이지로 전환됨
    public void InfoOptBtnClick()
    {
        SubMenu.SetActive(false);
        InfoOptPage.SetActive(true);
    }
    //옵션 버튼을 누르면 인게임내 설정 페이지로 전환됨
    public void OptBtnClick()
    {
        SubMenu.SetActive(false);
        OptPage.SetActive(true);
    }
    //나의 포트폴리오 종목 정보를 리스트뷰로 볼 수 있는 정보창이 나옴
    public void myPortfolioBtnClick()
    {

        if (totalGainPage.activeSelf)//이미 켜져있으면 꺼지고 꺼져있으면 켜지도록 제어
        {
            totalGainPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //다른 창을 다 내리기
            SubMenu.SetActive(false);
            EditPage.SetActive(false);
            InfoOptPage.SetActive(false);
            OptPage.SetActive(false);
            totalGainPage.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().StockInfo.SetActive(false);

            pagePopUp = true;
            totalGainPage.SetActive(true);
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            GameObject tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
            tmp.transform.SetParent(content.transform, false);

            float gainSum = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().totalGainSet();
            float tmp1 = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                tmp1 += myPortfolio.stockInfo[key].avgCostPerShare * myPortfolio.stockInfo[key].shares;
            }
            if (tmp1 != 0) { tmp1 = (gainSum - tmp1) / tmp1 * 100; }
            totalGainInfo.text = "자산: $" + gainSum.ToString("F2");
            totalGainInfo.text += " 수익률: " + tmp1.ToString("F2")+"%";

            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                //개별 종목의 보유수량이 0개인 경우 배치에서 제외
                if (myPortfolio.stockInfo[key].shares == 0) { continue; }
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/logo"));
                tmp.transform.SetParent(content.transform, false);
                tmp.GetComponent<Image>().sprite = Resources.Load("Sprites/" + key, typeof(Sprite)) as Sprite;

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/totalGainList"));
                tmp.transform.SetParent(content.transform, false);
                int cnt = myPortfolio.stockInfo[key].shares;//보유수량
                float market = myPortfolio.updateGain(key);//현재 평가금액
                float apc = myPortfolio.stockInfo[key].avgCostPerShare;//평균 단가
                
                float current = list.apiInfo[key].api_marketprice;//현재 주가
                float gainPercent = (market - apc*cnt)/(apc*cnt)*100;//수익률

                TextMeshProUGUI info = tmp.GetComponent<TextMeshProUGUI>();

                info.text = "종목 코드: "+key+"\n";
                info.text += "현재 주가: $" + current.ToString("F2") + "\n";
                info.text += "평균 단가: $" + apc.ToString("F2") + "\n";
                info.text += "보유 수량: "+ cnt.ToString() + "개\n";
                info.text += "수익률: "+ gainPercent.ToString("F2") + "%\n";
                info.text += "평가금액: $" + market.ToString("F2");

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);
            }
        }
    }
    //서브페이지에서의 Back버튼을 누르면 서브메뉴 페이지로 전환됨
    public void QuitBtnClick()
    {
        //서브메뉴 화면으로 이동
        EditPage.SetActive(false);
        OptPage.SetActive(false);
        InfoOptPage.SetActive(false);
        SubMenu.SetActive(true);
    }
    public void ExitBtnClick()
    {
        //게임을 저장하고 메인 메뉴 화면으로 이동
        Save();
        SceneManager.LoadScene("MainMenu");
    }
    //저장을 위한 자료구조
    [Serializable]
    public class stockInfo
    {
        public string code;//종목코드
        public int shares;//보유수량
        public float apc;//평단가
    }
    public class savData
    {
        public float Cash;
        public List<stockInfo> stocks;
    }
    void Load()
    {
        //Json파일 가져온 다음에 게임과 연동시키기 
        try
        {
            string path = Application.dataPath + "/" + "user1" + ".Json";
            string json = File.ReadAllText(path);
            savData loadData = JsonUtility.FromJson<savData>(json);
            float preTotalGain = 0f;
            float totalGain = 0f;
            myPortfolio.Cash = loadData.Cash;
            foreach (var tmp in loadData.stocks)
            {
                preTotalGain += tmp.shares * tmp.apc;
                myPortfolio.addTrade(tmp.code, "0000-0-0", tmp.shares, tmp.apc, false);
            }
            //현재 업데이트 된 포트폴리오 평가금액 갱신
            foreach (var tmp in loadData.stocks)
            {
                totalGain += myPortfolio.updateGain(tmp.code);
            }
            //현재 업데이트 된 포트폴리오 평가금액과 이 전 포트폴리오 평가금액 비교하여 날씨 파악
            float percent = (totalGain - preTotalGain) / preTotalGain * 100;
            if (percent < -50 )
            {
                GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().isRain = 10f;
            }
            else if ((-50 < percent) && (percent < -30))
            {
                GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().isRain = 5f;
            }
            else if ((-30 < percent) && (percent < 0))
            {
                GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().isRain = 2.5f;
            }
            else
            {
                GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().isRain = 0f;
            }
        }
        catch (Exception e)
        {
            //세이브 데이터가 없는 경우 날씨는 화창함으로 표현됨
            GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().isRain = 0f;
        }
    }
    void Save()
    {
        //Json파일 형태로 저장시키기
        savData sav = new savData();
        sav.Cash = myPortfolio.Cash;
        sav.stocks = new List<stockInfo>();
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //개별 종목의 보유수량이 0개인 경우 저장 데이터에서 제외
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            stockInfo tmp = new stockInfo();
            tmp.code = key;
            tmp.shares = myPortfolio.stockInfo[key].shares;
            tmp.apc = myPortfolio.stockInfo[key].avgCostPerShare;
            sav.stocks.Add(tmp);
        }
        string json = JsonUtility.ToJson(sav);
        string path = Application.dataPath + "/" + "user1" + ".Json";
        File.WriteAllText(path, json);
    }
}
