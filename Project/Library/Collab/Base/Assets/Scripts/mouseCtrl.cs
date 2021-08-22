using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCtrl : MonoBehaviour
{
    private Camera cam; //게임 화면 카메라
    private GameObject mouseOn; //이벤트 조건별 발생하는 이펙트UI
    private Text symbol;
    private RaycastHit hit;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mouseOn = GameObject.Find("Canvas").transform.Find("mouseOn").gameObject;
        symbol = mouseOn.transform.GetChild(0).GetComponent<Text>();
        mouseOn.SetActive(false); //말풍선 on
    }

    //해당 오브젝트 위에 마우스가 올라가면 말풍선 on
    private void OnMouseEnter()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition); //현재 마우스 커서 위치
        var wantedPos = cam.WorldToScreenPoint(transform.position);

        Physics.Raycast(ray, out hit);
        var logo = hit.transform.GetChild(3).transform.position;

        symbol.text = hit.collider.gameObject.name; //말풍선 텍스트 종목 이름으로 변경
        mouseOn.transform.position = new Vector3(wantedPos.x + 20f, wantedPos.y + 375f, wantedPos.z); //말풍선 위치 변경
        mouseOn.SetActive(true);

    }

    //해당 오브젝트 위에 마우스가 올라가면 말풍선 off
    private void OnMouseExit()
    {
        mouseOn.SetActive(false); 
    }
}
