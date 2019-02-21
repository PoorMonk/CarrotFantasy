using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSceneState : IBaseSceneState
{
    protected UIFacade m_uiFacade;

    public BaseSceneState(UIFacade uiFacade)
    {
        m_uiFacade = uiFacade;
    }

    public virtual void EnterScene()
    {
        m_uiFacade.InitDict();
    }

    public virtual void ExitScene()
    {
        m_uiFacade.ClearDict();
    }
}
