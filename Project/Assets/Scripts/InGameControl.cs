using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InGameControl : MonoBehaviour
{
     public StockList list; //종목API정보 저장자료
     public portfolio myPortfolio; //포트폴리오 정보 저장자료
     public GameObject SubMenu; //서브메뉴창
     public GameObject EditPage; //포트폴리오 정보 수정 페이지
     public GameObject OptPage; //정보범위 수정 페이지
     public bool subMenuPopUp; //서브메뉴창이 떠있는지 확인하는 bool변수
     public Toggle myStockOpt; //내 종목만 표시하는 bool변수
     public GameObject[] totalMode; //전체 주식시장 모드일때 나와야 되는 오브젝트 리스트
     public GameObject[] portfolioMode; //포트폴리오 모드일때 나와야 되는 오브젝트 리스트

    void Start()
     {
         subMenuPopUp = false;
         totalBtnClick();
         //BuildingScaleSet();
     }
    void Update()
    {
        if (myStockOpt.isOn)
        {
            portfolioBtnClick();
        }
        else
        {
            totalBtnClick();
        }
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
     }

    //totalMode 에서 시가총액에 따라 스케일 조정작업
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

    //portfolio버튼을 누르면 인게임이 포트폴리오 모드로 전환됨
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
       /* foreach (GameObject tmp in myPortfolio.myStocks)
        {
            tmp.SetActive(true);
        }*/

        //건물 설치
        settingPortfolio();

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

        //섹터 갯수를 위한 카운트 저장 초기화
        /*foreach (var key in sectorCnt.Keys.ToList())
        {
            sectorCnt[key] = 0;
        }*/
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
        //전체 수익금 = 전체 평가금액 - 나의 전체 투자금액
        //totalGain.text = (total - myinvest).ToString();

        //해당하는 섹터 위치에 차례대로 일정 간격으로 정렬시키기
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
            a.gameObject.tag = "myStock";
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
        }
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
}
