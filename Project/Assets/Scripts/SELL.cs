using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SELL : MonoBehaviour
{
    public void Sell()
    {
 /*       try
        {
            //Text �Է¶��� �����ִ� ���� �ڵ��� ������ �����ɴϴ�.
            string symbol = codeText.text;
            string count = cntText.text;
            string cost = costText.text; //����ڰ� �Է��� �ŵ� �ݾ�(1�ִ�) ����

            Int32.TryParse(count, out int mycnt); //mycnt = �ŵ� ����
            Int32.TryParse(cost, out int mycost); //mycost = �ŵ� �ݾ�(1�ִ�)
            float myprice = mycnt * mycost; //myprice = �� �ŵ� �ݾ�

            await BeginNetwork(symbol); //API ���� �ҷ�����
            float market_price = apiInfo[symbol].api_marketprice;

            //�̹� ���� �����̶�� ������ ���� ����
            if (myStocks.ContainsKey(symbol))
            {
                //�� ���� �������� �ŵ� ������ �� ũ�ٸ�
                if (myStocks[symbol].c_count < mycnt)
                {
                    Debug.Log("�����ϰ� �ִ� �������� �� ���� �ŵ��� �� �����ϴ�.");
                }
                else
                {
                    myStocks[symbol].c_count -= mycnt; //�� ���� ����
                    myStocks[symbol].c_cost -= myprice; //�� �ŵ� �ݾ�
                    myStocks[symbol].c_myavg = myStocks[symbol].c_cost / myStocks[symbol].c_count; //��� �ż� �ݾ�(1�ִ�)
                    myStocks[symbol].c_marketprice = myStocks[symbol].c_count * market_price; //�� �ݾ�
                    totalAsset -= myprice;

                    addStock = true;
                }

                //DEBUG
                Debug.Log("symbol : " + symbol + "   �� ���� ���� : " + myStocks[symbol].c_count + "   �� �ż� �ݾ� : " + myStocks[symbol].c_cost
                   + "   ��� �ż� �ݾ�: " + myStocks[symbol].c_myavg + "   �� �ݾ� : " + myStocks[symbol].c_marketprice
                   + "   ���ͷ� : " + (market_price / myStocks[symbol].c_myavg - 1) * 100 + "%");


                //���̻� �ش� ������ �����ϰ� ���� ���� ���
                if (myStocks[symbol].c_count == 0)
                {
                    myStocks.Remove(symbol); //��ųʸ����� ���� ����
                    apiInfo.Remove(symbol); //��ųʸ����� ���� ����
                    Destroy(GameObject.Find(symbol)); //�ش� ���� ��ü ����
                }
            }

            //���ο� �����̶�� �ŵ� �Ұ�
            else
            {
                Debug.Log("�����ϰ� �ִ� ������ ������ �Է����ּ���");
            }

        }
        catch (Exception)
        {
            Debug.Log("����ε� ����� ������ �Է����ּ���");
        }*/
    }
}
