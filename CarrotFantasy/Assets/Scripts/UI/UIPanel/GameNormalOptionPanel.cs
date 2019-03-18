using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNormalOptionPanel : BasePanel
{
    [HideInInspector]
    public bool IsInBigLevelPanel = true;

    public void OnReturnBtnClicked()
    {
        if (IsInBigLevelPanel)
        {
            m_uiFacade.ChangeSceneState(new MainSceneState(m_uiFacade));
        }
        else
        {
            m_uiFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel].ExitPanel();
            m_uiFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].EnterPanel();
        }
        IsInBigLevelPanel = true;
        m_uiFacade.PlayButtonAudioClip();
    }

    public void OnHelpBtnClicked()
    {
        m_uiFacade.PlayButtonAudioClip();
        m_uiFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }
}
