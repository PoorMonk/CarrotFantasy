using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Memento
{
    //存储
    public void SaveToJson()
    {
        PlayerManager playerManager = GameManager.Instance.m_playerManager;
        string filePath = Application.streamingAssetsPath + "/Json/" + "playerManager.json";
        string saveJsonStr = JsonMapper.ToJson(playerManager);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
	
    //读取
    public PlayerManager LoadFormJson()
    {
        PlayerManager playerManager = new PlayerManager();
        string filePath = "";
        if (GameManager.Instance.initPlayerManager)
        {
            //filePath = Application.persistentDataPath + "/Json/" + "playerManagerInitData.json";
            filePath = Application.streamingAssetsPath + "/Json/" + "playerManagerInitData.json";
        }
        else
        {
            filePath = Application.streamingAssetsPath + "/Json/" + "playerManager.json";
        }

        WWW wWW = new WWW(filePath);
        if (wWW.error != null)
        {
            Debug.Log("WWW error:" + filePath);
            return null;
        }

        while (!wWW.isDone)
        {

        }

        playerManager = JsonMapper.ToObject<PlayerManager>(wWW.text);
        return playerManager;
        //if (File.Exists(filePath))
        //{
        //    StreamReader sr = new StreamReader(filePath);
        //    string jsonStr = sr.ReadToEnd();
        //    sr.Close();
        //    playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
        //    return playerManager;
        //}
        //else
        //{
        //    Debug.Log("PlayerManager读取失败: " + filePath);
        //    return null;
        //}
    }
    
}
