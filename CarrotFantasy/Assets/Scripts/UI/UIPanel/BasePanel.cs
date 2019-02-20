using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour, IBasePanel
{
    protected UIFacade m_uiFacade;

    protected virtual void Awake()
    {
        m_uiFacade = GameManager.Instance.m_uiManager.m_uiFacade;
    }

    public virtual void EnterPanel()
    {
    }

    public virtual void ExitPanel()
    {
    }

    public virtual void InitPanel()
    {
    }

    public virtual void UpdatePanel()
    {
    }
}
