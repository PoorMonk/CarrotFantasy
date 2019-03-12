using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class GridPoint : MonoBehaviour {

    
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

    private SpriteRenderer m_spriteRenderer;
    public GridIndex gridIndex;
    public GridState gridState;
    public bool isHasTower;

    private Sprite m_gridSprite;        //格子图片资源
    private Sprite m_startSprite;       //开始时格子的图片显示
    private Sprite m_cantBuildSprite;   //禁止建塔提示图片

    private GameController m_gameController;
    private GameObject m_towerListGo;
    public GameObject m_handleTowerCanvas;
    private Transform m_upLevelBtnTrans;
    private Transform m_sellBtnTrans;
    private Vector3 m_upLevelInitPos;
    private Vector3 m_sellInitPos;

    //有塔时的属性
    public GameObject towerGo;
    public Tower tower;
    public int towerLevel;
    private GameObject m_levelUpSignalGo;                 //可以升级的信号
    public TowerPersonalProperty towerPersonalProperty;

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
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();
#if Game
        m_gameController = GameController.Instance;
        m_gridSprite = m_gameController.GetSprite("NormalModel/Game/Grid");
        m_startSprite = m_gameController.GetSprite("NormalModel/Game/StartSprite");
        m_cantBuildSprite = m_gameController.GetSprite("NormalModel/Game/cantBuild");
        m_spriteRenderer.sprite = m_startSprite;
        Tween tween = DOTween.To(() => m_spriteRenderer.color, toColor => m_spriteRenderer.color = toColor, new Color(1, 1, 1, 0.2f), 1f);
        tween.OnComplete(ChangeSpriteToGrid);
        m_towerListGo = m_gameController.towerList;
        m_handleTowerCanvas = m_gameController.handleTowerCanvas;
        m_upLevelBtnTrans = m_handleTowerCanvas.transform.Find("Btn_UpLevel");
        m_sellBtnTrans = m_handleTowerCanvas.transform.Find("Btn_SellTower");
        m_upLevelInitPos = m_upLevelBtnTrans.localPosition;
        m_sellInitPos = m_sellBtnTrans.localPosition;

        m_levelUpSignalGo = transform.Find("LevelUPSignal").gameObject;
        m_levelUpSignalGo.SetActive(false);
#endif
    }

    private void Update()
    {
        if (m_levelUpSignalGo != null)
        {
            if (isHasTower)
            {
                if (towerPersonalProperty.upLevelPrice <= m_gameController.coin && towerLevel < 3)
                {
                    m_levelUpSignalGo.SetActive(true);
                }
                else
                {
                    m_levelUpSignalGo.SetActive(false);
                }
            }
            else
            {
                if (m_levelUpSignalGo.activeSelf)
                {
                    m_levelUpSignalGo.SetActive(false);
                }
            }
        }
    }

#if Game
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("ispointover");
            return;
        }
        m_gameController.HandleGrid(this);
        //Debug.Log("OnMouseDown");
    }

    public void AfterBuild()
    {
        m_spriteRenderer.enabled = false;
        //对塔的后续处理
        towerGo = transform.GetChild(1).gameObject;
        tower = towerGo.GetComponent<Tower>();
        towerPersonalProperty = towerGo.GetComponent<TowerPersonalProperty>();
        towerLevel = towerPersonalProperty.towerLevel;
    }

    private Vector3 CorretTowerListPosition()
    {
        Vector3 posVec = Vector3.zero;
        if (gridIndex.xIndex >=0 && gridIndex.xIndex <= 3)
        {
            posVec += new Vector3(m_gameController.mapMaker.m_gridWidth, 0, 0);
        }
        else if (gridIndex.xIndex >= 8 && gridIndex.xIndex <= 11)
        {
            posVec -= new Vector3(m_gameController.mapMaker.m_gridWidth, 0, 0);
        }
        if (gridIndex.yIndex >= 0 && gridIndex.yIndex <= 3)
        {
            posVec += new Vector3(0, m_gameController.mapMaker.m_gridHeight, 0);
        }
        else if (gridIndex.yIndex >= 4 && gridIndex.yIndex <= 7)
        {
            posVec -= new Vector3(0, m_gameController.mapMaker.m_gridHeight, 0);
        }
        posVec += transform.position;
        return posVec;
    }

    private void CorretUpLevelAndSellBtnPos()
    {
        m_upLevelBtnTrans.localPosition = Vector3.zero;
        m_sellBtnTrans.localPosition = Vector3.zero;
        if (gridIndex.yIndex <= 0)
        {
            m_sellBtnTrans.localPosition += new Vector3(-m_gameController.mapMaker.m_gridWidth * 3 / 4, m_gameController.mapMaker.m_gridHeight * 3 / 4, 0);
            m_upLevelBtnTrans.localPosition = m_upLevelInitPos;
        }
        else if (gridIndex.yIndex >= 7)
        {
            if (gridIndex.xIndex == 0)
            {
                m_upLevelBtnTrans.localPosition += new Vector3(m_gameController.mapMaker.m_gridWidth * 3 / 4, -m_gameController.mapMaker.m_gridHeight * 3 / 4, 0);
            }
            else
            {
                m_upLevelBtnTrans.localPosition += new Vector3(-m_gameController.mapMaker.m_gridWidth * 3 / 4, -m_gameController.mapMaker.m_gridHeight * 3 / 4, 0);
            }
            m_sellBtnTrans.localPosition = m_sellInitPos;
        }
        else
        {
            m_upLevelBtnTrans.localPosition = m_upLevelInitPos;
            m_sellBtnTrans.localPosition = m_sellInitPos;
        }
    }

    public void ShowGrid()
    {
        if (!isHasTower)
        {
            m_spriteRenderer.enabled = true;
            //显示建塔列表
            m_towerListGo.transform.position = CorretTowerListPosition();
            m_towerListGo.SetActive(true);
        }
        else
        {
            m_handleTowerCanvas.transform.position = transform.position;
            CorretUpLevelAndSellBtnPos();
            m_handleTowerCanvas.SetActive(true);
            //显示范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(true);
        }
    }

    public void HideGrid()
    {
        if (!isHasTower)
        {
            m_towerListGo.SetActive(false);
        }
        else
        {
            m_handleTowerCanvas.SetActive(false);
            towerGo.transform.Find("attackRange").gameObject.SetActive(false);
        }
        m_spriteRenderer.enabled = false;
    }

    public void ShowCantBuild()
    {
        m_spriteRenderer.enabled = true;
        Tween tween = DOTween.To(() => m_spriteRenderer.color, toColor => m_spriteRenderer.color = toColor,
            new Color(1, 1, 1, 0), 2f);
        tween.OnComplete(() =>
        {
            m_spriteRenderer.enabled = false;
            m_spriteRenderer.color = new Color(1, 1, 1, 1);
        });
    }

    private void ChangeSpriteToGrid()
    {
        m_spriteRenderer.enabled = false;
        m_spriteRenderer.color = new Color(1, 1, 1, 1);

        if (gridState.canBuild)
        {
            m_spriteRenderer.sprite = m_gridSprite;
        }
        else
        {
            m_spriteRenderer.sprite = m_cantBuildSprite;
        }
    }

    //更新格子状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            m_spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            m_spriteRenderer.enabled = false;
        }
    }

    private void CreateItem()
    {
        GameObject go = GameController.Instance.GetGameObject(GameController.Instance.mapMaker.bigLevelID.ToString() + "/Item/" + gridState.itemID.ToString());
        go.transform.SetParent(GameController.Instance.transform);
        go.GetComponent<Item>().itemID = gridState.itemID;
        Vector3 createPos = transform.position - new Vector3(0, 0, 3);
        if (gridState.itemID <= 2)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.m_gridWidth, GameController.Instance.mapMaker.m_gridHeight, 0) / 2;
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
        m_spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if Tool
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem);
#endif

#if Game
        towerGo = null;
        towerPersonalProperty = null;
        isHasTower = false;
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
