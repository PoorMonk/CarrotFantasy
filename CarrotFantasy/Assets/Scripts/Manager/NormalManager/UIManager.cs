﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责管理UI的Manager
/// </summary>
public class UIManager  
{
    public UIFacade m_uiFacade;
    public Dictionary<string, GameObject> m_currentScenePanelDict;
    private GameManager m_gameManager;

    public UIManager()
    {
        m_gameManager = GameManager.Instance;
        m_currentScenePanelDict = new Dictionary<string, GameObject>();
        m_uiFacade = new UIFacade(this);
    }

    private void PushUIPanel(string uiPanelPath, GameObject uiPanelGo)
    {
        m_gameManager.PushGameObjectToFactory(FactoryType.UIPanelFactory, uiPanelPath, uiPanelGo);
    }

    public void ClearDict()
    {
        foreach (var item in m_currentScenePanelDict)
        {
            PushUIPanel(item.Value.name, item.Value);
        }
        m_currentScenePanelDict.Clear();
    }
}
