using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class portfolioControl : MonoBehaviour
{
    public Text totalGain;//전체 수익 텍스트 UI
    public Text divGain;//배당 수익 텍스트 UI

    //edit페이지에 있는 입력필드
    public Text cash;// 현금 입력
    public Text code;// 종목코드 입력
    public Text date;// 매매날짜 입력
    public Text share;// 매매종목수 입력
    public Text costPerShare;// 평단가 입력 
   
    //포트폴리오 모드에서 활성화시키는 오브젝트의 실행 스크립트입니다.
    public StockList list;//api주식 정보 저장 데이터
    public portfolio myPortfolio;//보유 종목 저장 데이터
    //포트폴리오의 산업별 종목 갯수
    public Dictionary<string, int> sectorCnt = new Dictionary<string, int>();
    void Start()
    {
        //산업별 보유 종목 갯수 초기화
        sectorCnt.Add("Technology", 0);
        sectorCnt.Add("Communication Services", 0);
        sectorCnt.Add("Consumer Cyclical", 0);
        sectorCnt.Add("Financial Services", 0);
    }
    // Update is called once per frame
    void Update()
    {
        //포트폴리오가 갱신된 경우 객체 배치 반영
        if (myPortfolio.renew)
        {
            settingPortfolio();
            myPortfolio.renew = false;
        }
    }

    //자신의 보유 종목을 객체로 배치하기
    void settingPortfolio()
    {
        //전체 주식평가금액 저장변수
        float total = 0;
        string path = "";
        float x = 0, z = 0;
        int loc = 0;

        //"myStock" 태그가 달린 객체(포트폴리오 배치된 건물)를 전부 삭제(갱신이 될때 마다 반복)
        foreach(GameObject tmp in myPortfolio.myStocks){
            Destroy(tmp);
        }
        myPortfolio.myStocks.Clear();
        //섹터 갯수를 위한 카운트 저장 초기화
        foreach (string tmp in sectorCnt.Keys)
        {
            sectorCnt[tmp] = 0;
        }
        //전체 평가금액 계산 UI에 표시
        foreach (string key in myPortfolio.stockInfo.Keys)
        {
            //개별 종목의 보유수량이 0개인 경우 카운트에서 제외
            if (myPortfolio.stockInfo[key].shares==0) { continue; }
            total += myPortfolio.updateGain(key);
            Debug.Log(total);
        }
        totalGain.text = total.ToString();
        //해당하는 섹터 위치에 차례대로 일정 간격으로 정렬시키기
        foreach (string key in myPortfolio.stockInfo.Keys)
        {
            //개별 종목의 보유수량이 0개인 경우 배치에서 제외
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            if (list.apiInfo[key].api_sector.Equals("Technology"))
            {
                x = -8.7f;
                z = -0.7f;
            }
            else if (list.apiInfo[key].api_sector.Equals("Communication Services"))
            {
                x = 0.2f;
                z = -0.7f;
            }
            else if (list.apiInfo[key].api_sector.Equals("Consumer Cyclical"))
            {
                x = 0.2f;
                z = 8f;
            }
            else if (list.apiInfo[key].api_sector.Equals("Financial Services"))
            {
                x = -8.7f;
                z = 8f;
            }
            path = "Prefabs/Buildings/" + key;
            loc = ++sectorCnt[list.apiInfo[key].api_sector];
            GameObject a = (GameObject)Instantiate(Resources.Load(path));
            a.name = key;
            a.gameObject.tag = "myStock";
            myPortfolio.myStocks.Add(a);
            //한 행에 4칸을 배치를 계획한 배치 
            a.transform.position = new Vector3(x + (loc % 4) * 2.5f, 2, z - (loc / 4) * 2f);
            //자산 대비 스케일 비율 정하기(1~4단계로 비율 정함)
            float ratio = myPortfolio.updateGain(key) / total;
            float scale = 1f;
            if (ratio < 0.25){ scale = 0.75f; }
            else if (ratio < 0.5){ scale = 1f; }
            else if (ratio < 0.75){ scale = 1.25f; }
            else if (ratio < 1){ scale = 1.75f; }
            //단계에 따라 정해진 비율만큼 객체 스케일 조정하기
            a.transform.localScale = new Vector3(scale * a.transform.localScale.x, scale * a.transform.localScale.y, scale * a.transform.localScale.z);
        }
    }
    public void CashPlusBtnClick()
    {
        if (checkCashEditInput())
        {
            Debug.Log(float.Parse(cash.text));
            myPortfolio.Cash += float.Parse(cash.text);
        }
        else{ Debug.Log("모든 입력 필드에 값을 입력하세요");}
    }

    public void CashMinusBtnClick()
    {
        if (checkCashEditInput())
        {
            if (myPortfolio.Cash >= float.Parse(cash.text)) {myPortfolio.Cash -= float.Parse(cash.text);}
            else{Debug.Log("포트폴리오내 있는 자금보다 작거나 같은 값을 입력하세요");}
        }
        else{Debug.Log("모든 입력 필드에 값을 입력하세요");}
    }

    public void buyBtnClick()
    {
        Debug.Log(code.text);
        Debug.Log(date.text);
        Debug.Log(Int32.Parse(share.text));
        Debug.Log(float.Parse(costPerShare.text));
        
        if (checkStockEditInput())
        {
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text),false);
        }
        else
        {
            Debug.Log("모든 입력 필드에 값을 입력하세요");
        }
    }
    public void sellBtnClick()
    {
        Debug.Log(code.text);
        Debug.Log(date.text);
        Debug.Log(Int32.Parse(share.text));
        Debug.Log(float.Parse(costPerShare.text));

        if (checkStockEditInput()){
            myPortfolio.addTrade(code.text, date.text, Int32.Parse(share.text), float.Parse(costPerShare.text), true);
        }
        else{Debug.Log("모든 입력 필드에 값을 입력하세요");}
    }

    bool checkStockEditInput()
    {
        Text[] stockInputs = {cash,date,share,costPerShare};
        for(int i = 0; i < stockInputs.Length; i++){if(stockInputs[i].text == ""){return false;}}
        return true;
    }

    bool checkCashEditInput()
    {
        if (cash.text == ""){ return false; }
        return true;
    }
}
