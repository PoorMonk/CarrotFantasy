using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏总管理，负责管理所有的管理者
/// </summary>
public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	public PlayerManager m_playerManager;
	public FactoryManager m_factoryManager;
	public AudioSourceManager m_audioSourceManager;
	public UIManager m_uiManager;

    public Stage m_currentStage;

    public static GameManager Instance { get { return _instance;} }

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		_instance = this;
        Debug.Log("GameManager");
		// 各Manager初始化的时候注意顺序
		m_playerManager = new PlayerManager();
		m_factoryManager = new FactoryManager();
		m_audioSourceManager = new AudioSourceManager();
		///m_uiManager = new UIManager();
        ///m_uiManager.m_uiFacade.currentSceneState.EnterScene();
	}

    /// <summary>
    /// 为没有继承MonoBehaviour的类生成实例对象
    /// </summary>
    /// <param name="item">要实例的对象</param>
    /// <returns></returns>
    public GameObject CreateItem(GameObject item)
    {
        return Instantiate(item);
    }

    // 获取Sprite资源
    public Sprite GetSprite(string spritePath)
    {
        return m_factoryManager.m_spriteFactory.GetSingleResource(spritePath);
    }

    // 获取AudioClip资源
    public AudioClip GetAudioClip(string audioClipPath)
    {
        return m_factoryManager.m_audioClipFactory.GetSingleResource(audioClipPath);
    }

    // 获取动画控制器资源
    public RuntimeAnimatorController GetRuntimeAnimatorController(string animatorPath)
    {
        return m_factoryManager.m_runtimeAnimatorControllerFactory.GetSingleResource(animatorPath);
    }

    // 获取游戏物体资源
    public GameObject GetGameObjectResource(FactoryType factoryType, string ObjectPath)
    {
        return m_factoryManager.m_factoryDict[factoryType].GetItem(ObjectPath);
    }

    // 将游戏物体放回对象池
    public void PushGameObjectToFactory(FactoryType factoryType, string ObjectPath, GameObject itemGo)
    {
        m_factoryManager.m_factoryDict[factoryType].PushItem(ObjectPath, itemGo);
    }
}
