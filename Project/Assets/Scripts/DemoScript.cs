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

        public GameObject StockInfo; //종목정보UI 페이지
        public Image StockPicture; //종목 로고 이미지
        public Image SectorIcon; //섹터별 아이콘
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

        public Text timeAlarm; //개장 시간 정보

        private Vector3[] SectorPos = { new Vector3(-25.7f, 53f, 25.8f), new Vector3(17.1f, 55f, 27f), new Vector3(60.4f, 53f, 25.8f), new Vector3(-26f, 53f, -48f), new Vector3(18.5f, 56f, -45.3f), new Vector3(59.9f, 53f, -47f) };
        private string[] SectorNames = { "산업", "소비재", "헬스케어", "금융", "기술", "부동산" };
        Dictionary<string, string> dic = new Dictionary<string, string>(){{ "Technology","기술"},{ "Communication Services","기술"}, { "Real Estate" , "부동산"},
                                                                            { "Industrials","산업" },{ "Consumer Defensive","소비재" }, { "Consumer Cyclical","소비재" } ,
                                                                               { "Financial Services","금융"},{ "Healthcare", "헬스케어" } };
        bool isDay; //주식 시장 장 시간 저장 변수
        bool cameraMove; //카메라가 이동하는 지 확인하는 상태변수
        public bool cityView; //전체 주식 시장 뷰를 보는지 확인하는 상태변수
        
        private int SectorIndex;
        private bool posChange;//종목 위치 변경을 선택했는지 확인
        private string pos; //선택된 종목명 저장변수
        private string prePos; //위치를 바꾸고 싶은 종목명 저장 변수
        public TextMeshProUGUI posTxt; // 버튼 텍스트 

        private void Start()
        {
            SectorName.text = "";
            Camera.main.orthographicSize = 13;
            SectorIndex = 0;
            RainScript.RainIntensity = 0f;
            RainScript.EnableWind = true;
            cameraMove = false;
            cityView = false;
            posChange = false;
            isDay = marketTimeCheck();
            //전날 대비 포트폴리오 평가금액 변화에 따라 날씨 제어(데이터 로드시 확인)
        }
        // Update is called once per frame
        private void FixedUpdate()
        {
            UpdateKeyboard();
            UpdateSectorName();
            clickCheck();
            optCheck();
            marketTimeCheck();
            if (cameraMove)
            {
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
        }
        void clickCheck()
        {
            //다른 창이 화면에 떠있는 경우 실행되지 않도록 리턴
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp) { return; }
            //클릭한 객체 이름 출력
            if (Input.GetMouseButtonDown(0))
            {
                //클릭 인식범위에서 벗어나는 경우 바로 반환
                if ((Input.mousePosition.x < 300) || (Input.mousePosition.x > 1600) || (Input.mousePosition.y < 300) || (Input.mousePosition.y > 900)) { return; }
                string tmpname = "";
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("stock"))//건물 오브젝트를 클릭한 경우
                    {
                        pos = hit.collider.gameObject.transform.name;
                        StockInfo.SetActive(true);
                        GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp = true;
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
            GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp = false;
        }
        //종목 위치를 변경하는 버튼
        public void posChangeBtnClick()
        {
            Vector3 tmp;
            if (!posChange)//위치 변경할 한 개의 건물을 선택한 경우
            {
                prePos = pos;
                posTxt.text = "이 위치와 변경";
                posChange = true;
            }
            else//위치 변경할 두 개의 건물을 선택한 경우
            {
                tmp = GameObject.Find(pos).gameObject.transform.position;
                //두 개의 위치를 서로 바꾼다.
                GameObject.Find(pos).gameObject.transform.position = GameObject.Find(prePos).gameObject.transform.position;
                GameObject.Find(prePos).gameObject.transform.position = tmp;
                posTxt.text = "종목 위치 변경";
                posChange = false;
            }
        }
        private void UpdateSectorName()
        {
            //카메라가 이동하면 섹터 정보를 표시하지 않는다
            if (cameraMove) { SectorName.text = ""; return; }
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
            //섹터 아이콘 변경
            SectorIcon.sprite = Resources.Load("Prefabs/Sector Sign/ic_" + dic[list.apiInfo[code].api_sector], typeof(Sprite)) as Sprite;

            stockMarketPrice.text = "현재 주가: $" + list.apiInfo[code].api_marketprice.ToString("F2");
            stockPreviousClose.text = "전날 종가: $" + list.apiInfo[code].api_preclose.ToString("F2");
            stockPer.text = "주가 수익 비율: " + list.apiInfo[code].api_per.ToString("F2");
            stockSector.text = "산업군: " + dic[list.apiInfo[code].api_sector];
            stock52Week.text = "52주 간 변화율: " + list.apiInfo[code].api_52week.ToString("F2") + "%";

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
                stockTotal.text = "평가 금액: $" + myPortfolio.updateGain(code);
                //배당 정보가 유효한 경우에만 정보 표시하기
                string result = divDate(code);
                if (result.Length > 0)
                {
                    stockDiv.text = "배당금: $" + dividend(code);
                    stockDivDate.text = "예정 배당일: " + result;
                }
            }
        }
        private void UpdateKeyboard()
        {
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp) { return; }
            float speed = 10.0f * Time.deltaTime;
            float XSpeed = 0f;
            float YSpeed = 0f;

           if (Input.GetKey(KeyCode.Q))
            {
                Camera.main.orthographicSize = 13;
                cityView = false;
            }
            else if (Input.GetKey(KeyCode.E))
            { //시티 뷰일 경우 카메라 시점 고정과 카메라 움직임 X
                Camera.main.orthographicSize = 45;
                Camera.main.transform.position = new Vector3(31.3f, 67.7f, -26.2f);
                cityView = true;
            }
            if (cityView) { return; }
            else
            {
                if (Input.GetKey(KeyCode.W)) { YSpeed = speed; }
                else if (Input.GetKey(KeyCode.S)) { YSpeed = -speed; }
                else if (Input.GetKey(KeyCode.A)) { XSpeed = -speed; }
                else if (Input.GetKey(KeyCode.D)) { XSpeed = speed; }
                Camera.main.transform.Translate(XSpeed, YSpeed, 0.0f);
            }
        }
        private bool marketTimeCheck()
        {
            //미국 시장 기준으로 23:30~6:00까지 열림

            DateTime now = DateTime.Now; //현재 시간
            DateTime openTime = new DateTime(now.Year, now.Month, now.Day, 23, 30, 00); //개장 : 23:30
            DateTime closeTime = new DateTime(now.Year, now.Month, now.Day, 06, 01, 00); //마감 : 6:01
            TimeSpan timer; //남은 시간

            string ck = "";

            //Debug.Log("현재시간 : " + now.Hour + "시 " + now.Minute + "분");

            //시간 체크
            if (now.Hour == 6)
            {
                if (now.Minute == 0) { ck = "open"; }
                else { ck = "close"; }
            }
            else if (now.Hour == 23)
            {
                if (now.Minute >= 30)
                {
                    closeTime = closeTime.AddDays(1);
                    ck = "open";
                }
                else { ck = "close"; }
            }
            else
            {
                if ((now.Hour >= 7) && (now.Hour <= 22)) { ck = "close"; }
                else { ck = "open"; }
            }

            //text setting
            if (ck.Equals("open"))
            {
                timer = closeTime - now;
                //Debug.Log("남은 시간 : " + timer.ToString(@"hh\:mm\:ss"));
                timeAlarm.text = "마감까지 " + timer.ToString(@"hh\:mm\:ss");

                return true;
            }
            else
            {
                timer = openTime - now;
                //Debug.Log("남은 시간 : " + timer.ToString(@"hh\:mm\:ss"));
                timeAlarm.text = "개장까지 " + timer.ToString(@"hh\:mm\:ss");

                return false;
            }

        }
        public string divDate(string code)
        {
            int compareDate; //날짜 비교 결과 저장 변수
            DateTime today = DateTime.Now; //오늘 날짜
            DateTime divDate; //배당 날짜

            if (!list.apiInfo.ContainsKey(code)) { return ""; }
            string tmp = list.apiInfo[code].api_divDate; //배당 날짜 불러오기

            try
            {
                 divDate = Convert.ToDateTime(tmp); //배당 날짜 - 날짜 타입으로 변환
                 compareDate = DateTime.Compare(today, divDate); //오늘 날짜와 배당 날짜 비교

                 //아직 다음 배당 날짜가 지나지 않았다면
                 if (compareDate <= 0)
                 {
                     return divDate.ToString("yyyy-MM-dd");
                 }

                  //이미 배당 날짜가 지났을 때
                  else
                  {
                      return "";
                  }
            }
            // 배당주가 아닐 때
            catch (FormatException fe)
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
        public float totalGainSet()
        {
            float sum = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                sum += myPortfolio.updateGain(key);
            }
            return sum;
        }
        //배당익 합산
        public float divGainSet()
        {
            float sum = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                sum += dividend(key);
            }
            return sum;
        }
    }
}