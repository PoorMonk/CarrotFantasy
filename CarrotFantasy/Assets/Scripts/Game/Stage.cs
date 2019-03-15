using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 每一关卡所需的信息类
/// </summary>
public class Stage
{
    public int[] m_towerIDList;         //本关卡可以建的塔种类
    public int m_towerIDListLength;     //建塔数组长度
    public bool m_allClear;             //是否清空此关卡道具
    public int m_carrotState;           //萝卜状态
    public int m_levelID;               //小关卡ID
    public int m_bigLevelID;            //大关卡ID
    public bool m_unLocked;             //此关卡是否解锁
    public bool m_isRewardLevel;        //是否为奖励关卡
    public int m_totalRound;            //一共几波怪

    //public Stage(int totalRound, int towerIDListLength, int[] towerIDList,
    //    bool allClear, int carrotState, int levelID, int bigLevelID, bool unLocked, bool isReward)
    //{
    //    m_towerIDList = towerIDList;
    //    m_towerIDListLength = towerIDListLength;
    //    m_allClear = allClear;
    //    m_carrotState = carrotState;
    //    m_levelID = levelID;
    //    m_bigLevelID = bigLevelID;
    //    m_unLocked = unLocked;
    //    m_isRewardLevel = isReward; 
    //    m_totalRound = totalRound;
    //}
}
