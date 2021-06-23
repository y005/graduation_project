using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUY : MonoBehaviour
{
    public void Buy()
    {
/*        try
        {
            //Text 입력란에 적혀있는 종목 코드명과 수량을 가져옵니다.
            string symbol = codeText.text;
            string count = cntText.text;
            string cost = costText.text; //사용자가 입력한 매수 금액(1주당) 정보

            Int32.TryParse(count, out int mycnt); //mycnt = 매수 수량
            Int32.TryParse(cost, out int mycost); //mycost = 매수 금액(1주당)
            float myprice = mycnt * mycost; //myprice = 총 매수 금액

            await BeginNetwork(symbol); //API 정보 불러오기
            float market_price = apiInfo[symbol].api_marketprice;

            //이미 넣은 종목이라면 기존의 정보 수정
            if (myStocks.ContainsKey(symbol))
            {
                myStocks[symbol].c_count += mycnt; //총 보유 수량
                myStocks[symbol].c_cost += myprice; //총 매수 금액
                myStocks[symbol].c_myavg = myStocks[symbol].c_cost / myStocks[symbol].c_count; //평균 매수 금액(1주당)
                myStocks[symbol].c_marketprice = myStocks[symbol].c_count * market_price; //평가 금액
                totalAsset += myprice;
            }

            //새로운 종목이라면 객체를 생성하고 배치한 후 종목정보를 배열에 저장
            else
            {    
                GameObject a = (GameObject)Instantiate(Resources.Load(path));

                //딕셔너리에 값 추가 (종목 객체/총 보유 수량/총 매수 금액/평균 매수 금액(1주당)/평가 금액)
                myStocks.Add(symbol, new StockData(a, mycnt, mycost * mycnt, mycost, market_price * mycnt));

                totalAsset += myprice; //총 자산 업데이트

                //3X3 격자구조 안에서 차례대로 새로운 종목 배치
                int x = 6 * ((myStocks.Count - 1) % 3) - 6;
                int z = -6 * ((myStocks.Count - 1) / 3) + 6;
                //생성된 객체를 랜덤하게 배치
                a.name = symbol;
                a.transform.position = new Vector3(x, 1, z);

                i++;
            }
            addStock = true;

            //DEBUG
            Debug.Log("symbol : " + symbol + "   총 보유 수량 : " + myStocks[symbol].c_count + "   총 매수 금액 : " + myStocks[symbol].c_cost
                + "   평균 매수 금액: " + myStocks[symbol].c_myavg + "   평가 금액 : " + myStocks[symbol].c_marketprice
                + "   수익률 : " + (market_price / myStocks[symbol].c_myavg - 1) * 100 + "%");
        }
        catch (Exception)
        {
            Debug.Log("제대로된 종목과 수량을 입력해주세요");
        }*/
    }
}
