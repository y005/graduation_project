using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "StockList")]
public class StockList : ScriptableObject
{
    //주식시장의 공포와 탐욕지수
    public int fearAndGreed;
    //종목코드와 얻은 yahoo finance api 자료를 저장한 딕셔너리
    public Dictionary<string, APIData> apiInfo = new Dictionary<string, APIData>();
    //종목코드와 얻은 youtube data v3 api 자료를 저장한 딕셔너리
    public Dictionary<string, youtubeData> youtubeInfo = new Dictionary<string, youtubeData>();
    //종목코드와 관련 뉴스 기사들의 감성 비율을 저장한 딕셔너리
    public Dictionary<string, sentimentData> sentimentInfo = new Dictionary<string, sentimentData>();
    //관심도 순위(최근 한 달 간 1만 조회수 이상인 동영상의 갯수로 카운트) 
    public List<KeyValuePair<string, int>> vidCntRank = new List<KeyValuePair<string, int>>();
    //관심도 순위(최근 한 달 간 1만 조회수 이상인 동영상의 조회수 합으로 카운트) 
    public List<KeyValuePair<string, int>> viewRank = new List<KeyValuePair<string, int>>();

    //1. yahoo finance api에서 불러온 데이터 저장하기 위한 자료
    public class APIData 
    {
        public float api_marketprice; //현재 시가
        public string api_divDate; //배당일
        public float api_divRate; //배당률
        public string api_sector; //관련 업종별 분류
        public float api_marketcap; //시가총액
        public float api_per; //PER
        public float api_52week; //시가 성장 변화(52 week change)
        public float api_preclose; //이전 마감가
        public float api_volume; //거래량
        public float api_avgVolume; //평균 10일 거래량

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
    //2. youtube data v3 api에서 불러온 데이터 저장하기 위한 자료
    public class youtubeData
    {
        public int api_cnt; //일주일 간 1만 조회수 이상인 영상 수
        public int api_view; //일주일 간 관련 영상 조회수 합산
        public int api_like; //일주일 간 관련 영상 좋아요수 합산
        public int api_dislike; //일주일 간 관련 영상 싫어요수 합산
        public int api_comment; //일주일 간 관련 영상 댓글수 합산

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
    //3. ec2서버 api에서 불러온 데이터 저장하기 위한 자료
    public class sentimentData
    {
        public int api_positive; //긍정감성으로 분류된 뉴스 기사 비율
        public int api_negative; //부정감성으로 분류된 뉴스 기사 비율

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

        //동영상 수로 순위 매기기
        vidCntRank.Sort(delegate (KeyValuePair<string, int> a, KeyValuePair<string, int> b) {
            return a.Value.CompareTo(b.Value);
        });
        vidCntRank.Reverse();

        //조회수로 순위 매기기
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
