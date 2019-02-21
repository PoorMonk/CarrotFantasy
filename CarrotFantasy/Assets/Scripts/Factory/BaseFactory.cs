using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        GameObject itemGo = null;

        if (m_objectPoolDict.ContainsKey(itemName))
        {
            if (m_objectPoolDict[itemName].Count == 0)
            {
                GameObject go = GetResource(itemName);
                itemGo = GameManager.Instance.CreateItem(go);
            }
            else
            {
                itemGo = m_objectPoolDict[itemName].Pop();
                itemGo.SetActive(true); // 池中对象默认隐藏，需要手动设置为显示
            }
        }
        else
        {
            m_objectPoolDict.Add(itemName, new Stack<GameObject>());
            GameObject go = GetResource(itemName);
            itemGo = GameManager.Instance.CreateItem(go);
        }

        if (null == itemGo)
        {
            Debug.Log("获取实例失败: " + itemName);
        }

        return itemGo;
    }

    /// <summary>
    /// 获取资源，并未直接生成对象
    /// </summary>
    /// <param name="itemName">具体资源对象</param>
    /// <returns></returns>
    private GameObject GetResource(string itemName)
    {
        string loadPath = m_loadPath + itemName;
        GameObject itemGo = null;
        if (m_factoryDict.ContainsKey(itemName))
        {
            itemGo = m_factoryDict[itemName];
        }
        else
        {
            itemGo = Resources.Load<GameObject>(loadPath);
            m_factoryDict.Add(itemName, itemGo);
        }
        if (itemGo == null)
        {
            Debug.Log(itemName + "的资源获取失败");
            Debug.Log("失败路径:" + loadPath);
        }
        return itemGo;
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
