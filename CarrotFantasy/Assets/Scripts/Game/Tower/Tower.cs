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
        m_circleCollider2D.radius = 5;
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

    public void GetTowerProperty()
    {

    }
}
