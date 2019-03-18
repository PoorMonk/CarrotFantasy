using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalGameOptionSceneState : BaseSceneState {


    public NormalGameOptionSceneState(UIFacade uiFacade) : base(uiFacade)
    {

    }

    public override void EnterScene()
    {
        m_uiFacade.AddPanelToDict(StringManager.GameNormalOptionPanel);
        m_uiFacade.AddPanelToDict(StringManager.GameNormalBigLevelPanel);
        m_uiFacade.AddPanelToDict(StringManager.GameNormalLevelPanel);
        m_uiFacade.AddPanelToDict(StringManager.GameLoadPanel);
        m_uiFacade.AddPanelToDict(StringManager.HelpPanel);
        base.EnterScene();
    }

    public override void ExitScene()
    {
        GameNormalOptionPanel gameNormalOptionPanel = m_uiFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
        if (gameNormalOptionPanel.IsInBigLevelPanel)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
        gameNormalOptionPanel.IsInBigLevelPanel = true;
        base.ExitScene();
    }
}
