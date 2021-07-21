using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class portfolioControl : MonoBehaviour
{
    public Text totalGain;//��ü ���� �ؽ�Ʈ UI
    public Text divGain;//��� ���� �ؽ�Ʈ UI

    //edit�������� �ִ� �Է��ʵ�
    public InputField cash;// ���� �Է�
    public InputField code;// �����ڵ� �Է�
    public InputField date;// �Ÿų�¥ �Է�
    public InputField share;// �Ÿ������ �Է�
    public InputField costPerShare;// ��ܰ� �Է� 
    
    //��Ʈ������ ��忡�� Ȱ��ȭ��Ű�� ������Ʈ�� ���� ��ũ��Ʈ�Դϴ�.
    public StockList list;//api�ֽ� ���� ���� ������
    public portfolio myPortfolio;//���� ���� ���� ������
    //��Ʈ�������� ����� ���� ����
    public Dictionary<string, int> sectorCnt = new Dictionary<string, int>();

    public GameObject[] totalMode;

    void Start()
    {
        myPortfolio.renew =false;//��Ʈ������ ���� �÷��� false
        //����� ���� ���� ���� �ʱ�ȭ
        sectorCnt.Add("Health Care", 0);
        sectorCnt.Add("Financial", 0);
        sectorCnt.Add("Real Estate", 0);
        sectorCnt.Add("IT", 0);
        sectorCnt.Add("Consumer", 0);
        sectorCnt.Add("Industrial", 0);
    }
    // Update is called once per frame
    void Update()
    {
        //��Ʈ�������� ���ŵ� ��� ��ü ��ġ �ݿ�
        /*if (myPortfolio.renew)
        {
           settingPortfolio();
            myPortfolio.renew = false;
        }*/
    }

    //�ڽ��� ���� ������ ��ü�� ��ġ�ϱ�
    void settingPortfolio()
    {
        float total = 0;//��ü �ֽ��򰡱ݾ� ���庯��
        float myinvest = 0; //��ü ���ڱ� ���庯��
        string path = "";//������ �ε� ���
        //float x = 0, z = 0;
        //int loc = 0;//�ǹ� ��ü�� ��ġ�� y��ǥ

        //"myStock" �±װ� �޸� ��ü(��Ʈ������ ��ġ�� �ǹ�)�� ���� ����(������ �ɶ� ���� �ݺ�)
        foreach(GameObject tmp in myPortfolio.myStocks){Destroy(tmp);}
        myPortfolio.myStocks.Clear();

        //���� ������ ���� ī��Ʈ ���� �ʱ�ȭ
        foreach (var key in sectorCnt.Keys.ToList())
        {
            sectorCnt[key] = 0;
        }
        //��ü �򰡱ݾ� ���
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //���� ������ ���������� 0���� ��� ī��Ʈ���� ����
            if (myPortfolio.stockInfo[key].shares==0) { continue; }
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
            //���� �ǹ� ��ġ ��ȯ
            Vector3 pos;
            pos = totalMode[0].transform.Find(key).position;

            //���� ������ ���������� 0���� ��� ��ġ���� ����
            /*if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            if (list.apiInfo[key].api_sector.Equals("Technology")) { x = -8.7f; z = -0.7f; }
            else if (list.apiInfo[key].api_sector.Equals("Communication Services")) { x = 0.2f; z = -0.7f; }
            else if (list.apiInfo[key].api_sector.Equals("Consumer Cyclical")) { x = 0.2f; z = 8f; }
            else if (list.apiInfo[key].api_sector.Equals("Financial Services")) { x = -8.7f; z = 8f; }*/

            //loc = ++sectorCnt[list.apiInfo[key].api_sector];

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
            else if(ratio >= 70 && ratio <= 100){ scale = 1.25f; }
            
            //�ܰ迡 ���� ������ ������ŭ ��ü ������ �����ϱ�
            a.transform.localScale = new Vector3(scale * a.transform.localScale.x, scale * a.transform.localScale.y, scale * a.transform.localScale.z);        
        }
    }
    public void CashPlusBtnClick()
    {
        if (checkCashEditInput())
        {
            myPortfolio.Cash += float.Parse(cash.text);
      		Debug.Log("cash = " + myPortfolio.Cash);
            cash.text = "";
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
    }

    public void CashMinusBtnClick()
    {
        if (checkCashEditInput())
        {
            if (myPortfolio.Cash >= float.Parse(cash.text)) {
                myPortfolio.Cash -= float.Parse(cash.text);
               inputsClear();
            }
            else
            {
                Debug.Log("��Ʈ�������� �ִ� �ڱݺ��� �۰ų� ���� ���� �Է��ϼ���");
            }
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
    }

    public void buyBtnClick()
    {
        Debug.Log("BUY button click");
        if (checkStockEditInput())
        {
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), false);
            inputsClear();
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
    }

    public void sellBtnClick()
    {
        Debug.Log("SELL button click");
        if (checkStockEditInput())
        {
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), true);
            inputsClear();
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
    }

    bool checkStockEditInput()
    {
        string[] stockInputs = {date.text,share.text,costPerShare.text};
        for(int i = 0; i < stockInputs.Length; i++)
        {
            if(stockInputs[i] == "")
            {
                return false;
            }
        }
        return true;
    }

    bool checkCashEditInput()
    {
        if (cash.text == "") { return false; }
        return true;
    }
    //���� ���� ������ �Ǹ� ��ǲ�ʵ� ���� ��������.
    void inputsClear()
    {
        code.text = "";
        date.text = "";
        share.text = "";
        costPerShare.text = "";
    }
}
