using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUY : MonoBehaviour
{
    public void Buy()
    {
/*        try
        {
            //Text �Է¶��� �����ִ� ���� �ڵ��� ������ �����ɴϴ�.
            string symbol = codeText.text;
            string count = cntText.text;
            string cost = costText.text; //����ڰ� �Է��� �ż� �ݾ�(1�ִ�) ����

            Int32.TryParse(count, out int mycnt); //mycnt = �ż� ����
            Int32.TryParse(cost, out int mycost); //mycost = �ż� �ݾ�(1�ִ�)
            float myprice = mycnt * mycost; //myprice = �� �ż� �ݾ�

            await BeginNetwork(symbol); //API ���� �ҷ�����
            float market_price = apiInfo[symbol].api_marketprice;

            //�̹� ���� �����̶�� ������ ���� ����
            if (myStocks.ContainsKey(symbol))
            {
                myStocks[symbol].c_count += mycnt; //�� ���� ����
                myStocks[symbol].c_cost += myprice; //�� �ż� �ݾ�
                myStocks[symbol].c_myavg = myStocks[symbol].c_cost / myStocks[symbol].c_count; //��� �ż� �ݾ�(1�ִ�)
                myStocks[symbol].c_marketprice = myStocks[symbol].c_count * market_price; //�� �ݾ�
                totalAsset += myprice;
            }

            //���ο� �����̶�� ��ü�� �����ϰ� ��ġ�� �� ���������� �迭�� ����
            else
            {    
                GameObject a = (GameObject)Instantiate(Resources.Load(path));

                //��ųʸ��� �� �߰� (���� ��ü/�� ���� ����/�� �ż� �ݾ�/��� �ż� �ݾ�(1�ִ�)/�� �ݾ�)
                myStocks.Add(symbol, new StockData(a, mycnt, mycost * mycnt, mycost, market_price * mycnt));

                totalAsset += myprice; //�� �ڻ� ������Ʈ

                //3X3 ���ڱ��� �ȿ��� ���ʴ�� ���ο� ���� ��ġ
                int x = 6 * ((myStocks.Count - 1) % 3) - 6;
                int z = -6 * ((myStocks.Count - 1) / 3) + 6;
                //������ ��ü�� �����ϰ� ��ġ
                a.name = symbol;
                a.transform.position = new Vector3(x, 1, z);

                i++;
            }
            addStock = true;

            //DEBUG
            Debug.Log("symbol : " + symbol + "   �� ���� ���� : " + myStocks[symbol].c_count + "   �� �ż� �ݾ� : " + myStocks[symbol].c_cost
                + "   ��� �ż� �ݾ�: " + myStocks[symbol].c_myavg + "   �� �ݾ� : " + myStocks[symbol].c_marketprice
                + "   ���ͷ� : " + (market_price / myStocks[symbol].c_myavg - 1) * 100 + "%");
        }
        catch (Exception)
        {
            Debug.Log("����ε� ����� ������ �Է����ּ���");
        }*/
    }
}
