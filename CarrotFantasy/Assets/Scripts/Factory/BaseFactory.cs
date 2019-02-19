using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFactory : IBaseFactory
{
    // 当前拥有的gameobject类型的资源（UI、UIPanel、Game），存放的是游戏物体资源
    protected Dictionary<string, GameObject> m_factoryDict = new Dictionary<string, GameObject>();

    // 每一种对象对应一个对象池
    protected Dictionary<string, Stack<GameObject>> m_objectPoolDict = new Dictionary<string, Stack<GameObject>>();

    // 加载路径
    protected string m_loadPath;

    public BaseFactory()
    {
        m_loadPath = "Prefabs/";
    }

    public GameObject GetItem(string itemName)
    {
        throw new System.NotImplementedException();
    }

    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform);
        if (m_objectPoolDict.ContainsKey(itemName))
        {
            m_objectPoolDict[itemName].Push(item);
        }
        else
        {
            Debug.Log("当前字典没有" + itemName + "的栈");
        }
    }
}
