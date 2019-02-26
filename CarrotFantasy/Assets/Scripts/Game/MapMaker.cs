using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour {

    public bool IsDrawLine;     //是否画线
    public GameObject GridGo;

    private float m_mapWidth;
    private float m_mapHeight;

    public float m_gridWidth;
    public float m_gridHeight;

    public const int yRow = 8;
    public const int xCol = 12;

    public int bigLevelID;
    public int levelID;

    //全部格子对象
    public GridPoint[,] gridPoints;

    //怪物路劲点
    public List<GridPoint.GridIndex> monsterPath;
    public List<Vector3> monsterPathPos; //根据索引获取到具体的位置

    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;

    //每一波产生的怪物ID列表
    public List<Round.RoundInfo> roundInfos;

    private static MapMaker _instance;

    public static MapMaker Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        InitMapMaker();
    }

    private void InitMapMaker()
    {
        CalculateSize();
        monsterPath = new List<GridPoint.GridIndex>();
        gridPoints = new GridPoint[xCol, yRow];
        for (int x = 0; x < xCol; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                GameObject go = Instantiate(GridGo, transform.position, Quaternion.identity);
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
        return new Vector3(x - m_mapWidth / 2 + m_gridWidth / 2, y - m_mapHeight / 2 + m_gridHeight / 2, -7);
    }

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
}
