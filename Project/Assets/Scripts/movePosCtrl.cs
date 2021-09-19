using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class movePosCtrl : MonoBehaviour
{
    public GameObject movePos; //건물 위치 변경 페이지
    private RaycastHit hit; //마우스에 클릭된 객체
    public TextMeshProUGUI SectorName; //화면에 띄울 섹터 정보 텍스트 UI
    public Button[] posBtn; //섹터별 이동 가능한 건물 선택 배열
    public Text[] btnText; //버튼 안의 텍스트 배열

    Dictionary<string, List<string>> sectorCtrl
        = new Dictionary<string, List<string>>() { { "기술", new List<string> { "MSFT", "ORCL", "AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS" } },
                                                   { "산업", new List<string> { "HON", "UNP", "MMM", "TT", "LMT" } },
                                                   { "소비재", new List<string> { "AMZN", "TSLA", "SBUX", "NKE", "WMT", "COST", "KO", "PEP" } },
                                                   { "헬스케어", new List<string> { "JNJ", "PFE", "UNH", "AMGN", "LLY" } },
                                                   { "부동산", new List<string> { "AMT", "EQIX", "PLD", "O" } },
                                                   { "금융", new List<string> { "V", "PYPL", "BAC", "C", "WFC" } } };

    // Update is called once per frame
    void FixedUpdate()
    {
        rigntClick();
    }

    void rigntClick()
    {
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp) { return; }

        //우클릭시 - 건물 위치 이동
        if (Input.GetMouseButtonDown(1))
        {
            //클릭 인식범위에서 벗어나는 경우 바로 반환
            if ((Input.mousePosition.x < 300) || (Input.mousePosition.x > 1600) || (Input.mousePosition.y < 300) || (Input.mousePosition.y > 900)) { return; }
            string tmpname = "";
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("stock"))//건물 오브젝트를 클릭한 경우
                {
                    movePos.SetActive(true);
                    GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp = true;
                    tmpname = hit.collider.gameObject.name;
                    movePageSet(tmpname);
                }
            }
        }
    }

    void movePageSet(string building)
    {
        string curSector = SectorName.text; //현재 섹터명
        List<string> secList = sectorCtrl[SectorName.text]; //현재 섹터 종목 리스트

        //버튼 초기 세팅
        for (int i = 0; i < 8; i++)
        {
            posBtn[i].interactable = false;
            btnText[i].text = "-";
        }

        //섹터별 버튼 세팅
        for (int i = 0; i < secList.Count; i++)
        {
            btnText[i].text = secList[i];
            if (!btnText[i].text.Equals(building)) { posBtn[i].interactable = true; } //현재 빌딩이 아닐 경우 버튼 활성화
        }

        // + swich 기능 외에 건물 생성/삭제 기능
    }

    public void moveBtnClick()
    {
        Vector3 targetPos, beforePos;
        int index = -1 ;

        //클릭한 버튼 찾기
        string clicked = EventSystem.current.currentSelectedGameObject.name;
        for(int i = 0; i < 8; i++)
        { 
            if(clicked.Equals("pos" + i)) { index = i; break; }
        }

        //건물 위치 반환
        targetPos = hit.collider.gameObject.transform.position; //클릭한 건물의 위치 (목표 위치)
        beforePos = GameObject.Find(btnText[index].text).gameObject.transform.position; //이동될 건물의 위치 (버튼으로 클릭한 오브젝트의 기존 위치)

        //건물 이동
        GameObject.Find(btnText[index].text).gameObject.transform.position = new Vector3(targetPos.x, beforePos.y, targetPos.z);
        hit.collider.gameObject.transform.position = new Vector3(beforePos.x, targetPos.y, beforePos.z);

        movePos.SetActive(false); //페이지 내리기
        GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp = false;
    }

    public void ExitBtnClick()
    {
        movePos.SetActive(false);
        GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp = false;
    }
}
