using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SetPanel : BasePanel
{

    private GameObject m_optionPage;
    private GameObject m_statisticsPage;
    private GameObject m_producerPage;
    private GameObject m_panelReset;
    private Tween m_setPanelTween;
    private bool m_isPlayBGMusic = true;
    private bool m_isPlayEffectMusic = true;
    public Sprite[] btnSprites; // 0.音效开 1.音效关 2. BG音乐开 3. BG音乐关
    private Image m_btnAudioImg;
    private Image m_btnBgMusicImg;
    public Text[] m_statisticesTexts; // 数据统计页面各Text文本

    protected override void Awake()
    {
        base.Awake();
        m_setPanelTween = transform.DOLocalMoveX(0, 0.5f);
        m_setPanelTween.SetAutoKill(false);
        m_setPanelTween.Pause();

        m_optionPage = transform.Find("OptionPage").gameObject;
        m_statisticsPage = transform.Find("StatisticsPage").gameObject;
        m_producerPage = transform.Find("ProducerPage").gameObject;

        m_btnAudioImg = m_optionPage.transform.Find("Btn_Audio").GetComponent<Image>();
        m_btnBgMusicImg = m_optionPage.transform.Find("Btn_BgMusic").GetComponent<Image>();

        m_panelReset = transform.Find("Panel_Reset").gameObject;

        //InitPanel();
    }

    public void ShowOptionPage()
    {
        if (!m_optionPage.activeSelf)
        {
            m_optionPage.SetActive(true);
            m_uiFacade.PlayButtonAudioClip();
        }
        m_producerPage.SetActive(false);
        m_statisticsPage.SetActive(false);
    }

    public void ShowStatisticsPage()
    {
        m_uiFacade.PlayButtonAudioClip();
        m_optionPage.SetActive(false);
        m_producerPage.SetActive(false);
        m_statisticsPage.SetActive(true);
        ShowStatistics();
    }

    public void ShowProducerPage()
    {
        m_uiFacade.PlayButtonAudioClip();
        m_optionPage.SetActive(false);
        m_producerPage.SetActive(true);
        m_statisticsPage.SetActive(false);
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-1920, 0, 0);
        transform.SetSiblingIndex(2); // 设置渲染层级
    }
    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
        m_uiFacade.PlayButtonAudioClip();
        m_setPanelTween.PlayBackwards();
        m_uiFacade.currentScenePanelDict[StringManager.MainPanel].EnterPanel();
        InitPanel();
    }

    private void MoveToCenter()
    {
        m_setPanelTween.PlayForward();
    }

    public void CloseOrOpenBGMusic()
    {
        m_isPlayBGMusic = !m_isPlayBGMusic;
        m_uiFacade.CloseOrOpenBGMusic();
        if (m_isPlayBGMusic)
        {
            m_btnBgMusicImg.sprite = btnSprites[2];
        }
        else
        {
            m_btnBgMusicImg.sprite = btnSprites[3];
        }
        m_uiFacade.PlayButtonAudioClip();
    }

    public void CloseOrOpenEffectMusic()
    {
        m_isPlayEffectMusic = !m_isPlayEffectMusic;
        m_uiFacade.CloseOrOpenEffectMusic();
        if (m_isPlayEffectMusic)
        {
            m_btnAudioImg.sprite = btnSprites[0];
        }
        else
        {
            m_btnAudioImg.sprite = btnSprites[1];
        }
        m_uiFacade.PlayButtonAudioClip();
    }

    //数据显示 Todo
    public void ShowStatistics()
    {
        PlayerManager playerManager = m_uiFacade.m_playerManager;
        m_statisticesTexts[0].text = playerManager.adventrueModelNum.ToString();
        m_statisticesTexts[1].text = playerManager.burriedLevelNum.ToString();
        m_statisticesTexts[2].text = playerManager.bossModelNum.ToString();
        m_statisticesTexts[3].text = playerManager.coin.ToString();
        m_statisticesTexts[4].text = playerManager.killMonsterNum.ToString();
        m_statisticesTexts[5].text = playerManager.killBossNum.ToString();
        m_statisticesTexts[6].text = playerManager.clearItemNum.ToString();
    }

    //重置游戏 Todo
    public void ResetGame()
    {
        GameManager.Instance.initPlayerManager = true;
        GameManager.Instance.m_playerManager.ReadData();
        ShowStatistics();
        CloseResetPanel();
        m_uiFacade.PlayButtonAudioClip();
    }

    public void ShowResetPanel()
    {
        m_panelReset.SetActive(true);
    }

    public void CloseResetPanel()
    {
        m_panelReset.SetActive(false);
        m_uiFacade.PlayButtonAudioClip();
    }
}
