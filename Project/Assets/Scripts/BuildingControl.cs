using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class BuildingControl : MonoBehaviour
{
    public StockList list;//�ֽ� api������ �̿��ϱ� ���� stockList
    public portfolio myPortfolio; //��Ʈ������ ���� �����ڷ�
    private Camera cam; //���� ȭ�� ī�޶�
    private GameObject effect; //�̺�Ʈ ���Ǻ� �߻��ϴ� ����ƮUI
    private bool apiFlag; //api���� ���� Ȯ��

    // Start is called before the first frame update
    void Start()
    {
        apiFlag = true;

        //���� ������Ʈ(ī�޶�,UI)�� �����۾� ����
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(transform.name + "Effect").gameObject;
        effect.SetActive(true);
        effectOff();
    }

    void Update()
    {
        //���� ���� api��û ������ �ִ� ��� ���� Ȯ�� �� UI ǥ��
        if (list.apiInfo.ContainsKey(transform.name)) { settingUI(); }
        if (myPortfolio.renew) { checkCloseDiv(); }
        checkLayer();
    }
    void checkLayer()
    {
        Material[] mats = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
        //���̾ �ɼ��� ���� ��� ���� ������ �ƴ��� Ȯ���ϰ� ���̾ �Ҵ�
        if (GameObject.Find("InGameControl").GetComponent<InGameControl>().layerFlag)
        {
            //���� ������ �ƴ� ��� ���̾ �Ҵ�
            if (!myPortfolio.stockInfo.ContainsKey(this.gameObject.name))
            {
                for (int j = 0; j < mats.Length; j++)
                {
                    Color tempcolor;
                    tempcolor = mats[j].color;
                    tempcolor.a = 0.5f;
                    mats[j].color = tempcolor;
                }
            }
            else
            {
                //���� ���� ������ 0�� ��� ���̾ �Ҵ�
                if (myPortfolio.stockInfo[this.gameObject.name].shares == 0)
                {
                    for (int j = 0; j < mats.Length; j++)
                    {
                        Color tempcolor;
                        tempcolor = mats[j].color;
                        tempcolor.a = 0.5f;
                        mats[j].color = tempcolor;
                    }
                }
                //���� ������ 1�� �̻��� ��� ���̾ ����.
                else
                {
                    for (int j = 0; j < mats.Length; j++)
                    {
                        Color tempcolor;
                        tempcolor = mats[j].color;
                        tempcolor.a = 0f;
                        mats[j].color = tempcolor;
                    }
                }
            }
        }   
        //���̾� �ɼ��� ���� ��� ���̾ ����
        else
        {
            for (int j = 0; j < mats.Length; j++)
            {
                Color tempcolor;
                tempcolor = mats[j].color;
                tempcolor.a = 0f;
                mats[j].color = tempcolor;
            }
        }
    }
    //����Ʈ �߻� ������ Ȯ���ϰ� UI �����ϴ� �Լ�
    void settingUI()
    {
        var wantedPos = cam.WorldToScreenPoint(transform.position);
        //������ ���� UI ��ġ�ϱ�
        for (int i=0;i< effect.transform.childCount; i++)
         {
            //ī�޶󿡼� ���̴� ���� ���� ���� UI ��ġ ������
            effect.transform.GetChild(i).gameObject.transform.position = new Vector3(wantedPos.x+20f, wantedPos.y + 375f, wantedPos.z);
            //API�� ������ ���� ������ ��쿡�� ����Ʈ �߻� ���� Ȯ���ϰ� �ش��ϴ� ���� ui�� Ȱ��ȭ
            if (!apiFlag) { continue; }
            //����Ʈ UI �߻� ���� Ȯ��
            //1. ���ɵ� ����
            //2. �ŷ��� �޵�
            //3. ���� ��� �ð� ��ȭ �޵��
            float volumeChange = list.apiInfo[transform.name].api_volume - list.apiInfo[transform.name].api_avgVolume;
            float priceChange = 100f * (list.apiInfo[transform.name].api_marketprice - list.apiInfo[transform.name].api_preclose) / list.apiInfo[transform.name].api_preclose;
            switch (i)
            {
                case 0:
                    //���ɵ� ��ǥ ������ Ȯ��(��Ʃ�� api ���� �����ؼ� �����ϱ�)
                    if (volumeChange == 0f) { effect.transform.GetChild(i).gameObject.SetActive(true); }
                    break;
                case 1:
                    //�ŷ��� �޵� Ȯ��(10�� ��� �ŷ��� ��� �������� Ȯ��)
                    if(volumeChange > 0f) { effect.transform.GetChild(i).gameObject.SetActive(true); }
                    break;
                case 2:
                    //���� ���� ��� �ð� �޵� Ȯ��
                    if (priceChange > 0f){effect.transform.GetChild(i).gameObject.SetActive(true);}
                    break;
                case 3:
                    //���� ���� ��� �ð� �޶� Ȯ�� 
                    if (priceChange < 0f){effect.transform.GetChild(i).gameObject.SetActive(true);}
                    break;
            }
         }
        if (apiFlag) { apiFlag = false; }
    }
    void effectOff()
    {
        for (int i = 0; i < effect.transform.childCount; i++)
        {
            effect.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void checkCloseDiv()
    {
        //��Ʈ������ ���� ���� �� ���� ����� ������� ���� �̸��� �������� ��Ÿ��
    }
}
