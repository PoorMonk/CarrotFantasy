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
        GameController.Instance.isPause = false;
        transform.gameObject.SetActive(false);
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
