using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class BuildingControl : MonoBehaviour
{
    public StockList list;//주식 api정보를 이용하기 위한 stockList
    public portfolio myPortfolio; //포트폴리오 정보 저장자료
    private Camera cam; //게임 화면 카메라
    private GameObject effect; //이벤트 조건별 발생하는 이펙트UI
    private bool apiFlag; //api정보 갱신 확인

    // Start is called before the first frame update
    void Start()
    {
        apiFlag = true;

        //게임 오브젝트(카메라,UI)들 연결작업 실행
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject;
        effect.SetActive(true);
        effectOff();
    }

    void Update()
    {
        //종목에 대한 api요청 정보가 있는 경우 조건 확인 후 UI 표시
        if (list.apiInfo.ContainsKey(transform.name)) { settingUI(); }
        if (myPortfolio.renew) { checkCloseDiv(); }
        checkLayer();
    }
    void checkLayer()
    {
        Material[] mats = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
        //레이어를 옵션이 켜진 경우 보유 종목이 아닌지 확인하고 레이어를 켠다
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().layerFlag)
        {
            //보유 종목이 아닌 경우 레이어를 켠다
            if (!myPortfolio.stockInfo.ContainsKey(this.gameObject.name))
            {
                for (int j = 0; j < mats.Length; j++)
                {
                    Color tempcolor;
                    tempcolor = mats[j].color;
                    tempcolor.a = 0.5f;
                    mats[j].color = tempcolor;
                }
            }
            else
            {
                //실제 보유 수량이 0인 경우 레이어를 켠다
                if (myPortfolio.stockInfo[this.gameObject.name].shares == 0)
                {
                    for (int j = 0; j < mats.Length; j++)
                    {
                        Color tempcolor;
                        tempcolor = mats[j].color;
                        tempcolor.a = 0.5f;
                        mats[j].color = tempcolor;
                    }
                }
                //보유 수량이 1개 이상인 경우 레이어를 끈다.
                else
                {
                    for (int j = 0; j < mats.Length; j++)
                    {
                        Color tempcolor;
                        tempcolor = mats[j].color;
                        tempcolor.a = 0f;
                        mats[j].color = tempcolor;
                    }
                }
            }
        }   
        //레이어 옵션이 꺼진 경우 레이어를 끈다
        else
        {
            for (int j = 0; j < mats.Length; j++)
            {
                Color tempcolor;
                tempcolor = mats[j].color;
                tempcolor.a = 0f;
                mats[j].color = tempcolor;
            }
        }
    }
    //이펙트 발생 조건을 확인하고 UI 선택하는 함수
    void settingUI()
    {
        var wantedPos = cam.WorldToScreenPoint(transform.position);
        //종목의 상태 UI 배치하기
        for (int i=0;i< effect.transform.childCount; i++)
         {
            //카메라에서 보이는 종목에 대한 상태 UI 위치 재조정
            effect.transform.GetChild(i).gameObject.transform.position = new Vector3(wantedPos.x+20f, wantedPos.y + 375f, wantedPos.z);
            //API로 정보를 새로 가져온 경우에만 이펙트 발생 조건 확인하고 해당하는 상태 ui만 활성화
            if (!apiFlag) { continue; }
            //이펙트 UI 발생 조건 확인
            //1. 관심도 상위
            //2. 거래량 급등
            //3. 종가 대비 시가 변화 급등락
            float volumeChange = list.apiInfo[transform.name].api_volume - list.apiInfo[transform.name].api_avgVolume;
            float priceChange = 100f * (list.apiInfo[transform.name].api_marketprice - list.apiInfo[transform.name].api_preclose) / list.apiInfo[transform.name].api_preclose;
            switch (i)
            {
                case 0:
                    //관심도 지표 높은지 확인(유튜브 api 정보 연결해서 수정하기)
                    if (volumeChange == 0f) { effect.transform.GetChild(i).gameObject.SetActive(true); }
                    break;
                case 1:
                    //거래량 급등 확인(10일 평균 거래량 대비 증가한지 확인)
                    if(volumeChange > 0f) { effect.transform.GetChild(i).gameObject.SetActive(true); }
                    break;
                case 2:
                    //전날 종가 대비 시가 급등 확인
                    if (priceChange > 0f){effect.transform.GetChild(i).gameObject.SetActive(true);}
                    break;
                case 3:
                    //전날 종가 대비 시가 급락 확인 
                    if (priceChange < 0f){effect.transform.GetChild(i).gameObject.SetActive(true);}
                    break;
            }
         }
        if (apiFlag) { apiFlag = false; }
    }
    void effectOff()
    {
        for (int i = 0; i < effect.transform.childCount; i++)
        {
            effect.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void checkCloseDiv()
    {
        //포트폴리오 보유 종목 중 가장 가까운 배당일인 종목 이름과 게이지를 나타냄
    }
}
