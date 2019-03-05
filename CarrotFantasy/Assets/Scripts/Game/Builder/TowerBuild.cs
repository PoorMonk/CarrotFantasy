using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuild : IBuilder<Tower>
{
    public int towerID;
    public int towerLevel;
    private GameObject m_towerGo;

    public void GetData(Tower productClass)
    {
        productClass.towerID = towerID;
    }

    public void GetOtherResource(Tower productClass)
    {
        productClass.GetTowerProperty();
    }

    public GameObject GetProduct()
    {
        GameObject towerGo = GameController.Instance.GetGameObject("Tower/ID" + towerID.ToString() + "/TowerSet/" + towerLevel.ToString());
        Tower tower = GetProductClass(towerGo);
        GetData(tower);
        GetOtherResource(tower);
        return towerGo;
    }

    public Tower GetProductClass(GameObject go)
    {
        return go.GetComponent<Tower>();
    }
}
