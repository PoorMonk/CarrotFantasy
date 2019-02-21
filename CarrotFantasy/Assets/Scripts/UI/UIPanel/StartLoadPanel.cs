using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLoadPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
        Invoke("StartNextScene", 1f);
    }

    private void StartNextScene()
    {
        m_uiFacade.ChangeSceneState(new MainSceneState(m_uiFacade));
    }

    public override void EnterPanel()
    {
        base.EnterPanel();
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
    }

    public override void InitPanel()
    {
        base.InitPanel();
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
    }
}
