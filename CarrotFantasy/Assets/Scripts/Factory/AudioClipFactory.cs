using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipFactory : IBaseResourceFactory<AudioClip>
{
    private string m_loadPath;

    private Dictionary<string, AudioClip> m_audioClipDict = new Dictionary<string, AudioClip>();

    public AudioClipFactory()
    {
        m_loadPath = "AudioClips/";
    }

    public AudioClip GetSingleResource(string resourcePath)
    {
        AudioClip itemGo = null;
        string itemLoadPath = m_loadPath + resourcePath;
        if (m_audioClipDict.ContainsKey(resourcePath))
        {
            itemGo = m_audioClipDict[resourcePath];
        }
        else
        {
            itemGo = Resources.Load<AudioClip>(itemLoadPath);
            m_audioClipDict.Add(resourcePath, itemGo);
        }

        if (null == itemGo)
        {
            Debug.Log("获取AudioClip失败，路径： " + resourcePath);
        }

        return itemGo;
    }
}
