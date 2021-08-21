using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InGameControl : MonoBehaviour
{
     public StockList list; //����API���� �����ڷ�
     public portfolio myPortfolio; //��Ʈ������ ���� �����ڷ�
     public GameObject SubMenu; //����޴�â
     public GameObject EditPage; //��Ʈ������ ���� ���� ������
     public GameObject OptPage; //�������� ���� ������
     public bool subMenuPopUp; //����޴�â�� ���ִ��� Ȯ���ϴ� bool����
     public Toggle myStockOpt; //�� ���� ǥ���ϴ� bool����
     public GameObject[] totalMode; //��ü �ֽĽ��� ����϶� ���;� �Ǵ� ������Ʈ ����Ʈ
     public GameObject[] portfolioMode; //��Ʈ������ ����϶� ���;� �Ǵ� ������Ʈ ����Ʈ

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
        //����޴��� ��������â�� ȭ�鿡 ������ â����
        if (!SubMenu.activeSelf && !GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().StockInfoMenuPopUp)
         {
            SubMenu.SetActive(true);
            subMenuPopUp = true;
        }
        //����޴��� ȭ�鿡 ������ â������
        else
        {
            SubMenu.SetActive(false);
            subMenuPopUp = false;
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
       /* foreach (GameObject tmp in myPortfolio.myStocks)
        {
            tmp.SetActive(true);
        }*/

        //�ǹ� ��ġ
        settingPortfolio();

    }
    
    //��Ʈ������ �ǹ� ��ġ
    void settingPortfolio()
    {
        float total = 0;//��ü �ֽ��򰡱ݾ� ���庯��
        float myinvest = 0; //��ü ���ڱ� ���庯��
        string path = "";//������ �ε� ���

        //"myStock" �±װ� �޸� ��ü(��Ʈ������ ��ġ�� �ǹ�)�� ���� ����(������ �ɶ� ���� �ݺ�)
        foreach (GameObject tmp in myPortfolio.myStocks) { Destroy(tmp); }
        myPortfolio.myStocks.Clear();

        //���� ������ ���� ī��Ʈ ���� �ʱ�ȭ
        /*foreach (var key in sectorCnt.Keys.ToList())
        {
            sectorCnt[key] = 0;
        }*/
        //��ü �򰡱ݾ� ���
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //���� ������ ���������� 0���� ��� ī��Ʈ���� ����
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            total += myPortfolio.updateGain(key);
        }
        //���� ��ü ���ڱݾ� ���
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            myinvest += myPortfolio.stockInfo[key].shares * myPortfolio.stockInfo[key].avgCostPerShare;
        }
        //��ü ���ͱ� = ��ü �򰡱ݾ� - ���� ��ü ���ڱݾ�
        //totalGain.text = (total - myinvest).ToString();

        //�ش��ϴ� ���� ��ġ�� ���ʴ�� ���� �������� ���Ľ�Ű��
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //���� ������ ���������� 0���� ��� ��ġ���� ����
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }

            //���� �ǹ� ��ġ ��ȯ
            Vector3 pos;
            pos = totalMode[0].transform.Find(key).position;

            //�ǹ� ��ġ
            path = "Prefabs/Buildings/" + key;
            GameObject a = (GameObject)Instantiate(Resources.Load(path));
            a.name = key;
            a.gameObject.tag = "myStock";
            myPortfolio.myStocks.Add(a);

            //������ ���� ��ġ�� �ǹ� ��ġ
            a.transform.position = new Vector3(pos.x, pos.y, pos.z);

            //�ڻ� ��� ������ ���� ���ϱ�(1~3�ܰ�)
            float ratio = myPortfolio.updateGain(key) / total * 100; //��ü �򰡱ݾ� �� �ش� ���� �򰡱ݾ� ����
            float scale = 1f;
            if (ratio < 30) { scale = 0.75f; }
            else if (ratio >= 30 && ratio < 70) { scale = 1f; }
            else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }

            //�ܰ迡 ���� ������ ������ŭ ��ü ������ �����ϱ�
            a.transform.localScale = new Vector3(scale * a.transform.localScale.x, scale * a.transform.localScale.y, scale * a.transform.localScale.z);
        }
    }

    //edit��ư�� ������ �ΰ����� �������� ���� �������� ��ȯ��
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //option��ư�� ������ �ΰ��ӳ� �������� ���� �������� ��ȯ��
    public void OptBtnClick()
    {
        SubMenu.SetActive(false);
        OptPage.SetActive(true);
    }
    //���������������� Back��ư�� ������ ����޴� �������� ��ȯ��
    public void QuitBtnClick()
    {
        //����޴� ȭ������ �̵�
        EditPage.SetActive(false);
        OptPage.SetActive(false);
        SubMenu.SetActive(true);
    }
}
