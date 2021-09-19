using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using TMPro;
public class InGameControl : MonoBehaviour
{
    public StockList list; //종목API정보 저장자료
    public portfolio myPortfolio; //포트폴리오 정보 저장자료

    public GameObject SubMenu; //서브메뉴창
    public GameObject EditPage; //포트폴리오 정보 수정 페이지
    public GameObject InfoOptPage; //정보범위 수정 페이지
    public GameObject OptPage; //정보범위 수정 페이지
    public GameObject StockInfoPage; //종목 상세 정보 페이지

    public Slider fearAndGreedIndex;//공포와 탐욕 지수 
    public Image stateImg; //상태별 아이콘 이미지
    public Image fill; //게이지 색상

    public GameObject totalGainPage; //포트폴리오 정보 페이지
    public GameObject content; //포트폴리오 정보를 넣는 오브젝트
    public TextMeshProUGUI totalGainInfo; //자산 정보를 입력하는 텍스트 UI

    public GameObject totalRankPage; //인기 순위 정보 페이지
    public GameObject content1; //포트폴리오 정보를 넣는 오브젝트
    
    public Toggle priceOpt; //주가 정보 표시하는 옵션
    public Toggle gainOpt; //주가 정보 표시하는 옵션
    public Toggle volumeOpt; //거래량 정보 표시하는 옵션
    public Toggle divOpt; //배당 정보 표시하는 옵션
    public Toggle vidCntOpt; // 영상 갯수 기준 등수 표시하는 옵션
    public Toggle viewCntOpt; // 영상 조회수 기준 등수 표시하는 옵션
    public Toggle newsSentiOpt; // 뉴스 감성분석 정보를 표시하는 옵션

    public Toggle myStockOpt; //내 종목만 표시하는 옵션
    public Toggle layerOpt; //내 종목만 투명하게 표시하는 옵션
    public Toggle weatherOpt; //포트폴리오 이익과 연동되는 날씨 옵션
    public Toggle marketTimeOpt; //주식 시장 시간과 연동하는 시간 옵션 

    public bool priceFlag;
    public bool gainFlag;
    public bool volumeFlag;
    public bool divFlag;
    public bool vidCntFlag;
    public bool viewCntFlag;
    public bool newsSentiFlag;

    public bool myStockFlag;
    public bool layerFlag;
    public bool weatherFlag;
    public bool marketTimeFlag;
    public bool pagePopUp; //창이 현재 게임화면에 떠있는지 확인하는 bool변수
    public TextMeshProUGUI optInfo; //선택한 레이어 정보 표시창

    public GameObject building;

    private List<Vector3> scales = new List<Vector3>();//종목의 초기 디폴트 스케일 설정 저장 배열
    private float totalGainChange;//이전 총 자산 대비 총 자산 비율

    void Start()
    {
        optInfo.text = "레이어를 선택하세요";
        //종목의 스케일 정보 저장
        for (int i = 0; i < building.transform.childCount; i++)
        {
            Vector3 tmp = building.transform.GetChild(i).gameObject.transform.localScale;
            scales.Add(new Vector3(tmp.x, tmp.y, tmp.z));
        }
        Load();//저장된 데이터를 불러온 뒤 날씨 제어
        checkOpt();
        list.rankUpdate();
        pagePopUp = false;
        totalBtnClick();
        weatherOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(weatherOpt); });
        myStockOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(myStockOpt); });
        priceOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(priceOpt); });
        gainOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(gainOpt);});
        volumeOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(volumeOpt); });
        divOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(divOpt); });
        vidCntOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(vidCntOpt); });
        viewCntOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(viewCntOpt); });
        newsSentiOpt.onValueChanged.AddListener(delegate { ToggleValueChanged(newsSentiOpt); });
    }
    void Update()
    {
        checkIndex();//공포와 탐욕지수 게이지 표시
        checkOpt(); //켜진 옵션 확인
    }
    void checkIndex()
    {
        if (list.fearAndGreed < 50) {
            //종목에 해당하는 로고 사진 첨부
            stateImg.sprite = Resources.Load("Prefabs/etc/fearIcon", typeof(Sprite)) as Sprite;
            fill.sprite = Resources.Load("Prefabs/etc/fear", typeof(Sprite)) as Sprite;
        }
        else
        {
            //종목에 해당하는 로고 사진 첨부
            stateImg.sprite = Resources.Load("Prefabs/etc/greedIcon", typeof(Sprite)) as Sprite;
            fill.sprite = Resources.Load("Prefabs/etc/greed", typeof(Sprite)) as Sprite;
        }
        fearAndGreedIndex.value = list.fearAndGreed;
    }
    void checkOpt()//매번 옵션 선택 확인
    {
        myStockFlag = myStockOpt.isOn;
        layerFlag = layerOpt.isOn;
        weatherFlag = weatherOpt.isOn;
        marketTimeFlag = marketTimeOpt.isOn;

        priceFlag = priceOpt.isOn;
        gainFlag = gainOpt.isOn; 
        volumeFlag = volumeOpt.isOn; 
        divFlag = divOpt.isOn; 
        vidCntFlag = vidCntOpt.isOn;
        viewCntFlag = viewCntOpt.isOn;
        newsSentiFlag = newsSentiOpt.isOn;

        if (priceFlag) { optInfo.text = "주가 변화율"; }
        else if(gainFlag){optInfo.text = "수익율";}
        else if (volumeFlag){ optInfo.text = "거래량"; }
        else if (divFlag) { optInfo.text = "배당 정보"; }
        else if (vidCntFlag) { optInfo.text = "비디오 수 랭킹"; }
        else if (viewCntFlag) { optInfo.text = "조회수 랭킹"; }
        else if (newsSentiFlag) { optInfo.text = "뉴스 감성 분석"; }
        else { optInfo.text = "레이어를 선택하세요"; }
    }
    void ToggleValueChanged(Toggle change)
    {
        if (change == myStockOpt)
        {
            if (change.isOn)//내 보유 종목만 보는 설정
            {
                portfolioBtnClick();
            }
            else//전체 주식 시장 종목들만 보는 설정 
            {
                totalBtnClick();
            }
            return;
        }
        if(change == weatherOpt)
        {
            if (change.isOn)//이전 자산 대비 변화비율에 따라 날씨 결정
            {
                if (totalGainChange < -50)
                {
                    GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().RainScript.RainIntensity = 10f;
                }
                else if ((-50 < totalGainChange) && (totalGainChange < -30))
                {
                    GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().RainScript.RainIntensity = 5f;
                }
                else if ((-30 < totalGainChange) && (totalGainChange < 0))
                {
                    GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().RainScript.RainIntensity = 2.5f;
                }
                else
                {
                    GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().RainScript.RainIntensity = 0f;
                }
            }
            else// 기본 설정인 화창한 날로 설정
            {
                GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().RainScript.RainIntensity = 0f;
            }
            return;
        }
        if (change.isOn)
        {
            optInfo.text = "";
            offToggleExcept(change);
        }
    }
    void offToggleExcept(Toggle change)
    {
        if (change == priceOpt)
        {
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            vidCntOpt.isOn = false;
            viewCntOpt.isOn = false;
            gainOpt.isOn = false;
            priceOpt.isOn = true;
            newsSentiOpt.isOn = false;
        }
        else if (change == gainOpt)
        {
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            vidCntOpt.isOn = false;
            viewCntOpt.isOn = false;
            gainOpt.isOn = true;
            priceOpt.isOn = false;
            newsSentiOpt.isOn = false;
        }
        else if (change == volumeOpt)
        {
            gainOpt.isOn = false;
            divOpt.isOn = false;
            vidCntOpt.isOn = false;
            viewCntOpt.isOn = false;
            volumeOpt.isOn = true;
            priceOpt.isOn = false;
            newsSentiOpt.isOn = false;
        }
        else if (change == divOpt)
        {
            volumeOpt.isOn = false;
            gainOpt.isOn = false;
            vidCntOpt.isOn = false;
            viewCntOpt.isOn = false;
            divOpt.isOn = true;
            priceOpt.isOn = false;
            newsSentiOpt.isOn = false;
        }
        else if(change == vidCntOpt)
        {
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            gainOpt.isOn = false;
            vidCntOpt.isOn = true;
            viewCntOpt.isOn = false;
            priceOpt.isOn = false;
            newsSentiOpt.isOn = false;
        }
        else if (change == newsSentiOpt)
        {
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            gainOpt.isOn = false;
            vidCntOpt.isOn = false;
            viewCntOpt.isOn = false;
            priceOpt.isOn = false;
            newsSentiOpt.isOn = true;
        }
        else
        {
            volumeOpt.isOn = false;
            divOpt.isOn = false;
            gainOpt.isOn = false;
            vidCntOpt.isOn = false;
            viewCntOpt.isOn = true;
            priceOpt.isOn = false;
            newsSentiOpt.isOn = false;
        }
    }
    public void SubMenuBtnClick()
    {
        if (EditPage.activeSelf || OptPage.activeSelf)
        {
            EditPage.SetActive(false);
            OptPage.SetActive(false);
            pagePopUp = false;
            return;
        }
        //서브메뉴가 화면에 없으면 창 띄우기
        if (!SubMenu.activeSelf)
        {
            //다른 창을 다 내리기
            totalGainPage.SetActive(false);
            totalRankPage.SetActive(false);
            StockInfoPage.SetActive(false);
            InfoOptPage.SetActive(false);
            SubMenu.SetActive(true);
            pagePopUp = true;
        }
        //서브메뉴가 화면에 있으면 창내리기
        else
        {
            SubMenu.SetActive(false);
            pagePopUp = false;
        }
    }
    //total버튼을 누르면 인게임이 전체 주식시장 모드로 전환됨
    public void totalBtnClick()
    {
        GameObject stock;
        GameObject effect;
        string key;
        float tmp_cap = 0;
        float scale = 1f;

        //보유한 종목만 말풍선과 객체 활성화
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            stock.SetActive(true);
            effect.SetActive(true);
            stock.transform.localScale = new Vector3(scales[i].x, scales[i].y, scales[i].z);
            /*총 시가총액 만큼 객체 스케일 조정하기
            if (list.apiInfo.ContainsKey(key))
            {
                tmp_cap = list.apiInfo[key].api_marketcap / 1000000000000;
                if (tmp_cap >= 0.9) { scale = 1.25f; }
                else if (tmp_cap < 0.9 && tmp_cap >= 0.3) { scale = 1f; }
                else if (tmp_cap < 0.3) { scale = 0.75f; }
                stock.transform.localScale = new Vector3(scale * scales[i].x,scale * scales[i].y,scale * scales[i].z);
            }
            else
            {
                stock.transform.localScale = new Vector3(scales[i].x, scales[i].y, scales[i].z);
            }
            */
        }
    }

    //portfolio모드인 경우 인게임이 포트폴리오 모드로 전환됨
    public void portfolioBtnClick()
    {
        GameObject stock;
        GameObject effect;
        float total = 0;//전체 주식평가금액 저장변수
        string key;

        //전체 평가금액 계산
        foreach (var key1 in myPortfolio.stockInfo.Keys.ToList())
        {
            //개별 종목의 보유수량이 0개인 경우 카운트에서 제외
            if (myPortfolio.stockInfo[key1].shares == 0) { continue; }
            total += myPortfolio.updateGain(key1);
        }
        //보유한 종목만 말풍선과 객체 활성화
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            //보유하지 않는 경우 비활성화
            if(!myPortfolio.stockInfo.ContainsKey(key)){
                stock.SetActive(false);
                effect.SetActive(false);
            }
            else
            {
                if (myPortfolio.stockInfo[key].shares == 0) {
                    stock.SetActive(false);
                    effect.SetActive(false); 
                    continue; 
                }
                stock.SetActive(true);
                effect.SetActive(true);
                //총 자산 대비 종목 비중 만큼 객체 스케일 조정하기
                float ratio = myPortfolio.updateGain(key) / total * 100;
                float scale = 1f;
                if (ratio < 30) { scale = 0.75f; }
                else if (ratio >= 30 && ratio < 70) { scale = 1f; }
                else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }
                stock.transform.localScale = new Vector3(scale * scales[i].x, scale * scales[i].y, scale * scales[i].z);
            }
        }
    }

    //종목 추가버튼을 누르면 인게임이 보유종목 수정 페이지로 전환됨
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //정보 설정버튼을 누르면 인게임내 정보표시 설정 페이지로 전환됨
    public void InfoOptBtnClick()
    {
        if (InfoOptPage.activeSelf)
        {
            InfoOptPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //다른 창을 다 내리기
            SubMenu.SetActive(false);
            EditPage.SetActive(false);
            InfoOptPage.SetActive(false);
            StockInfoPage.SetActive(false);
            OptPage.SetActive(false);
            totalGainPage.SetActive(false);
            totalRankPage.SetActive(false);
            pagePopUp = true;
            InfoOptPage.SetActive(true);
        }
    }
    //옵션 버튼을 누르면 인게임내 설정 페이지로 전환됨
    public void OptBtnClick()
    {
        SubMenu.SetActive(false);
        OptPage.SetActive(true);
    }
    //나의 포트폴리오 종목 정보를 리스트뷰로 볼 수 있는 정보창이 나옴
    public void myPortfolioBtnClick()
    {

        if (totalGainPage.activeSelf)//이미 켜져있으면 꺼지고 꺼져있으면 켜지도록 제어
        {
            totalGainPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //다른 창을 다 내리기
            SubMenu.SetActive(false);
            EditPage.SetActive(false);
            InfoOptPage.SetActive(false);
            StockInfoPage.SetActive(false);
            OptPage.SetActive(false);
            totalGainPage.SetActive(false);
            totalRankPage.SetActive(false);
            pagePopUp = true;
            totalGainPage.SetActive(true);
            for (int i = 0; i < content.transform.childCount; i++)
            {
                Destroy(content.transform.GetChild(i).gameObject);
            }
            GameObject tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
            tmp.transform.SetParent(content.transform, false);

            float gainSum = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().totalGainSet();
            float tmp1 = 0;
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                tmp1 += myPortfolio.stockInfo[key].avgCostPerShare * myPortfolio.stockInfo[key].shares;
            }
            if (tmp1 != 0) { tmp1 = (gainSum - tmp1) / tmp1 * 100; }
            totalGainInfo.text = "자산: $" + gainSum.ToString("F2");
            totalGainInfo.text += " 수익률: " + tmp1.ToString("F2") + "%";
            float tmp2 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divGainSet();
            totalGainInfo.text += " 배당액: $" + tmp2.ToString("F2");
            
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                //개별 종목의 보유수량이 0개인 경우 배치에서 제외
                if (myPortfolio.stockInfo[key].shares == 0) { continue; }
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/slot"));
                tmp.transform.SetParent(content.transform, false);
                tmp.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Sprites/" + key, typeof(Sprite)) as Sprite;

                int cnt = myPortfolio.stockInfo[key].shares;//보유수량
                float market = myPortfolio.updateGain(key);//현재 평가금액
                float apc = myPortfolio.stockInfo[key].avgCostPerShare;//평균 단가

                float current = list.apiInfo[key].api_marketprice;//현재 주가
                float gainPercent = (market - apc * cnt) / (apc * cnt) * 100;//수익률
                float div = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().dividend(key);

                TextMeshProUGUI info = tmp.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                info.text = "종목 코드: " + key + "\n";
                info.text += "현재 주가: $" + current.ToString("F2") + "\n";
                info.text += "평균 단가: $" + apc.ToString("F2") + "\n";
                info.text += "보유 수량: " + cnt.ToString() + "개\n";
                info.text += "수익률: " + gainPercent.ToString("F2") + "%\n";
                info.text += "평가금액: $" + market.ToString("F2");
                if (div != 0) {
                    info.text += "\n예상 배당액: $" + div.ToString("F2");
                }
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);

            }
        }
    }    //유튜브 관련 통계 정보를 리스트뷰로 볼 수 있는 정보창이 나옴
    public void rankBtnClick()
    {

        if (totalRankPage.activeSelf)//이미 켜져있으면 꺼지고 꺼져있으면 켜지도록 제어
        {
            totalRankPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //다른 창을 다 내리기
            SubMenu.SetActive(false);
            EditPage.SetActive(false);
            InfoOptPage.SetActive(false);
            StockInfoPage.SetActive(false);
            OptPage.SetActive(false);
            totalGainPage.SetActive(false);
            pagePopUp = true;
            totalRankPage.SetActive(true);

            for (int i = 0; i < content1.transform.childCount; i++)
            {
                Destroy(content1.transform.GetChild(i).gameObject);
            }
            GameObject tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
            tmp.transform.SetParent(content1.transform, false);
            
            foreach(var key in list.vidCntRank)
            {
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/slot"));
                tmp.transform.SetParent(content1.transform, false);
                tmp.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Sprites/" + key.Key, typeof(Sprite)) as Sprite;

                TextMeshProUGUI info = tmp.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                int viewCnt = list.youtubeInfo[key.Key].api_view;
                int likeCnt = list.youtubeInfo[key.Key].api_like;
                int dislikeCnt = list.youtubeInfo[key.Key].api_dislike;
                int cmtCnt = list.youtubeInfo[key.Key].api_comment;
                int vidCnt = list.youtubeInfo[key.Key].api_cnt;

                info.text = "종목 코드: " + key.Key + "\n";
                info.text += "조회수: " + viewCnt.ToString() + "\n";
                info.text += "좋아요: " + likeCnt.ToString() + "\n";
                info.text += "싫어요: " + dislikeCnt.ToString() + "\n";
                info.text += "댓글 수: " + cmtCnt.ToString() + "\n";
                info.text += "영상 갯수: " + vidCnt.ToString() + "\n";

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content1.transform, false);
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content1.transform, false);
            }
        }
    }
    public void UpdateBtnClick()
    {
        //게임을 저장하고 갱신
        Save();
        list.eraseData();
        myPortfolio.eraseData();
        SceneManager.LoadScene("Load");
    }
    //서브페이지에서의 Back버튼을 누르면 서브메뉴 페이지로 전환됨
    public void QuitBtnClick()
    {
        //서브메뉴 화면으로 이동
        EditPage.SetActive(false);
        OptPage.SetActive(false);
        SubMenu.SetActive(true);
    }
    public void ExitBtnClick()
    {
        //게임을 저장하고 메인 메뉴 화면으로 이동
        Save();
        SceneManager.LoadScene("MainMenu");
    }
    //저장을 위한 자료구조
    [Serializable]
    public class stockInfo
    {
        public string code;//종목코드
        public int shares;//보유수량
        public float apc;//평단가
    }
    public class savData
    {
        public float Cash;
        public List<stockInfo> stocks;
    }
    //저장을 위한 자료구조
    [Serializable]
    public class stockApiInfo
    {
        public string code;
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
    }
    public class savApiData
    {
        public List<stockApiInfo> stocksApi;
    }//저장을 위한 자료구조
    [Serializable]
    public class youtubeInfo
    {
        public string code;
        public int api_cnt; //일주일 간 1만 조회수 이상인 영상 수
        public int api_view; //일주일 간 관련 영상 조회수 합산
        public int api_like; //일주일 간 관련 영상 좋아요수 합산
        public int api_dislike; //일주일 간 관련 영상 싫어요수 합산
        public int api_comment; //일주일 간 관련 영상 댓글수 합산
    }
    public class savYoutubeData
    {
        public List<youtubeInfo> youtubeInfos;
    }
    void Load()
    {
        //Json파일 가져온 다음에 게임과 연동시키기 
        try
        {
            //1. API정보 로드
            yahooApiLoad();
            //2. 유저 포트폴리오 정보 로드
            portfolioLoad();
            //3. 유튜브 정보 가져오기
            youtubeApiLoad();
        }
        catch (Exception e)
        {
            totalGainChange = 0;
        }
    }
    void yahooApiLoad()
    {
        string path = Application.dataPath + "/" + "api" + ".Json";
        string json = File.ReadAllText(path);
        savApiData loadData = JsonUtility.FromJson<savApiData>(json);
        foreach (var tmp in loadData.stocksApi)
        {
            list.add(tmp.code, tmp.api_marketprice, tmp.api_divDate, tmp.api_divRate, tmp.api_sector, tmp.api_marketcap, tmp.api_per, tmp.api_52week, tmp.api_preclose, tmp.api_volume, tmp.api_avgVolume);
        }
    }
    void youtubeApiLoad()
    {
        string path = Application.dataPath + "/" + "youtube" + ".Json";
        string json = File.ReadAllText(path);
        savYoutubeData loadData = JsonUtility.FromJson<savYoutubeData>(json);
        foreach (var tmp in loadData.youtubeInfos)
        {
            list.addYoutube(tmp.code, tmp.api_cnt, tmp.api_view, tmp.api_like, tmp.api_dislike, tmp.api_comment);
        }
    }
    void portfolioLoad()
    {
        string path = Application.dataPath + "/" + "user1" + ".Json";
        string json = File.ReadAllText(path);
        savData loadData = JsonUtility.FromJson<savData>(json);
        float preTotalGain = 0f;
        float totalGain = 0f;
        myPortfolio.Cash = loadData.Cash;
        foreach (var tmp in loadData.stocks)
        {
            preTotalGain += tmp.shares * tmp.apc;
            myPortfolio.addTrade(tmp.code, "0000-0-0", tmp.shares, tmp.apc, false);
        }
        //현재 업데이트 된 포트폴리오 평가금액 갱신
        foreach (var tmp in loadData.stocks)
        {
            totalGain += myPortfolio.updateGain(tmp.code);
        }
        //현재 업데이트 된 포트폴리오 평가금액과 이 전 포트폴리오 평가금액 비교하여 날씨 파악
        float totalGainChange = (totalGain - preTotalGain) / preTotalGain * 100;
        Debug.Log(totalGainChange);
    }
    void Save()
    {
        //1. 유저 포트폴리오 정보 저장하기
        portfolioSave();

        //2. api 정보 저장하기
        //yahooApiSave();

        //3. api 정보 저장하기
        //youtubeApiSave();
    }
    void portfolioSave()
    {
        savData sav = new savData();
        sav.Cash = myPortfolio.Cash;
        sav.stocks = new List<stockInfo>();
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //개별 종목의 보유수량이 0개인 경우 저장 데이터에서 제외
            if (myPortfolio.stockInfo[key].shares == 0) { continue; }
            stockInfo tmp = new stockInfo();
            tmp.code = key;
            tmp.shares = myPortfolio.stockInfo[key].shares;
            tmp.apc = myPortfolio.stockInfo[key].avgCostPerShare;
            sav.stocks.Add(tmp);
        }
        string json = JsonUtility.ToJson(sav);
        string path = Application.dataPath + "/" + "user1" + ".Json";
        File.WriteAllText(path, json);
    }
    void yahooApiSave()
    {
        savApiData sav = new savApiData();
        sav.stocksApi = new List<stockApiInfo>();
        foreach (var key in list.apiInfo.Keys.ToList())
        {
            stockApiInfo tmp = new stockApiInfo();
            tmp.code = key;
            tmp.api_marketprice = list.apiInfo[key].api_marketprice;
            tmp.api_divDate = list.apiInfo[key].api_divDate;
            tmp.api_divRate = list.apiInfo[key].api_divRate;
            tmp.api_sector = list.apiInfo[key].api_sector;
            tmp.api_marketcap = list.apiInfo[key].api_marketcap;
            tmp.api_per = list.apiInfo[key].api_per;
            tmp.api_52week = list.apiInfo[key].api_52week;
            tmp.api_preclose = list.apiInfo[key].api_preclose;
            tmp.api_volume = list.apiInfo[key].api_volume;
            tmp.api_avgVolume = list.apiInfo[key].api_avgVolume;
            sav.stocksApi.Add(tmp);
        }
        string json = JsonUtility.ToJson(sav);
        string path = Application.dataPath + "/" + "api" + ".Json";
        File.WriteAllText(path, json);
    }
    void youtubeApiSave()
    {
        savYoutubeData sav = new savYoutubeData();
        sav.youtubeInfos = new List<youtubeInfo>();
        foreach (var key in list.youtubeInfo.Keys.ToList())
        {
            youtubeInfo tmp = new youtubeInfo();
            tmp.code = key;
            tmp.api_cnt = list.youtubeInfo[key].api_cnt;
            tmp.api_view = list.youtubeInfo[key].api_view;
            tmp.api_like = list.youtubeInfo[key].api_like;
            tmp.api_dislike = list.youtubeInfo[key].api_dislike;
            tmp.api_comment = list.youtubeInfo[key].api_comment;
            sav.youtubeInfos.Add(tmp);
        }
        string json = JsonUtility.ToJson(sav);
        string path = Application.dataPath + "/" + "youtube" + ".Json";
        File.WriteAllText(path, json);
    }
}
