using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase_Manager : MonoBehaviour
{
    public static DataBase_Manager Instance;

    public GameObject[] unitDataObjArr;
    public GameObject[] foodDataObjArr;

    public Unit_Data[] unitDataArr
    {
        get
        {
            return m_UnitDataArr;
        }
    }
    [SerializeField]
    private Unit_Data[] m_UnitDataArr;

    public Food_Data[] foodDataArr
    {
        get
        {
            return m_FoodDataArr;
        }
    }
    [SerializeField]
    private Food_Data[] m_FoodDataArr;

    public Skill_Data[] skillDataArr
    {
        get
        {
            return m_SkillDataArr;
        }
    }
    [SerializeField]
    private Skill_Data[] m_SkillDataArr;
    
    public Sprite populationPointSprite;
    public Sprite[] wealthSpriteArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        int _unitDataObjNum = unitDataObjArr.Length;
        m_UnitDataArr = new Unit_Data[_unitDataObjNum];
        for (int i = 0; i < _unitDataObjNum; i++)
        {
            Unit_Script _unitClass = unitDataObjArr[i].GetComponent<Unit_Script>();
            m_UnitDataArr[i].SetData_Func(_unitClass, i);
        }

        int _foodDataObjNum = foodDataObjArr.Length;
        m_FoodDataArr = new Food_Data[_foodDataObjNum];
        for (int i = 0; i < _foodDataObjNum; i++)
        {
            Food_Script _foodClass = foodDataObjArr[i].GetComponent<Food_Script>();
            _foodClass.foodId = i;
            m_FoodDataArr[i].SetData_Func(_foodClass);
        }

        yield break;
    }

    public string GetUnitName_Func(int _unitID)
    {
        return m_UnitDataArr[_unitID].charName;
    }
    public string GetFoodName_Func(int _foodID)
    {
        return m_FoodDataArr[_foodID].foodName;
    }
}
