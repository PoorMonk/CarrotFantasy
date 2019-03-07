using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPersonalProperty : MonoBehaviour {
    //属性值
    public int towerLevel;          //当前塔的等级
    protected float timeVal;        //攻击计时器
    public float attackCD;          //攻击CD
    [HideInInspector]
    public int upLevelPrice;        //升级价格
    [HideInInspector]
    public int sellPrice;           //卖出价格
    public int price;               //当前塔的价格

    //资源
    protected GameObject bulletGo;  //子弹资源，用于获取对象的属性和方法

    //引用
    public Tower tower;
    public Transform targetTrans;
    public Animator animator;
    private GameController m_gameController;

    public void Init()
    {
        tower = null;
    }

    public void SellTower()
    {
        m_gameController.ChangeCoin(sellPrice);
        m_gameController.GetGameObject("BuildEffect");
        tower.DestroyTower();
    }

    public void UpLevelTower()
    {
        m_gameController.ChangeCoin(-upLevelPrice);
        m_gameController.GetGameObject("UpLevelEffect");
        tower.DestroyTower();
    }

    protected virtual void Start()
    {
        m_gameController = GameController.Instance;
        upLevelPrice = (int)(price * 1.5f);
        sellPrice = price / 2;
        animator = transform.Find("tower").GetComponent<Animator>();
        timeVal = attackCD;
    }

    protected virtual void Update()
    {
        if (m_gameController.isPause || targetTrans == null)
        {
            return;
        }
        if (!targetTrans.gameObject.activeSelf)
        {
            targetTrans = null;
            return;
        }

        if (timeVal >= attackCD / m_gameController.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
        if (targetTrans.gameObject.tag.Equals("Item"))
        {
            transform.LookAt(targetTrans.position + new Vector3(0, 0, 3));
        }
        else
        {
            transform.LookAt(targetTrans.position);
        }
        if (transform.eulerAngles.y == 0)
        {
            transform.eulerAngles += new Vector3(0, 90, 0);
        }
    }

    protected virtual void Attack()
    {
        if (targetTrans == null)
        {
            return;
        }
        animator.Play("Attack");
        bulletGo = m_gameController.GetGameObject("Tower/ID" + tower.towerID.ToString() + "/Bullet/" + towerLevel.ToString());
        bulletGo.transform.position = transform.position;
        bulletGo.GetComponent<Bullet>().targetTrans = targetTrans;
    }
}
