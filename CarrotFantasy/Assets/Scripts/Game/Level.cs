using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int totalRound;
    public int currentRound;
    public Round[] roundList;
	
    public Level(int roundNum, List<Round.RoundInfo> roundInfoList)
    {
        totalRound = roundNum;
        roundList = new Round[totalRound];
        for (int i = 0; i < totalRound; i++)
        {
            roundList[i] = new Round(roundInfoList[i].monsterIDList, i, this);
        }
        for (int i = 0; i < totalRound - 1; i++)
        {
            roundList[i].SetNextRound(roundList[i + 1]);
        }
    }

    public void HandleRound()
    {
        if (currentRound >= totalRound)
        {
            //胜利
        }
        else if (currentRound == totalRound - 1)
        {
            //处理最后一波怪的音乐
        }
        else
        {
            roundList[currentRound].Handle(currentRound);
        }
    }

    public void HandleFinalRound()
    {
        roundList[currentRound].Handle(currentRound);
    }

    public void AddRoundNum()
    {
        currentRound++;
    }
}
