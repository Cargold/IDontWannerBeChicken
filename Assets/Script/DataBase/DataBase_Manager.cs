using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase_Manager : MonoBehaviour
{
    public static DataBase_Manager Instance;

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

        yield break;
    }
}
