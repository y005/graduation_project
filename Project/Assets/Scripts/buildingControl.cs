using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    //최대 밝기 저장 변수
    int lightLevel = 0;
    //프레임수 저장 변수
    int frame = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //업데이트 함수는 1초에 60번 정도로 호출되기 때문에(60프레임)
        //0.5초 주기로 밝기를 변화하기 위해서는 카운트 변수를 따로 만들고 확인
        if(frame == 0)
        {
            lightLevel = (lightLevel + 1) % 3;
        }
        transform.GetChild(0).gameObject.GetComponent<Light>().intensity = lightLevel;
        frame = (frame + 1) % 60;
    }
}
