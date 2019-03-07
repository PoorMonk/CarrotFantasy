using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    public GridPoint gridPoint;

    public int itemID;

    private GameController m_gameController;
    private int m_prize;
    private int m_HP;
    private int m_currentHP;
    private Slider m_slider;

    private float m_timeVal;  //显示/隐藏血条
    private bool m_isShowHP;

    private void OnEnable()
    {
        if (itemID != 0)
        {
            InitItem();
        }
    }

    private void Start()
    {
        m_gameController = GameController.Instance;
        m_slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
        InitItem();
    }

    private void Update()
    {
        if (m_isShowHP)
        {
            if (m_timeVal <= 0)
            {
                m_slider.gameObject.SetActive(false);
                m_isShowHP = false;
                m_timeVal = 3;
            }
            else
            {
                m_timeVal -= Time.deltaTime;
            }
        }
    }

#if Game
    private void TakeDamage(int attackValue)
    {
        m_slider.gameObject.SetActive(true);
        m_currentHP -= attackValue;
        if (m_currentHP <= 0)
        {
            DestroyItem();
            return;
        }
        m_slider.value = (float)m_currentHP / m_HP;
        m_isShowHP = false;
        m_timeVal = 3;
    }

    private void DestroyItem()
    {
        if (m_gameController.targetTrans == transform)
        {
            m_gameController.HideSignal();
        }

        GameObject coinGo = m_gameController.GetGameObject("CoinCanvas");
        coinGo.transform.Find("Emp_Coin").GetComponent<Coin>().prize = m_prize;
        coinGo.transform.SetParent(m_gameController.transform);
        coinGo.transform.position = transform.position;

        GameObject effectGo = m_gameController.GetGameObject("DestoryEffect");
        effectGo.transform.SetParent(m_gameController.transform);
        effectGo.transform.position = transform.position;

        m_gameController.PushGameObjectToFactory(m_gameController.mapMaker.bigLevelID.ToString() + "/Item/" + itemID, gameObject);
        gridPoint.gridState.hasItem = false;
        InitItem();
    }

    private void InitItem()
    {
        m_prize = 1000 - 100 * itemID;
        m_HP = 1500 - 100 * itemID;
        m_currentHP = m_HP;
    }

    private void OnMouseDown()
    {
        if (m_gameController.targetTrans == null)
        {
            m_gameController.targetTrans = transform;
            m_gameController.ShowSignal();
        }
        else if (m_gameController.targetTrans != transform)
        {
            m_gameController.HideSignal();
            m_gameController.targetTrans = transform;
            m_gameController.ShowSignal();
        }
        else
        {
            m_gameController.HideSignal();
        }
    }
#endif
}
