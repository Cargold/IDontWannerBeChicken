using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase_Manager : MonoBehaviour
{
    public static DataBase_Manager Instance;

    public GameObject[] unitDataObjArr;
    public GameObject[] foodDataObjArr;

    public Character_Data[] charDataArr
    {
        get
        {
            return m_CharDataArr;
        }
    }
    [SerializeField]
    private Character_Data[] m_CharDataArr;

    public Food_Data[] foodDataArr
    {
        get
        {
            return m_FoodDataArr;
        }
    }
    [SerializeField]
    private Food_Data[] m_FoodDataArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        int _unitDataObjNum = unitDataObjArr.Length;
        m_CharDataArr = new Character_Data[_unitDataObjNum];
        for (int i = 0; i < _unitDataObjNum; i++)
        {
            Unit_Script _unitClass = unitDataObjArr[i].GetComponent<Unit_Script>();
            m_CharDataArr[i].SetData_Func(_unitClass);
        }

        int _foodDataObjNum = foodDataObjArr.Length;
        m_FoodDataArr = new Food_Data[_foodDataObjNum];
        for (int i = 0; i < _foodDataObjNum; i++)
        {
            Food_Script _foodClass = foodDataObjArr[i].GetComponent<Food_Script>();
            m_FoodDataArr[i].SetData_Func(_foodClass);
        }

        yield break;
    }

    public string GetUnitName_Func(int _unitID)
    {
        return m_CharDataArr[_unitID].charName;
    }
    public string GetFoodName_Func(int _foodID)
    {
        return m_FoodDataArr[_foodID].foodName;
    }
}
