using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFood_ClassData
{
    public FoodType foodType;
    public int foodID;
    public int level;
    public float remainExp;
    public Vector3 pos;
    public Vector3 rot;
    
    [SerializeField]
    private Food_Script foodClass;

    public PlayerFood_ClassData(FoodType _foodType, int _foodID, int _level, float _exp = 0f)
    {
        foodType = _foodType;
        foodID = _foodID;
        level = _level;
        remainExp = _exp;

        pos = Vector3.zero;
        rot = Vector3.zero;

        foodClass = null;
    }
    public PlayerFood_ClassData(Food_Script _foodClass)
    {
        SetData_Func(_foodClass);
    }
    public PlayerFood_ClassData(SaveSystem_Manager.SaveFoodDataStr _saveFoodDataStr)
    {
        foodType = _saveFoodDataStr.foodType;
        foodID = _saveFoodDataStr.foodID;
        level = _saveFoodDataStr.foodLevel;
        remainExp = _saveFoodDataStr.foodExp;

        pos = _saveFoodDataStr.foodPos;
        rot = _saveFoodDataStr.foodRot;

        foodClass = null;
    }

    public void SetData_Func(Food_Script _foodClass)
    {
        foodType = _foodClass.foodType;
        level = _foodClass.level;
        remainExp = _foodClass.remainExp;
        foodID = _foodClass.foodId;

        pos = _foodClass.transform.position;
        rot = _foodClass.transform.rotation.eulerAngles;

        foodClass = _foodClass;
    }
    public Food_Script GetFoodClass_Func()
    {
        if (foodClass != null)
            return foodClass;
        else
        {
            Debug.LogError("Bug : 현 상황에서 음식 클래스를 반환하는 상황이 발생하면 안 됨");
            // 구조체여서 동일한 음식끼리 구분 짓는게 어려움
            // 그래서 이곳의 음식 클래스는 임시로 구분 용도로 사용하는 거임
            return null;
        }
    }
}