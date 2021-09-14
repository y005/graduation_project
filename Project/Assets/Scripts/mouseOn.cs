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
        obj.SetActive(false); //��ǳ�� off
        setMsg();
    }

    void setMsg()
    {
        if (score>80)
        {
            msg.text = score.ToString() + "������ ���� ���� ��Ȳ�� �ſ� Ž������ ���·� �ŵ� ��ȸ�Դϴ�!";
        }
        else if (score > 50)
        {
            msg.text = score.ToString()+"������ ���� ������ �ټ� Ž�� �����Դϴ�!";
        }
        else if (score > 20)
        {
            msg.text = score.ToString() + "������ ���� ������ �ټ� ���� �����Դϴ�!";
        }
        else
        {
            msg.text = score.ToString()+"������ ���� ������ �ſ� �������� ���·� �ż� ��ȸ�Դϴ�!";
        }
    }

    //�ش� ������Ʈ ���� ���콺�� �ö󰡸� ��ǳ�� on
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp)
        {
            obj.SetActive(true);
        }
    }

    //�ش� ������Ʈ ���� ���콺�� �ö󰡸� ��ǳ�� off
    public void OnPointerExit(PointerEventData eventData)
    {
        obj.SetActive(false);
    }

}
