using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class GridPoint : MonoBehaviour {

    private SpriteRenderer spriteRenderer;

    private Sprite gridSprite;

#if Tool
    private Sprite monsterPathSprite; //怪物路点图片
    public GameObject[] itemPrefabs;    //道具数组
    public GameObject currentItem;      //当前格子道具
#endif

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
#if Tool
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
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();
    }

#if Game
    //更新格子状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    private void CreateItem()
    {
        GameObject go = GameController.Instance.GetGameObject(GameController.Instance.mapMaker.bigLevelID.ToString() + "/Item/" + gridState.itemID.ToString());
        go.transform.SetParent(GameController.Instance.transform);

        Vector3 createPos = transform.position - new Vector3(0, 0, 3);
        if (gridState.itemID <= 2)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.m_gridWidth, GameController.Instance.mapMaker.m_gridHeight) / 2;
        }
        else if (gridState.itemID <= 4)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.m_gridWidth, 0, 0) / 2;
        }
        go.transform.position = createPos;
        go.GetComponent<Item>().gridPoint = this;
    }
#endif

    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if Tool
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem);
#endif
    }

#if Tool
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
            gridState.itemID++;
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
            gridState.hasItem = true;
        }
        else if (!gridState.isMonsterPoint)
        {
            gridState.isMonsterPoint = false;
            gridState.canBuild = !gridState.canBuild;
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
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
        //Debug.Log(gridState.itemID);
        currentItem = Instantiate(itemPrefabs[gridState.itemID], pos, Quaternion.identity);
        currentItem.transform.SetParent(transform);
    }

    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            if (gridState.isMonsterPoint)
            {
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }
#endif
}
