using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameControl : MonoBehaviour
{
    //��ü ���� �ؽ�Ʈ UI
    public Text a;
    //��� ���� �ؽ�Ʈ UI
    public Text b;
    //����޴�â UI
    public GameObject SubMenu;
    //����޴�1 UI
    public GameObject SubPage1;
    public GameObject SubPage2;
    public GameObject SubPage3;
    public GameObject SubPage4;
    public StockList list; 

    // Start is called before the first frame update
    void Start()
    {
        //��ü ����
        int x = 0, z = 0;
        foreach (string key in list.apiInfo.Keys)
        {
            GameObject a = (GameObject)Instantiate(Resources.Load("Prefabs/Cube"));
            Debug.Log(list.apiInfo[key].api_marketprice);

            if (list.apiInfo[key].api_sector.Equals("Technology"))
            {
                x = -5;
                z = -5;
            }
            else if (list.apiInfo[key].api_sector.Equals("Communication Services"))
            {
                x = 5;
                z = -5;
            }
            else if (list.apiInfo[key].api_sector.Equals("Consumer Cyclical"))
            {
                x = 5;
                z = 5;
            }
            else if (list.apiInfo[key].api_sector.Equals("Financial Services"))
            {
                x = -5;
                z = 5;
            }

            a.transform.position = new Vector3(x, 2, z);
            Debug.Log(list.apiInfo[key].api_sector);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SubMenuBtnClick()
    {
        //����޴��� ȭ�鿡 ������ â������
        if (SubMenu.activeSelf)
        {
            SubMenu.SetActive(false);
        }
        //����޴��� ȭ�鿡 ������ â����
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
        //����޴� ȭ������ �̵�
        SubPage1.SetActive(false);
        SubPage2.SetActive(false);
        SubPage3.SetActive(false);
        SubPage4.SetActive(false);
        SubMenu.SetActive(true);
    }
}
