using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterNestSceneState : BaseSceneState {

    public MonsterNestSceneState(UIFacade uiFacade) : base(uiFacade)
    {

    }

    public override void EnterScene()
    {
        m_uiFacade.AddPanelToDict(StringManager.GameLoadPanel);
        m_uiFacade.AddPanelToDict(StringManager.MonsterNestPanel);
        base.EnterScene();
        GameManager.Instance.m_audioSourceManager.PlayBGMusic(GameManager.Instance.m_factoryManager.m_audioClipFactory.GetSingleResource("MonsterNest/BGMusic02")); 
    }

    public override void ExitScene()
    {
        base.ExitScene();
        SceneManager.LoadScene(1);
    }
}
