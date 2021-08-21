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
    public StockList list; //����API���� �����ڷ�
    public portfolio myPortfolio; //��Ʈ������ ���� �����ڷ�

    public GameObject SubMenu; //����޴�â
    public GameObject EditPage; //��Ʈ������ ���� ���� ������
    public GameObject OptPage; //�������� ���� ������

    public Toggle myStockOpt; //�� ���� ǥ���ϴ� �ɼ�
    public Toggle layerOpt; //�� ���� �����ϰ� ǥ���ϴ� �ɼ�
    public Toggle weatherOpt; //��Ʈ������ ���Ͱ� �����Ǵ� ���� �ɼ�
    public Toggle marketTimeOpt; //�ֽ� ���� �ð��� �����ϴ� �ð� �ɼ� 

    public bool myStockFlag;
    public bool layerFlag;
    public bool weatherFlag;
    public bool marketTimeFlag;
    public bool subMenuPopUp; //����޴�â�� ���ִ��� Ȯ���ϴ� bool����

    public GameObject[] totalMode; //��ü �ֽĽ��� ����϶� ���;� �Ǵ� ������Ʈ ����Ʈ
    public GameObject[] portfolioMode; //��Ʈ������ ����϶� ���;� �Ǵ� ������Ʈ ����Ʈ

    void Start()
    {
        Load();//����� �����͸� �ҷ��� �� ���� ����
        checkOpt();
        subMenuPopUp = false;
        totalBtnClick();
    }
    void Update()
    {
        checkOpt(); //���� �ɼ� Ȯ��
        if (myPortfolio.renew)//��Ʈ������ ������ �Ǹ� ���� ���� �缳ġ �۾� ����
        {
            settingPortfolio(); 
        }
        //���� ���� ���� �ɼǿ� ������ ��� ��Ʈ������ ��带 ����
        if (myStockFlag)
        {
            portfolioBtnClick();
        }
        else
        {
            totalBtnClick();
        }
    }
    void checkOpt()//�Ź� �ɼ� ���� Ȯ��
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
        //BuildingScaleSet();
    }

    //total����� ��� �ð��Ѿ׿� ���� ������ �����۾�
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

    //portfolio����� ��� �ΰ����� ��Ʈ������ ���� ��ȯ��
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
        foreach (GameObject tmp in myPortfolio.myStocks)
        {
            tmp.SetActive(true);
        }
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

        //������ ��ġ�� ���� ���� �ǹ��� ��ġ(�򰡱ݾ� ���߿� ���� �����ϸ�)
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
            a.gameObject.tag = "stock";
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

            //DEBUG ::: ������� üũ
            string divDate = GameObject.Find("portfolioControl").GetComponent<dividendCtrl>().divDate(key);
            float dividend = GameObject.Find("portfolioControl").GetComponent<dividendCtrl>().dividend(key);
            Debug.Log(key + " - ��� ��¥ : " + divDate + ", " + "���� : " + dividend);
        }
        myPortfolio.renew = false;
        //��Ʈ�������� ���ŵǸ� ���� ����� ������� �����ϰ� ����� �������� ǥ��
        GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divDateSet();
        //��Ʈ�������� ���ŵǸ� �򰡱ݾ��� �����ϰ� �򰡱ݾ� �ؽ�Ʈ UI�� ǥ��
        GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().totalGainSet();
        //��Ʈ�������� ���ŵǸ� ������� �����ϰ� �ؽ�Ʈ UI�� ǥ��
        GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divGainSet();
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
    public void ExitBtnClick()
    {
        //������ �����ϰ� ���� �޴� ȭ������ �̵�
        Save();
        SceneManager.LoadScene("MainMenu");
    }
    //������ ���� �ڷᱸ��
    [Serializable]
    public class stockInfo
    {
        public string code;//�����ڵ�
        public int shares;//��������
        public float apc;//��ܰ�
    }
    public class savData
    {
        public float Cash;
        public List<stockInfo> stocks;
    }
    void Load()
    {
        //Json���� ������ ������ ���Ӱ� ������Ű�� 
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
            //���� ������Ʈ �� ��Ʈ������ �򰡱ݾ� ����
            foreach (var tmp in loadData.stocks)
            {
                totalGain += myPortfolio.updateGain(tmp.code);
            }
            //���� ������Ʈ �� ��Ʈ������ �򰡱ݾװ� �� �� ��Ʈ������ �򰡱ݾ� ���Ͽ� ���� �ľ�
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
            //���̺� �����Ͱ� ���� ��� ������ ȭâ������ ǥ����
            GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().isRain = 0f;
        }
    }
    void Save()
    {
        //Json���� ���·� �����Ű��
        savData sav = new savData();
        sav.Cash = myPortfolio.Cash;
        sav.stocks = new List<stockInfo>();
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //���� ������ ���������� 0���� ��� ���� �����Ϳ��� ����
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
