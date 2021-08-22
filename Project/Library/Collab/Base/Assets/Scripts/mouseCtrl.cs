using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCtrl : MonoBehaviour
{
    private Camera cam; //���� ȭ�� ī�޶�
    private GameObject mouseOn; //�̺�Ʈ ���Ǻ� �߻��ϴ� ����ƮUI
    private Text symbol;
    private RaycastHit hit;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        mouseOn = GameObject.Find("Canvas").transform.Find("mouseOn").gameObject;
        symbol = mouseOn.transform.GetChild(0).GetComponent<Text>();
        mouseOn.SetActive(false); //��ǳ�� on
    }

    //�ش� ������Ʈ ���� ���콺�� �ö󰡸� ��ǳ�� on
    private void OnMouseEnter()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition); //���� ���콺 Ŀ�� ��ġ
        var wantedPos = cam.WorldToScreenPoint(transform.position);

        Physics.Raycast(ray, out hit);
        var logo = hit.transform.GetChild(3).transform.position;

        symbol.text = hit.collider.gameObject.name; //��ǳ�� �ؽ�Ʈ ���� �̸����� ����
        mouseOn.transform.position = new Vector3(wantedPos.x + 20f, wantedPos.y + 375f, wantedPos.z); //��ǳ�� ��ġ ����
        mouseOn.SetActive(true);

    }

    //�ش� ������Ʈ ���� ���콺�� �ö󰡸� ��ǳ�� off
    private void OnMouseExit()
    {
        mouseOn.SetActive(false); 
    }
}
