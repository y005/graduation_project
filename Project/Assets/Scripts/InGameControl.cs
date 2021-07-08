using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameControl : MonoBehaviour
{
     public StockList list; //����API���� �����ڷ�
     public portfolio myPortfolio; //��Ʈ������ ���� �����ڷ�
     public GameObject SubMenu; //����޴�â
     public GameObject EditPage; //��Ʈ������ ���� ���� ������
     public Camera getCamera; //���� ���� ī�޶�
     public Text thisSymbol; //ȭ�鿡 ��� �����
     public GameObject[] totalMode; //��ü �ֽĽ��� ����϶� ���;� �Ǵ� ������Ʈ ����Ʈ
     public GameObject[] portfolioMode; //��Ʈ������ ����϶� ���;� �Ǵ� ������Ʈ ����Ʈ
     private RaycastHit hit; //���콺�� Ŭ���� ��ü 
     private bool subMenuPopUp; //����޴�â�� ���ִ��� Ȯ���ϴ� bool����

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
         //Ŭ���� ��ü �̸� ���
         if (Input.GetMouseButtonDown(0))
         {
             string tmpname = "";
             Ray ray = getCamera.ScreenPointToRay(Input.mousePosition);
             if (Physics.Raycast(ray, out hit))
             {
                 if(hit.collider.gameObject.CompareTag("stock"))//�ǹ� ������Ʈ�� Ŭ���� ���
                 {
                     tmpname = hit.collider.gameObject.name;
                     thisSymbol.text = tmpname;

                     Debug.Log("����� = " + list.apiInfo[tmpname].api_divDate
                              + "  ���� = " + list.apiInfo[tmpname].api_divRate
                              + "  sector = " + list.apiInfo[tmpname].api_sector
                              + "  �ð��Ѿ� = " + list.apiInfo[tmpname].api_marketcap
                              + "  PER = " + list.apiInfo[tmpname].api_per
                              + "  52week = " + list.apiInfo[tmpname].api_52week
                              + "  previous close = " + list.apiInfo[tmpname].api_preclose);
                 }
             }
         }
     }
     public void SubMenuBtnClick()
     {
         //���������� ������
         EditPage.SetActive(false);

         //����޴��� ȭ�鿡 ������ â������
         if (SubMenu.activeSelf)
         {
             SubMenu.SetActive(false);
             subMenuPopUp = false;
         }
         //����޴��� ȭ�鿡 ������ â����
         else
         {
             SubMenu.SetActive(true);
             subMenuPopUp = true;
        }
     }
     //total��ư�� ������ �ΰ����� ��ü �ֽĽ��� ���� ��ȯ��
     public void totalBtnClick()
     {
        //��Ʈ�������ʿ� ��ġ�� �ǹ� ��ü�� ��Ȱ��ȭ
        foreach (GameObject tmp in myPortfolio.myStocks)
        {
             tmp.SetActive(false);
        }
        //portfolioMode �±װ� �޸� ��ü�� ���� ��Ȱ��ȭ(total UI��ư, edit UI ��ư, totalGain,divGain �ؽ�Ʈ UI)
        for (int i = 0; i < portfolioMode.Length; i++)
         {
            portfolioMode[i].SetActive(false);
         }
         //totalMode �±װ� �޸� ��ü�� ���� Ȱ��ȭ(��Ʈ������ UI ��ư�̶� Building �θ� ��ü)
         for (int i = 0; i < totalMode.Length; i++)
         {
            totalMode[i].SetActive(true);
         }  
     }

    //totalMode ���� �ð��Ѿ׿� ���� ������ �����۾�
    void BuildingScaleSet()
    {
        float tmp_cap = 0;
        float scale = 1f;

        //totalMode[0]�� Building����(�����ֽĵ��� �θ� ��ü�� �ִ� �ڽ� ��ü���� gameObject.transform.localScale ����) 
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

    //portfolio��ư�� ������ �ΰ����� ��Ʈ������ ���� ��ȯ��
    public void portfolioBtnClick()
    {
        thisSymbol.text = "";

        //totalMode �±װ� �޸� ��ü�� ���� ��Ȱ��ȭ(��Ʈ������ UI ��ư�̶� Building �θ� ��ü)
        for (int i = 0; i < totalMode.Length; i++)
        {
            totalMode[i].SetActive(false);
        }
        //portfolioMode �±װ� �޸� ��ü�� ���� Ȱ��ȭ(total UI��ư, edit UI ��ư, totalGain,divGain �ؽ�Ʈ UI)
        for (int i = 0; i < portfolioMode.Length; i++)
        {
            portfolioMode[i].SetActive(true);
        }
        //��Ʈ�������ʿ� ��ġ�� �ǹ� ��ü�� Ȱ��ȭ
        foreach (GameObject tmp in myPortfolio.myStocks)
        {
            tmp.SetActive(true);
        }
    }
    //edit��ư�� ������ �ΰ����� �������� ���� �������� ��ȯ��
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //���������������� Back��ư�� ������ ����޴� �������� ��ȯ��
    public void QuitBtnClick()
    {
        //����޴� ȭ������ �̵�
        EditPage.SetActive(false);
        SubMenu.SetActive(true);
    }
}
