using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "portfolio")]
public class portfolio : ScriptableObject
{
    public StockList list; //�ֽ�����API

    public float Cash = 0; //���� ���� �ݾ�
    
    //���񺰿� ���� ����, ��ܰ��� �ϳ��� ��ųʸ��� ����
    public Dictionary<string, StockStat> stockInfo = new Dictionary<string, StockStat>();
    
    //�����ڵ�� �ŷ� ������ ����Ʈ�� �ϳ��� ��ųʸ��� ����
    public Dictionary<string, List<Trade> > tradeList = new Dictionary<string, List<Trade> >();

    //struct ����ü�� ��� .�� ���� ����ü �Ӽ� ������ �Ұ����Ͽ� �Ȱ��� ����ü ��ü�� ������
    //�� �������� �Ӽ������� �� �� ��ü�� ��ü�ؾ� ��
    public struct StockStat//���� ������ ��ܰ��� ����(���� �Ÿ� ����� �ջ��Ͽ� ����� ����ü)
    {
        public int shares;
        public float avgCostPerShare;
    };

    public class Trade //API���� �ҷ��� ������ return �ϱ� ���� Ŭ����
    {
        public string TradeDate;//�Ÿ� ��¥
        public int Shares;//�Ÿ� �ֽļ�
        public float CostPerShare;//��ܰ�
        public bool State; //�ż�: 0, �ŵ�: 1 ���� 
        public Trade(string TradeDate, int Shares, float CostPerShare, bool State)
        {
            this.TradeDate = TradeDate;
            this.Shares = Shares;
            this.CostPerShare = CostPerShare;
            this.State = State;
        }
    }
    public void addTrade(string code,string TradeDate,int Shares, float CostPerShare, bool State)
    {
        //�̹� ��Ʈ�������� ������ ���� ���� �Ÿ�
        if (tradeList.ContainsKey(code))
        {
            //�ŵ��� ���
            if (State)
            {
                //���� ���񺸴� ���� ������ ���
                if (stockInfo[code].shares < Shares)
                {
                    Debug.Log("�������񺸴� ���� �ŵ��� �� �����ϴ�.");
                }
                //���� ���񺸴� �۰ų� ���� ������ ���(��� ������ �ŵ��� ��쿡�� �ش� ���� ���� �ǹ� ��ġ�� �ϸ� �ȵ�)
                else
                {
                    //������ ���� �ŷ� ������ �߰�
                    tradeList[code].Add(new Trade(TradeDate, Shares, CostPerShare, State));

                    //���������� ���� ������ ��ü ��ܰ� �����ϱ�(���� �ŵ��� �ݾ��� ���� �� �ŵ� �ݾ׿��� �� �� ������ ���񰹼��� ����)
                    StockStat tmp = stockInfo[code];
                    if (tmp.shares == Shares)
                    {
                        tmp.avgCostPerShare = 0;
                    }
                    else
                    {
                        tmp.avgCostPerShare = (tmp.avgCostPerShare * tmp.shares - Shares * CostPerShare) / (tmp.shares - Shares);
                    }
                    tmp.shares -= Shares;
                    stockInfo[code] = tmp;
                    Cash += Shares * CostPerShare;
                }
            }
            //�ż��� ���
            else
            {
                //�Է��� �ڱ��� �������� ���� ��쿡�� 
                if (Cash >= Shares * CostPerShare)
                {
                    //������ ���� �ŷ� ������ �߰�
                    tradeList[code].Add(new Trade(TradeDate, Shares, CostPerShare, State));

                    //���������� ���� ������ ��ü ��ܰ� �����ϱ�(���� �ż��� �ݾװ� ���� �� �ż� �ݾ��� �ջ� �� ������ ���񰹼��� ����)
                    StockStat tmp = stockInfo[code];
                    tmp.avgCostPerShare = (Shares * CostPerShare + tmp.avgCostPerShare * tmp.shares) / (tmp.shares + Shares);
                    tmp.shares += Shares;
                    stockInfo[code] = tmp;
                    Cash -= Shares * CostPerShare;
                }
                else
                {
                    Debug.Log("�ż��� �� �ִ� �ڱ��� �����մϴ�.");
                }
            }
        }
        //��Ʈ�������� ���� ���ο� ���� ���� �Ÿ�
        else
        {
            //�ŵ��� ���
            if (State)
            {
                Debug.Log("��Ʈ�������� ���� ������ �ŵ��� �� �����ϴ�");
            }
            else//�ż��� ���
            {
                //�Է��� �ڱ��� �������� ���� ��쿡�� 
                if (Cash >= Shares * CostPerShare)
                {
                    tradeList.Add(code, new List<Trade>());

                    //���� ���� ���� �ŷ� ������ ���� �߰�
                    tradeList[code].Add(new Trade(TradeDate, Shares, CostPerShare, State));

                    //������ ��ü ������ ��ܰ� ���� �߰�
                    stockInfo.Add(code, new StockStat());
                    StockStat tmp = stockInfo[code];
                    tmp.shares = Shares;
                    tmp.avgCostPerShare = CostPerShare;
                    stockInfo[code] = tmp;
                    Cash -= Shares * CostPerShare;
                }
                else
                {
                    Debug.Log("�ż��� �� �ִ� �ڱ��� �����մϴ�.");
                }
            }
        }
    }
    public float updateGain(string code)
    {
        //�ش� ������ �򰡱ݾ��� ���(���尡�� X ����)
        return stockInfo[code].shares * list.apiInfo[code].api_marketprice;   
    }
    public void eraseData()
    {
        stockInfo.Clear();
        tradeList.Clear();
    }
}