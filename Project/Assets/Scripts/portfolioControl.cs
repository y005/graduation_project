using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class portfolioControl : MonoBehaviour
{
    public Text totalGain;//전체 수익 텍스트 UI
    public Text divGain;//배당 수익 텍스트 UI

    //edit페이지에 있는 입력필드
    public InputField cash;// 현금 입력
    public InputField code;// 종목코드 입력
    public InputField date;// 매매날짜 입력
    public InputField share;// 매매종목수 입력
    public InputField costPerShare;// 평단가 입력 
    
    //포트폴리오 모드에서 활성화시키는 오브젝트의 실행 스크립트입니다.
    public StockList list;//api주식 정보 저장 데이터
    public portfolio myPortfolio;//보유 종목 저장 데이터
    //포트폴리오의 산업별 종목 갯수
    public Dictionary<string, int> sectorCnt = new Dictionary<string, int>();

    public GameObject[] totalMode;

    void Start()
    {
        myPortfolio.renew =false;//포트폴리오 갱신 플래그 false
        //산업별 보유 종목 갯수 초기화
        sectorCnt.Add("Health Care", 0);
        sectorCnt.Add("Financial", 0);
        sectorCnt.Add("Real Estate", 0);
        sectorCnt.Add("IT", 0);
        sectorCnt.Add("Consumer", 0);
        sectorCnt.Add("Industrial", 0);
    }
    // Update is called once per frame
    void Update()
    {
        //포트폴리오가 갱신된 경우 객체 배치 반영
        /*if (myPortfolio.renew)
        {
           settingPortfolio();
            myPortfolio.renew = false;
        }*/
    }

    //자신의 보유 종목을 객체로 배치하기
    void settingPortfolio()
    {
        float total = 0;//전체 주식평가금액 저장변수
        float myinvest = 0; //전체 투자금 저장변수
        string path = "";//프리팹 로드 경로
        //float x = 0, z = 0;
        //int loc = 0;//건물 객체를 설치할 y좌표

        //"myStock" 태그가 달린 객체(포트폴리오 배치된 건물)를 전부 삭제(갱신이 될때 마다 반복)
        foreach(GameObject tmp in myPortfolio.myStocks){Destroy(tmp);}
        myPortfolio.myStocks.Clear();

        //섹터 갯수를 위한 카운트 저장 초기화
        foreach (var key in sectorCnt.Keys.ToList())
        {
            sectorCnt[key] = 0;
        }
        //전체 평가금액 계산
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //개별 종목의 보유수량이 0개인 경우 카운트에서 제외
            if (myPortfolio.stockInfo[key].shares==0) { continue; }
            total += myPortfolio.updateGain(key);
        }
        //나의 전체 투자금액 계산
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            myinvest += myPortfolio.stockInfo[key].shares * myPortfolio.stockInfo[key].avgCostPerShare;
        }
        //전체 수익금 = 전체 평가금액 - 나의 전체 투자금액
        //totalGain.text = (total - myinvest).ToString();

        //해당하는 섹터 위치에 차례대로 일정 간격으로 정렬시키기
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //기존 건물 위치 반환
            Vector3 pos;
            pos = totalMode[0].transform.Find(key).position;

            //개별 종목의 보유수량이 0개인 경우 배치에서 제외
            /*if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            if (list.apiInfo[key].api_sector.Equals("Technology")) { x = -8.7f; z = -0.7f; }
            else if (list.apiInfo[key].api_sector.Equals("Communication Services")) { x = 0.2f; z = -0.7f; }
            else if (list.apiInfo[key].api_sector.Equals("Consumer Cyclical")) { x = 0.2f; z = 8f; }
            else if (list.apiInfo[key].api_sector.Equals("Financial Services")) { x = -8.7f; z = 8f; }*/

            //loc = ++sectorCnt[list.apiInfo[key].api_sector];

            //건물 설치
            path = "Prefabs/Buildings/" + key;      
            GameObject a = (GameObject)Instantiate(Resources.Load(path));
            a.name = key;
            a.gameObject.tag = "myStock";
            myPortfolio.myStocks.Add(a);

            //기존과 같은 위치에 건물 배치
            a.transform.position = new Vector3(pos.x, pos.y, pos.z);
            
            //자산 대비 스케일 비율 정하기(1~3단계)
            float ratio = myPortfolio.updateGain(key) / total * 100; //전체 평가금액 중 해당 종목 평가금액 비율
            float scale = 1f;
            if (ratio < 30) { scale = 0.75f; }
            else if (ratio >= 30 && ratio < 70) { scale = 1f; }
            else if(ratio >= 70 && ratio <= 100){ scale = 1.25f; }
            
            //단계에 따라 정해진 비율만큼 객체 스케일 조정하기
            a.transform.localScale = new Vector3(scale * a.transform.localScale.x, scale * a.transform.localScale.y, scale * a.transform.localScale.z);        
        }
    }
    public void CashPlusBtnClick()
    {
        if (checkCashEditInput())
        {
            myPortfolio.Cash += float.Parse(cash.text);
      		Debug.Log("cash = " + myPortfolio.Cash);
            cash.text = "";
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
        }
    }

    public void CashMinusBtnClick()
    {
        if (checkCashEditInput())
        {
            if (myPortfolio.Cash >= float.Parse(cash.text)) {
                myPortfolio.Cash -= float.Parse(cash.text);
               inputsClear();
            }
            else
            {
                Debug.Log("포트폴리오내 있는 자금보다 작거나 같은 값을 입력하세요");
            }
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
        }
    }

    public void buyBtnClick()
    {
        Debug.Log("BUY button click");
        if (checkStockEditInput())
        {
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), false);
            inputsClear();
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
        }
    }

    public void sellBtnClick()
    {
        Debug.Log("SELL button click");
        if (checkStockEditInput())
        {
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), true);
            inputsClear();
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
        }
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
        code.text = "";
        date.text = "";
        share.text = "";
        costPerShare.text = "";
    }
}
