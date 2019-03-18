using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopPage : MonoBehaviour {

    //引用
    private Text m_coinTxt;
    private Text m_roundCount;
    private Text m_totalRound;
    private Image m_gameSpeedImg;
    private Image m_pauseImg;
    private GameObject m_pauseEmp;
    private GameObject m_playingGo;
    private NormalModelPanel m_normalModelPanel;

    //按钮图片
    public Sprite[] gameSpeedSps;
    public Sprite[] pauseSps;

    private bool m_isNormalSpeed;
    private bool m_isPause;


    private void Awake()
    {
        m_normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        m_coinTxt = transform.Find("Tex_Coin").GetComponent<Text>();
        m_roundCount = transform.Find("Emp_TotalCount").Find("Text_CurrentCount").GetComponent<Text>();
        m_totalRound = transform.Find("Emp_TotalCount").Find("Text_TotalCount").GetComponent<Text>();
        m_gameSpeedImg = transform.Find("Btn_Speed").GetComponent<Image>();
        m_pauseImg = transform.Find("Btn_Pause").GetComponent<Image>();
        m_pauseEmp = transform.Find("Emp_Pause").gameObject;
        m_playingGo = transform.Find("Emp_TotalCount").gameObject;
        m_isNormalSpeed = true;
    }

    private void OnEnable()
    {
        //Debug.Log("Top Enable");
        UpdateCoinText();
        m_totalRound.text = m_normalModelPanel.totalRound.ToString();
        m_pauseImg.sprite = pauseSps[0];
        m_gameSpeedImg.sprite = gameSpeedSps[0];
        m_isPause = false;
        m_isNormalSpeed = true;
        m_pauseEmp.SetActive(false);
        m_playingGo.SetActive(true);
    }

    public void UpdateCoinText()
    {
        //Debug.Log(m_coinTxt); 
        //Debug.Log("coin:" + GameController.Instance);
        m_coinTxt.text = GameController.Instance.coin.ToString();
    }

    public void UpdateRoundText()
    {
        m_normalModelPanel.ShowRoundText(m_roundCount);
    }

    public void ChangeGameSpeed()
    {
        GameManager.Instance.m_audioSourceManager.PlayButtonAudioClip();
        m_isNormalSpeed = !m_isNormalSpeed;
        if (m_isNormalSpeed)
        {
            GameController.Instance.gameSpeed = 1;
            m_gameSpeedImg.sprite = gameSpeedSps[0];
        }
        else
        {
            GameController.Instance.gameSpeed = 2;
            m_gameSpeedImg.sprite = gameSpeedSps[1];
        }
    }

    public void PauseGame()
    {
        GameManager.Instance.m_audioSourceManager.PlayButtonAudioClip();
        m_isPause = !m_isPause;
        GameController.Instance.isPause = m_isPause;
        if (m_isPause)
        {
            m_pauseImg.sprite = pauseSps[1];
            m_pauseEmp.SetActive(true);
            m_playingGo.SetActive(false);
        }
        else
        {
            m_pauseImg.sprite = pauseSps[0];
            m_pauseEmp.SetActive(false);
            m_playingGo.SetActive(true);
        }
    }

    public void ShowMenu()
    {
        GameManager.Instance.m_audioSourceManager.PlayButtonAudioClip();
        m_normalModelPanel.ShowMenuPage();
    }
}
