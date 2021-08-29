using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCtrl : MonoBehaviour
{
    //1. ��� ������ �ö���� ����
    private Camera cam; //���� ȭ�� ī�޶�
    private GameObject mouseOn; //�̺�Ʈ ���Ǻ� �߻��ϴ� ����ƮUI
    private Text symbol;
    private portfolio myStocks;
    private StockList api;
    Vector3 m_Size;

    void Start()
    {
        myStocks = GameObject.Find("InGameControl").GetComponent<InGameControl>().myPortfolio;
        api = GameObject.Find("InGameControl").GetComponent<InGameControl>().list;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        m_Size = GetComponent<Collider>().bounds.size;
        mouseOn = (GameObject)Instantiate(Resources.Load("Prefabs/etc/mouseOn"));
        mouseOn.transform.SetParent(GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject.transform, false);
        symbol = mouseOn.transform.GetChild(0).GetComponent<Text>();
        mouseOn.SetActive(false);
    }

    void FixedUpdate()
    {
        //�ݶ��̴� ť���� ���� �̿��ؼ� ��ġ�� ��ġ�� ���� ����� ��ǥ�� ã�Ƴ�
        Vector3 tmp = new Vector3(transform.position.x - (m_Size.x / 2), transform.position.y + m_Size.y*1.6f, transform.position.z - (m_Size.z / 2));
        var wantedPos = cam.WorldToScreenPoint(tmp);
        mouseOn.transform.position = new Vector3(wantedPos.x, wantedPos.y, wantedPos.z); //��ǳ�� ��ġ ����

        //����1 Ȯ��:ȭ�鿡 �ٸ� â(����â,����â,��������â)�� ���ִ� ��쿡�� ����� X
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().pagePopUp) { mouseOn.SetActive(false); return; }
        //��ǳ�� ��ġ ����� �� �ӽ��ڵ�
        //if (true) { symbol.text = ""; mouseOn.SetActive(true); return; }

        //���� 2 Ȯ��: ��Ƽ���� ��� ��ǳ���� ������ ����
        if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView){mouseOn.transform.localScale = new Vector3(0.2f,0.2f,1f);}
        else{mouseOn.transform.localScale = new Vector3(0.8f, 0.8f, 1f);}
        //����3 Ȯ��: ������ ���̾��� ���� ������ ������ ��쿡�� ��ǳ�� ������ ���
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().priceFlag)
        {
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            if (myStocks.stockInfo[transform.name].shares == 0) { mouseOn.SetActive(false); return; }
            //���� ������ ��ܰ� ��� ���� �ְ� ���� ���
            float percent = (api.apiInfo[transform.name].api_marketprice - myStocks.stockInfo[transform.name].avgCostPerShare) / myStocks.stockInfo[transform.name].avgCostPerShare * 100;
            symbol.text = percent.ToString("F2") + "%";
            //��Ƽ�� �� ��� ���� ������ ǥ��
            if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView)
            {
                if (percent < 0) { mouseOn.GetComponent<Image>().color = new Color(0, 0, 1); }
                else { mouseOn.GetComponent<Image>().color = new Color(1, 0, 0); }
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205 / 255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            else
            {
                mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205/255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            mouseOn.SetActive(true);
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().volumeFlag)
        {
            //���� ������ �ƴ� ��� ���� ǥ�� ����
            if (!api.apiInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            //10�� ��� �ŷ��� ��� ���� �ŷ��� ���� ���
            float percent = (api.apiInfo[transform.name].api_volume - api.apiInfo[transform.name].api_avgVolume) / api.apiInfo[transform.name].api_avgVolume * 100;
            symbol.text = percent.ToString("F2") + "%";

            //��Ƽ�� �� ��� ���� ������ ǥ��
            if (GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().cityView)
            {
                if (percent < 0) { mouseOn.GetComponent<Image>().color = new Color(0, 0, 1); }
                else { mouseOn.GetComponent<Image>().color = new Color(1, 0, 0); }
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205 / 255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            else
            {
                mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
                Color color = mouseOn.GetComponent<Image>().color;
                color.a = 205 / 255f;
                mouseOn.GetComponent<Image>().color = color;
            }
            mouseOn.SetActive(true);
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().divFlag)
        {
            //���� ����� ������ ���� ���
            if (!api.apiInfo.ContainsKey(transform.name)) { mouseOn.SetActive(false); return; }
            string date1 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divDate(transform.name);
            if (date1.Length < 1) { mouseOn.SetActive(false); return; }
            TimeSpan dateDiff = DateTime.Parse(date1) - DateTime.Now;
            int tmp1 = dateDiff.Days;
            symbol.text = "D-" + tmp1.ToString() + "\n";
            //���� ������ ��쿡�� ���� ���� ���� ǥ��
            mouseOn.SetActive(true);
            if (!myStocks.stockInfo.ContainsKey(transform.name)) { return; }
            if (myStocks.stockInfo[transform.name].shares == 0) { return; }
            float tmp2 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().dividend(transform.name);
            symbol.text += tmp2.ToString("F2") + "$";
            mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
            Color color = mouseOn.GetComponent<Image>().color;
            color.a = 205 / 255f;
            mouseOn.GetComponent<Image>().color = color;
        }
        else if (GameObject.Find("InGameControl").GetComponent<InGameControl>().interestFlag)
        {
            //���ɵ� ���� ����
            symbol.text = "";
            mouseOn.SetActive(true);
            mouseOn.GetComponent<Image>().color = new Color(1, 1, 1);
            Color color = mouseOn.GetComponent<Image>().color;
            color.a = 205 / 255f;
            mouseOn.GetComponent<Image>().color = color;
        }
        else { mouseOn.SetActive(false); return; }//üũ�� �ɼ��� ���� ��� ǥ�� X
    }
}
