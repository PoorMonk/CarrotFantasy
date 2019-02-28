using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 游戏控制管理，负责整个游戏逻辑控制
/// </summary>
public class GameController : MonoBehaviour {
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    public Level level;
    private GameManager m_gameManager;
    public int[] monsterIDList;     //当前波次的产怪列表
    public int monsterIDIndex;
    public Stage currentStage;

    //游戏UI的面板
    public NormalModelPanel normalModelPanel;

    public MapMaker mapMaker;

    private void Awake()
    {
#if Game
        _instance = this;
        m_gameManager = GameManager.Instance;
        currentStage = m_gameManager.m_currentStage;
        normalModelPanel = m_gameManager.m_uiManager.m_uiFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentStage.m_bigLevelID, currentStage.m_levelID);
#endif
    }

    public Sprite GetSprite(string resourcePath)
    {
        return m_gameManager.GetSprite(resourcePath);
    }

    public AudioClip GetAudioClip(string resourcePath)
    {
        return m_gameManager.GetAudioClip(resourcePath);
    }

    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return m_gameManager.GetRuntimeAnimatorController(resourcePath);
    }

    public GameObject GetGameObject(string resourcePath)
    {
        return m_gameManager.GetGameObjectResource(FactoryType.GameFactory, resourcePath);
    }

    public void PushGameObjectToFactory(string resourcePath, GameObject go)
    {
        m_gameManager.PushGameObjectToFactory(FactoryType.GameFactory, resourcePath, go);
    }
}
