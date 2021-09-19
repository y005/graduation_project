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
    public GameObject movePos; //�ǹ� ��ġ ���� ������
    private RaycastHit hit; //���콺�� Ŭ���� ��ü
    public TextMeshProUGUI SectorName; //ȭ�鿡 ��� ���� ���� �ؽ�Ʈ UI
    public Button[] posBtn; //���ͺ� �̵� ������ �ǹ� ���� �迭
    public Text[] btnText; //��ư ���� �ؽ�Ʈ �迭

    Dictionary<string, List<string>> sectorCtrl
        = new Dictionary<string, List<string>>() { { "���", new List<string> { "MSFT", "ORCL", "AAPL", "IBM", "GOOGL", "FB", "NFLX", "DIS" } },
                                                   { "���", new List<string> { "HON", "UNP", "MMM", "TT", "LMT" } },
                                                   { "�Һ���", new List<string> { "AMZN", "TSLA", "SBUX", "NKE", "WMT", "COST", "KO", "PEP" } },
                                                   { "�ｺ�ɾ�", new List<string> { "JNJ", "PFE", "UNH", "AMGN", "LLY" } },
                                                   { "�ε���", new List<string> { "AMT", "EQIX", "PLD", "O" } },
                                                   { "����", new List<string> { "V", "PYPL", "BAC", "C", "WFC" } } };

    // Update is called once per frame
    void FixedUpdate()
    {
        rigntClick();
    }

    void rigntClick()
    {
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp) { return; }

        //��Ŭ���� - �ǹ� ��ġ �̵�
        if (Input.GetMouseButtonDown(1))
        {
            //Ŭ�� �νĹ������� ����� ��� �ٷ� ��ȯ
            if ((Input.mousePosition.x < 300) || (Input.mousePosition.x > 1600) || (Input.mousePosition.y < 300) || (Input.mousePosition.y > 900)) { return; }
            string tmpname = "";
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("stock"))//�ǹ� ������Ʈ�� Ŭ���� ���
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
        string curSector = SectorName.text; //���� ���͸�
        List<string> secList = sectorCtrl[SectorName.text]; //���� ���� ���� ����Ʈ

        //��ư �ʱ� ����
        for (int i = 0; i < 8; i++)
        {
            posBtn[i].interactable = false;
            btnText[i].text = "-";
        }

        //���ͺ� ��ư ����
        for (int i = 0; i < secList.Count; i++)
        {
            btnText[i].text = secList[i];
            if (!btnText[i].text.Equals(building)) { posBtn[i].interactable = true; } //���� ������ �ƴ� ��� ��ư Ȱ��ȭ
        }

        // + swich ��� �ܿ� �ǹ� ����/���� ���
    }

    public void moveBtnClick()
    {
        Vector3 targetPos, beforePos;
        int index = -1 ;

        //Ŭ���� ��ư ã��
        string clicked = EventSystem.current.currentSelectedGameObject.name;
        for(int i = 0; i < 8; i++)
        { 
            if(clicked.Equals("pos" + i)) { index = i; break; }
        }

        //�ǹ� ��ġ ��ȯ
        targetPos = hit.collider.gameObject.transform.position; //Ŭ���� �ǹ��� ��ġ (��ǥ ��ġ)
        beforePos = GameObject.Find(btnText[index].text).gameObject.transform.position; //�̵��� �ǹ��� ��ġ (��ư���� Ŭ���� ������Ʈ�� ���� ��ġ)

        //�ǹ� �̵�
        GameObject.Find(btnText[index].text).gameObject.transform.position = new Vector3(targetPos.x, beforePos.y, targetPos.z);
        hit.collider.gameObject.transform.position = new Vector3(beforePos.x, targetPos.y, beforePos.z);

        movePos.SetActive(false); //������ ������
        GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp = false;
    }

    public void ExitBtnClick()
    {
        movePos.SetActive(false);
        GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp = false;
    }
}
