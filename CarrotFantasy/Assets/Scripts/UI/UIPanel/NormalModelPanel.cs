using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalModelPanel : BasePanel {

    private GameObject m_topPageGo;
    private GameObject m_gameoverPageGo;
    private GameObject m_gameWinPageGo;
    private GameObject m_menuPageGo;
    private GameObject m_finalWaveImg;
    private GameObject m_startGameImg;
    private GameObject m_prizePageGo;

    //引用
    public TopPage m_topPage;
    private GameController m_gameController;

    public int totalRound;

    protected override void Awake()
    {
        base.Awake();
        m_gameController = GameController.Instance;
        //Debug.Log("gamecontroller: " + m_gameController);
        m_topPageGo = transform.Find("Img_TopPage").gameObject;
        m_gameoverPageGo = transform.Find("GameOverPage").gameObject;
        m_gameWinPageGo = transform.Find("GameWinPage").gameObject;
        m_menuPageGo = transform.Find("MenuPage").gameObject;
        m_finalWaveImg = transform.Find("Img_FinalWave").gameObject;
        m_startGameImg = transform.Find("StartAnimationPage").gameObject;
        m_prizePageGo = transform.Find("PrizePage").gameObject;
        m_topPage = m_topPageGo.GetComponent<TopPage>();
    }

    private void Start()
    {
        InvokeRepeating("PlayAudio", 0.5f, 1);
        Invoke("StartGame", 3.5f);
    }

    //播放开始游戏倒计时声音
    private void PlayAudio()
    {
        m_startGameImg.SetActive(true);
        GameController.Instance.PlayEffectMusic("NormalModel/CountDown");
    }

    private void StartGame()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/GO");
#if Game
        m_gameController.StartGame();
#endif
        m_startGameImg.SetActive(false);
        CancelInvoke();
    }

    public override void EnterPanel()
    {
        //Debug.Log("EnterPanel");
        if (m_gameController == null)
        {
            m_gameController = GameController.Instance;
        }
        base.EnterPanel();
        totalRound = m_gameController.currentStage.m_totalRound;
        m_topPageGo.SetActive(true);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        m_topPage.UpdateCoinText();
        m_topPage.UpdateRoundText();
    }

    public void ShowPrizePage()
    {
        m_prizePageGo.SetActive(true);
        GameController.Instance.isPause = true;
    }

    public void ClosePrizePage()
    {
        m_uiFacade.PlayButtonAudioClip();
        GameController.Instance.isPause = false;
        m_prizePageGo.SetActive(false);
    }

    public void ShowMenuPage()
    {
        m_uiFacade.PlayButtonAudioClip();
        GameController.Instance.isPause = true;
        m_menuPageGo.SetActive(true);
    }

    public void CloseMenuPage()
    {
        m_uiFacade.PlayButtonAudioClip();
        GameController.Instance.isPause = false;
        m_menuPageGo.SetActive(false);
    }

    public void ShowGameWinPage()
    {
        Stage stage = GameManager.Instance.m_playerManager.unLockedNormalModelLevelList[m_gameController.currentStage.m_levelID - 1 + 
            (m_gameController.currentStage.m_bigLevelID - 1) * 5];
        if (m_gameController.IsAllClear())
        {
            stage.m_allClear = true;
        }
        //萝卜徽章更新
        int carrotState = m_gameController.GetCarrotState();
        if (carrotState != 0 && stage.m_carrotState != 0)
        {
            if (stage.m_carrotState > carrotState)
            {
                stage.m_carrotState = carrotState;
            }
        }
        else if (stage.m_carrotState == 0)
        {
            stage.m_carrotState = carrotState;
        }
        //解锁下一关卡
        if (m_gameController.currentStage.m_levelID % 5 != 0 && (m_gameController.currentStage.m_levelID - 1 +
            (m_gameController.currentStage.m_bigLevelID - 1) * 5) < GameManager.Instance.m_playerManager.unLockedNormalModelLevelList.Count)
        {
            GameManager.Instance.m_playerManager.unLockedNormalModelLevelList[m_gameController.currentStage.m_levelID +
           (m_gameController.currentStage.m_bigLevelID - 1) * 5].m_unLocked = true;
        }
        UpdatePlayerManagerData();
        m_gameWinPageGo.SetActive(true);
        m_gameController.IsGameOver = false;
        GameManager.Instance.m_playerManager.adventrueModelNum++;

        GameController.Instance.PlayEffectMusic("NormalModel/Perfect");
    }

    public void ShowRoundText(Text roundText)
    {
        int roundNum = m_gameController.level.currentRound + 1;
        string roundStr = "";
        if (roundNum < 10)
        {
            roundStr = "0  " + roundNum.ToString();
        }
        else
        {
            roundStr = (roundNum / 10).ToString() + "  " + (roundNum % 10).ToString();
        }
        roundText.text = roundStr;
    }

    public void ShowGameOverPage()
    {
        UpdatePlayerManagerData();
        m_gameoverPageGo.SetActive(true);
        m_gameController.IsGameOver = false;
        GameController.Instance.PlayEffectMusic("NormalModel/Lose");
    }

    public void ShowFinalWave()
    {
        GameController.Instance.PlayEffectMusic("NormalModel/Finalwave");
        m_finalWaveImg.SetActive(false);
        Invoke("CloseFinalWave", 0.508f);
    }

    private void CloseFinalWave()
    {
        m_finalWaveImg.SetActive(false);
    }

    //更新基础数据
    private void UpdatePlayerManagerData()
    {
        GameManager.Instance.m_playerManager.coin += m_gameController.coin;
        GameManager.Instance.m_playerManager.killMonsterNum += m_gameController.killMonsterTotalNum;
        GameManager.Instance.m_playerManager.clearItemNum += m_gameController.clearItemNum;
    }

    //重置游戏
    private void ResetGame()
    {
        SceneManager.LoadScene(3);
        ResetUI();
        gameObject.SetActive(true);
    }

    //重置页面UI显示状态
    public void ResetUI()
    {
        m_gameoverPageGo.SetActive(false);
        m_gameWinPageGo.SetActive(false);
        m_menuPageGo.SetActive(false);
        gameObject.SetActive(false);
    }

    public void Replay()
    {
        UpdatePlayerManagerData();
        m_uiFacade.ChangeSceneState(new NormalModelSceneState(m_uiFacade));
        m_gameController.IsGameOver = false;
        Invoke("ResetGame", 2f);
    }

    public void ChooseOtherLevel()
    {
        m_uiFacade.PlayButtonAudioClip();
        m_gameController.IsGameOver = false;
        UpdatePlayerManagerData();
        Invoke("ToOtherScene", 2f);
        m_uiFacade.ChangeSceneState(new NormalGameOptionSceneState(m_uiFacade));
    }

    public void ToOtherScene()
    {
        m_gameController.IsGameOver = false;
        ResetUI();
        SceneManager.LoadScene(2);
    }
}
