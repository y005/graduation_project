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
