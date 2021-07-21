using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class BuildingControl : MonoBehaviour
{
    //�ֽ� api������ �̿��ϱ� ���� stockList
    public StockList list;
    public portfolio myPortfolio; //��Ʈ������ ���� �����ڷ�
    public Toggle myStockOpt; //�� ���� ǥ���ϴ� bool����
    //�ִ� ��� ���� ����
    //int lightLevel = 0;
    //�����Ӽ� ���� ����
    //int frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        //���� ��ü�� �̸��� �ڵ�� �ؾ� �ش��ϴ� �ֽ������� �о�� �� �ֽ��ϴ�.
        //Debug.Log(list.apiInfo[transform.name].api_marketprice);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.name);
        //lightControl();
        //�Լ� ���ο��� myPortfolio.renew = false�� �ٲ�� ������ ������ ������ ������ �ȴ�.(������ ������ �ȵǴµ�)
        if (myPortfolio.renew){
            updateBuilding();
        }
    }
    void updateBuilding()
    {
        //���������� ��� ������ Ű�� �򰡱ݾ��� ���߿� ���� �����ϸ� �۾� 
        if (myPortfolio.stockInfo.ContainsKey(this.gameObject.name))
        {
            //������ 0�� ��� ���� ������ �ƴϱ� ������ �Ѿ
            if (myPortfolio.stockInfo[this.gameObject.name].shares == 0) { myPortfolio.renew = false; return; }

            transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 3f;
            //��ü �򰡱ݾ� ���
            float total = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                //���� ������ ���������� 0���� ��� ī��Ʈ���� ����
                if (myPortfolio.stockInfo[key].shares == 0) { continue; }
                total += myPortfolio.updateGain(key);
            }
            //�ڻ� ��� ������ ���� ���ϱ�(1~3�ܰ�)
            float ratio = myPortfolio.updateGain(this.gameObject.name) / total * 100; //��ü �򰡱ݾ� �� �ش� ���� �򰡱ݾ� ����
            float scale = 1f;
            if (ratio < 30) { scale = 0.75f; }
            else if (ratio >= 30 && ratio < 70) { scale = 1f; }
            else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }
            Debug.Log(myPortfolio.updateGain(this.gameObject.name));
            Debug.Log(ratio);
            //�ܰ迡 ���� ������ ������ŭ ��ü ������ �����ϱ�
            transform.localScale = new Vector3(scale * transform.localScale.x, scale * transform.localScale.y, scale * transform.localScale.z);
            myPortfolio.renew = false;
        }
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
        //float change = (list.apiInfo[transform.name].api_marketprice - list.apiInfo[transform.name].api_preclose) / list.apiInfo[transform.name].api_marketprice * 100;
        //if (change >= 0) { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 15; } //������� +
        //else { transform.GetChild(0).gameObject.GetComponent<Light>().intensity = 0; } //������� -
    }
}
