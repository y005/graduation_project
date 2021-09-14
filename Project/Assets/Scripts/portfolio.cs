using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "portfolio")]
public class portfolio : ScriptableObject
{
    public StockList list; //주식정보API

    public float Cash = 0; //보유 현금 금액
    
    //종목별에 대한 갯수, 평단가를 하나의 딕셔너리로 저장
    public Dictionary<string, StockStat> stockInfo = new Dictionary<string, StockStat>();
    
    //종목코드와 거래 데이터 리스트를 하나의 딕셔너리로 저장
    public Dictionary<string, List<Trade> > tradeList = new Dictionary<string, List<Trade> >();

    //struct 구조체의 경우 .을 통한 구조체 속성 변경이 불가능하여 똑같은 구조체 객체를 복사한
    //뒤 직접적인 속성변경한 후 그 객체로 대체해야 함
    public struct StockStat//개별 종목의 평단가와 갯수(일일 매매 기록을 합산하여 계산한 구조체)
    {
        public int shares;
        public float avgCostPerShare;
    };

    public class Trade //API에서 불러온 데이터 return 하기 위한 클래스
    {
        public string TradeDate;//매매 날짜
        public int Shares;//매매 주식수
        public float CostPerShare;//평단가
        public bool State; //매수: 0, 매도: 1 여부 
        public Trade(string TradeDate, int Shares, float CostPerShare, bool State)
        {
            this.TradeDate = TradeDate;
            this.Shares = Shares;
            this.CostPerShare = CostPerShare;
            this.State = State;
        }
    }
    public void addTrade(string code,string TradeDate,int Shares, float CostPerShare, bool State)
    {
        //이미 포트폴리오에 존재한 종목에 대한 매매
        if (tradeList.ContainsKey(code))
        {
            //매도인 경우
            if (State)
            {
                //보유 종목보다 많은 갯수인 경우
                if (stockInfo[code].shares < Shares)
                {
                    Debug.Log("보유종목보다 많이 매도할 수 없습니다.");
                }
                //보유 종목보다 작거나 같은 갯수인 경우(모든 물량을 매도한 경우에는 해당 종목에 대한 건물 배치를 하면 안됨)
                else
                {
                    //종목의 일일 거래 데이터 추가
                    tradeList[code].Add(new Trade(TradeDate, Shares, CostPerShare, State));

                    //개별종목의 보유 갯수와 전체 평단가 재계산하기(새로 매도한 금액을 이전 총 매도 금액에서 뺀 후 보유한 종목갯수로 나눔)
                    StockStat tmp = stockInfo[code];
                    if (tmp.shares == Shares)
                    {
                        tmp.avgCostPerShare = 0;
                    }
                    else
                    {
                        tmp.avgCostPerShare = (tmp.avgCostPerShare * tmp.shares - Shares * CostPerShare) / (tmp.shares - Shares);
                    }
                    tmp.shares -= Shares;
                    stockInfo[code] = tmp;
                    Cash += Shares * CostPerShare;
                }
            }
            //매수인 경우
            else
            {
                //입력한 자금이 부족하지 않은 경우에만 
                if (Cash >= Shares * CostPerShare)
                {
                    //종목의 일일 거래 데이터 추가
                    tradeList[code].Add(new Trade(TradeDate, Shares, CostPerShare, State));

                    //개별종목의 보유 갯수와 전체 평단가 재계산하기(새로 매수한 금액과 이전 총 매수 금액을 합산 후 보유한 종목갯수로 나눔)
                    StockStat tmp = stockInfo[code];
                    tmp.avgCostPerShare = (Shares * CostPerShare + tmp.avgCostPerShare * tmp.shares) / (tmp.shares + Shares);
                    tmp.shares += Shares;
                    stockInfo[code] = tmp;
                    Cash -= Shares * CostPerShare;
                }
                else
                {
                    Debug.Log("매수할 수 있는 자금이 부족합니다.");
                }
            }
        }
        //포트폴리오에 없는 새로운 종목에 대한 매매
        else
        {
            //매도인 경우
            if (State)
            {
                Debug.Log("포트폴리오에 없는 종목을 매도할 수 없습니다");
            }
            else//매수인 경우
            {
                //입력한 자금이 부족하지 않은 경우에만 
                if (Cash >= Shares * CostPerShare)
                {
                    tradeList.Add(code, new List<Trade>());

                    //종목에 대한 일일 거래 데이터 새로 추가
                    tradeList[code].Add(new Trade(TradeDate, Shares, CostPerShare, State));

                    //종목의 전체 갯수와 평단가 새로 추가
                    stockInfo.Add(code, new StockStat());
                    StockStat tmp = stockInfo[code];
                    tmp.shares = Shares;
                    tmp.avgCostPerShare = CostPerShare;
                    stockInfo[code] = tmp;
                    Cash -= Shares * CostPerShare;
                }
                else
                {
                    Debug.Log("매수할 수 있는 자금이 부족합니다.");
                }
            }
        }
    }
    public float updateGain(string code)
    {
        //해당 종목의 평가금액을 계산(시장가격 X 수량)
        return stockInfo[code].shares * list.apiInfo[code].api_marketprice;   
    }
    public void eraseData()
    {
        stockInfo.Clear();
        tradeList.Clear();
    }
}