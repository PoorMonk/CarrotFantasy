using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T>
{
    // 获取游戏对象上的脚本
    T GetProductClass(GameObject go);

    //获取具体的游戏对象
    GameObject GetProduct();

    //获取数据信息
    void GetData(T productClass);

    //获取特有资源与信息
    void GetOtherResource(T productClass);
}
