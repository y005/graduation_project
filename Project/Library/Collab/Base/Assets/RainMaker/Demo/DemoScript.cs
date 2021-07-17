using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DigitalRuby.RainMaker
{
    public class DemoScript : MonoBehaviour
    {   
        public StockList list;//주식 api정보를 이용하기 위한 stockList
        public RainScript RainScript;//날씨 제어를 위한 오브젝트
        private RaycastHit hit; //마우스에 클릭된 객체
        public GameObject Sun;//낮/밤 제어를 위한 방향광 오브젝트
        public GameObject StockInfo; //종목정보창
        public bool StockInfoMenuPopUp; //객체정보창이 띄워져있는지 확인하는 변수
        public Text infomation; //종목정보창에 올릴 정보
        public Text thisSymbol; //화면에 띄울 종목명

        private void Start()
        {
            RainScript.RainIntensity = 0f;
            RainScript.EnableWind = true;
            StockInfoMenuPopUp = false;
            marketTimeCheck();
            UpdateRain();
        }
        // Update is called once per frame
        private void Update()
        {
            UpdateMovement();
            clickCheck();
        }
        void clickCheck()
        {
            //종목 정보창 또는 서브메뉴창이 화면에 떠있는 경우 실행되지 않도록 리턴
            if (GameObject.Find("InGameControl").GetComponent<InGameControl>().subMenuPopUp || StockInfoMenuPopUp ) { return; }
            //클릭한 객체 이름 출력
            if (Input.GetMouseButtonDown(0))
            {
                string tmpname = "";
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("stock"))//건물 오브젝트를 클릭한 경우
                    {
                        StockInfo.SetActive(true);
                        StockInfoMenuPopUp = true;
                        tmpname = hit.collider.gameObject.name;
                        thisSymbol.text = tmpname;
                        settingStockInfo(tmpname);
                        /*Debug.Log("배당일 = " + list.apiInfo[tmpname].api_divDate
                                 + "  배당률 = " + list.apiInfo[tmpname].api_divRate
                                 + "  sector = " + list.apiInfo[tmpname].api_sector
                                 + "  시가총액 = " + list.apiInfo[tmpname].api_marketcap
                                 + "  PER = " + list.apiInfo[tmpname].api_per
                                 + "  52week = " + list.apiInfo[tmpname].api_52week
                                 + "  previous close = " + list.apiInfo[tmpname].api_preclose);

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
        public void ExitBtnClick()
        {
            StockInfo.SetActive(false);
            StockInfoMenuPopUp = false;
        }
        private void settingStockInfo(string code)
        {
            //객체 정보창에서 정보 띄움(이미지도 코드에 해당하는 기업정보로 자동 전달되기)
            infomation.text = "Market Price: " + list.apiInfo[code].api_marketprice.ToString("F3") + "$\n";
            infomation.text += "\nPER: " + list.apiInfo[code].api_per.ToString("F3");
            /*"배당일 = "list.apiInfo[code].api_divDate + "  배당률 = " + list.apiInfo[tmpname].api_divRate
            + "  sector = " + list.apiInfo[code].api_sector
            + "  시가총액 = " + list.apiInfo[code].api_marketcap
            + "  PER = " + list.apiInfo[code].api_per
            + "  52week = " + list.apiInfo[code].api_52week
            + "  previous close = " + list.apiInfo[code].api_preclose);*/
        }
        private void UpdateRain()
        {
            //전날 종가에 대한 현재 시장가의 평균 변화율이 양수인 경우 화창
            if (marketMoodCheck()) { RainScript.RainIntensity = 0.0f; }
            //전날 종가에 대한 현재 시장가의 평균 변화율이 음수인 경우 비
            else { RainScript.RainIntensity = 0.5f; }
        }
        private void UpdateMovement()
        {
            float speed = 10.0f * Time.deltaTime;
            float XSpeed = 0f;
            float YSpeed = 0f;
            
            if (Input.GetKey(KeyCode.W)){YSpeed = speed;}
            else if (Input.GetKey(KeyCode.S)){YSpeed = -speed;}
            else if (Input.GetKey(KeyCode.A)){XSpeed = -speed;}
            else if (Input.GetKey(KeyCode.D)){XSpeed = speed;}
            Camera.main.transform.Translate(XSpeed, YSpeed, 0.0f);
        }
        public void CameraZoomIn()
        {
            Camera.main.orthographicSize = 5;
        }
        public void CameraZoomOut()
        {
            Camera.main.orthographicSize = 10;
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