using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    //주식 api정보를 이용하기 위한 stockList
    public StockList list;
    //최대 밝기 저장 변수
    int lightLevel = 0;
    //프레임수 저장 변수
    int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        //예제 객체의 이름을 코드로 해야 해당하는 주식정보를 읽어올 수 있습니다.
        //Debug.Log(list.apiInfo[transform.name].api_marketprice);
    }

    // Update is called once per frame
    void Update()
    {
       //lightControl();
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
        float change = (list.apiInfo[transform.name].api_marketprice - list.apiInfo[transform.name].api_preclose) / list.apiInfo[transform.name].api_marketprice * 100;
        if (change >= 0) { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 15; } //전날대비 +
        else { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0; } //전날대비 -
    }
}
