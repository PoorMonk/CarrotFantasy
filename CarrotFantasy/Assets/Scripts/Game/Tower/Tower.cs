using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public int towerID;
    public bool isTarget;   //是否是集火目标
    public bool hasTarget;  //是否有目标

    private SpriteRenderer m_attackRangeSr;
    private CircleCollider2D m_circleCollider2D;
    private TowerPersonalProperty m_towerPersonalProperty;

    private void Init()
    {
        m_circleCollider2D = GetComponent<CircleCollider2D>();
        m_towerPersonalProperty = GetComponent<TowerPersonalProperty>();
        m_towerPersonalProperty.tower = this;
        m_attackRangeSr = transform.Find("attackRange").GetComponent<SpriteRenderer>();
        m_attackRangeSr.gameObject.SetActive(false);
        //m_attackRangeSr.transform.localScale = new Vector3(m_towerPersonalProperty.towerLevel, m_towerPersonalProperty.towerLevel, 1);
        //m_circleCollider2D.radius = 1.1f * m_towerPersonalProperty.towerLevel;
        switch (m_towerPersonalProperty.towerLevel)
        {
            case 1:
                m_attackRangeSr.transform.localScale = new Vector3(1.75f, 1.75f, 1);
                m_circleCollider2D.radius = 1.8f;
                break;
            case 2:
                m_attackRangeSr.transform.localScale = new Vector3(m_towerPersonalProperty.towerLevel, m_towerPersonalProperty.towerLevel, 1);
                m_circleCollider2D.radius = 2.2f;
                break;
            case 3:
                m_attackRangeSr.transform.localScale = new Vector3(m_towerPersonalProperty.towerLevel, m_towerPersonalProperty.towerLevel, 1);
                m_circleCollider2D.radius = 3.3f;
                break;
            default:
                break;
        }
        isTarget = false;
        hasTarget = false;
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (GameController.Instance.isPause || GameController.Instance.IsGameOver)
        {
            return;
        }
        if (isTarget)
        {
            if (m_towerPersonalProperty.targetTrans != GameController.Instance.targetTrans)
            {
                m_towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
        if (hasTarget)
        {
            if (!m_towerPersonalProperty.targetTrans.gameObject.activeSelf)
            {
                m_towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
    }

    public void GetTowerProperty()
    {

    }

    private void TriggerLogic(Collider2D collision)
    {
        if (!collision.tag.Equals("Monster") && !collision.tag.Equals("Item") && isTarget)
        {
            return;
        }
        Transform targetTrans = GameController.Instance.targetTrans;
        if (targetTrans != null) //有集火目标
        {
            if (!isTarget) //还没有找到集火目标
            {
                //是物品且是集火目标
                if (collision.tag.Equals("Item") && targetTrans == collision.transform)
                {
                    m_towerPersonalProperty.targetTrans = targetTrans;
                    hasTarget = true;
                    isTarget = true;
                }
                else if (collision.tag.Equals("Monster")) //找到的是怪物
                {
                    if (targetTrans == collision.transform) //是集火目标
                    {
                        m_towerPersonalProperty.targetTrans = targetTrans;
                        hasTarget = true;
                        isTarget = true;
                    }
                    else if (targetTrans != collision.transform && !hasTarget)
                    {
                        m_towerPersonalProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        }
        else
        {
            if (!hasTarget && collision.tag.Equals("Monster"))
            {
                m_towerPersonalProperty.targetTrans = collision.transform;
                hasTarget = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerLogic(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerLogic(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (m_towerPersonalProperty.targetTrans == collision.transform)
        {
            m_towerPersonalProperty.targetTrans = null;
            hasTarget = false;
            isTarget = false;
        }
    }

    public void DestroyTower()
    {
        m_towerPersonalProperty.Init();
        Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + towerID.ToString() 
            + "/TowerSet/" + m_towerPersonalProperty.towerLevel.ToString(), gameObject);
    }
}
