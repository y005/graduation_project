using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameControl : MonoBehaviour
{
    //전체 수익 텍스트 UI
    public Text a;
    //배당 수익 텍스트 UI
    public Text b;
    //서브메뉴창 UI
    public GameObject SubMenu;
    //서브메뉴1 UI
    public GameObject SubPage1;
    public GameObject SubPage2;
    public GameObject SubPage3;
    public GameObject SubPage4;
    public StockList list;
    public Camera getCamera; //게임 시점 카메라
    private RaycastHit hit; //마우스에 클릭된 객체 
    public Text thisSymbol; //화면에 띄울 종목명

    // Start is called before the first frame update
    void Start()
    {
        string path = "";
        int x = 0, z = 0;

        //섹터별 배치
        foreach (string key in list.apiInfo.Keys)
        {
            if (list.apiInfo[key].api_sector.Equals("Technology"))
            {
                path = "Prefabs/Buildings/cube1";
                x = -5;
                z = -5;
            }
            else if (list.apiInfo[key].api_sector.Equals("Communication Services"))
            {
                path = "Prefabs/Buildings/cube2";
                x = 5;
                z = -5;
            }
            else if (list.apiInfo[key].api_sector.Equals("Consumer Cyclical"))
            {
                path = "Prefabs/Buildings/cube3";
                x = 5;
                z = 5;
            }
            else if (list.apiInfo[key].api_sector.Equals("Financial Services"))
            {
                path = "Prefabs/Buildings/cube4";
                x = -5;
                z = 5;
            }
            GameObject a = (GameObject)Instantiate(Resources.Load(path));
            a.name = key;
            a.transform.position = new Vector3(x, 2, z);
            Debug.Log(list.apiInfo[key].api_sector);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //클릭한 객체 이름 출력
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                thisSymbol.text = hit.collider.gameObject.name;
            }
        }
    }

    public void SubMenuBtnClick()
    {
        //서브메뉴가 화면에 있으면 창내리기
        if (SubMenu.activeSelf)
        {
            SubMenu.SetActive(false);
        }
        //서브메뉴가 화면에 없으면 창열기
        else
        {
            SubMenu.SetActive(true);
        }
    }

    public void SubPage1BtnClick()
    {
        SubMenu.SetActive(false);
        SubPage1.SetActive(true);

        /* symbol, 매매 수량, 매매 금액, BUY, SELL 버튼 선택*/
    }

    public void SubPage2BtnClick()
    {
        SubMenu.SetActive(false);
        SubPage2.SetActive(true);
    }

    public void SubPage3BtnClick()
    {
        SubMenu.SetActive(false);
        SubPage3.SetActive(true);
    }
    public void SubPage4BtnClick()
    {
        SubMenu.SetActive(false);
        SubPage4.SetActive(true);
    }
    public void QuitBtnClick()
    {
        //서브메뉴 화면으로 이동
        SubPage1.SetActive(false);
        SubPage2.SetActive(false);
        SubPage3.SetActive(false);
        SubPage4.SetActive(false);
        SubMenu.SetActive(true);
    }
}
