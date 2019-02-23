using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalLevelPanel : BasePanel
{
    public int currentBigLevelID;   //大关卡ID
    public int currentLevelID;      //小关卡ID
    private string filePath;        //资源加载路径

    private Transform levelContentTrans;    //关卡Content
    private GameObject imgLockBtnGo;        //开始按钮上关卡锁定遮罩对象
    private Transform empTowerTrans;        //关卡能使用的炮塔父对象
    private Image imgBGLeft;                //左边背景图片
    private Image imgBGRight;               //右边背景图片
    private Image imgCarrot;                //萝卜星级图片
    private Image imgAllClear;              //道具清除图片
    private Text texTotalWaves;             //怪物波数

    private PlayerManager m_playerManager;
    private SlideScrollView m_slideScrollView;

    private List<GameObject> m_levelContentImageList;
    private List<GameObject> m_towerContentImageList;

    private string m_spritePath;

    protected override void Awake()
    {
        base.Awake();
        filePath = "GameOption/Normal/Level/";
        m_playerManager = m_uiFacade.m_playerManager;
        m_levelContentImageList = new List<GameObject>();
        m_towerContentImageList = new List<GameObject>();
        levelContentTrans = transform.Find("Scroll View").Find("Viewport").Find("Content").transform;
        imgLockBtnGo = transform.Find("Img_LockBtn").gameObject;
        empTowerTrans = transform.Find("Emp_Tower");
        imgBGLeft = transform.Find("Img_BGLeft").GetComponent<Image>();
        imgBGRight = transform.Find("Img_BGRight").GetComponent<Image>();
        texTotalWaves = transform.Find("Img_TotalWaves").Find("Text").GetComponent<Text>();
        m_slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();
        currentBigLevelID = 1;
        currentLevelID = 1;
    }

    public override void InitPanel()
    {
        gameObject.SetActive(false);
    }

    public override void EnterPanel()
    {
        gameObject.SetActive(true);
        DestroyMapUI();
        m_spritePath = filePath + currentBigLevelID.ToString() + "/";
        //Debug.Log(m_spritePath);
        UpdateMapUI(m_spritePath);
        UpdateLevelUI(m_spritePath);
        m_slideScrollView.Init();
    }

    public override void UpdatePanel()
    {
        UpdateLevelUI(m_spritePath);
    }

    public override void ExitPanel()
    {
        gameObject.SetActive(false);
    }

    public void EnterGamePanel()
    {
        GameManager.Instance.m_currentStage = m_playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
        m_uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        m_uiFacade.ChangeSceneState(new NormalModelSceneState(m_uiFacade));
    }

    private void LoadResource()
    {
        m_uiFacade.GetSprite(filePath + "AllClear");
        m_uiFacade.GetSprite(filePath + "Carrot_1");
        m_uiFacade.GetSprite(filePath + "Carrot_2");
        m_uiFacade.GetSprite(filePath + "Carrot_3");
        for (int i = 1; i <= 3; i++)
        {
            string levelPath = filePath + i.ToString() + "/";
            m_uiFacade.GetSprite(levelPath + "BG_Left");
            m_uiFacade.GetSprite(levelPath + "BG_Right");
            for (int j = 1; j <= 5; j++)
            {
                m_uiFacade.GetSprite(levelPath + "Level_" + j.ToString());
            }
        }
        for (int i = 1; i <= 12; i++)
        {
            m_uiFacade.GetSprite(filePath + "Tower/Tower_" + i.ToString());
        }
    }

    //更新静态UI（按钮遮罩、关卡可以用的炮塔）
    public void UpdateLevelUI(string spritePath)
    {
        DestroyLevelUI();
        
        Stage stage = m_playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + currentLevelID - 1];
        //Debug.Log("allclear:" + stage.m_allClear)
        if (stage.m_unLocked) //已解锁
        {
            imgLockBtnGo.SetActive(false);
        }
        else
        {
            imgLockBtnGo.SetActive(true);
        }
        for (int i = 0; i < stage.m_towerIDListLength; i++)
        {
            m_towerContentImageList.Add(CreateUI("Img_Tower", empTowerTrans));
            m_towerContentImageList[i].GetComponent<Image>().sprite = m_uiFacade.GetSprite(filePath +
                "Tower/Tower_" + stage.m_towerIDList[i].ToString());
        }
    }

    //清除静态UI
    private void DestroyLevelUI()
    {
        if (m_towerContentImageList.Count != 0)
        {
            for (int i = 0; i < m_towerContentImageList.Count; i++)
            {
                m_towerContentImageList[i].GetComponent<Image>().sprite = null;
                m_uiFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Img_Tower", m_towerContentImageList[i]);
            }
            m_towerContentImageList.Clear();
        }
    }

    //更新地图UI（动态UI）
    public void UpdateMapUI(string spritePath)
    {
        imgBGLeft.sprite = m_uiFacade.GetSprite(spritePath + "BG_Left");
        imgBGRight.sprite = m_uiFacade.GetSprite(spritePath + "BG_Right");
        for (int i = 0; i < 5; i++)
        {
            m_levelContentImageList.Add(CreateUI("Img_LevelMap", levelContentTrans));
            //更换关卡图片
            m_levelContentImageList[i].GetComponent<Image>().sprite = m_uiFacade.GetSprite(spritePath + "Level_" + (i + 1).ToString());
            m_levelContentImageList[i].transform.Find("Img_Carrot").gameObject.SetActive(false);
            m_levelContentImageList[i].transform.Find("Img_AllClear").gameObject.SetActive(false);
            Stage stage = m_playerManager.unLockedNormalModelLevelList[(currentBigLevelID - 1) * 5 + i];
            if (stage.m_unLocked) //已解锁
            {
                if (stage.m_allClear)
                {
                    m_levelContentImageList[i].transform.Find("Img_AllClear").gameObject.SetActive(true);
                }
                if (stage.m_carrotState != 0) //已通关
                {
                    Image carrotImg = m_levelContentImageList[i].transform.Find("Img_Carrot").GetComponent<Image>();
                    carrotImg.gameObject.SetActive(true);
                    carrotImg.sprite = m_uiFacade.GetSprite(filePath + "Carrot_" + stage.m_carrotState.ToString());
                }
                m_levelContentImageList[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                m_levelContentImageList[i].transform.Find("Img_BG").gameObject.SetActive(false);
            }
            else //未解锁
            {
                if (stage.m_isRewardLevel)//奖励关卡
                {
                    m_levelContentImageList[i].transform.Find("Img_Lock").gameObject.SetActive(false);
                    m_levelContentImageList[i].transform.Find("Img_BG").gameObject.SetActive(true);
                    Image monsterImg = m_levelContentImageList[i].transform.Find("Img_Monster").GetComponent<Image>();
                    monsterImg.sprite = m_uiFacade.GetSprite("MonsterNest/Monster/Baby/" + currentBigLevelID.ToString());
                    monsterImg.SetNativeSize();
                    monsterImg.transform.localScale = new Vector3(2, 2, 2);
                }
                else
                {
                    m_levelContentImageList[i].transform.Find("Img_Lock").gameObject.SetActive(true);
                    m_levelContentImageList[i].transform.Find("Img_BG").gameObject.SetActive(false);
                }
            }
        }
        m_slideScrollView.SetContent(5);
    }

    public void DestroyMapUI()
    {
        if (m_levelContentImageList.Count > 0)
        {
            for (int i = 0; i < 5; i++)
            {
                m_uiFacade.PushGameObjectToFactory(FactoryType.UIFactory, "Img_LevelMap", m_levelContentImageList[i]);
            }
            m_slideScrollView.InitContent();
            m_levelContentImageList.Clear();
        }
    }

    public GameObject CreateUI(string uiName, Transform parentTrans)
    {
        GameObject itemGo = m_uiFacade.GetGameObjectResource(FactoryType.UIFactory, uiName);
        itemGo.SetActive(parentTrans);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        return itemGo;
    }

    public void ToThisPanel(int currentBigLevel)
    {
        currentBigLevelID = currentBigLevel;
        Debug.Log(currentBigLevelID);
        currentLevelID = 1;
        EnterPanel();
    }

    public void ToNextStage()
    {
        currentLevelID++;
        UpdateLevelUI(m_spritePath);
    }

    public void ToLastStage()
    {
        currentLevelID--;
        UpdateLevelUI(m_spritePath);
    }
}
