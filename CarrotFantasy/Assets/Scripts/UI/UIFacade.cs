using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// UI中介，负责UIManager与各Panel之间的交互
/// </summary>
public class UIFacade
{
    private UIManager m_uiManager;
    private GameManager m_gameManager;
    private AudioSourceManager m_audioSourceManager;
    private PlayerManager m_playerManager;

    // UI面板
    public Dictionary<string, IBasePanel> currentScenePanelDict = new Dictionary<string, IBasePanel>();

    private GameObject m_mask;              // 面板切换时的遮罩对象
    private Image m_maskImg;                // 遮罩图片
    private Transform m_canvasTransform;    // 让所有Panel对象都位于Canvas之下

    // 场景状态
    public IBaseSceneState currentSceneState;
    public IBaseSceneState lastSceneState;

    public UIFacade(UIManager uiManager)
    {
        m_gameManager = GameManager.Instance;
        m_audioSourceManager = m_gameManager.m_audioSourceManager;
        m_playerManager = m_gameManager.m_playerManager;
        m_uiManager = uiManager;

        InitMask();
    }

    public void InitMask()
    {
        m_canvasTransform = GameObject.Find("Canvas").transform;
        m_mask = CreateUI("ImgMask");
        m_maskImg = m_mask.GetComponent<Image>();
    }

    public void ChangeSceneState(IBaseSceneState baseSceneState)
    {
        lastSceneState = currentSceneState;
        currentSceneState = baseSceneState; // ???To Test
        ShowMask();
    }

    public void ShowMask()
    {
        m_mask.transform.SetSiblingIndex(10); // 值越大越后渲染
        Tween t = DOTween.To(() => m_maskImg.color, toColor => m_maskImg.color = toColor, new Color(0, 0, 0, 1), 2f);
        t.OnComplete(ExitSceneComplelte);
    }

    public void ExitSceneComplelte()
    {
        lastSceneState.ExitScene();
        currentSceneState.EnterScene();
        HideMask();
    }

    public void HideMask()
    {
        m_mask.transform.SetSiblingIndex(10); // 再设置一次防止新加的UI后渲染
        DOTween.To(() => m_maskImg.color, toColor => m_maskImg.color = toColor, new Color(0, 0, 0, 0), 2f);
    }

    public void AddPanelToDict(string uiPanelPath)
    {
        m_uiManager.m_currentScenePanelDict.Add(uiPanelPath, GetGameObjectResource(FactoryType.UIPanelFactory, uiPanelPath));
    }

    // 实例化当前场景所有面板，并存入字典
    public void InitDict()
    {
        foreach (var item in m_uiManager.m_currentScenePanelDict)
        {
            item.Value.transform.SetParent(m_canvasTransform);
            item.Value.transform.localPosition = Vector3.zero;
            item.Value.transform.localScale = Vector3.one;
            IBasePanel basePanel = item.Value.GetComponent<IBasePanel>();
            if (null == basePanel)
            {
                Debug.Log("获取面板IBasePanel脚本失败");
            }
            basePanel.InitPanel();
            currentScenePanelDict.Add(item.Key, basePanel);
        }
    }

    public void ClearDict()
    {
        currentScenePanelDict.Clear();
        m_uiManager.ClearDict();
    }


    /// <summary>
    /// 创建UI对象，需要手动设置其LocalPosition和LocalScale
    /// </summary>
    /// <param name="uiPath">ui路径</param>
    /// <returns></returns>
    public GameObject CreateUI(string uiPath)
    {
        GameObject go = GetGameObjectResource(FactoryType.UIFactory, uiPath);
        go.transform.SetParent(m_canvasTransform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        return go;
    }

    // 获取Sprite资源
    public Sprite GetSprite(string spritePath)
    {
        return m_gameManager.GetSprite(spritePath);
    }

    // 获取AudioClip资源
    public AudioClip GetAudioClip(string audioClipPath)
    {
        return m_gameManager.GetAudioClip(audioClipPath);
    }

    // 获取动画控制器资源
    public RuntimeAnimatorController GetRuntimeAnimatorController(string animatorPath)
    {
        return m_gameManager.GetRuntimeAnimatorController(animatorPath);
    }

    // 获取游戏物体资源
    public GameObject GetGameObjectResource(FactoryType factoryType, string ObjectPath)
    {
        return m_gameManager.GetGameObjectResource(factoryType, ObjectPath);
    }

    // 将游戏物体放回对象池
    public void PushGameObjectToFactory(FactoryType factoryType, string ObjectPath, GameObject itemGo)
    {
        m_gameManager.PushGameObjectToFactory(factoryType, ObjectPath, itemGo);
    }

    // 开关音乐
    public void CloseOrOpenBGMusic()
    {
        m_audioSourceManager.CloseOrOpenBGMusic();
    }

    public void CloseOrOpenEffectMusic()
    {
        m_audioSourceManager.CloseOrOpenEffectMusic();
    }
}
