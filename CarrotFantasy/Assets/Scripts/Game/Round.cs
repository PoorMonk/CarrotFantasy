using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round
{
    [System.Serializable]
    public struct RoundInfo
    {
        public int[] monsterIDList;
    }

    public RoundInfo roundInfo;

    protected Round m_nextRound;
    protected Level m_level;
    protected int m_currentRoundID;

    public Round(int[] monsterIDList, int roundID, Level level)
    {
        m_level = level;
        m_currentRoundID = roundID;
        roundInfo.monsterIDList = monsterIDList;
    }

    public void SetNextRound(Round nextRound)
    {
        m_nextRound = nextRound;
    }
	
    public void Handle(int roundID)
    {
        if (m_currentRoundID < roundID)
        {
            m_nextRound.Handle(roundID);
        }
        else
        {
            //产生怪物
            GameController.Instance.monsterIDList = roundInfo.monsterIDList;
            GameController.Instance.CreateMonster();
        }
    }
}
