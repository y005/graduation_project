using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;
using UnityEngine.SceneManagement;
public class InGameControl : MonoBehaviour
{
    public StockList list; //종목API정보 저장자료
    public portfolio myPortfolio; //포트폴리오 정보 저장자료

    public GameObject SubMenu; //서브메뉴창
    public GameObject EditPage; //포트폴리오 정보 수정 페이지
    public GameObject OptPage; //정보범위 수정 페이지

    public Toggle myStockOpt; //내 종목만 표시하는 옵션
    public Toggle layerOpt; //내 종목만 투명하게 표시하는 옵션
    public Toggle weatherOpt; //포트폴리오 이익과 연동되는 날씨 옵션
    public Toggle marketTimeOpt; //주식 시장 시간과 연동하는 시간 옵션 

    public bool myStockFlag;
    public bool layerFlag;
    public bool weatherFlag;
    public bool marketTimeFlag;
    public bool subMenuPopUp; //서브메뉴창이 떠있는지 확인하는 bool변수

    public GameObject[] totalMode; //전체 주식시장 모드일때 나와야 되는 오브젝트 리스트
    public GameObject[] portfolioMode; //포트폴리오 모드일때 나와야 되는 오브젝트 리스트

    void Start()
    {
        Load();//저장된 데이터를 불러온 뒤 날씨 제어
        checkOpt();
        subMenuPopUp = false;
        totalBtnClick();
    }
    void Update()
    {
        checkOpt(); //켜진 옵션 확인
        if (myPortfolio.renew)//포트폴리오 갱신이 되면 보유 종목 재설치 작업 실행
        {
            settingPortfolio(); 
        }
        //보유 종목만 보는 옵션에 설정된 경우 포트폴리오 모드를 실행
        if (myStockFlag)
        {
            portfolioBtnClick();
        }
        else
        {
            totalBtnClick();
        }
    }
    void checkOpt()//매번 옵션 선택 확인
    {
        myStockFlag = myStockOpt.isOn;
        layerFlag = layerOpt.isOn;
        weatherFlag = weatherOpt.isOn;
        marketTimeFlag = marketTimeOpt.isOn;
    }
    public void SubMenuBtnClick()
    {
        EditPage.SetActive(false);
        OptPage.SetActive(false);
        //서브메뉴와 종목정보창이 화면에 없으면 창열기
        if (!SubMenu.activeSelf && !GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().StockInfoMenuPopUp)
        {
            SubMenu.SetActive(true);
            subMenuPopUp = true;
        }
        //서브메뉴가 화면에 있으면 창내리기
        else
        {
            SubMenu.SetActive(false);
            subMenuPopUp = false;
        }
    }
    //total버튼을 누르면 인게임이 전체 주식시장 모드로 전환됨
    public void totalBtnClick()
    {
        //포트폴리오맵에 설치된 건물 객체들 비활성화
        foreach (GameObject tmp in myPortfolio.myStocks)
        {
            tmp.SetActive(false);
        }
        //portfolioMode 태그가 달린 객체를 전부 비활성화(total UI버튼, edit UI 버튼, totalGain,divGain 텍스트 UI)
        for (int i = 0; i < portfolioMode.Length; i++)
        {
            portfolioMode[i].SetActive(false);
        }
        //totalMode 태그가 달린 객체를 전부 활성화(포트폴리오 UI 버튼이랑 Building 부모 객체)
        for (int i = 0; i < totalMode.Length; i++)
        {
            totalMode[i].SetActive(true);
        }
        //BuildingScaleSet();
    }

    //total모드인 경우 시가총액에 따라 스케일 조정작업
    void BuildingScaleSet()
    {
        float tmp_cap = 0;
        float scale = 1f;

        //totalMode[0]에 Building있음(종목주식들의 부모 객체에 있는 자식 객체들의 gameObject.transform.localScale 조절) 
        for (int i = 0; i < totalMode[0].transform.childCount; i++)
        {
            tmp_cap = list.apiInfo[totalMode[0].transform.GetChild(i).name].api_marketcap / 1000000000000;
            if (tmp_cap >= 0.9) { scale = 1.25f; }
            else if (tmp_cap < 0.9 && tmp_cap >= 0.3) { scale = 1f; }
            else if (tmp_cap < 0.3) { scale = 0.75f; }
            totalMode[0].transform.GetChild(i).gameObject.transform.localScale
                = new Vector3(scale * totalMode[i].transform.localScale.x,
                scale * totalMode[i].transform.localScale.y,
                scale * totalMode[i].transform.localScale.z);
        }
    }

    //portfolio모드인 경우 인게임이 포트폴리오 모드로 전환됨
    public void portfolioBtnClick()
    {
        //totalMode 태그가 달린 객체를 전부 비활성화(포트폴리오 UI 버튼이랑 Building 부모 객체)
        for (int i = 0; i < totalMode.Length; i++)
        {
            totalMode[i].SetActive(false);
        }
        //portfolioMode 태그가 달린 객체를 전부 활성화(total UI버튼, edit UI 버튼, totalGain,divGain 텍스트 UI)
        for (int i = 0; i < portfolioMode.Length; i++)
        {
            portfolioMode[i].SetActive(true);
        }
        //포트폴리오맵에 설치된 건물 객체들 활성화
        foreach (GameObject tmp in myPortfolio.myStocks)
        {
            tmp.SetActive(true);
        }
    }

    //포트폴리오 건물 설치
    void settingPortfolio()
    {
        float total = 0;//전체 주식평가금액 저장변수
        float myinvest = 0; //전체 투자금 저장변수
        string path = "";//프리팹 로드 경로

        //"myStock" 태그가 달린 객체(포트폴리오 배치된 건물)를 전부 삭제(갱신이 될때 마다 반복)
        foreach (GameObject tmp in myPortfolio.myStocks) { Destroy(tmp); }
        myPortfolio.myStocks.Clear();

        //전체 평가금액 계산
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //개별 종목의 보유수량이 0개인 경우 카운트에서 제외
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            total += myPortfolio.updateGain(key);
        }
        //나의 전체 투자금액 계산
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            myinvest += myPortfolio.stockInfo[key].shares * myPortfolio.stockInfo[key].avgCostPerShare;
        }

        //종목의 위치에 보유 종목 건물을 설치(평가금액 비중에 따라 스케일링)
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //개별 종목의 보유수량이 0개인 경우 배치에서 제외
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }

            //기존 건물 위치 반환
            Vector3 pos;
            pos = totalMode[0].transform.Find(key).position;

            //건물 설치
            path = "Prefabs/Buildings/" + key;
            GameObject a = (GameObject)Instantiate(Resources.Load(path));
            a.name = key;
            a.gameObject.tag = "stock";
            myPortfolio.myStocks.Add(a);

            //기존과 같은 위치에 건물 배치
            a.transform.position = new Vector3(pos.x, pos.y, pos.z);

            //자산 대비 스케일 비율 정하기(1~3단계)
            float ratio = myPortfolio.updateGain(key) / total * 100; //전체 평가금액 중 해당 종목 평가금액 비율
            float scale = 1f;
            if (ratio < 30) { scale = 0.75f; }
            else if (ratio >= 30 && ratio < 70) { scale = 1f; }
            else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }

            //단계에 따라 정해진 비율만큼 객체 스케일 조정하기
            a.transform.localScale = new Vector3(scale * a.transform.localScale.x, scale * a.transform.localScale.y, scale * a.transform.localScale.z);

            //DEBUG ::: 배당정보 체크
            string divDate = GameObject.Find("portfolioControl").GetComponent<dividendCtrl>().divDate(key);
            float dividend = GameObject.Find("portfolioControl").GetComponent<dividendCtrl>().dividend(key);
            Debug.Log(key + " - 배당 날짜 : " + divDate + ", " + "배당금 : " + dividend);
        }
        myPortfolio.renew = false;
        //포트폴리오가 갱신되면 가장 가까운 배당일을 갱신하고 배당일 게이지에 표현
        GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divDateSet();
        //포트폴리오가 갱신되면 평가금액을 갱신하고 평가금액 텍스트 UI에 표현
        GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().totalGainSet();
        //포트폴리오가 갱신되면 배당익을 갱신하고 텍스트 UI에 표현
        GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divGainSet();
    }

    //edit버튼을 누르면 인게임이 보유종목 수정 페이지로 전환됨
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //option버튼을 누르면 인게임내 정보범위 설정 페이지로 전환됨
    public void OptBtnClick()
    {
        SubMenu.SetActive(false);
        OptPage.SetActive(true);
    }
    //서브페이지에서의 Back버튼을 누르면 서브메뉴 페이지로 전환됨
    public void QuitBtnClick()
    {
        //서브메뉴 화면으로 이동
        EditPage.SetActive(false);
        OptPage.SetActive(false);
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
