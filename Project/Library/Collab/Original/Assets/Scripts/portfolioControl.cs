using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class portfolioControl : MonoBehaviour
{
    public Text totalGain;//��ü ���� �ؽ�Ʈ UI
    public Text divGain;//��� ���� �ؽ�Ʈ UI

    //edit�������� �ִ� �Է��ʵ�
    public Text cash;// ���� �Է�
    public Text code;// �����ڵ� �Է�
    public Text date;// �Ÿų�¥ �Է�
    public Text share;// �Ÿ������ �Է�
    public Text costPerShare;// ��ܰ� �Է� 
   
    //��Ʈ������ ��忡�� Ȱ��ȭ��Ű�� ������Ʈ�� ���� ��ũ��Ʈ�Դϴ�.
    public StockList list;//api�ֽ� ���� ���� ������
    public portfolio myPortfolio;//���� ���� ���� ������
    //��Ʈ�������� ����� ���� ����
    public Dictionary<string, int> sectorCnt = new Dictionary<string, int>();
    void Start()
    {
        //����� ���� ���� ���� �ʱ�ȭ
        sectorCnt.Add("Technology", 0);
        sectorCnt.Add("Communication Services", 0);
        sectorCnt.Add("Consumer Cyclical", 0);
        sectorCnt.Add("Financial Services", 0);
    }
    // Update is called once per frame
    void Update()
    {
        //��Ʈ�������� ���ŵ� ��� ��ü ��ġ �ݿ�
        if (myPortfolio.renew)
        {
            settingPortfolio();
            myPortfolio.renew = false;
        }
    }

    //�ڽ��� ���� ������ ��ü�� ��ġ�ϱ�
    void settingPortfolio()
    {
        //��ü �ֽ��򰡱ݾ� ���庯��
        float total = 0;
        string path = "";
        float x = 0, z = 0;
        int loc = 0;

        //"myStock" �±װ� �޸� ��ü(��Ʈ������ ��ġ�� �ǹ�)�� ���� ����(������ �ɶ� ���� �ݺ�)
        foreach(GameObject tmp in myPortfolio.myStocks){
            Destroy(tmp);
        }
        myPortfolio.myStocks.Clear();
        //���� ������ ���� ī��Ʈ ���� �ʱ�ȭ
        foreach (string tmp in sectorCnt.Keys)
        {
            sectorCnt[tmp] = 0;
        }
        //��ü �򰡱ݾ� ��� UI�� ǥ��
        foreach (string key in myPortfolio.stockInfo.Keys)
        {
            //���� ������ ���������� 0���� ��� ī��Ʈ���� ����
            if (myPortfolio.stockInfo[key].shares==0) { continue; }
            total += myPortfolio.updateGain(key);
            Debug.Log(total);
        }
        totalGain.text = total.ToString();
        //�ش��ϴ� ���� ��ġ�� ���ʴ�� ���� �������� ���Ľ�Ű��
        foreach (string key in myPortfolio.stockInfo.Keys)
        {
            //���� ������ ���������� 0���� ��� ��ġ���� ����
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            if (list.apiInfo[key].api_sector.Equals("Technology"))
            {
                x = -8.7f;
                z = -0.7f;
            }
            else if (list.apiInfo[key].api_sector.Equals("Communication Services"))
            {
                x = 0.2f;
                z = -0.7f;
            }
            else if (list.apiInfo[key].api_sector.Equals("Consumer Cyclical"))
            {
                x = 0.2f;
                z = 8f;
            }
            else if (list.apiInfo[key].api_sector.Equals("Financial Services"))
            {
                x = -8.7f;
                z = 8f;
            }
            path = "Prefabs/Buildings/" + key;
            loc = ++sectorCnt[list.apiInfo[key].api_sector];
            GameObject a = (GameObject)Instantiate(Resources.Load(path));
            a.name = key;
            a.gameObject.tag = "myStock";
            myPortfolio.myStocks.Add(a);
            //�� �࿡ 4ĭ�� ��ġ�� ��ȹ�� ��ġ 
            a.transform.position = new Vector3(x + (loc % 4) * 2.5f, 2, z - (loc / 4) * 2f);
            //�ڻ� ��� ������ ���� ���ϱ�(1~4�ܰ�� ���� ����)
            float ratio = myPortfolio.updateGain(key) / total;
            float scale = 1f;
            if (ratio < 0.25){ scale = 0.75f; }
            else if (ratio < 0.5){ scale = 1f; }
            else if (ratio < 0.75){ scale = 1.25f; }
            else if (ratio < 1){ scale = 1.75f; }
            //�ܰ迡 ���� ������ ������ŭ ��ü ������ �����ϱ�
            a.transform.localScale = new Vector3(scale * a.transform.localScale.x, scale * a.transform.localScale.y, scale * a.transform.localScale.z);
        }
    }
    public void CashPlusBtnClick()
    {
        if (checkCashEditInput())
        {
            Debug.Log(float.Parse(cash.text));
            myPortfolio.Cash += float.Parse(cash.text);
        }
        else{ Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");}
    }

    public void CashMinusBtnClick()
    {
        if (checkCashEditInput())
        {
            if (myPortfolio.Cash >= float.Parse(cash.text)) {myPortfolio.Cash -= float.Parse(cash.text);}
            else{Debug.Log("��Ʈ�������� �ִ� �ڱݺ��� �۰ų� ���� ���� �Է��ϼ���");}
        }
        else{Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");}
    }

    public void buyBtnClick()
    {
        Debug.Log(code.text);
        Debug.Log(date.text);
        Debug.Log(Int32.Parse(share.text));
        Debug.Log(float.Parse(costPerShare.text));
        
        if (checkStockEditInput())
        {
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text),false);
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
    }
    public void sellBtnClick()
    {
        Debug.Log(code.text);
        Debug.Log(date.text);
        Debug.Log(Int32.Parse(share.text));
        Debug.Log(float.Parse(costPerShare.text));

        if (checkStockEditInput()){
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), true);
        }
        else{Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");}
    }

    bool checkStockEditInput()
    {
        Text[] stockInputs = {cash,date,share,costPerShare};
        for(int i = 0; i < stockInputs.Length; i++){if(stockInputs[i].text == ""){return false;}}
        return true;
    }

    bool checkCashEditInput()
    {
        if (cash.text == ""){ return false; }
        return true;
    }
}
