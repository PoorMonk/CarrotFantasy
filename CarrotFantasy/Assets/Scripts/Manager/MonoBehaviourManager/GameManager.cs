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

    public static GameManager Instance { get { return _instance;} }

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		_instance = this;

		// 各Manager初始化的时候注意顺序
		m_playerManager = new PlayerManager();
		m_factoryManager = new FactoryManager();
		m_audioSourceManager = new AudioSourceManager();
		m_uiManager = new UIManager();
	}
}
