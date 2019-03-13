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
    public List<MonsterPetData> monsterPetDatas;

    //用于测试
    public PlayerManager()
    {
        adventrueModelNum = 100;
        burriedLevelNum = 100;
        bossModelNum = 100;
        coin = 1000;
        killBossNum = 10;
        killMonsterNum = 200;
        clearItemNum = 100;

        unLockedNormalModelLevelNum = new List<int>() { 2, 2, 2 };
        unLockedNormalModelBigLevelList = new List<bool>() { true, true, true };
        unLockedNormalModelLevelList = new List<Stage>() {
            new Stage(10, 2, new int[]{ 1,2 }, false, 0, 1, 1, true, false),
            new Stage(10, 2, new int[]{ 3,2 }, false, 0, 2, 1, true, false),
            new Stage(10, 2, new int[]{ 4,2 }, false, 0, 3, 1, true, false),
            new Stage(10, 2, new int[]{ 5,2 }, false, 0, 4, 1, true, false),
            new Stage(10, 2, new int[]{ 6,2 }, false, 0, 5, 1, true, true),
            new Stage(10, 2, new int[]{ 6,2 }, false, 0, 1, 2, true, true),
            new Stage(10, 2, new int[]{ 6,2 }, false, 0, 2, 2, true, true),
            new Stage(10, 2, new int[]{ 6,2 }, false, 0, 3, 2, true, true),
            new Stage(10, 2, new int[]{ 6,2 }, false, 0, 4, 2, true, true),
            new Stage(10, 2, new int[]{ 6,2 }, false, 0, 5, 2, true, true)
        };
    }
}
