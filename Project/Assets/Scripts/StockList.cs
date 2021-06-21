using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StockList")]
public class StockList : ScriptableObject
{
    //종목코드와 얻은 api 자료를 저장한 딕셔너리
    public Dictionary<string, APIData> apiInfo = new Dictionary<string, APIData>();
    public class APIData //API에서 불러온 데이터 return 하기 위한 클래스
    {
        public float api_marketprice; //현재 시가
        public string api_divDate; //배당일
        public float api_divRate; //배당률
        public string api_sector; //관련 업종별 분류
        public float api_marketcap; //시가총액
        public float api_per; //PER
        public float api_52week; //시가 성장 변화(52 week change)

        public APIData(float api_marketprice, string api_divDate, float api_divRate, string api_sector, float api_marketcap, float api_per, float api_52week)
        {
            this.api_marketprice = api_marketprice;
            this.api_divDate = api_divDate;
            this.api_divRate = api_divRate;
            this.api_sector = api_sector;
            this.api_marketcap = api_marketcap;
            this.api_per = api_per;
            this.api_52week = api_52week;
        }
    }
    public void add(string code, float send_price, string send_divdate, float send_divrate, string send_sector,float send_marketcap,float send_per,float send_52)
    {
        apiInfo.Add(code,new APIData(send_price, send_divdate, send_divrate, send_sector, send_marketcap, send_per, send_52));
    }
}
