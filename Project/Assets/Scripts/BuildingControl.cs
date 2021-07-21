using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class BuildingControl : MonoBehaviour
{
    //주식 api정보를 이용하기 위한 stockList
    public StockList list;
    public portfolio myPortfolio; //포트폴리오 정보 저장자료
    public Toggle myStockOpt; //내 종목만 표시하는 bool변수
    //최대 밝기 저장 변수
    //int lightLevel = 0;
    //프레임수 저장 변수
    //int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        //예제 객체의 이름을 코드로 해야 해당하는 주식정보를 읽어올 수 있습니다.
        //Debug.Log(list.apiInfo[transform.name].api_marketprice);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.name);
        //lightControl();
        //함수 내부에서 myPortfolio.renew = false로 바꿔야 스케일 조정과 조명이 적용이 된다.(순차적 실행이 안되는듯)
        if (myPortfolio.renew){
            updateBuilding();
        }
    }
    void updateBuilding()
    {
        //보유종목인 경우 조명을 키고 평가금액의 비중에 따라 스케일링 작업 
        if (myPortfolio.stockInfo.ContainsKey(this.gameObject.name))
        {
            //수량이 0인 경우 보유 종목이 아니기 때문에 넘어감
            if (myPortfolio.stockInfo[this.gameObject.name].shares == 0) { myPortfolio.renew = false; return; }

            transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 3f;
            //전체 평가금액 계산
            float total = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                //개별 종목의 보유수량이 0개인 경우 카운트에서 제외
                if (myPortfolio.stockInfo[key].shares == 0) { continue; }
                total += myPortfolio.updateGain(key);
            }
            //자산 대비 스케일 비율 정하기(1~3단계)
            float ratio = myPortfolio.updateGain(this.gameObject.name) / total * 100; //전체 평가금액 중 해당 종목 평가금액 비율
            float scale = 1f;
            if (ratio < 30) { scale = 0.75f; }
            else if (ratio >= 30 && ratio < 70) { scale = 1f; }
            else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }
            Debug.Log(myPortfolio.updateGain(this.gameObject.name));
            Debug.Log(ratio);
            //단계에 따라 정해진 비율만큼 객체 스케일 조정하기
            transform.localScale = new Vector3(scale * transform.localScale.x, scale * transform.localScale.y, scale * transform.localScale.z);
            myPortfolio.renew = false;
        }
    }

    void lightControl()
    {
        //업데이트 함수는 1초에 60번 정도로 호출되기 때문에(60프레임)
        //0.5초 주기로 밝기를 변화하기 위해서는 카운트 변수를 따로 만들고 확인
        /*if (frame == 0)
        {
            lightLevel = (lightLevel + 1) % 2;
        }
        transform.GetChild(0).gameObject.GetComponent<Light>().intensity = lightLevel;
        frame = (frame + 1) % 60;*/

        //시가 변화율에 따라 조명 on/off
        //float change = (list.apiInfo[transform.name].api_marketprice - list.apiInfo[transform.name].api_preclose) / list.apiInfo[transform.name].api_marketprice * 100;
        //if (change >= 0) { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 15; } //전날대비 +
        //else { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0; } //전날대비 -
    }
}
