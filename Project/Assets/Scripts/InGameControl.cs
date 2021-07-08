using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameControl : MonoBehaviour
{
     public StockList list; //종목API정보 저장자료
     public portfolio myPortfolio; //포트폴리오 정보 저장자료
     public GameObject SubMenu; //서브메뉴창
     public GameObject EditPage; //포트폴리오 정보 수정 페이지
     public Camera getCamera; //게임 시점 카메라
     public Text thisSymbol; //화면에 띄울 종목명
     public GameObject[] totalMode; //전체 주식시장 모드일때 나와야 되는 오브젝트 리스트
     public GameObject[] portfolioMode; //포트폴리오 모드일때 나와야 되는 오브젝트 리스트
     private RaycastHit hit; //마우스에 클릭된 객체 
     private bool subMenuPopUp; //서브메뉴창이 떠있는지 확인하는 bool변수

    void Start()
     {
         subMenuPopUp = false;
         totalBtnClick();
         BuildingScaleSet();
     }

     void Update()
     {
         clickCheck();
     }

     void clickCheck()
     {
         if (subMenuPopUp){return;}
         //클릭한 객체 이름 출력
         if (Input.GetMouseButtonDown(0))
         {
             string tmpname = "";
             Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit))
             {
                 if(hit.collider.gameObject.CompareTag("stock"))//건물 오브젝트를 클릭한 경우
                 {
                     tmpname = hit.collider.gameObject.name;
                     thisSymbol.text = tmpname;

                     Debug.Log("배당일 = " + list.apiInfo[tmpname].api_divDate
                              + "  배당률 = " + list.apiInfo[tmpname].api_divRate
                              + "  sector = " + list.apiInfo[tmpname].api_sector
                              + "  시가총액 = " + list.apiInfo[tmpname].api_marketcap
                              + "  PER = " + list.apiInfo[tmpname].api_per
                              + "  52week = " + list.apiInfo[tmpname].api_52week
                              + "  previous close = " + list.apiInfo[tmpname].api_preclose);
                 }
             }
         }
     }
     public void SubMenuBtnClick()
     {
         //서브페이지 내리기
         EditPage.SetActive(false);

         //서브메뉴가 화면에 있으면 창내리기
         if (SubMenu.activeSelf)
         {
             SubMenu.SetActive(false);
             subMenuPopUp = false;
         }
         //서브메뉴가 화면에 없으면 창열기
         else
         {
             SubMenu.SetActive(true);
             subMenuPopUp = true;
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
        thisSymbol.text = "";

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
    //edit버튼을 누르면 인게임이 보유종목 수정 페이지로 전환됨
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //서브페이지에서의 Back버튼을 누르면 서브메뉴 페이지로 전환됨
    public void QuitBtnClick()
    {
        //서브메뉴 화면으로 이동
        EditPage.SetActive(false);
        SubMenu.SetActive(true);
    }
}
