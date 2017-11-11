using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Drink_Data
{
    public int drinkID;
    public DrinkType drinkType;
    public float effectValue;
    public Sprite drinkSprite;
    public Sprite drinkBtnSprite;

    public void SetData_Func(Drink_Script _drinkClass)
    {
        drinkID        = _drinkClass.drinkID;
        drinkType      = _drinkClass.drinkType;
        effectValue = _drinkClass.effectValue * 0.01f;
        drinkSprite    = _drinkClass.drinkSprite;
        drinkBtnSprite = _drinkClass.drinkBtnSprite;
    }
}
