using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "StockList")]
public class StockList : ScriptableObject
{
    //�ֽĽ����� ������ Ž������
    public int fearAndGreed;
    //�����ڵ�� ���� yahoo finance api �ڷḦ ������ ��ųʸ�
    public Dictionary<string, APIData> apiInfo = new Dictionary<string, APIData>();
    //�����ڵ�� ���� youtube data v3 api �ڷḦ ������ ��ųʸ�
    public Dictionary<string, youtubeData> youtubeInfo = new Dictionary<string, youtubeData>();
    //�����ڵ�� ���� ���� ������ ���� ������ ������ ��ųʸ�
    public Dictionary<string, sentimentData> sentimentInfo = new Dictionary<string, sentimentData>();
    //���ɵ� ����(�ֱ� �� �� �� 1�� ��ȸ�� �̻��� �������� ������ ī��Ʈ) 
    public List<KeyValuePair<string, int>> vidCntRank = new List<KeyValuePair<string, int>>();
    //���ɵ� ����(�ֱ� �� �� �� 1�� ��ȸ�� �̻��� �������� ��ȸ�� ������ ī��Ʈ) 
    public List<KeyValuePair<string, int>> viewRank = new List<KeyValuePair<string, int>>();

    //1. yahoo finance api���� �ҷ��� ������ �����ϱ� ���� �ڷ�
    public class APIData 
    {
        public float api_marketprice; //���� �ð�
        public string api_divDate; //�����
        public float api_divRate; //����
        public string api_sector; //���� ������ �з�
        public float api_marketcap; //�ð��Ѿ�
        public float api_per; //PER
        public float api_52week; //�ð� ���� ��ȭ(52 week change)
        public float api_preclose; //���� ������
        public float api_volume; //�ŷ���
        public float api_avgVolume; //��� 10�� �ŷ���

        public APIData(float api_marketprice, string api_divDate, float api_divRate, string api_sector, float api_marketcap, float api_per, float api_52week, float api_preclose, float api_volume, float api_avgVolume)
        {
            this.api_marketprice = api_marketprice;
            this.api_divDate = api_divDate;
            this.api_divRate = api_divRate;
            this.api_sector = api_sector;
            this.api_marketcap = api_marketcap;
            this.api_per = api_per;
            this.api_52week = api_52week;
            this.api_preclose = api_preclose;
            this.api_volume = api_volume;
            this.api_avgVolume = api_avgVolume;
        }
    }
    public void add(string code, float send_price, string send_divdate, float send_divrate, string send_sector, float send_marketcap, float send_per, float send_52, float send_preclose, float send_volume, float send_avgVolume)
    {
        apiInfo.Add(code, new APIData(send_price, send_divdate, send_divrate, send_sector, send_marketcap, send_per, send_52, send_preclose, send_volume, send_avgVolume));
    }
    //2. youtube data v3 api���� �ҷ��� ������ �����ϱ� ���� �ڷ�
    public class youtubeData
    {
        public int api_cnt; //������ �� 1�� ��ȸ�� �̻��� ���� ��
        public int api_view; //������ �� ���� ���� ��ȸ�� �ջ�
        public int api_like; //������ �� ���� ���� ���ƿ�� �ջ�
        public int api_dislike; //������ �� ���� ���� �Ⱦ��� �ջ�
        public int api_comment; //������ �� ���� ���� ��ۼ� �ջ�

        public youtubeData(int count,int viewCount, int likeCount, int dislikeCount, int commentCount)
        {
            this.api_cnt = count;
            this.api_view = viewCount;
            this.api_like = likeCount;
            this.api_dislike = dislikeCount;
            this.api_comment = commentCount;
        }
    }
    public void addYoutube(string code, int count, int viewCount, int likeCount, int dislikeCount, int commentCount)
    {
        youtubeInfo.Add(code, new youtubeData(count, viewCount, likeCount, dislikeCount, commentCount));
    }
    //3. ec2���� api���� �ҷ��� ������ �����ϱ� ���� �ڷ�
    public class sentimentData
    {
        public int api_positive; //������������ �з��� ���� ��� ����
        public int api_negative; //������������ �з��� ���� ��� ����

        public sentimentData(int pos, int neg)
        {
            this.api_positive = pos;
            this.api_negative = neg;
        }
    }
    public void addSenti(string code,int pos,int neg)
    {
        sentimentInfo.Add(code, new sentimentData(pos,neg));
    }
    public void rankUpdate()
    {
        foreach(var tmp in youtubeInfo)
        {
            vidCntRank.Add(new KeyValuePair<string, int>(tmp.Key,tmp.Value.api_cnt));
        }
        foreach (var tmp in youtubeInfo)
        {
            viewRank.Add(new KeyValuePair<string, int>(tmp.Key, tmp.Value.api_view));
        }

        //������ ���� ���� �ű��
        vidCntRank.Sort(delegate (KeyValuePair<string, int> a, KeyValuePair<string, int> b) {
            return a.Value.CompareTo(b.Value);
        });
        vidCntRank.Reverse();

        //��ȸ���� ���� �ű��
        viewRank.Sort(delegate (KeyValuePair<string, int> a, KeyValuePair<string, int> b) {
            return a.Value.CompareTo(b.Value);
        });
        viewRank.Reverse();
    }
    public int getVidCntRank(string code)
    {

        int ans = 1;
        foreach (var tmp in vidCntRank)
        {
            if(code == tmp.Key){ break; }
            ans++;
        }
        return ans;
    }
    public int getViewRank(string code)
    {
        int ans = 1;
        foreach (var tmp in viewRank)
        {
            if (code == tmp.Key) { break; }
            ans++;
        }
        return ans;
    }
    public void eraseData()
    {
        apiInfo.Clear();
        youtubeInfo.Clear();
        sentimentInfo.Clear();
        vidCntRank.Clear();
        viewRank.Clear();
    }
}
