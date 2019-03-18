using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    public float m_animationTime;
    public string m_resourcePath;

    private void OnEnable()
    {
        Invoke("DestroyEffect", m_animationTime); 
    }

    private void DestroyEffect()
    {
        GameManager.Instance.m_factoryManager.m_factoryDict[FactoryType.UIFactory].PushItem(m_resourcePath, gameObject);
    }
}
