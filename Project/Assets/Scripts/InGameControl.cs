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
    public StockList list; //����API���� �����ڷ�
    public portfolio myPortfolio; //��Ʈ������ ���� �����ڷ�

    public GameObject SubMenu; //����޴�â
    public GameObject EditPage; //��Ʈ������ ���� ���� ������
    public GameObject InfoOptPage; //�������� ���� ������
    public GameObject OptPage; //�������� ���� ������
    public GameObject StockInfoPage; //���� �� ���� ������

    public Slider fearAndGreedIndex;//������ Ž�� ���� 
    public Image stateImg; //���º� ������ �̹���
    public Image fill; //������ ����

    public GameObject totalGainPage; //��Ʈ������ ���� ������
    public GameObject content; //��Ʈ������ ������ �ִ� ������Ʈ
    public TextMeshProUGUI totalGainInfo; //�ڻ� ������ �Է��ϴ� �ؽ�Ʈ UI

    public GameObject totalRankPage; //�α� ���� ���� ������
    public GameObject content1; //��Ʈ������ ������ �ִ� ������Ʈ
    
    public Toggle priceOpt; //�ְ� ���� ǥ���ϴ� �ɼ�
    public Toggle gainOpt; //�ְ� ���� ǥ���ϴ� �ɼ�
    public Toggle volumeOpt; //�ŷ��� ���� ǥ���ϴ� �ɼ�
    public Toggle divOpt; //��� ���� ǥ���ϴ� �ɼ�
    public Toggle vidCntOpt; // ���� ���� ���� ��� ǥ���ϴ� �ɼ�
    public Toggle viewCntOpt; // ���� ��ȸ�� ���� ��� ǥ���ϴ� �ɼ�
    public Toggle newsSentiOpt; // ���� �����м� ������ ǥ���ϴ� �ɼ�

    public Toggle myStockOpt; //�� ���� ǥ���ϴ� �ɼ�
    public Toggle layerOpt; //�� ���� �����ϰ� ǥ���ϴ� �ɼ�
    public Toggle weatherOpt; //��Ʈ������ ���Ͱ� �����Ǵ� ���� �ɼ�
    public Toggle marketTimeOpt; //�ֽ� ���� �ð��� �����ϴ� �ð� �ɼ� 

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
    public bool pagePopUp; //â�� ���� ����ȭ�鿡 ���ִ��� Ȯ���ϴ� bool����
    public TextMeshProUGUI optInfo; //������ ���̾� ���� ǥ��â

    public GameObject building;

    private List<Vector3> scales = new List<Vector3>();//������ �ʱ� ����Ʈ ������ ���� ���� �迭
    private float totalGainChange;//���� �� �ڻ� ��� �� �ڻ� ����

    void Start()
    {
        optInfo.text = "���̾ �����ϼ���";
        //������ ������ ���� ����
        for (int i = 0; i < building.transform.childCount; i++)
        {
            Vector3 tmp = building.transform.GetChild(i).gameObject.transform.localScale;
            scales.Add(new Vector3(tmp.x, tmp.y, tmp.z));
        }
        Load();//����� �����͸� �ҷ��� �� ���� ����
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
        checkIndex();//������ Ž������ ������ ǥ��
        checkOpt(); //���� �ɼ� Ȯ��
    }
    void checkIndex()
    {
        if (list.fearAndGreed < 50) {
            //���� �ش��ϴ� �ΰ� ���� ÷��
            stateImg.sprite = Resources.Load("Prefabs/etc/fearIcon", typeof(Sprite)) as Sprite;
            fill.sprite = Resources.Load("Prefabs/etc/fear", typeof(Sprite)) as Sprite;
        }
        else
        {
            //���� �ش��ϴ� �ΰ� ���� ÷��
            stateImg.sprite = Resources.Load("Prefabs/etc/greedIcon", typeof(Sprite)) as Sprite;
            fill.sprite = Resources.Load("Prefabs/etc/greed", typeof(Sprite)) as Sprite;
        }
        fearAndGreedIndex.value = list.fearAndGreed;
    }
    void checkOpt()//�Ź� �ɼ� ���� Ȯ��
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

        if (priceFlag) { optInfo.text = "�ְ� ��ȭ��"; }
        else if(gainFlag){optInfo.text = "������";}
        else if (volumeFlag){ optInfo.text = "�ŷ���"; }
        else if (divFlag) { optInfo.text = "��� ����"; }
        else if (vidCntFlag) { optInfo.text = "���� �� ��ŷ"; }
        else if (viewCntFlag) { optInfo.text = "��ȸ�� ��ŷ"; }
        else if (newsSentiFlag) { optInfo.text = "���� ���� �м�"; }
        else { optInfo.text = "���̾ �����ϼ���"; }
    }
    void ToggleValueChanged(Toggle change)
    {
        if (change == myStockOpt)
        {
            if (change.isOn)//�� ���� ���� ���� ����
            {
                portfolioBtnClick();
            }
            else//��ü �ֽ� ���� ����鸸 ���� ���� 
            {
                totalBtnClick();
            }
            return;
        }
        if(change == weatherOpt)
        {
            if (change.isOn)//���� �ڻ� ��� ��ȭ������ ���� ���� ����
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
            else// �⺻ ������ ȭâ�� ���� ����
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
        //����޴��� ȭ�鿡 ������ â ����
        if (!SubMenu.activeSelf)
        {
            //�ٸ� â�� �� ������
            totalGainPage.SetActive(false);
            totalRankPage.SetActive(false);
            StockInfoPage.SetActive(false);
            InfoOptPage.SetActive(false);
            SubMenu.SetActive(true);
            pagePopUp = true;
        }
        //����޴��� ȭ�鿡 ������ â������
        else
        {
            SubMenu.SetActive(false);
            pagePopUp = false;
        }
    }
    //total��ư�� ������ �ΰ����� ��ü �ֽĽ��� ���� ��ȯ��
    public void totalBtnClick()
    {
        GameObject stock;
        GameObject effect;
        string key;
        float tmp_cap = 0;
        float scale = 1f;

        //������ ���� ��ǳ���� ��ü Ȱ��ȭ
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            stock.SetActive(true);
            effect.SetActive(true);
            stock.transform.localScale = new Vector3(scales[i].x, scales[i].y, scales[i].z);
            /*�� �ð��Ѿ� ��ŭ ��ü ������ �����ϱ�
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

    //portfolio����� ��� �ΰ����� ��Ʈ������ ���� ��ȯ��
    public void portfolioBtnClick()
    {
        GameObject stock;
        GameObject effect;
        float total = 0;//��ü �ֽ��򰡱ݾ� ���庯��
        string key;

        //��ü �򰡱ݾ� ���
        foreach (var key1 in myPortfolio.stockInfo.Keys.ToList())
        {
            //���� ������ ���������� 0���� ��� ī��Ʈ���� ����
            if (myPortfolio.stockInfo[key1].shares == 0) { continue; }
            total += myPortfolio.updateGain(key1);
        }
        //������ ���� ��ǳ���� ��ü Ȱ��ȭ
        for (int i = 0; i < building.transform.childCount; i++)
        {
            stock = building.transform.GetChild(i).gameObject;
            key = stock.transform.name;
            effect = GameObject.Find("Canvas").transform.Find("effectUI").gameObject.transform.Find(key + "Effect").gameObject;

            //�������� �ʴ� ��� ��Ȱ��ȭ
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
                //�� �ڻ� ��� ���� ���� ��ŭ ��ü ������ �����ϱ�
                float ratio = myPortfolio.updateGain(key) / total * 100;
                float scale = 1f;
                if (ratio < 30) { scale = 0.75f; }
                else if (ratio >= 30 && ratio < 70) { scale = 1f; }
                else if (ratio >= 70 && ratio <= 100) { scale = 1.25f; }
                stock.transform.localScale = new Vector3(scale * scales[i].x, scale * scales[i].y, scale * scales[i].z);
            }
        }
    }

    //���� �߰���ư�� ������ �ΰ����� �������� ���� �������� ��ȯ��
    public void EditBtnClick()
    {
        SubMenu.SetActive(false);
        EditPage.SetActive(true);
    }
    //���� ������ư�� ������ �ΰ��ӳ� ����ǥ�� ���� �������� ��ȯ��
    public void InfoOptBtnClick()
    {
        if (InfoOptPage.activeSelf)
        {
            InfoOptPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //�ٸ� â�� �� ������
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
    //�ɼ� ��ư�� ������ �ΰ��ӳ� ���� �������� ��ȯ��
    public void OptBtnClick()
    {
        SubMenu.SetActive(false);
        OptPage.SetActive(true);
    }
    //���� ��Ʈ������ ���� ������ ����Ʈ��� �� �� �ִ� ����â�� ����
    public void myPortfolioBtnClick()
    {

        if (totalGainPage.activeSelf)//�̹� ���������� ������ ���������� �������� ����
        {
            totalGainPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //�ٸ� â�� �� ������
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
            totalGainInfo.text = "�ڻ�: $" + gainSum.ToString("F2");
            totalGainInfo.text += " ���ͷ�: " + tmp1.ToString("F2") + "%";
            float tmp2 = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().divGainSet();
            totalGainInfo.text += " ����: $" + tmp2.ToString("F2");
            
            foreach (var key in myPortfolio.stockInfo.Keys.ToList())
            {
                //���� ������ ���������� 0���� ��� ��ġ���� ����
                if (myPortfolio.stockInfo[key].shares == 0) { continue; }
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/slot"));
                tmp.transform.SetParent(content.transform, false);
                tmp.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load("Sprites/" + key, typeof(Sprite)) as Sprite;

                int cnt = myPortfolio.stockInfo[key].shares;//��������
                float market = myPortfolio.updateGain(key);//���� �򰡱ݾ�
                float apc = myPortfolio.stockInfo[key].avgCostPerShare;//��� �ܰ�

                float current = list.apiInfo[key].api_marketprice;//���� �ְ�
                float gainPercent = (market - apc * cnt) / (apc * cnt) * 100;//���ͷ�
                float div = GameObject.Find("Main Camera").GetComponent<DigitalRuby.RainMaker.DemoScript>().dividend(key);

                TextMeshProUGUI info = tmp.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

                info.text = "���� �ڵ�: " + key + "\n";
                info.text += "���� �ְ�: $" + current.ToString("F2") + "\n";
                info.text += "��� �ܰ�: $" + apc.ToString("F2") + "\n";
                info.text += "���� ����: " + cnt.ToString() + "��\n";
                info.text += "���ͷ�: " + gainPercent.ToString("F2") + "%\n";
                info.text += "�򰡱ݾ�: $" + market.ToString("F2");
                if (div != 0) {
                    info.text += "\n���� ����: $" + div.ToString("F2");
                }
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content.transform, false);

            }
        }
    }    //��Ʃ�� ���� ��� ������ ����Ʈ��� �� �� �ִ� ����â�� ����
    public void rankBtnClick()
    {

        if (totalRankPage.activeSelf)//�̹� ���������� ������ ���������� �������� ����
        {
            totalRankPage.SetActive(false);
            pagePopUp = false;
        }
        else
        {
            //�ٸ� â�� �� ������
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

                info.text = "���� �ڵ�: " + key.Key + "\n";
                info.text += "��ȸ��: " + viewCnt.ToString() + "\n";
                info.text += "���ƿ�: " + likeCnt.ToString() + "\n";
                info.text += "�Ⱦ��: " + dislikeCnt.ToString() + "\n";
                info.text += "��� ��: " + cmtCnt.ToString() + "\n";
                info.text += "���� ����: " + vidCnt.ToString() + "\n";

                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content1.transform, false);
                tmp = (GameObject)Instantiate(Resources.Load("Prefabs/etc/empty"));
                tmp.transform.SetParent(content1.transform, false);
            }
        }
    }
    public void UpdateBtnClick()
    {
        //������ �����ϰ� ����
        Save();
        list.eraseData();
        myPortfolio.eraseData();
        SceneManager.LoadScene("Load");
    }
    //���������������� Back��ư�� ������ ����޴� �������� ��ȯ��
    public void QuitBtnClick()
    {
        //����޴� ȭ������ �̵�
        EditPage.SetActive(false);
        OptPage.SetActive(false);
        SubMenu.SetActive(true);
    }
    public void ExitBtnClick()
    {
        //������ �����ϰ� ���� �޴� ȭ������ �̵�
        Save();
        SceneManager.LoadScene("MainMenu");
    }
    //������ ���� �ڷᱸ��
    [Serializable]
    public class stockInfo
    {
        public string code;//�����ڵ�
        public int shares;//��������
        public float apc;//��ܰ�
    }
    public class savData
    {
        public float Cash;
        public List<stockInfo> stocks;
    }
    //������ ���� �ڷᱸ��
    [Serializable]
    public class stockApiInfo
    {
        public string code;
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
    }
    public class savApiData
    {
        public List<stockApiInfo> stocksApi;
    }//������ ���� �ڷᱸ��
    [Serializable]
    public class youtubeInfo
    {
        public string code;
        public int api_cnt; //������ �� 1�� ��ȸ�� �̻��� ���� ��
        public int api_view; //������ �� ���� ���� ��ȸ�� �ջ�
        public int api_like; //������ �� ���� ���� ���ƿ�� �ջ�
        public int api_dislike; //������ �� ���� ���� �Ⱦ��� �ջ�
        public int api_comment; //������ �� ���� ���� ��ۼ� �ջ�
    }
    public class savYoutubeData
    {
        public List<youtubeInfo> youtubeInfos;
    }
    void Load()
    {
        //Json���� ������ ������ ���Ӱ� ������Ű�� 
        try
        {
            //1. API���� �ε�
            yahooApiLoad();
            //2. ���� ��Ʈ������ ���� �ε�
            portfolioLoad();
            //3. ��Ʃ�� ���� ��������
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
        //���� ������Ʈ �� ��Ʈ������ �򰡱ݾ� ����
        foreach (var tmp in loadData.stocks)
        {
            totalGain += myPortfolio.updateGain(tmp.code);
        }
        //���� ������Ʈ �� ��Ʈ������ �򰡱ݾװ� �� �� ��Ʈ������ �򰡱ݾ� ���Ͽ� ���� �ľ�
        float totalGainChange = (totalGain - preTotalGain) / preTotalGain * 100;
        Debug.Log(totalGainChange);
    }
    void Save()
    {
        //1. ���� ��Ʈ������ ���� �����ϱ�
        portfolioSave();

        //2. api ���� �����ϱ�
        //yahooApiSave();

        //3. api ���� �����ϱ�
        //youtubeApiSave();
    }
    void portfolioSave()
    {
        savData sav = new savData();
        sav.Cash = myPortfolio.Cash;
        sav.stocks = new List<stockInfo>();
        foreach (var key in myPortfolio.stockInfo.Keys.ToList())
        {
            //���� ������ ���������� 0���� ��� ���� �����Ϳ��� ����
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
