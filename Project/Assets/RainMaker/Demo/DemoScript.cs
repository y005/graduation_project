using System;
using UnityEngine;
using System.Collections;

namespace DigitalRuby.RainMaker
{
    public class DemoScript : MonoBehaviour
    {   //주식 api정보를 이용하기 위한 stockList
        public StockList list;
        //날씨 제어를 위한 오브젝트
        public RainScript RainScript;
        //낮/밤 제어를 위한 오브젝트
        public GameObject Sun;

        // Use this for initialization
        private void Start()
        {
            RainScript.RainIntensity = 0f;
            RainScript.EnableWind = true;
            marketTimeCheck();
            UpdateRain();
        }
        // Update is called once per frame
        private void Update()
        {
            UpdateMovement();
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