using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWin : MonoBehaviour {

    private Text m_currentLevelTxt;
    private Text m_roundCountTxt;
    private Text m_totalRoundTxt;
    private Image m_carrotImg;
    public Sprite[] carrotSprites;
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        m_currentLevelTxt = transform.Find("Img_LevelInfo").Find("Text_CurrentLevel").GetComponent<Text>();
        m_roundCountTxt = transform.Find("Img_Info").Find("Text_CurrentCount").GetComponent<Text>();
        m_totalRoundTxt = transform.Find("Img_Info").Find("Text_TotalCount").GetComponent<Text>();
        m_carrotImg = transform.Find("Img_Carrot").GetComponent<Image>();
        carrotSprites = new Sprite[3];
        for (int i = 0; i < 3; i++)
        {
            carrotSprites[i] = GameController.Instance.GetSprite("GameOption/Normal/Level/Carrot_" + (i + 1).ToString());
        }
    }

    private void OnEnable()
    {
        m_totalRoundTxt.text = normalModelPanel.totalRound.ToString();
        m_currentLevelTxt.text = (GameController.Instance.currentStage.m_levelID + (
            GameController.Instance.currentStage.m_bigLevelID - 1) * 5).ToString();
        normalModelPanel.ShowRoundText(m_roundCountTxt);
        if (GameController.Instance.carrotHP >= 4)
        {
            m_carrotImg.sprite = carrotSprites[0];
        }
        else if (GameController.Instance.carrotHP >= 2)
        {
            m_carrotImg.sprite = carrotSprites[1];
        }
        else
        {
            m_carrotImg.sprite = carrotSprites[2];
        }
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
