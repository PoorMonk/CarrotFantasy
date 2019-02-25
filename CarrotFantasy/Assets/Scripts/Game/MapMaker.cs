using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour {

    public bool IsDrawLine;     //是否画线
    public GameObject GridGo;

    private float m_mapWidth;
    private float m_mapHeight;

    private float m_gridWidth;
    private float m_gridHeight;

    public const int yRow = 8;
    public const int xCol = 12;

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
        InitMap();
    }

    private void InitMap()
    {
        CalculateSize();
        for (int x = 0; x < xCol; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                GameObject go = Instantiate(GridGo, transform.position, Quaternion.identity);
                go.transform.position = CorretPosition(x * m_gridWidth, y * m_gridHeight);
                go.transform.SetParent(transform);
            }
        }
    }

    private Vector3 CorretPosition(float x, float y)
    {
        return new Vector3(x - m_mapWidth / 2 + m_gridWidth / 2, y - m_mapHeight / 2 + m_gridHeight / 2, 0);
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
}
