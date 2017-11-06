using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerTrophy_Data
{
    public TrophyType trophyType;
    public int haveNum;
    public int haveNumLimit;

    public void AddTrophy_Func()
    {
        if(haveNum + 1 <= haveNumLimit)
        {
            haveNum++;
        }
    }
}
