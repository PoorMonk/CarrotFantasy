using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPage : MonoBehaviour {

    private Text m_currentLevelTxt;
    private Text m_roundCountTxt;
    private Text m_totalRoundTxt;
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        m_currentLevelTxt = transform.Find("Img_LevelInfo").Find("Text_CurrentLevel").GetComponent<Text>();
        m_roundCountTxt = transform.Find("Img_Info").Find("Text_CurrentCount").GetComponent<Text>();
        m_totalRoundTxt = transform.Find("Img_Info").Find("Text_TotalCount").GetComponent<Text>();
    }

    private void OnEnable()
    {
        m_totalRoundTxt.text = normalModelPanel.totalRound.ToString();
        m_currentLevelTxt.text = (GameController.Instance.currentStage.m_levelID + (
            GameController.Instance.currentStage.m_bigLevelID - 1) * 5).ToString();
        normalModelPanel.ShowRoundText(m_roundCountTxt);
    }

    public void Replay()
    {
        normalModelPanel.Replay();
    }

    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
}
