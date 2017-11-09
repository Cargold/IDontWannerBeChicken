using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase_Manager : MonoBehaviour
{
    public static DataBase_Manager Instance;

    // Hero Data
    public float heroAttackRate;

    // Unit Data
    public GameObject[] unitDataObjArr;
    public Unit_Data[] unitDataArr
    {
        get
        {
            return m_UnitDataArr;
        }
    }
    [SerializeField]
    private Unit_Data[] m_UnitDataArr;
    public Dictionary<int, Unit_Script> unitClassDic;
    public int[] unitUnlockConditionArr;

    // Monster Data
    public GameObject[] monsterDataObjArr;
    public Unit_Data[] monsterDataArr
    {
        get
        {
            return m_MonsterDataArr;
        }
    }
    [SerializeField]
    private Unit_Data[] m_MonsterDataArr;
    public Dictionary<int, Unit_Script> monsterClassDic;
    public float chickenHouseHp_Default;

    // Food Data
    public GameObject[] foodDataObjArr;
    public Food_Data[] foodDataArr
    {
        get
        {
            return m_FoodDataArr;
        }
    }
    [SerializeField]
    private Food_Data[] m_FoodDataArr;

    // Skill Data
    public GameObject[] skillDataObjArr;
    public Skill_Data[] skillDataArr
    {
        get
        {
            return m_SkillDataArr;
        }
    }
    [SerializeField]
    private Skill_Data[] m_SkillDataArr;
    
    // Trophy Data
    public Trophy_Data[] trophyDataArr
    {
        get
        {
            return m_TrophyDataArr;
        }
    }
    [SerializeField]
    private Trophy_Data[] m_TrophyDataArr;

    // Shell Data
    public GameObject[] shellObjArr;

    // Effect Data
    public GameObject[] effectObjArr;

    // Reference Data
    public Sprite populationPointSprite;
    public Sprite[] wealthSpriteArr;
    public Sprite[] populationSpriteArr;
    public Sprite[] manaCostSpriteArr;

    public Color textColor;

    public float[] foodGradePenaltyValue;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield return InitUnitData_Cor();
        yield return InitMonsterData_Cor();
        yield return InitFoodData_Cor();
        yield return InitSkillData_Cor();
        
        yield break;
    }
    IEnumerator InitUnitData_Cor()
    {
        int _unitDataObjNum = unitDataObjArr.Length;
        m_UnitDataArr = new Unit_Data[_unitDataObjNum];
        for (int i = 0; i < _unitDataObjNum; i++)
        {
            Unit_Script _unitClass = unitDataObjArr[i].GetComponent<Unit_Script>();
            m_UnitDataArr[i].SetData_Func(_unitClass, i);

            yield return null;
        }

        unitClassDic = new Dictionary<int, Unit_Script>();
    }
    IEnumerator InitMonsterData_Cor()
    {
        int _monsterDataObjNum = monsterDataObjArr.Length;
        m_MonsterDataArr = new Unit_Data[_monsterDataObjNum];
        for (int i = 0; i < _monsterDataObjNum; i++)
        {
            Unit_Script _monsterClass = monsterDataObjArr[i].GetComponent<Unit_Script>();
            m_MonsterDataArr[i].SetData_Func(_monsterClass, i);

            yield return null;
        }

        monsterClassDic = new Dictionary<int, Unit_Script>();

        yield break;
    }
    IEnumerator InitFoodData_Cor()
    {
        int _foodDataObjNum = foodDataObjArr.Length;
        m_FoodDataArr = new Food_Data[_foodDataObjNum];
        for (int i = 0; i < _foodDataObjNum; i++)
        {
            Food_Script _foodClass = foodDataObjArr[i].GetComponent<Food_Script>();
            _foodClass.foodId = i;
            m_FoodDataArr[i].SetData_Func(_foodClass);

            yield return null;
        }
    }
    IEnumerator InitSkillData_Cor()
    {
        int _skillDataObjNum = skillDataObjArr.Length;
        m_SkillDataArr = new Skill_Data[_skillDataObjNum];
        for (int i = 0; i < _skillDataObjNum; i++)
        {
            Skill_Parent _skillClass = skillDataObjArr[i].GetComponent<Skill_Parent>();
            m_SkillDataArr[i].SetData_Func(_skillClass);

            yield return null;
        }
    }

    // Unit
    public string GetUnitName_Func(int _unitID)
    {
        return m_UnitDataArr[_unitID].unitName;
    }
    public Unit_Script GetUnitClass_Func(int _unitID)
    {
        Unit_Script _unitClass = null;

        if (unitClassDic.TryGetValue(_unitID, out _unitClass) == false)
        {
            Debug.LogError("Bug : 유닛ID가 설정치를 벗어났슴다");
        }

        return _unitClass;
    }
    public int GetUnitMaxNum_Func()
    {
        return unitDataObjArr.Length;
    }

    // Food
    public string GetFoodName_Func(int _foodID)
    {
        return m_FoodDataArr[_foodID].foodName;
    }

    // Monster
    public string GetMonsterName_Func(int _unitID)
    {
        return m_MonsterDataArr[_unitID].unitName;
    }
    public Unit_Script GetMonsterClass_Func(int _unitID)
    {
        Unit_Script _unitClass = null;

        if (monsterClassDic.TryGetValue(_unitID, out _unitClass) == false)
        {
            Debug.LogError("Bug : 몬스터ID가 설정치를 벗어났슴다");
        }

        return _unitClass;
    }
    public int GetMonsterMaxNum_Func()
    {
        return monsterDataObjArr.Length;
    }
    public MonsterType GetMonsterType_Func(int _monsterID)
    {
        return monsterClassDic[_monsterID].monsterType;
    }
}
