using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFood_Data
{
    public int level;
    public float foodExp;
    public int haveFoodID;
    public Food_Script foodClass;
    
    public void SetData_Func(Food_Script _foodClass)
    {
        level = _foodClass.level;
        foodExp = _foodClass.remainExp;
        haveFoodID = _foodClass.foodId;
        foodClass = _foodClass;
    }
}

public struct PlayerFood_DataTemp
{
    public int level;
    public int haveFoodID;
}