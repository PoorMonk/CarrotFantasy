using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalModelSceneState : BaseSceneState {

    public NormalModelSceneState (UIFacade uiFacade) : base (uiFacade) {

    }

    public override void EnterScene () {
        m_uiFacade.AddPanelToDict (StringManager.GameLoadPanel);
        m_uiFacade.AddPanelToDict (StringManager.NormalModelPanel);
        base.EnterScene ();
        GameManager.Instance.m_audioSourceManager.CloseBGMusic();
    }

    public override void ExitScene () {
        base.ExitScene ();
        GameManager.Instance.m_audioSourceManager.OpenBGMusic();
    }
}