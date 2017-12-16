using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Food_Data
{
    public FoodType foodType;
    public int foodId;
    public string[] nameArr;
    public FoodGrade foodGrade;
    public FoodEffect_Main effectMain;
    public FoodEffect_Sub effectSub;
    public float mainEffectValue;
    public float subEffectValue;
    public Sprite foodSprite;

    public void SetData_Func(Food_Script _foodClass)
    {
        foodType           = _foodClass.foodType;
        foodId             = _foodClass.foodId;
        nameArr           = _foodClass.nameArr;
        foodGrade          = _foodClass.foodGrade;
        effectMain         = _foodClass.effectMain;
        effectSub          = _foodClass.effectSub;
        mainEffectValue    = _foodClass.GetMainEffectValue_Func();
        subEffectValue     = _foodClass.GetSubEffectValue_Func();
        foodSprite         = _foodClass.foodImage.sprite;

        int _nameNum = _foodClass.nameArr.Length;
        nameArr = new string[_nameNum];
        for (int i = 0; i < _nameNum; i++)
        {
            nameArr[i] = _foodClass.nameArr[i];
        }
    }
}
