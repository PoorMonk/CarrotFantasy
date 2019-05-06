using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int monsterID;
    private GameObject m_monsterGo;

    public void GetData(Monster productClass)
    {
        productClass.monsterID = monsterID;
        productClass.HP = monsterID * 100;
        productClass.currentHP = productClass.HP;
        productClass.initMoveSpeed = 1 + (monsterID / 3);
        productClass.moveSpeed = 1 + (monsterID / 3);
        productClass.prize = monsterID * 50;
    }

    public void GetOtherResource(Monster productClass)
    {
        productClass.GetMonsterProperty();
    }

    public GameObject GetProduct()
    {
        GameObject go = GameController.Instance.GetGameObject("MonsterPrefab");
        Monster monster = GetProductClass(go);
        GetData(monster);
        GetOtherResource(monster);
        return go;
    }

    public Monster GetProductClass(GameObject go)
    {
        return go.GetComponent<Monster>();
    }
}
