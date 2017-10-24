using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Food_Data
{
    public int foodId;
    public string foodName;
    public FoodGrade foodGrade;
    public FoodEffect_Main effectMain;
    public FoodEffect_Sub effectSub;
    public float mainEffectValue;
    public float subEffectValue;
    public Sprite foodSprite;

    public void SetData_Func(Food_Script _foodClass)
    {
         foodId             = _foodClass.foodId;
         foodName           = _foodClass.foodName;
         foodGrade          = _foodClass.foodGrade;
         effectMain         = _foodClass.effectMain;
         effectSub          = _foodClass.effectSub;
         mainEffectValue    = _foodClass.GetMainEffectValue_Func();
         subEffectValue     = _foodClass.GetSubEffectValue_Func();
         foodSprite         = _foodClass.foodImage.sprite;
    }
}
