using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingControl : MonoBehaviour
{
    //�ֽ� api������ �̿��ϱ� ���� stockList
    public StockList list;
    //�ִ� ��� ���� ����
    int lightLevel = 0;
    //�����Ӽ� ���� ����
    int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        //���� ��ü�� �̸��� �ڵ�� �ؾ� �ش��ϴ� �ֽ������� �о�� �� �ֽ��ϴ�.
        //Debug.Log(list.apiInfo[transform.name].api_marketprice);
    }

    // Update is called once per frame
    void Update()
    {
       //lightControl();
    }

    void lightControl()
    {
        //������Ʈ �Լ��� 1�ʿ� 60�� ������ ȣ��Ǳ� ������(60������)
        //0.5�� �ֱ�� ��⸦ ��ȭ�ϱ� ���ؼ��� ī��Ʈ ������ ���� ����� Ȯ��
        /*if (frame == 0)
        {
            lightLevel = (lightLevel + 1) % 2;
        }
        transform.GetChild(0).gameObject.GetComponent<Light>().intensity = lightLevel;
        frame = (frame + 1) % 60;*/

        //�ð� ��ȭ���� ���� ���� on/off
        float change = (list.apiInfo[transform.name].api_marketprice - list.apiInfo[transform.name].api_preclose) / list.apiInfo[transform.name].api_marketprice * 100;
        if (change >= 0) { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 15; } //������� +
        else { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0; } //������� -
    }
}
