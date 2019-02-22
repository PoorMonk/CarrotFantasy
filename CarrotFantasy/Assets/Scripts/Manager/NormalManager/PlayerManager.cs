using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager  
{
    //数据显示
    public int adventrueModelNum;   //冒险模式解锁的地图个数
    public int burriedLevelNum;     //隐藏关卡解锁的地图个数
    public int bossModelNum;        //boss模式KO的BOSS
    public int coin;                //获得金币的总数
    public int killMonsterNum;      //杀怪总数
    public int killBossNum;         //杀掉BOSS的总数
    public int clearItemNum;        //清理道具的总数

    public List<bool> unLockedNormalModelBigLevelList;  //大关卡
    public List<Stage> unLockedNormalModelLevelList;    //所有的小关卡
    public List<int> unLockedNormalModelLevelNum;       //解锁小关卡数

    //怪物窝
    public int cookies;
    public int milk;
    public int nest;
    public int diamands;
}
