using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责管理各种类型的工厂以及对象池
/// </summary>
public class FactoryManager  
{
    public Dictionary<FactoryType, IBaseFactory> m_factoryDict = new Dictionary<FactoryType, IBaseFactory>();
    public AudioClipFactory m_audioClipFactory;
    public SpriteFactory m_spriteFactory;
    public RuntimeAnimatorControllerFactory m_runtimeAnimatorControllerFactory;

    public FactoryManager()
    {
        m_factoryDict.Add(FactoryType.UIPanelFactory, new UIPanelFactory());
        m_factoryDict.Add(FactoryType.UIFactory, new UIFactory());
        m_factoryDict.Add(FactoryType.GameFactory, new GameFactory());
        m_audioClipFactory = new AudioClipFactory();
        m_spriteFactory = new SpriteFactory();
        m_runtimeAnimatorControllerFactory = new RuntimeAnimatorControllerFactory();
    }
}
