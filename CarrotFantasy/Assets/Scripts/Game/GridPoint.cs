using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridPoint : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    private Sprite gridSprite;
    private Sprite monsterPathSprite; //怪物路点图片

    public GameObject[] itemPrefabs;    //道具数组
    public GameObject currentItem;      //当前格子道具

    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
    }

    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }

    public GridIndex gridIndex;
    public GridState gridState;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        gridSprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalModel/Game/1/Monster/6-1");

        itemPrefabs = new GameObject[10];
        string itemPath = "Prefabs/Game/" + MapMaker.Instance.bigLevelID.ToString() + "/Item/";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(itemPath + i.ToString());
            if (!itemPrefabs[i])
            {
                Debug.Log("加载失败，路径：" + itemPath + i);
            }
        }
        InitGrid();
    }

    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
    }


    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.P)) //怪物路点
        {
            gridState.canBuild = false;
            spriteRenderer.enabled = true;
            gridState.isMonsterPoint = !gridState.isMonsterPoint;
            if (gridState.isMonsterPoint)
            {
                MapMaker.Instance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                MapMaker.Instance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                gridState.canBuild = true;
            }
        }
        else if (Input.GetKey(KeyCode.I)) //道具
        {         
            if (gridState.itemID == itemPrefabs.Length)
            {
                gridState.itemID = 0;
                Destroy(currentItem);
                gridState.hasItem = false;
                return;
            }
            if (currentItem == null)
            {
                CreateItem();
            }
            else
            {
                Destroy(currentItem);
                CreateItem();
            }
            gridState.itemID++;
        }
        else if (!gridState.isMonsterPoint)
        {

        }
    }

    private void CreateItem()
    {
        Vector3 pos = transform.position;
        if (gridState.itemID <= 2)
        {
            pos += new Vector3(MapMaker.Instance.m_gridWidth, -MapMaker.Instance.m_gridHeight) / 2;
        }
        else if (gridState.itemID <=4)
        {
            pos += new Vector3(MapMaker.Instance.m_gridWidth, 0, 0) / 2;
        }
        currentItem = Instantiate(itemPrefabs[gridState.itemID], pos, Quaternion.identity);
        currentItem.transform.SetParent(transform);
    }
}
