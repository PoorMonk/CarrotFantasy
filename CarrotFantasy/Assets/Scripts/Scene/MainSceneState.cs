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
        GameManager.Instance.m_audioSourceManager.PlayBGMusic(GameManager.Instance.m_factoryManager.m_audioClipFactory.GetSingleResource("Main/BGMusic"));
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
            SceneManager.LoadScene(4);
        }
        else
        {
            SceneManager.LoadScene(6);
        }
    }
}
