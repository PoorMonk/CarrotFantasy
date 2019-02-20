using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeAnimatorControllerFactory : IBaseResourceFactory<RuntimeAnimatorController>
{
    protected Dictionary<string, RuntimeAnimatorController> m_runtimeAnimatorControllerDict = new Dictionary<string, RuntimeAnimatorController>();
    protected string m_loadPath;

    public RuntimeAnimatorControllerFactory()
    {
        m_loadPath = "Animator/";
    }

    public RuntimeAnimatorController GetSingleResource(string resourcePath)
    {
        RuntimeAnimatorController itemGo = null;

        string itemLoadPath = m_loadPath + resourcePath;

        if (m_runtimeAnimatorControllerDict.ContainsKey(resourcePath))
        {
            itemGo = m_runtimeAnimatorControllerDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<RuntimeAnimatorController>(itemLoadPath);
            m_runtimeAnimatorControllerDict.Add(resourcePath, itemGo);
        }

        if (null == itemGo)
        {
            Debug.Log("获取runtimeAnimatorController失败，路径： " + itemLoadPath);
        }

        return itemGo;
    }
}
