using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class dividendCtrl : MonoBehaviour
{
    public portfolio myPortfolio; //���� ���� ���� ������
    public StockList stockList; //�ֽ� ���� ���� ��ü

    public string divDate(string code)
    {
        int compareDate; //��¥ �� ��� ���� ����
        DateTime today = DateTime.Now; //���� ��¥
        DateTime divDate; //��� ��¥

        string tmp = stockList.apiInfo[code].api_divDate; //��� ��¥ �ҷ�����

        //������� ��
        if(tmp != null)
        {
            divDate = Convert.ToDateTime(tmp); //��� ��¥ - ��¥ Ÿ������ ��ȯ
            compareDate = DateTime.Compare(today, divDate); //���� ��¥�� ��� ��¥ ��

            //���� ���� ��� ��¥�� ������ �ʾҴٸ�
            if (compareDate <= 0)
            {
                return divDate.ToString("yyyy-MM-dd");
            }

            //�̹� ��� ��¥�� ������ �� or ����ְ� �ƴ� ��
            else
            {
                return "���� ��� ��¥ ���� ����";
            }
        }

        //����ְ� �ƴ� ��
        else
        {
            return "����ְ� �ƴ�";
        }
        
    }

    //���� ���� ��ȯ �Լ�
    public float dividend(string code)
    {
        return myPortfolio.stockInfo[code].shares * stockList.apiInfo[code].api_divRate;
    }
}