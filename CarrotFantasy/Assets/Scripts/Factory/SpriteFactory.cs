using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFactory : IBaseResourceFactory<Sprite>
{
    protected Dictionary<string, Sprite> m_runtimeAnimatorControllerDict = new Dictionary<string, Sprite>();
    protected string m_loadPath;

    public SpriteFactory()
    {
        m_loadPath = "Pictures/";
    }

    public Sprite GetSingleResource(string resourcePath)
    {
        Sprite itemGo = null;

        string itemLoadPath = m_loadPath + resourcePath;

        if (m_runtimeAnimatorControllerDict.ContainsKey(resourcePath))
        {
            itemGo = m_runtimeAnimatorControllerDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<Sprite>(itemLoadPath);
            m_runtimeAnimatorControllerDict.Add(resourcePath, itemGo);
        }

        if (null == itemGo)
        {
            Debug.Log("获取Sprite失败，路径： " + itemLoadPath);
        }

        return itemGo;
    }

}
