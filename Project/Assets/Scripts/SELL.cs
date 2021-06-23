using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SELL : MonoBehaviour
{
    public void Sell()
    {
 /*       try
        {
            //Text 입력란에 적혀있는 종목 코드명과 수량을 가져옵니다.
            string symbol = codeText.text;
            string count = cntText.text;
            string cost = costText.text; //사용자가 입력한 매도 금액(1주당) 정보

            Int32.TryParse(count, out int mycnt); //mycnt = 매도 수량
            Int32.TryParse(cost, out int mycost); //mycost = 매도 금액(1주당)
            float myprice = mycnt * mycost; //myprice = 총 매도 금액

            await BeginNetwork(symbol); //API 정보 불러오기
            float market_price = apiInfo[symbol].api_marketprice;

            //이미 넣은 종목이라면 기존의 정보 수정
            if (myStocks.ContainsKey(symbol))
            {
                //총 보유 수량보다 매도 수량이 더 크다면
                if (myStocks[symbol].c_count < mycnt)
                {
                    Debug.Log("보유하고 있는 수량보다 더 많이 매도할 수 없습니다.");
                }
                else
                {
                    myStocks[symbol].c_count -= mycnt; //총 보유 수량
                    myStocks[symbol].c_cost -= myprice; //총 매도 금액
                    myStocks[symbol].c_myavg = myStocks[symbol].c_cost / myStocks[symbol].c_count; //평균 매수 금액(1주당)
                    myStocks[symbol].c_marketprice = myStocks[symbol].c_count * market_price; //평가 금액
                    totalAsset -= myprice;

                    addStock = true;
                }

                //DEBUG
                Debug.Log("symbol : " + symbol + "   총 보유 수량 : " + myStocks[symbol].c_count + "   총 매수 금액 : " + myStocks[symbol].c_cost
                   + "   평균 매수 금액: " + myStocks[symbol].c_myavg + "   평가 금액 : " + myStocks[symbol].c_marketprice
                   + "   수익률 : " + (market_price / myStocks[symbol].c_myavg - 1) * 100 + "%");


                //더이상 해당 종목을 보유하고 있지 않을 경우
                if (myStocks[symbol].c_count == 0)
                {
                    myStocks.Remove(symbol); //딕셔너리에서 정보 삭제
                    apiInfo.Remove(symbol); //딕셔너리에서 정보 삭제
                    Destroy(GameObject.Find(symbol)); //해당 종목 객체 삭제
                }
            }

            //새로운 종목이라면 매도 불가
            else
            {
                Debug.Log("보유하고 있는 종목의 정보를 입력해주세요");
            }

        }
        catch (Exception)
        {
            Debug.Log("제대로된 종목과 수량을 입력해주세요");
        }*/
    }
}
