using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneState : BaseSceneState {

    public MainSceneState(UIFacade uiFacade) : base(uiFacade)
    {

    }

    public override void EnterScene()
    {
        m_uiFacade.AddPanelToDict(StringManager.MainPanel);
        m_uiFacade.AddPanelToDict(StringManager.SetPanel);
        m_uiFacade.AddPanelToDict(StringManager.HelpPanel);
        m_uiFacade.AddPanelToDict(StringManager.GameLoadPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        base.ExitScene();
        if (m_uiFacade.currentSceneState.GetType() == typeof(NormalGameOptionSceneState))
        {
            SceneManager.LoadScene(2);
        }
        else if (m_uiFacade.currentSceneState.GetType() == typeof(BossGameOptionSceneState))
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
    }
}
