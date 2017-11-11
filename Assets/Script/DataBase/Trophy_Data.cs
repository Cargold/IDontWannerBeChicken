using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Trophy_Data
{
    public int trophyID;
    public Sprite trophySprite;
    public TrophyType effectType;
    public int amountLimit;
    public float effectValue;

    public void SetData_Func(Trophy_Script _trophyClass)
    {
        trophyID     = _trophyClass.trophyID;
        trophySprite = _trophyClass.trophySprite;
        effectType   = _trophyClass.effectType;
        amountLimit  = _trophyClass.amountLimit;
        effectValue  = _trophyClass.effectValue;
    }
}
