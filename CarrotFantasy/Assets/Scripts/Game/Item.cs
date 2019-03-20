using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
#if Game
            InitItem();
#endif
        }
    }

    private void Start()
    {
#if Tool
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Mask").GetComponent<BoxCollider>().enabled = false;
#endif
        m_gameController = GameController.Instance;
        m_slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
#if Game
        InitItem();
#endif
        m_slider.gameObject.SetActive(false);
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
        m_gameController.ChangeCoin(m_prize);

        GameObject effectGo = m_gameController.GetGameObject("DestoryEffect");
        effectGo.transform.SetParent(m_gameController.transform);
        effectGo.transform.position = transform.position;

        m_gameController.PushGameObjectToFactory(m_gameController.mapMaker.bigLevelID.ToString() + "/Item/" + itemID, gameObject);
        gridPoint.gridState.hasItem = false;
        InitItem();

        m_gameController.PlayEffectMusic("NormalModel/Item");
    }

    private void InitItem()
    {
        m_prize = 1000 - 100 * itemID;
        m_HP = 1500 - 100 * itemID;
        m_currentHP = m_HP;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //点击到的是UI
        {
            return;
        }
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
