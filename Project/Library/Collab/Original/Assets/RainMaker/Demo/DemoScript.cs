using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DigitalRuby.RainMaker
{
    public class DemoScript : MonoBehaviour
    {   
        public StockList list;//주식 api정보를 이용하기 위한 stockList
        private RaycastHit hit; //마우스에 클릭된 객체
        public RainScript RainScript;//날씨 제어를 위한 오브젝트
        public GameObject Sun;//낮/밤 제어를 위한 방향광 오브젝트
        public GameObject StockInfo; //종목정보UI 페이지
        public bool StockInfoMenuPopUp; //종목정보창이 띄워져있는지 확인하는 변수
        public Image StockPicture; //종목정보UI 페이지에 올릴 로고 이미지
        public Text infomation; //종목정보UI 페이지에 올릴 정보(시가,시가총액,전일종가 등)
        public Text thisSymbol; //화면에 띄울 종목이름 정보 텍스트 UI
        public Text SectorName; //화면에 띄울 섹터 정보 텍스트 UI
        private Vector3[] SectorPos = { new Vector3(-25.7f, 53f, 25.8f), new Vector3(17.4f, 53f, 25.8f), new Vector3(60.4f, 53f, 25.8f), new Vector3(-27.1f, 56.1f, -46.7f), new Vector3(17.4f, 56.1f, -46.7f), new Vector3(59.7f, 56.1f, -46.7f) };
        private string[] SectorNames = { "Industrial", "Consumer", "Health Care", "Financial", "IT", "Real Estate" };
        private int SectorIndex;
        public Toggle WeatherOpt;//날씨 반영 옵션
        public Toggle TimeOpt;//시간 연동 옵션

        private void Start()
        {
            SectorName.text = "";
            SectorIndex = 0;
            RainScript.RainIntensity = 0f;
            RainScript.EnableWind = true;
            StockInfoMenuPopUp = false;
            marketTimeCheck();
            UpdateRain();
        }
        // Update is called once per frame
        private void Update()
        {
            UpdateKeyboard();
            UpdateSectorName();
            clickCheck();
            optCheck();
        }
        void optCheck()
        {
            if (!TimeOpt.isOn) { Sun.transform.rotation = Quaternion.Euler(90f, 0.0f, 0.0f); }
            if (!WeatherOpt.isOn) { RainScript.RainIntensity = 0.0f; }
        }
        void clickCheck()
        {
            //종목 정보창 또는 서브메뉴창이 화면에 떠있는 경우 실행되지 않도록 리턴
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp || StockInfoMenuPopUp ) { return; }
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
                        //thisSymbol.text = tmpname;
                        settingStockInfo(tmpname);
                        /*
                        //클릭한 위치에 해당하는 평면을 생성(법선벡터 y축) 
                        Plane plane = new Plane(Vector3.up, Vector3.zero);
                        Vector3 v3Center = new Vector3(0.5f, 0.5f, 0.0f);
                        //카메라 평면에서 중앙에서 레이 발사
                        ray = Camera.main.ViewportPointToRay(v3Center);
                        float fDist;
                        //해당 평면에서 레이와 충돌한 경우
                        if (plane.Raycast(ray, out fDist))
                        {
                            //충돌한 지점의 좌표구함
                            Vector3 v3Hit = ray.GetPoint(fDist);
                            //카메라 중앙에서 발사되어 정사영된 점과 건물 오브젝트의 거리차이 계산
                            Vector3 v3Delta = hit.collider.gameObject.transform.position - v3Hit;
                            //해당하는 거리차이 만큼 카메라를 이동시킨다.(수정 필요)
                            //Camera.main.transform.Translate(v3Delta);
                            //Camera.main.orthographicSize = 5;
                        }
                        */
                    }
                    
                }
            }
        }
        public void SectorUpBtnClick()
        {
            SectorIndex = (SectorIndex + 1) % 6;
            SectorName.text = SectorNames[SectorIndex];
            Camera.main.transform.position = SectorPos[SectorIndex];
        }
        public void SectorDownBtnClick()
        {
            SectorIndex = (SectorIndex - 1);
            if (SectorIndex < 0) { SectorIndex = 5; }
            SectorName.text = SectorNames[SectorIndex];
            Camera.main.transform.position = SectorPos[SectorIndex];
        }
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
            for(int i = 0; i < 6; i++)
            {
                dist = Vector3.Distance(Camera.main.transform.position, SectorPos[i]);
                if (minDist > dist) {
                    minIdx = i;
                    minDist = dist;
                }
            }
            SectorName.text = SectorNames[minIdx];
            return;
        }
        private void settingStockInfo(string code)
        {
            //종목에 해당하는 로고 사진 종목 정보 페이지에 첨부하기
            StockPicture.sprite = Resources.Load("Sprites/" + code, typeof(Sprite)) as Sprite;
            //종목 정보 페이지에서 정보 띄움(이미지도 코드에 해당하는 기업정보로 자동 전달되기)
            infomation.text = "Code: " + code;
            infomation.text = "\nMarket Price: " + list.apiInfo[code].api_marketprice.ToString("F3") + "$";
            infomation.text += "\nPER: " + list.apiInfo[code].api_per.ToString("F3");
            infomation.text += "\nSector: " + list.apiInfo[code].api_sector;
            infomation.text += "\nMarket Cap: " + list.apiInfo[code].api_marketcap.ToString("F3");
            infomation.text += "\n52week: " + list.apiInfo[code].api_52week;
            infomation.text += "\nPrevious Close: " + list.apiInfo[code].api_preclose.ToString("F3") + "$";
        }
        private void UpdateRain()
        {
            //전날 종가에 대한 현재 시장가의 평균 변화율이 양수인 경우 화창
            if (marketMoodCheck()) { RainScript.RainIntensity = 0.0f; }
            //전날 종가에 대한 현재 시장가의 평균 변화율이 음수인 경우 비
            else { RainScript.RainIntensity = 0.5f; }
        }
        private void UpdateKeyboard()
        {
            float speed = 10.0f * Time.deltaTime;
            float XSpeed = 0f;
            float YSpeed = 0f;
            
            if (Input.GetKey(KeyCode.W)){YSpeed = speed;}
            else if (Input.GetKey(KeyCode.S)){YSpeed = -speed;}
            else if (Input.GetKey(KeyCode.A)){XSpeed = -speed;}
            else if (Input.GetKey(KeyCode.D)){XSpeed = speed;}
            else if (Input.GetKey(KeyCode.Q)){ Camera.main.orthographicSize = 6; }
            else if (Input.GetKey(KeyCode.E)) { Camera.main.orthographicSize = 13; }
            Camera.main.transform.Translate(XSpeed, YSpeed, 0.0f);
        }

        private void marketTimeCheck()
        {
            //미국 시장 기준으로 23:30~6:00까지 열림
            //장이 열린 시간에는 낮시간으로 장마감 이후는 밤시간으로 표현
            string h = DateTime.Now.ToString(("HH"));
            int hour = Int32.Parse(h);
            if ((hour> 23) || (hour <7)) {Sun.transform.rotation = Quaternion.Euler(90f, 0.0f, 0.0f);}
            else{ Sun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); }
        }
        private bool marketMoodCheck()
        {
            float avgMood=0f;
            //전날 종가에 대한 현재 시장가의 평균 변화율 계산
            foreach (var tmp in list.apiInfo.Keys)
            {
                Debug.Log(tmp);
                Debug.Log(list.apiInfo[tmp].api_marketprice);
                Debug.Log(list.apiInfo[tmp].api_preclose);
                avgMood += (list.apiInfo[tmp].api_marketprice - list.apiInfo[tmp].api_preclose)/ list.apiInfo[tmp].api_preclose;
            }
            Debug.Log(avgMood/list.apiInfo.Count);
            //전날 종가에 대한 현재 시장가의 평균 변화율이 양수인지 반환
            return avgMood > 0;
        }
    }
}