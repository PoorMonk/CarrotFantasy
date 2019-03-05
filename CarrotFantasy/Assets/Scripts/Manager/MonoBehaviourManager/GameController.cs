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

    //引用
    public Level level;
    private GameManager m_gameManager;
    public int[] monsterIDList;     //当前波次的产怪列表
    public int monsterIDIndex;
    public Stage currentStage;
    public MapMaker mapMaker;
    public Transform targetTrans;   //集火目标
    public GameObject targetSignal; //集火信号
    public GridPoint selectedGrid;  //上一个选择的格子

    //游戏UI的面板
    public NormalModelPanel normalModelPanel;


    //游戏资源
    public RuntimeAnimatorController[] controllers; //怪物播放控制器

    //用于计数的成员变量
    public int killMonsterNum;      //当前波次杀怪数
    public int clearItemNum;        //清除道具数量
    public int killMonsterTotalNum; //杀怪总数

    //属性值
    public int carrotHP = 10;       //萝卜血量
    public int coin;                //金币数
    public int gameSpeed;           //当前游戏速度
    public bool isPause;            //是否暂停
    
    public bool IsCreateingMonster; //是否继续产怪
    public bool IsGameOver;         //是否游戏结束

    //建造者
    public MonsterBuilder monsterBuilder;
    public TowerBuild towerBuild;

    //建塔价格表
    public Dictionary<int, int> towerPriceDict;
    //建塔按钮列表
    public GameObject towerList;
    //处理塔升级与买卖的画布
    public GameObject handleTowerCanvas;



    private void Awake()
    {
#if Game
        _instance = this;
        m_gameManager = GameManager.Instance;
        //Debug.Log("GameController");
        //Debug.Log(m_gameManager);
        currentStage = m_gameManager.m_currentStage;
        normalModelPanel = m_gameManager.m_uiManager.m_uiFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker();
        mapMaker.LoadMap(currentStage.m_bigLevelID, currentStage.m_levelID);
        level = new Level(mapMaker.roundInfos.Count, mapMaker.roundInfos);
        monsterBuilder = new MonsterBuilder();
        towerBuild = new TowerBuild();
        gameSpeed = 1;
        

        for (int i = 0; i < m_gameManager.m_currentStage.m_towerIDList.Length; i++)
        {
            GameObject towerGo = m_gameManager.GetGameObjectResource(FactoryType.UIFactory, "Btn_TowerBuild");
            towerGo.GetComponent<ButtonTower>().towerID = m_gameManager.m_currentStage.m_towerIDList[i];
            towerGo.transform.SetParent(towerList.transform);
            towerGo.transform.localPosition = Vector3.zero;
            towerGo.transform.localScale = Vector3.one;
        }

        //建塔价格表
        towerPriceDict = new Dictionary<int, int>
        {
            {1, 100 },
            {2, 120 },
            {3, 140 },
            {4, 160 },
            {5, 160 }
        };

        controllers = new RuntimeAnimatorController[12];
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i] = GetRuntimeAnimatorController("AnimatorController/Monster/" + mapMaker.bigLevelID.ToString() + "/" + (i + 1).ToString());
        }
#endif
    }

    private void Update()
    {
#if Game
        if (!isPause)
        {
            if (killMonsterNum >= monsterIDList.Length)
            {
                AddRoundNum();
            }
            else
            {
                if (!IsCreateingMonster)
                {
                    CreateMonster();
                }
            }
        }
        else //暂停
        {
            StopCreateMonster();
            IsCreateingMonster = false;
        }
#endif
    }

#if Game
    public void HandleGrid(GridPoint grid)
    {
        if (grid.gridState.canBuild)
        {
            if (selectedGrid == null) //上一个格子不存在
            {
                selectedGrid = grid;
                selectedGrid.ShowGrid();
            }
            else if (grid == selectedGrid) //点击同一个格子
            {
                grid.HideGrid();
                selectedGrid = null;
            }
            else if (grid != selectedGrid)
            {
                selectedGrid.HideGrid();
                selectedGrid = grid;
                selectedGrid.ShowGrid();
            }
        }
        else
        {
            grid.HideGrid();
            grid.ShowCantBuild();
            if (selectedGrid != null)
            {
                selectedGrid.HideGrid();
            }
        }
    }

    public void ShowSignal()
    {
        targetSignal.transform.position = targetTrans.position + new Vector3(0, mapMaker.m_gridHeight / 2, 0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
    }

    public void HideSignal()
    {
        targetSignal.SetActive(false);
        targetTrans = null;
    }
#endif

    public void CreateMonster()
    {
        IsCreateingMonster = true;
        InvokeRepeating("InstiantiateMonster", (float)1 / gameSpeed, (float)1 / gameSpeed);
    }

    private void StopCreateMonster()
    {
        CancelInvoke();
    }

    public void AddRoundNum()
    {
        monsterIDIndex = 0;
        killMonsterNum = 0;
        level.AddRoundNum();
        level.HandleRound();
    }

    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
    }

    private void InstantiateMonster()
    {
        GameObject effectGo = GetGameObject("CreateEffect");
        effectGo.transform.SetParent(transform);
        effectGo.transform.position = mapMaker.monsterPathPos[0];
        
        if (monsterIDIndex < monsterIDList.Length)
        {
            monsterBuilder.monsterID = level.roundList[level.currentRound].roundInfo.monsterIDList[monsterIDIndex];
        }

        GameObject monsterGo = monsterBuilder.GetProduct();
        monsterGo.transform.SetParent(transform);
        monsterGo.transform.position = mapMaker.monsterPathPos[0];
        monsterIDIndex++;
        if (monsterIDIndex >= monsterIDList.Length)
        {
            StopCreateMonster();
        }
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
