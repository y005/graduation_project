using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using TMPro;
public class InGameControl : MonoBehaviour
{
    public StockList list; //����API���� �����ڷ�
    public portfolio myPortfolio; //��Ʈ������ ���� �����ڷ�

    public GameObject SubMenu; //����޴�â
    public GameObject EditPage; //��Ʈ������ ���� ���� ������
    public GameObject InfoOptPage; //�������� ���� ������
    public GameObject OptPage; //�������� ���� ������

    public GameObject totalGainPage; //��Ʈ������ ���� ������
    public GameObject content; //��Ʈ������ ������ �ִ� ������Ʈ
    public TextMeshProUGUI totalGainInfo; //�ڻ� ������ �Է��ϴ� �ؽ�Ʈ UI

    public Toggle priceOpt; //�ְ� ���� ǥ���ϴ� �ɼ�
    public Toggle volumeOpt; //�ŷ��� ���� ǥ���ϴ� �ɼ�
    public Toggle divOpt; //��� ���� ǥ���ϴ� �ɼ�
    public Toggle interestOpt; // ���ɵ� ���� ǥ���ϴ� �ɼ�

    public Toggle myStockOpt; //�� ���� ǥ���ϴ� �ɼ�
    public Toggle layerOpt; //�� ���� �����ϰ� ǥ���ϴ� �ɼ�
    public Toggle weatherOpt; //��Ʈ������ ���Ͱ� �����Ǵ� ���� �ɼ�
    public Toggle marketTimeOpt; //�ֽ� ���� �ð��� �����ϴ� �ð� �ɼ� 

    public bool priceFlag;
    public bool volumeFlag;
    public bool divFlag;
    public bool interestFlag;

    public bool myStockFlag;
    public bool layerFlag;
    public bool weatherFlag;
    public bool marketTimeFlag;
    public bool pagePopUp; //â�� ���� ����ȭ�鿡 ���ִ��� Ȯ���ϴ� bool����
    public TextMeshProUGUI optInfo; //������ ���̾� ���� ǥ��â

    public GameObject building;

    private List<Vector3> scales = new List<Vector3>();//������ �ʱ� ����Ʈ ������ ���� ���� �迭

    void Start()
    {
        optInfo.text = "";
        //������ ������ ���� ����
        for (int i = 0; i < building.transform.childCount; i++)
        {
            Vector3 tmp = building.transform.GetChild(i).gameObject.transform.localScale;
            scales.Add(new Vector3(tmp.x, tmp.y, tmp.z));
        }
        Load();//����� �����͸� �ҷ��� �� ���� ����
        checkOpt();
        pagePopUp = false;
        totalBtnClick();
        myStockOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(myStockOpt); });
        priceOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(priceOpt);});
        volumeOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(volumeOpt); });
        divOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(divOpt); });
        interestOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(interestOpt); });
    }
    void Update()
    {
        checkOpt(); //���� �ɼ� Ȯ��
    }
    void checkOpt()//�Ź� �ɼ� ���� Ȯ��
    {
        myStockFlag = myStockOpt.isOn;
        layerFlag = layerOpt.isOn;
        weatherFlag = weatherOpt.isOn;
        marketTimeFlag = marketTimeOpt.isOn;

        priceFlag = priceOpt.isOn; 
        volumeFlag = volumeOpt.isOn; 
        divFlag = divOpt.isOn; 
        interestFlag = interestOpt.isOn;
    }
    void ToggleValueChanged(Toggle change)
    {
        if (change == myStockOpt)
        {
            if (change.isOn)//�� ���� ���� ���� ����
            {
                portfolioBtnClick();
            }
            else//��ü �ֽ� ���� ����鸸 ���� ���� 
            {
                totalBtnClick();
            }
            return;
        }
        if (change.isOn)
        {
            optInfo.text = "";
            offToggleExcept(change);
        }
    }
    void offToggleExcept(Toggle change)
    {
        if (change == priceOpt)
        {
            optInfo.text = "���ͷ�";
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            interestOpt.isOn = false;
            priceOpt.isOn = true;
        }
        else if (change == volumeOpt)
        {
            optInfo.text = "�ŷ���";
            priceOpt.isOn = false;
            divOpt.isOn = false;
            interestOpt.isOn = false;
            volumeOpt.isOn = true;
        }
        else if (change == divOpt)
        {
            optInfo.text = "�����";
            volumeOpt.isOn = false;
            priceOpt.isOn = false;
            interestOpt.isOn = false;
            divOpt.isOn = true;
        }
        else
        {
            optInfo.text = "���ɵ�";
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            priceOpt.isOn = false;
            interestOpt.isOn = true;
        }
    }
    public void SubMenuBtnClick()
    {
        if (EditPage.activeSelf || InfoOptPage.activeSelf || OptPage.activeSelf)
        {
            EditPage.SetActive(false);
            InfoOptPage.SetActive(false);
            OptPage.SetActive(false);
            pagePopUp = false;
            return;
        }
        //����޴��� ȭ�鿡 ������ â ����
        if (!SubMenu.activeSelf)
        {
            totalGainPage.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().StockInfo.SetActive(false);

            SubMenu.SetActive(true);
            pagePopUp = true;
        }
        //����޴��� ȭ�鿡 ������ â������
        else
        {
            SubMenu.SetActive(false);
            pagePopUp = false;
        }
    }
    //total��ư�� ������ �ΰ����� ��ü �ֽĽ��� ���� ��ȯ��
    public void totalBtnClick()
    {
        GameObject stock;
        GameObject effect;
        string key;
        float tmp_cap = 0;
        float scale = 1f;

        //������ ���� ��ǳ���� ��ü Ȱ��ȭ
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            stock.SetActive(true);
            effect.SetActive(true);
            //�� �ڻ� ��� ���� ���� ��ŭ ��ü ������ �����ϱ�
            if (list.apiInfo.ContainsKey(key))
            {
                tmp_cap = list.apiInfo[key].api_marketcap / 1000000000000;
                if (tmp_cap >= 0.9) { scale = 1.25f; }
                else if (tmp_cap < 0.9 && tmp_cap >= 0.3) { scale = 1f; }
                else if (tmp_cap < 0.3) { scale = 0.75f; }
                stock.transform.localScale = new Vector3(scale * scales[i].x,scale * scales[i].y,scale * scales[i].z);
            }
            else
            {
                stock.transform.localScale = new Vector3(scales[i].x, scales[i].y, scales[i].z);
            }
        }
    }

    //portfolio����� ��� �ΰ����� ��Ʈ������ ���� ��ȯ��
    public void portfolioBtnClick()
    {
        GameObject stock;
        GameObject effect;
        float total = 0;//��ü �ֽ��򰡱ݾ� ���庯��
        string key;

        //��ü �򰡱ݾ� ���
        foreach (var key1 in myPortfolio.stockInfo.Keys.ToList())
        {
            //���� ������ ���������� 0���� ��� ī��Ʈ���� ����
            if (myPortfolio.stockInfo[key1].shares == 0) { continue; }
            total += myPortfolio.updateGain(key1);
        }
        //������ ���� ��ǳ���� ��ü Ȱ��ȭ
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            //�������� �ʴ� ��� ��Ȱ��ȭ
            if(!myPortfolio.stockInfo.ContainsKey(key)){
                stock.SetActive(false);
                effect.SetActive(false);
            }
            else
            {
                if (myPortfolio.stockInfo[key].shares == 0) {
                    stock.SetActive(false);
                    effect.SetActive(false); 
                    continue; 
                }
                stock.SetActive(true);
                effect.SetActive(true);
                //�� �ڻ� ��� ���� ���� ��ŭ ��ü ������ �����ϱ�
                float ratio = myPortfolio.updateGain(key) / total * 100;
                float scale = 1f;
                if (ratio < 30) { scale = 0.75f; }
                else if (ratio >= 30 && ratio < 70) { scale = 1f; }
                else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }
                stock.transform.localScale = new Vector3(scale * scales[i].x, scale * scales[i].y, scale * scales[i].z);
            }
        }
    }

    //���� �߰���ư�� ������ �ΰ����� �������� ���� �������� ��ȯ��
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //���� ������ư�� ������ �ΰ��ӳ� ����ǥ�� ���� �������� ��ȯ��
    public void InfoOptBtnClick()
    {
        SubMenu.SetActive(false);
        InfoOptPage.SetActive(true);
    }
    //�ɼ� ��ư�� ������ �ΰ��ӳ� ���� �������� ��ȯ��
    public void OptBtnClick()
    {
        SubMenu.SetActive(false);
        OptPage.SetActive(true);
    }
    //���� ��Ʈ������ ���� ������ ����Ʈ��� �� �� �ִ� ����â�� ����
    public void myPortfolioBtnClick()
    {

        if (totalGainPage.activeSelf)//�̹� ���������� ������ ���������� �������� ����
        {
            totalGainPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //�ٸ� â�� �� ������
            SubMenu.SetActive(false);
            EditPage.SetActive(false);
            InfoOptPage.SetActive(false);
            OptPage.SetActive(false);
            totalGainPage.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().StockInfo.SetActive(false);

            pagePopUp = true;
            totalGainPage.SetActive(true);
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            GameObject tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
            tmp.transform.SetParent(content.transform, false);

            float gainSum = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().totalGainSet();
            float tmp1 = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                tmp1 += myPortfolio.stockInfo[key].avgCostPerShare * myPortfolio.stockInfo[key].shares;
            }
            if (tmp1 != 0) { tmp1 = (gainSum - tmp1) / tmp1 * 100; }
            totalGainInfo.text = "�ڻ�: $" + gainSum.ToString("F2");
            totalGainInfo.text += " ���ͷ�: " + tmp1.ToString("F2")+"%";

            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                //���� ������ ���������� 0���� ��� ��ġ���� ����
                if (myPortfolio.stockInfo[key].shares == 0) { continue; }
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/logo"));
                tmp.transform.SetParent(content.transform, false);
                tmp.GetComponent<Image>().sprite = Resources.Load("Sprites/" + key, typeof(Sprite)) as Sprite;

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/totalGainList"));
                tmp.transform.SetParent(content.transform, false);
                int cnt = myPortfolio.stockInfo[key].shares;//��������
                float market = myPortfolio.updateGain(key);//���� �򰡱ݾ�
                float apc = myPortfolio.stockInfo[key].avgCostPerShare;//��� �ܰ�
                
                float current = list.apiInfo[key].api_marketprice;//���� �ְ�
                float gainPercent = (market - apc*cnt)/(apc*cnt)*100;//���ͷ�

                TextMeshProUGUI info = tmp.GetComponent<TextMeshProUGUI>();

                info.text = "���� �ڵ�: "+key+"\n";
                info.text += "���� �ְ�: $" + current.ToString("F2") + "\n";
                info.text += "��� �ܰ�: $" + apc.ToString("F2") + "\n";
                info.text += "���� ����: "+ cnt.ToString() + "��\n";
                info.text += "���ͷ�: "+ gainPercent.ToString("F2") + "%\n";
                info.text += "�򰡱ݾ�: $" + market.ToString("F2");

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);
            }
        }
    }
    //���������������� Back��ư�� ������ ����޴� �������� ��ȯ��
    public void QuitBtnClick()
    {
        //����޴� ȭ������ �̵�
        EditPage.SetActive(false);
        OptPage.SetActive(false);
        InfoOptPage.SetActive(false);
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
