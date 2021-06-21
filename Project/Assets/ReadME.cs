using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReadME : MonoBehaviour
{
    //주식정보 목록을 저장한 객체 자료형 StockList 선언
    public StockList list;

    //Asset/Scripts/Scriptable Objects 에 있는 StockList를 
    //인게임 컨트롤 객체의 스크립트에 드래그하여 연결합니다. 

    //Asset/Scripts에 있는 StockList에서 정의된 자료형 구조를 확인하여 사용하면 됩니다.
    //apiInfo는 정의된 자료형 안에 있는 종목코드,종목정보들이 쌍으로 저장된 딕셔너리입니다.
    //(이전에 만들 자료구조를 참고함)
    //참조예시: list.apiInfo["MSFT"].api_marketprice

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
