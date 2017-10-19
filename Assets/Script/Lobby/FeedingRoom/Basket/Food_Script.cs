using UnityEngine;
using UnityEngine.UI;

public class Food_Script : MonoBehaviour
{
    public FeedingRoom_Script feedingRoomClass;

    public int foodId;
    public string foodName;
    public FoodGrade foodGrade;
    public FoodEffect_Main effectMain;
    public FoodEffect_Sub effectSub;
    public float mainEffectValue;
    public float increaseEffectValue;
    public float subEffectValue;
    public Image foodImage;

    public int level;
    public float maxExp;
    public float recentExp;
    public float materialExp;

    public void Init_Func(FeedingRoom_Script _feedingRoomClass, Food_Data _foodData, int _level, float _exp = 0f)
    {
        feedingRoomClass = _feedingRoomClass;

        foodId = _foodData.foodId;
        foodName = _foodData.foodName;
        foodGrade = _foodData.foodGrade;
        effectMain = _foodData.effectMain;
        effectSub = _foodData.effectSub;
        mainEffectValue = _foodData.mainEffectValue;
        increaseEffectValue = _foodData.increaseEffectValue;
        subEffectValue = _foodData.subEffectValue;
        foodImage.sprite = _foodData.foodSprite;
        foodImage.SetNativeSize();
        foodImage.alphaHitTestMinimumThreshold = 0.5f;

        for (int i = 0; i < _level; i++)
        {
            LevelUp_Func();
        }
        recentExp = _exp;
    }
    
    public void ExpUp_Func(float _expValue)
    {
        while(0f<_expValue)
        {
            float _calcExp = maxExp - recentExp;
            if (0f <= _expValue - _calcExp)
            {
                _expValue -= _calcExp;
                LevelUp_Func();
            }
            else
            {
                recentExp += _expValue;
            }
        }
    }
    
    public void LevelUp_Func()
    {
        level++;

        maxExp = (level - 1) * 200f;
        maxExp += 100f;

        materialExp = 100f * level;
        switch (foodGrade)
        {
            case FoodGrade.Common:
                break;
            case FoodGrade.Rare:
                maxExp *= 2.0f;
                materialExp *= 2.0f;
                break;
            case FoodGrade.Legend:
                maxExp *= 4.0f;
                materialExp *= 4.0f;
                break;
        }
    }

    public void PointDown_Func()
    {
        feedingRoomClass.PointDown_Func(this);
    }

    public void PointUp_Func()
    {
        feedingRoomClass.PointUp_Func(this);
    }

    public void DragBegin_Func()
    {
        feedingRoomClass.DragBegin_Func(this);
    }

    public void Dragging_Func()
    {
        feedingRoomClass.Dragging_Func(this);
    }

    public void DragEnd_Func()
    {
        feedingRoomClass.DragEnd_Func(this);
    }
}
