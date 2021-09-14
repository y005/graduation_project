using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class mouseOn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private TextMeshProUGUI msg;
    private GameObject obj;
    private int score;

    void Start()
    {
        score = GameObject.Find("InGameControl").GetComponent<InGameControl>().list.fearAndGreed;
        obj = GameObject.Find("Canvas").transform.Find("mouseOn").gameObject;
        msg = obj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        obj.SetActive(false); //말풍선 off
        setMsg();
    }

    void setMsg()
    {
        if (score>80)
        {
            msg.text = score.ToString() + "점으로 현재 시장 상황은 매우 탐욕적인 상태로 매도 기회입니다!";
        }
        else if (score > 50)
        {
            msg.text = score.ToString()+"점으로 현재 시장은 다소 탐욕 상태입니다!";
        }
        else if (score > 20)
        {
            msg.text = score.ToString() + "점으로 현재 시장은 다소 공포 상태입니다!";
        }
        else
        {
            msg.text = score.ToString()+"점으로 현재 시장은 매우 공포적인 상태로 매수 기회입니다!";
        }
    }

    //해당 오브젝트 위에 마우스가 올라가면 말풍선 on
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp)
        {
            obj.SetActive(true);
        }
    }

    //해당 오브젝트 위에 마우스가 올라가면 말풍선 off
    public void OnPointerExit(PointerEventData eventData)
    {
        obj.SetActive(false);
    }

}
