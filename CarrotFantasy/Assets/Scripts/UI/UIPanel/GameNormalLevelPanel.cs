using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalLevelPanel : BasePanel
{
    public int currentBigLevelID;
    public int currentLevelID;
    private string filePath;

    private Transform levelContentTrans;
    private GameObject imgLockBtnGo;
    private Transform empTowerTrans;
    private Image imgBGLeft;
    private Image imgBGRight;
    private Image imgCarrot;
    private Image imgAllClear;
    private Text texTotalWaves;

    private PlayerManager m_playerManager;
    private SlideScrollView m_slideScrollView;

    private List<GameObject> m_levelContentImageList;
    private List<GameObject> m_towerContentImageList;

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

    private void LoadResource()
    {

    }

    public void ToThisPanel(int currentBigLevel)
    {
        currentBigLevelID = currentBigLevel;
        EnterPanel();
    }
}
