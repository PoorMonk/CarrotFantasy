﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainPanel : BasePanel {

    private Animator m_carrotAnimator;
    private Transform m_monsterTrans;
    private Transform m_cloudTrans;

    public Tween[] mainPanelTween; // 0.右移 1.左移
    private Tween m_exitTween;

    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(8); // 设置渲染层级
        m_carrotAnimator = transform.Find("Emp_Carrot").GetComponent<Animator>();
        m_carrotAnimator.Play("CarrotGrow");
        m_monsterTrans = transform.Find("Img_Monster");
        m_cloudTrans = transform.Find("Img_Cloud");

        mainPanelTween = new Tween[2];
        mainPanelTween[0] = transform.DOLocalMoveX(1920, 0.5f);
        mainPanelTween[0].SetAutoKill(false);
        mainPanelTween[0].Pause();
        mainPanelTween[1] = transform.DOLocalMoveX(-1920, 0.5f);
        mainPanelTween[1].SetAutoKill(false);
        mainPanelTween[1].Pause();

        PlayUITween();
    }

    public override void EnterPanel()
    {
        transform.SetSiblingIndex(8); // 设置渲染层级
        m_carrotAnimator.Play("CarrotGrow");
        if (m_exitTween != null)
        {
            m_exitTween.PlayBackwards();
        }
        m_cloudTrans.gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        m_exitTween.PlayForward();
        m_cloudTrans.gameObject.SetActive(false);
    }

    public void OnCloseBtnClicked()
    {
        //Debug.Log("OnCloseBtnClicked");
#if !UNITY_ANDROID
        GameManager.Instance.m_playerManager.SavaData();
#endif
        Application.Quit();
    }

    private void PlayUITween()
    {
        m_monsterTrans.DOLocalMoveY(400, 1.5f).SetLoops(-1, LoopType.Yoyo);
        m_cloudTrans.DOLocalMoveX(1200, 8f).SetLoops(-1, LoopType.Restart);
    }

    public void EnterNormalMode()
    {
        m_uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        m_uiFacade.ChangeSceneState(new NormalGameOptionSceneState(m_uiFacade));
        m_uiFacade.PlayButtonAudioClip();
    }

    public void EnterBossMode()
    {
        m_uiFacade.PlayButtonAudioClip();
        m_uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        m_uiFacade.ChangeSceneState(new BossGameOptionSceneState(m_uiFacade));
    }

    public void EnterMonsterNest()
    {
        m_uiFacade.PlayButtonAudioClip();
        m_uiFacade.currentScenePanelDict[StringManager.GameLoadPanel].EnterPanel();
        m_uiFacade.ChangeSceneState(new MonsterNestSceneState(m_uiFacade));
    }

    public void MoveToRight()
    {
        m_exitTween = mainPanelTween[0];
        m_uiFacade.currentScenePanelDict[StringManager.SetPanel].EnterPanel();
        m_uiFacade.PlayButtonAudioClip();
        ExitPanel();
    }

    public void MoveToLeft()
    {
        m_exitTween = mainPanelTween[1];
        m_uiFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
        m_uiFacade.PlayButtonAudioClip();
        ExitPanel();
    }


}
