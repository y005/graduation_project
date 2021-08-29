using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class portfolioControl : MonoBehaviour
{
    public TextMeshProUGUI totalGain;//��ü ���� �ؽ�Ʈ UI
    public TextMeshProUGUI divGain; //��� ���� �ؽ�Ʈ UI

    //edit�������� �ִ� �Է��ʵ�
    public TMP_InputField cash;// ���� �Է�
    public TMP_InputField code;// �����ڵ� �Է�
    public TMP_InputField date;// �Ÿų�¥ �Է�
    public TMP_InputField share;// �Ÿ������ �Է�
    public TMP_InputField costPerShare;// ��ܰ� �Է� 
    
    //��Ʈ������ ��忡�� Ȱ��ȭ��Ű�� ������Ʈ�� ���� ��ũ��Ʈ�Դϴ�.
    public StockList list;//api�ֽ� ���� ���� ������
    public portfolio myPortfolio;//���� ���� ���� ������
    public void CashPlusBtnClick()
    {
        if (checkCashEditInput())
        {
            try
            {
                myPortfolio.Cash += float.Parse(cash.text);
                Debug.Log("cash = " + myPortfolio.Cash);
            }
            catch (FormatException fe)
            {
                Debug.Log("�ùٸ� ���� �Է��ϼ���.");
            }
        }
        else
        {
            Debug.Log("���� �Է��ϼ���");
        }
        cash.text = "������ �Է��ϼ���";
    }

    public void CashMinusBtnClick()
    {
        if (checkCashEditInput())
        {
            try{
                if (myPortfolio.Cash >= float.Parse(cash.text))
                {
                    myPortfolio.Cash -= float.Parse(cash.text);
                    Debug.Log("cash = " + myPortfolio.Cash);
                }
                else
                {
                    Debug.Log("��Ʈ�������� �ִ� �ڱݺ��� �۰ų� ���� ���� �Է��ϼ���");
                }
            }
            catch (FormatException fe)
            {
                Debug.Log("�ùٸ� ���� �Է��ϼ���.");
            }
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
        cash.text = "Enter Cash...";
    }

    public void buyBtnClick()
    {
        Debug.Log("BUY button click");
        if (checkStockEditInput())
        {
            try
            {
                myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), false);
            }
            catch (FormatException fe)
            {
                Debug.Log("�ùٸ� ���� �Է��ϼ���.");
            }
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
        inputsClear();
    }

    public void sellBtnClick()
    {
        Debug.Log("SELL button click");
        if (checkStockEditInput())
        {
            try
            {
                myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), true);
            }
            catch (FormatException fe)
            {
                Debug.Log("�ùٸ� ���� �Է��ϼ���.");
            }
        }
        else
        {
            Debug.Log("��� �Է� �ʵ忡 ���� �Է��ϼ���");
        }
        inputsClear();
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
        code.text = "�����ڵ带 �Է� �ϼ���";
        date.text = "�Ÿ� ��¥�� �Է� �ϼ���";
        share.text = "���� ������ �Է� �ϼ���";
        costPerShare.text = "��ܰ��� �Է� �ϼ���";
    }
}
