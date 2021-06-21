using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameControl : MonoBehaviour
{
    public Text a;
    public Text b;
    public GameObject SubMenu;
    public GameObject SubPage1;
    public GameObject SubPage2;
    public GameObject SubPage3;
    public GameObject SubPage4;

    // Start is called before the first frame update
    void Start()
    {
        string pos;
        int x = 0, z = 0;
        //객체 생성
        //foreach (KeyValuePair<string,  > item in )
        for(int i = 0; i < 15; i++)
        {
            
            GameObject a = (GameObject)Instantiate(Resources.Load("Prefabs/cube"));

            //섹터확인
            /*
            if technology -> pos = tech
            else if communication services -> pos = cs
            else if consumer cyclical -> pos = cc
            else if financial -> pos = fin
             */

            //섹터에 따른 것으로 조건문 수정하기
            if (i < 4) 
            {
                x = -5;
                z = -5;
            }
            else if(i >= 4 && i < 8)
            {

            }
            else if (i >= 8 && i < 12)
            {

            }
            else
            {

            }
            a.transform.position = new Vector3(x, 1, z);

        }
    }

    // Update is called once per frame
    void Update()
    {
       
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
