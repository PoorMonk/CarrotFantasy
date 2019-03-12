using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalModelPanel : BasePanel {

    private GameObject m_topPageGo;
    private GameObject m_gameoverPageGo;
    private GameObject m_gameWinPageGo;
    private GameObject m_menuPageGo;
    private GameObject m_finalWaveImg;
    private GameObject m_startGameImg;
    private GameObject m_prizePageGo;

    //引用
    private TopPage m_topPage;
    private GameController m_gameController;

    public int totalRound;

    private void Awake()
    {
        m_gameController = GameController.Instance;
        m_topPageGo = transform.Find("Img_TopPage").gameObject;
        m_gameoverPageGo = transform.Find("GameOverPage").gameObject;
        m_gameWinPageGo = transform.Find("GameWinPage").gameObject;
        m_menuPageGo = transform.Find("MenuPage").gameObject;
        m_finalWaveImg = transform.Find("Img_FinalWave").gameObject;
        m_startGameImg = transform.Find("StartAnimationPage").gameObject;
        m_prizePageGo = transform.Find("PrizePage").gameObject;
        m_topPage = m_topPageGo.GetComponent<TopPage>();
    }

    private void OnEnable()
    {
        m_startGameImg.SetActive(true);
        InvokeRepeating("PlayAudio", 0, 1);
        Invoke("StartGame", 3);
    }

    private void PlayAudio()
    {

    }

    private void StartGame()
    {
        m_gameController.StartGame();
        m_startGameImg.SetActive(false);
        CancelInvoke();
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
        totalRound = m_gameController.currentStage.m_totalRound;
        m_topPageGo.SetActive(true);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
    }

    public void ShowPrizePage()
    {
        m_prizePageGo.SetActive(true);
    }

    public void ClosePrizePage()
    {
        m_prizePageGo.SetActive(false);
    }

    public void ShowMenuPage()
    {
        m_menuPageGo.SetActive(true);
    }

    public void CloseMenuPage()
    {
        m_menuPageGo.SetActive(false);
    }

    public void ShowGameWinPage()
    {

    }

    public void ShowGameOverPage()
    {

    }

    public void ShowFinalWave()
    {
        m_finalWaveImg.SetActive(false);
        Invoke("CloseFinalWave", 0.508f);
    }

    private void CloseFinalWave()
    {
        m_finalWaveImg.SetActive(false);
    }
}
