using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerUnit_Data
{
    public int level;
    public Unit_Script unitClass;

    public PlayerFood_Data[] playerFoodDataArr;
}
