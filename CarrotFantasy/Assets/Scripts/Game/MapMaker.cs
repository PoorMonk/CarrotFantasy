using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class MapMaker : MonoBehaviour {
#if Tool
    public bool IsDrawLine;     //是否画线
    public GameObject GridGo;

    private static MapMaker _instance;
    public static MapMaker Instance
    {
        get
        {
            return _instance;
        }
    }
#endif

    private float m_mapWidth;
    private float m_mapHeight;

    [HideInInspector]
    public float m_gridWidth;
    [HideInInspector]
    public float m_gridHeight;

    public const int yRow = 8;
    public const int xCol = 12;

    [HideInInspector]
    public int bigLevelID;
    [HideInInspector]
    public int levelID;

    //全部格子对象
    public GridPoint[,] gridPoints;

    //怪物路劲点
    [HideInInspector]
    public List<GridPoint.GridIndex> monsterPath;
    [HideInInspector]
    public List<Vector3> monsterPathPos; //根据索引获取到具体的位置

    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;

    //每一波产生的怪物ID列表
    public List<Round.RoundInfo> roundInfos;

    [HideInInspector]
    public Carrot carrot;

    private void Awake()
    {
#if Tool
        _instance = this;
        InitMapMaker();
#endif
    }

    public void InitMapMaker()
    {
        CalculateSize();
        monsterPath = new List<GridPoint.GridIndex>();
        gridPoints = new GridPoint[xCol, yRow];
        for (int x = 0; x < xCol; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
#if Tool
                GameObject go = Instantiate(GridGo, transform.position, Quaternion.identity);
#endif
#if Game
                GameObject go = GameController.Instance.GetGameObject("Grid");
#endif
                go.transform.position = CorretPosition(x * m_gridWidth, y * m_gridHeight);
                go.transform.SetParent(transform);
                gridPoints[x, y] = go.GetComponent<GridPoint>();
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;
            }
        }
        bgSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();
    }

    private Vector3 CorretPosition(float x, float y)
    {
        return new Vector3(x - m_mapWidth / 2 + m_gridWidth / 2, y - m_mapHeight / 2 + m_gridHeight / 2);
    }

#if Game
    //加载地图
    public void LoadMap(int bigLevel, int level)
    {
        bigLevelID = bigLevel;
        levelID = level;
        LoadLevelInfo(LoadLevelInfoFromJson("Level" + bigLevelID.ToString() + "_" + levelID.ToString() + ".json"));
        monsterPathPos = new List<Vector3>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            monsterPathPos.Add(gridPoints[monsterPath[i].xIndex, monsterPath[i].yIndex].transform.position);
        }

        //起始点与终止点
        GameObject startPoint = GameController.Instance.GetGameObject("startPoint");
        startPoint.transform.position = monsterPathPos[0];
        startPoint.transform.SetParent(transform);

        GameObject endPoint = GameController.Instance.GetGameObject("Carrot");
        endPoint.transform.position = monsterPathPos[monsterPathPos.Count - 1] - new Vector3(0, 0, 1);
        endPoint.transform.SetParent(transform);

        carrot = endPoint.GetComponent<Carrot>();
    }
#endif

    public void CalculateSize()
    {
        // 视口中左下右上的位置
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightTop = new Vector3(1, 1);

        Vector3 posOne = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 posTwo = Camera.main.ViewportToWorldPoint(rightTop);

        m_mapWidth = posTwo.x - posOne.x;
        m_mapHeight = posTwo.y - posOne.y;

        m_gridWidth = m_mapWidth / xCol;
        m_gridHeight = m_mapHeight / yRow;
        //Debug.Log(m_gridWidth);
        //Debug.Log(m_gridHeight);
    }

#if Tool
    private void OnDrawGizmos()
    {
        if (IsDrawLine)
        {
            CalculateSize();
            Gizmos.color = Color.green;

            //画行
            for (int y = 0; y <= yRow; y++)
            {
                Vector3 startPos = new Vector3(-m_mapWidth / 2, -m_mapHeight / 2 + y * m_gridHeight);
                Vector3 endPos = new Vector3(m_mapWidth / 2, -m_mapHeight / 2 + y * m_gridHeight);
                Gizmos.DrawLine(startPos, endPos);
                //Debug.Log(startPos);
            }

            //画列
            for (int x = 0; x <= xCol; x++)
            {
                Vector3 startPos = new Vector3(-m_mapWidth / 2 + x * m_gridWidth, -m_mapHeight / 2);
                Vector3 endPos = new Vector3(-m_mapWidth / 2 + x * m_gridWidth, m_mapHeight / 2);
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
#endif

    /// <summary>
    /// 有关地图编辑的方法
    /// </summary>
 
    //清除怪物路点
    public void ClearMonsterPath()
    {
        monsterPath.Clear();
    }

    //恢复地图编辑默认状态
    public void RecoverTowerPoint()
    {
        ClearMonsterPath();
        for (int x = 0; x < xCol; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].InitGrid();
            }
        }
    }

    //初始化地图
    public void InitMap()
    {
        bigLevelID = 0;
        levelID = 0;
        RecoverTowerPoint();
        roundInfos.Clear();
        bgSR.sprite = null;
        roadSR.sprite = null;
    }

#if Tool
    //生成LevelInfo对象来保存文件
    private LevelInfo CreateLevelInfo()
    {
        LevelInfo levelInfo = new LevelInfo()
        {
            bigLevelID = this.bigLevelID,
            levelID = this.levelID
        };
        levelInfo.gridPoints = new List<GridPoint.GridState>();
        for (int x = 0; x < xCol; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                levelInfo.gridPoints.Add(gridPoints[x, y].gridState);
            }
        }
        levelInfo.monsterPath = new List<GridPoint.GridIndex>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            levelInfo.monsterPath.Add(monsterPath[i]);
        }
        levelInfo.roundInfo = new List<Round.RoundInfo>();
        for (int i = 0; i < roundInfos.Count; i++)
        {
            levelInfo.roundInfo.Add(roundInfos[i]);
        }
        Debug.Log("保存成功");
        return levelInfo;
    }

    //将数据文件保存到Json
    public void SaveLevelfileToJson()
    {
        LevelInfo levelInfo = CreateLevelInfo();
        string filePath = Application.dataPath + "/Resources/Json/Level/Level" + bigLevelID.ToString() + "_" + levelID.ToString() + ".json";
        StreamWriter streamWriter = new StreamWriter(filePath);
        string content = JsonMapper.ToJson(levelInfo);
        streamWriter.Write(content);
        streamWriter.Close();
    }
#endif

    //读取Json文件
    public LevelInfo LoadLevelInfoFromJson(string fileName)
    {
        LevelInfo levelInfo = new LevelInfo();
        string filePath = Application.dataPath + "/Resources/Json/Level/" + fileName;
        if (!File.Exists(filePath))
        {
            Debug.Log("文件读取失败，filePath:" + filePath);
            return null;
        }
        StreamReader sr = new StreamReader(filePath);
        string content = sr.ReadToEnd();
        levelInfo = JsonMapper.ToObject<LevelInfo>(content);
        sr.Close();
        return levelInfo;
    }

    //加载LevelInfo信息到MapMaker
    public void LoadLevelInfo(LevelInfo levelInfo)
    {
        bigLevelID = levelInfo.bigLevelID;
        levelID = levelInfo.levelID;
        for (int x = 0; x < xCol; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].gridState = levelInfo.gridPoints[y + x * yRow];
#if Tool
                // 改变状态
                gridPoints[x, y].UpdateGrid();
#endif
                gridPoints[x, y].UpdateGrid();
            }
        }
        monsterPath.Clear();
        for (int i = 0; i < levelInfo.monsterPath.Count; i++)
        {
            monsterPath.Add(levelInfo.monsterPath[i]);
        }
        roundInfos = new List<Round.RoundInfo>();
        for (int i = 0; i < levelInfo.roundInfo.Count; i++)
        {
            roundInfos.Add(levelInfo.roundInfo[i]);
        }
        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/" + bigLevelID.ToString() + "/BG" + (levelID / 3).ToString());
        roadSR.sprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/" + bigLevelID.ToString() + "/Road" + levelID.ToString());
    }
}
