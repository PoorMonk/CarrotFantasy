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
        m_topPage.UpdateCoinText();
        m_topPage.UpdateRoundText();
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
        Stage stage = GameManager.Instance.m_playerManager.unLockedNormalModelLevelList[m_gameController.currentStage.m_levelID - 1 + 
            (m_gameController.currentStage.m_bigLevelID - 1) * 5];
        if (m_gameController.IsAllClear())
        {
            stage.m_allClear = true;
        }
        //萝卜徽章更新
        int carrotState = m_gameController.GetCarrotState();
        if (carrotState != 0)
        {
            if (stage.m_carrotState > carrotState)
            {
                stage.m_carrotState = carrotState;
            }
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
        SceneManager.LoadScene(4);
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
