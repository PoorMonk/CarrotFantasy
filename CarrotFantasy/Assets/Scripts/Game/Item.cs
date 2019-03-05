using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public GridPoint gridPoint;

    public int itemID;

    private GameController m_gameController;

    private void Start()
    {
        m_gameController = GameController.Instance;
    }

#if Game
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
