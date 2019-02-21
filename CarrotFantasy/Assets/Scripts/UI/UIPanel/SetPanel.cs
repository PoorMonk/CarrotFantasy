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
    private Text[] m_statisticesTexts; // 数据统计页面各Text文本

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

        InitPanel();
    }

    public override void InitPanel()
    {
        transform.localPosition = new Vector3(-1920, 0, 0);
        transform.SetSiblingIndex(2); // 设置渲染层级
    }

    public void ShowOptionPage()
    {
        m_optionPage.SetActive(true);
        m_producerPage.SetActive(false);
        m_statisticsPage.SetActive(false);
    }

    public void ShowStatisticsPage()
    {
        m_optionPage.SetActive(false);
        m_producerPage.SetActive(true);
        m_statisticsPage.SetActive(false);
    }

    public void ShowProducerPage()
    {
        m_optionPage.SetActive(false);
        m_producerPage.SetActive(false);
        m_statisticsPage.SetActive(true);
    }

    public override void EnterPanel()
    {
        ShowOptionPage();
        MoveToCenter();
    }

    public override void ExitPanel()
    {
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
    }
}
