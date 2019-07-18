using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : MonoBehaviour {

    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
    }

    public void GoOn()
    {
        GameManager.Instance.m_audioSourceManager.PlayButtonAudioClip();
        GameController.Instance.isPause = false;
        transform.gameObject.SetActive(false);
        normalModelPanel.m_topPage.UpdateStatus();
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
