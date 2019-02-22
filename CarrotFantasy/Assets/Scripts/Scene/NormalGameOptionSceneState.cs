using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        base.ExitScene();
    }
}
