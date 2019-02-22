using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HelpPanel : BasePanel
{
    private GameObject m_helpPage;
    private GameObject m_monsterPage;
    private GameObject m_towerPage;
    private SlideScrollView m_slideScrollView;
    private ScrollViewMove m_scrollViewMove; // 多划
    private Tween m_helpPanelTween;
    public Text m_helpInfo;
    public Text m_towerInfo;

    protected override void Awake()
    {
        base.Awake();
        m_helpPage = transform.Find("HelpPage").gameObject;
        m_monsterPage = transform.Find("MonsterPage").gameObject;
        m_towerPage = transform.Find("TowerPage").gameObject;

        m_slideScrollView = m_towerPage.transform.Find("Scroll View").GetComponent<SlideScrollView>();
        m_scrollViewMove = m_helpPage.transform.Find("Scroll View").GetComponent<ScrollViewMove>();

        m_helpPanelTween = transform.DOLocalMoveX(0, 0.5f);
        m_helpPanelTween.SetAutoKill(false);
        m_helpPanelTween.Pause();
    }

    public void ShowHelpPage()
    {
        m_helpPage.SetActive(true);
        m_monsterPage.SetActive(false);
        m_towerPage.SetActive(false);
        ChangeHelpInfo();
    }

    public void ShowMonsterPage()
    {
        m_helpPage.SetActive(false);
        m_monsterPage.SetActive(true);
        m_towerPage.SetActive(false);
    }

    public void ShowTowerPage()
    {
        m_helpPage.SetActive(false);
        m_monsterPage.SetActive(false);
        m_towerPage.SetActive(true);
        ChangeTowerInfo();
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(1920, 0, 0);
        transform.SetSiblingIndex(5);
        m_scrollViewMove.Init();   //将scroll view退回到初始状态
        m_slideScrollView.Init();
        
    }

    public override void EnterPanel()
    {
        gameObject.SetActive(true);
        m_scrollViewMove.Init();   //从其它面板跳转回来也需要初始化
        m_slideScrollView.Init();
        ShowHelpPage();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        m_helpPanelTween.PlayBackwards();
        m_uiFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();

    }

    private void MoveToCenter()
    {
        m_helpPanelTween.PlayForward();
    }

    public void ChangeHelpInfo()
    {
        m_helpInfo.text = m_scrollViewMove.CurrentIndex.ToString() + "/" + m_scrollViewMove.m_totalItemNum.ToString();
    }

    public void ChangeTowerInfo()
    {
        m_towerInfo.text = m_slideScrollView.CurrentIndex.ToString() + "/" + m_slideScrollView.m_totalItemNum.ToString();
    }
}
