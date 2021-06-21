using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class stockData
{
    public float marketprice; //���� �ð�
    public string divDate; //�����
    public float divRate; //����
    public string sector; //���� ������ �з�
    public float marketcap; //�ð��Ѿ�
    public float per; //PER
    public float week52; //�ð� ���� ��ȭ(52 week change)
}
[CreateAssetMenu(menuName = "StockList")]
public class StockList : ScriptableObject
{
    //�ֽ��ڵ�� ������ ��ųʸ��� ����
    public Dictionary<string,stockData> list;
    public void addStockList(string c, float mp, string dd, float dr, string s, float mc, float p, float w5)
    {
        stockData stock1 = null;
        stock1.marketprice = mp;
        stock1.divDate = dd;
        stock1.divRate = dr;
        stock1.sector = s;
        stock1.marketcap = mc;
        stock1.per = p;
        stock1.week52 = w5;
        list.Add(c,stock1);
    }
    public void delStockList(string c)
    {
        list.Remove(c);
    }
}
