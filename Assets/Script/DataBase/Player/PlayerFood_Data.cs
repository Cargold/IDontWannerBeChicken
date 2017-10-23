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

    public Vector3 pos;
    public Vector3 rot;
    
    public IEnumerator Init_Cor()
    {


        yield break;
    }

    public void SetData_Func(Food_Script _foodClass)
    {
        level = _foodClass.level;
        foodExp = _foodClass.remainExp;
        haveFoodID = _foodClass.foodId;
        foodClass = _foodClass;
    }

    public void SetFood_Func()
    {

    }
}

public struct PlayerFood_DataTemp
{
    public int level;
    public int haveFoodID;
}