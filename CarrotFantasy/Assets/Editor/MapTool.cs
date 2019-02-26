﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(MapMaker))]
public class MapTool : Editor {

    private MapMaker mapMaker;
    //关卡文件列表
    private List<FileInfo> fileList = new List<FileInfo>();
    private string[] fileNameList;

    //当前关卡索引
    private int selectedIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            mapMaker = MapMaker.Instance;

            EditorGUILayout.BeginHorizontal();
            fileNameList = GetNames(fileList);
            int currentIndex = EditorGUILayout.Popup(selectedIndex, fileNameList);
            if (currentIndex != selectedIndex) //当前选择对象是否改变
            {
                selectedIndex = currentIndex;

                //实例化地图
                mapMaker.InitMap();

                //加载当前选择的level文件
            }

            if (GUILayout.Button("读取Stage文件列表"))
            {
                LoadStageFiles();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("恢复地图编辑器默认状态"))
            {
                mapMaker.RecoverTowerPoint();
            }

            if (GUILayout.Button("清除怪物路点"))
            {
                mapMaker.ClearMonsterPath();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("保存当前关卡数据文件"))
            {

            }
        }
    }

    //加载关卡数据文件
    private void LoadStageFiles()
    {
        ClearList();
        fileList = GetStageFiles();
    }

    //清除文件列表
    private void ClearList()
    {
        fileList.Clear();
        selectedIndex = -1;
    }

    //具体读取关卡列表
    private List<FileInfo> GetStageFiles()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/Json/Level/", "*.json");
        List<FileInfo> fList = new List<FileInfo>();
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo file = new FileInfo(files[i]);
            fList.Add(file);
        }
        return fList;
    }

    //获取关卡文件的名字
    private string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (FileInfo file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }
}
