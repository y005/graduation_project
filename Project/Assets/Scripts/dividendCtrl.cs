using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class dividendCtrl : MonoBehaviour
{
    public portfolio myPortfolio; //보유 종목 저장 데이터
    public StockList stockList; //주식 정보 저장 객체

    public string divDate(string code)
    {
        int compareDate; //날짜 비교 결과 저장 변수
        DateTime today = DateTime.Now; //오늘 날짜
        DateTime divDate; //배당 날짜

        string tmp = stockList.apiInfo[code].api_divDate; //배당 날짜 불러오기

        //배당주일 때
        if(tmp != null)
        {
            divDate = Convert.ToDateTime(tmp); //배당 날짜 - 날짜 타입으로 변환
            compareDate = DateTime.Compare(today, divDate); //오늘 날짜와 배당 날짜 비교

            //아직 다음 배당 날짜가 지나지 않았다면
            if (compareDate <= 0)
            {
                return divDate.ToString("yyyy-MM-dd");
            }

            //이미 배당 날짜가 지났을 때 or 배당주가 아닐 때
            else
            {
                return "다음 배당 날짜 정보 없음";
            }
        }

        //배당주가 아닐 때
        else
        {
            return "배당주가 아님";
        }
        
    }

    //종목별 배당금 반환 함수
    public float dividend(string code)
    {
        return myPortfolio.stockInfo[code].shares * stockList.apiInfo[code].api_divRate;
    }
}