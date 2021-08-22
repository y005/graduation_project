using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DigitalRuby.RainMaker
{
    public class DemoScript : MonoBehaviour
    {
        public portfolio myPortfolio; //포트폴리오 정보 저장자료
        public StockList list;//주식 api정보를 이용하기 위한 stockList
        private RaycastHit hit; //마우스에 클릭된 객체
        public RainScript RainScript;//날씨 제어를 위한 오브젝트
        public GameObject Sun;//낮/밤 제어를 위한 방향광 오브젝트
        public TextMeshProUGUI SectorName; //화면에 띄울 섹터 정보 텍스트 UI
        
        public TextMeshProUGUI divCode; //근접한 배당일을 가진 종목 코드 텍스트 UI 
        public TextMeshProUGUI totalGain; //평가 수익 텍스트 UI
        public TextMeshProUGUI divGain; //배당익 텍스트 UI
        public Slider divDate1;//근접한 배당일을 표시하는 게이지 

        public bool StockInfoMenuPopUp; //종목정보창이 띄워져있는지 확인하는 변수
        public GameObject StockInfo; //종목정보UI 페이지
        public Image StockPicture; //종목 로고 이미지
        public TextMeshProUGUI stockCode; //종목 코드
        public TextMeshProUGUI stockMarketPrice; // 종목 현재 시가
        public TextMeshProUGUI stockPreviousClose; // 종목 전날 종가
        public TextMeshProUGUI stockPer; // 종목 전날 종가
        public TextMeshProUGUI stockSector; // 종목의 섹터
        public TextMeshProUGUI stockMarketCap; // 종목의 시총
        public TextMeshProUGUI stock52Week; // 종목의 시총
        public TextMeshProUGUI stockShares; // 종목의 보유 수
        public TextMeshProUGUI stockTotal; // 총 평가 금액
        public TextMeshProUGUI stockDiv; // 예상 배당액
        public TextMeshProUGUI stockDivDate; // 배당 예정일 
        public GameObject divDate2;//종목 정보창의 배당일 게이지 오브젝트 

        private Vector3[] SectorPos = { new Vector3(-25.7f, 53f, 25.8f), new Vector3(17.4f, 53f, 25.8f), new Vector3(60.4f, 53f, 25.8f), new Vector3(-26f, 53f, -48f), new Vector3(18f, 53f, -48f), new Vector3(59.9f, 53f, -47f) };
        private string[] SectorNames = { "산업", "소비재", "헬스 케어", "경제", "IT", "부동산" };
        private int SectorIndex;
        bool isDay; //화창한지 저장한 상태변수
        public float isRain; //비의 세기를 저장한 변수
        bool cameraMove; //카메라가 이동하는 지 확인하는 상태변수

        public AudioSource moveBgm;


        private void Start()
        {
            SectorName.text = "";
            SectorIndex = 0;
            RainScript.RainIntensity = 0f;
            RainScript.EnableWind = true;
            StockInfoMenuPopUp = false;
            cameraMove = false;
            isDay = marketTimeCheck();
            //전날 대비 포트폴리오 평가금액 변화에 따라 날씨 제어(데이터 로드시 확인됨)
            isRain = 0f;
            totalGain.text = "0";
            divGain.text = "0";
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            UpdateKeyboard();
            UpdateSectorName();
            clickCheck();
            optCheck();
            if (cameraMove)
            {
                //이동하는 효과음이 실행되지 않고 있으면 실행한다.
                if (!moveBgm.isPlaying)
                {
                    //moveBgm.Play();
                }
                //이동이 완료되면 카메라 이동을 멈춘다.
                if (Vector3.Distance(Camera.main.transform.position, SectorPos[SectorIndex]) < 0.1f) { cameraMove = false; }
                //섹터 위치로 카메라를 서서히 이동
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, SectorPos[SectorIndex], 0.1f);
            }
        }
        //옵션 체크에 따라 제어되는 이펙트(날씨,시간)
        void optCheck()
        {
            if (!GameObject.Find("InGameControl").GetComponent<InGameControl>().marketTimeFlag) { Sun.GetComponent<Light>().intensity = 1f; }
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().marketTimeFlag)
            {
                if (isDay) { Sun.GetComponent<Light>().intensity = 1f; }
                else { Sun.GetComponent<Light>().intensity = 0f; }
            }
            if (!GameObject.Find("InGameControl").GetComponent<InGameControl>().weatherFlag) { RainScript.RainIntensity = 0.0f; }
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().weatherFlag)
            {
                //보유 종목의 수익률에 따라 비가 오도록 설정
                if (isRain > 0) { RainScript.RainIntensity = isRain; }
                else { RainScript.RainIntensity = 0.0f; }
            }
        }
        void clickCheck()
        {
            //종목 정보창 또는 서브메뉴창이 화면에 떠있는 경우 실행되지 않도록 리턴
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp || StockInfoMenuPopUp) { return; }
            //클릭한 객체 이름 출력
            if (Input.GetMouseButtonDown(0))
            {
                //Debug.Log(Input.mousePosition);
                //클릭 인식범위에서 벗어나는 경우 바로 반환
                if ((Input.mousePosition.x < 300) || (Input.mousePosition.x > 1600) || (Input.mousePosition.y < 300) || (Input.mousePosition.y > 900)) { return; }
                string tmpname = "";
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("stock"))//건물 오브젝트를 클릭한 경우
                    {
                        StockInfo.SetActive(true);
                        StockInfoMenuPopUp = true;
                        tmpname = hit.collider.gameObject.name;
                        settingStockInfo(tmpname);
                    }

                }
            }
        }
        public void SectorUpBtnClick()
        {
            //카메라가 이동 중이면 이동이 완료되기 전까지 버튼이 동작하지 않는다
            if (cameraMove) { return; }
            SectorIndex = (SectorIndex + 1) % 6;
            SectorName.text = SectorNames[SectorIndex];
            cameraMove = true;
        }
        public void SectorDownBtnClick()
        {
            //카메라가 이동 중이면 이동이 완료되기 전까지 버튼이 동작하지 않는다
            if (cameraMove) { return; }
            SectorIndex = (SectorIndex - 1);
            if (SectorIndex < 0) { SectorIndex = 5; }
            SectorName.text = SectorNames[SectorIndex];
            cameraMove = true;
        }
        //종목 정보창에서 나가는 버튼
        public void ExitBtnClick()
        {
            StockInfo.SetActive(false);
            StockInfoMenuPopUp = false;
        }
        private void UpdateSectorName()
        {
            float minDist = 100000000000;
            float dist = 0f;
            int minIdx = 0;
            //제일 가까운 위치에 있는 섹터이름 탐색
            for (int i = 0; i < 6; i++)
            {
                dist = Vector3.Distance(Camera.main.transform.position, SectorPos[i]);
                if (minDist > dist)
                {
                    minIdx = i;
                    minDist = dist;
                }
            }
            SectorName.text = SectorNames[minIdx];
            return;
        }
        private void settingStockInfo(string code)
        {

            //종목에 해당하는 로고 사진 첨부
            StockPicture.sprite = Resources.Load("Sprites/" + code, typeof(Sprite)) as Sprite;

            //종목 정보 페이지에서 띄울 텍스트 UI처리
            stockCode.text = code;
            stockMarketPrice.text = "";
            stockPreviousClose.text = "";
            stockPer.text = "";
            stockSector.text = "";
            stock52Week.text = "";
            stockMarketCap.text = "";
            stockShares.text = "";
            stockTotal.text = "";
            stockDiv.text = "";
            stockDivDate.text = "";

            if (!list.apiInfo.ContainsKey(code)) { return; }
            stockMarketPrice.text = "현재 주가: " + list.apiInfo[code].api_marketprice.ToString("F2") + "$";
            stockPreviousClose.text = "전날 종가: " + list.apiInfo[code].api_preclose.ToString("F2") + "$";
            stockPer.text = "주가 수익 비율: " + list.apiInfo[code].api_per.ToString("F2");
            stockSector.text = "산업군: " + list.apiInfo[code].api_sector;
            stock52Week.text = "52주 간 변화율: " + list.apiInfo[code].api_52week.ToString("F2")+"%";

            //시가총액 표기
            float tmp = list.apiInfo[code].api_marketcap;
            string marketcap = "";
            if (tmp >= 1000000000000) { marketcap = (tmp / 1000000000000).ToString("F2") + " T"; } //trillion
            else if (tmp >= 100000000000) { marketcap = (tmp / 100000000000).ToString("F2") + " B"; } //billion
            else { marketcap = tmp + ""; } //T, B 외의 단위 있다면 추가하기
            stockMarketCap.text = "시가 총액: " + marketcap;

            //자신의 보유한 종목 정보일 경우에 수량, 평가금액, 배당일, 예상 배당액 표시
            if (myPortfolio.stockInfo.ContainsKey(code))
            {
                if (myPortfolio.stockInfo[code].shares == 0) { return; }
                stockShares.text = "보유 수량: " + myPortfolio.stockInfo[code].shares;
                stockTotal.text = "평가 금액: " + myPortfolio.updateGain(code) + "$";
                //배당 정보가 유효한 경우에만 정보 표시하기
                string result = divDate(code);
                if (result.Length > 0)
                {
                    stockDiv.text = "배당금: " + dividend(code) + "$";
                    stockDivDate.text = "예정 배당일: " + result;
                }
            }
        }
        private void UpdateKeyboard()
        {
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp || StockInfoMenuPopUp) { return; }
            float speed = 10.0f * Time.deltaTime;
            float XSpeed = 0f;
            float YSpeed = 0f;

            if (Input.GetKey(KeyCode.W)) { YSpeed = speed; }
            else if (Input.GetKey(KeyCode.S)) { YSpeed = -speed; }
            else if (Input.GetKey(KeyCode.A)) { XSpeed = -speed; }
            else if (Input.GetKey(KeyCode.D)) { XSpeed = speed; }
            else if (Input.GetKey(KeyCode.Q)) { Camera.main.orthographicSize = 6; }
            else if (Input.GetKey(KeyCode.E)) { Camera.main.orthographicSize = 13; }
            Camera.main.transform.Translate(XSpeed, YSpeed, 0.0f);
        }
        private bool marketTimeCheck()
        {
            //미국 시장 기준으로 23:30~6:00까지 열림
            //장이 열린 시간에는 낮시간으로 장마감 이후는 밤시간으로 표현
            string h = DateTime.Now.ToString(("HH"));
            int hour = Int32.Parse(h);
            if ((hour > 23) || (hour < 7)) { return true; }
            else { return false; }
        }
        public string divDate(string code)
        {
            int compareDate; //날짜 비교 결과 저장 변수
            DateTime today = DateTime.Now; //오늘 날짜
            DateTime divDate; //배당 날짜

            if (!list.apiInfo.ContainsKey(code)) { return ""; }
            string tmp = list.apiInfo[code].api_divDate; //배당 날짜 불러오기

            //배당주일 때
            if (tmp != null)
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
                    return "";
                }
            }

            //배당주가 아닐 때
            else
            {
                return "";
            }
        }

        public float dividend(string code)
        {
            //보유 주식수에 따라 얻게되는 배당금 표시
            return myPortfolio.stockInfo[code].shares * list.apiInfo[code].api_divRate;
        }

        //총 평가 금액을 계산 
        public void totalGainSet()
        {
            float sum = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                sum += myPortfolio.updateGain(key);
            }
            totalGain.text = sum.ToString()+"$";
        }
        //배당익 합산
        public void divGainSet()
        {
            float sum = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                sum += dividend(key);
            }
            divGain.text = sum.ToString()+"$";
        }
    }
}