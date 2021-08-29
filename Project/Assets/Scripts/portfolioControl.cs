using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class portfolioControl : MonoBehaviour
{
    public TextMeshProUGUI totalGain;//전체 수익 텍스트 UI
    public TextMeshProUGUI divGain; //배당 수익 텍스트 UI

    //edit페이지에 있는 입력필드
    public TMP_InputField cash;// 현금 입력
    public TMP_InputField code;// 종목코드 입력
    public TMP_InputField date;// 매매날짜 입력
    public TMP_InputField share;// 매매종목수 입력
    public TMP_InputField costPerShare;// 평단가 입력 
    
    //포트폴리오 모드에서 활성화시키는 오브젝트의 실행 스크립트입니다.
    public StockList list;//api주식 정보 저장 데이터
    public portfolio myPortfolio;//보유 종목 저장 데이터
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
                Debug.Log("올바른 값을 입력하세요.");
            }
        }
        else
        {
            Debug.Log("값을 입력하세요");
        }
        cash.text = "현금을 입력하세요";
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
                    Debug.Log("포트폴리오내 있는 자금보다 작거나 같은 값을 입력하세요");
                }
            }
            catch (FormatException fe)
            {
                Debug.Log("올바른 값을 입력하세요.");
            }
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
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
                Debug.Log("올바른 값을 입력하세요.");
            }
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
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
                Debug.Log("올바른 값을 입력하세요.");
            }
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
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
    //보유 종목 수정이 되면 인풋필드 값이 지워진다.
    void inputsClear()
    {
        code.text = "종목코드를 입력 하세요";
        date.text = "매매 날짜를 입력 하세요";
        share.text = "보유 수량을 입력 하세요";
        costPerShare.text = "평단가를 입력 하세요";
    }
}
