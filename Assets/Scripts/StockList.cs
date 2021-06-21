using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class stockData
{
    public float marketprice; //현재 시가
    public string divDate; //배당일
    public float divRate; //배당률
    public string sector; //관련 업종별 분류
    public float marketcap; //시가총액
    public float per; //PER
    public float week52; //시가 성장 변화(52 week change)
}
[CreateAssetMenu(menuName = "StockList")]
public class StockList : ScriptableObject
{
    //주식코드와 정보를 딕셔너리에 저장
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
