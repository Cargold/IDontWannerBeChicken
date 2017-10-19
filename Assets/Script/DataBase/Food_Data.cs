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
    public float increaseEffectValue;
    public float subEffectValue;
    public Sprite foodSprite;
}
