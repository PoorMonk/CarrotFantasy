using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameNormalBigLevelPanel : BasePanel
{
    public Transform bigLevelContentTrans;      //滚动视图的Content
    public int bigLevelPageCount;               //大关卡总数
    private SlideScrollView m_slideScrollView;
    private PlayerManager m_playerManager;
    private Transform[] bigLevelPage;           //大关卡按钮数组

    private bool hasRigisteredEvent;

    public void OnLeftBtnClicked()
    {
        m_slideScrollView.OnLeftPageBtnClicked();
    }

    public void OnRightBtnClicked()
    {
        m_slideScrollView.OnRightPageBtnClicked();
    }

    protected override void Awake()
    {
        base.Awake();
        m_playerManager = m_uiFacade.m_playerManager;
        bigLevelPage = new Transform[bigLevelPageCount];
        m_slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelInfo(m_playerManager.unLockedNormalModelBigLevelList[i],
                m_playerManager.unLockedNormalModelLevelNum[i], 5,
                bigLevelPage[i], i + 1);
        }
        hasRigisteredEvent = true; 
    }

    private void OnEnable()
    {
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelInfo(m_playerManager.unLockedNormalModelBigLevelList[i],
                m_playerManager.unLockedNormalModelLevelNum[i], 5,
                bigLevelPage[i], i + 1);
        }
    }

    public override void EnterPanel()
    {
        m_slideScrollView.Init();
        gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        gameObject.SetActive(false);
    }

    //显示大关卡信息
    public void ShowBigLevelInfo(bool unLocked, int unLockedLevelNum, int totalNum,
        Transform theBigLevelButtonTrans, int bigLevelID)
    {
        if (unLocked)
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            theBigLevelButtonTrans.Find("Img_Bookmark").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Bookmark").Find("Tex_Page").GetComponent<Text>().text =
                unLockedLevelNum.ToString() + "/" + totalNum.ToString();
            Button theBigLevelButton = theBigLevelButtonTrans.GetComponent<Button>();
            theBigLevelButton.interactable = true;
            if (!hasRigisteredEvent) //防止按钮事件重复注册
            {
                theBigLevelButton.onClick.AddListener(() =>
                {
                    //离开大关卡页面
                    m_uiFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
                    //进入小关卡
                    GameNormalLevelPanel gameNormalLevelPanel = m_uiFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel] as GameNormalLevelPanel;
                    gameNormalLevelPanel.ToThisPanel(bigLevelID);
                    GameNormalOptionPanel gameNormalOptionPanel = m_uiFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
                    gameNormalOptionPanel.IsInBigLevelPanel = false;
                });
                //theBigLevelButton.onClick.AddListener(delegate () { Test(bigLevelID); });
            }

        }
        else
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            theBigLevelButtonTrans.Find("Img_Bookmark").gameObject.SetActive(true);
            theBigLevelButtonTrans.GetComponent<Button>().interactable = false;
        }
    }

    private void Test(int bigLevelID)
    {
        Debug.Log("Test");
        m_uiFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
        GameNormalLevelPanel gameNormalLevelPanel = m_uiFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel] as GameNormalLevelPanel;
        gameNormalLevelPanel.ToThisPanel(bigLevelID);
        GameNormalOptionPanel gameNormalOptionPanel = m_uiFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
        gameNormalOptionPanel.IsInBigLevelPanel = false;
    }
}
