using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase_Manager : MonoBehaviour
{
    public static DataBase_Manager Instance;

    [Header("Hero Data")]
    public GameObject heroObj;
    public Hero_Data heroData;
    public float heroAttackRate;
    public float manaRegen;

    [Header("Unit Data")]
    public GameObject[] unitDataObjArr;
    public Unit_Data[] unitDataArr
    {
        get
        {
            return m_UnitDataArr;
        }
    }
    private Unit_Data[] m_UnitDataArr;
    private Dictionary<int, Unit_Script> unitClassDic;
    public float hero_LevelPerBonus;
    public float allyUnit_LevelPerBonus;
    public float enemyMonster_LevelPerBonus;

    [Header("Monster Data")]
    public GameObject[] monsterDataObjArr;
    public Unit_Data[] monsterDataArr
    {
        get
        {
            return m_MonsterDataArr;
        }
    }
    private Unit_Data[] m_MonsterDataArr;
    public Dictionary<int, Unit_Script> monsterClassDic;
    public float chickenHouseHp_Default;

    [Header("Food Data")]
    public GameObject[] foodDataObjArr;
    public Food_Data[] foodDataArr
    {
        get
        {
            return m_FoodDataArr;
        }
    }
    private Food_Data[] m_FoodDataArr;

    [Header("Stone Data")]
    public GameObject stoneDataObj;
    public Food_Data stoneData
    {
        get
        {
            return m_StoneData;
        }
    }
    private Food_Data m_StoneData;

    [Header("Source Data")]
    public GameObject[] sourceDataObjArr;
    public Food_Data[] sourceDataArr
    {
        get
        {
            return m_SourceDataArr;
        }
    }
    private Food_Data[] m_SourceDataArr;

    [Header("Skill Data")]
    public GameObject[] skillDataObjArr;
    public Skill_Data[] skillDataArr
    {
        get
        {
            return m_SkillDataArr;
        }
    }
    private Skill_Data[] m_SkillDataArr;

    [Header("Trophy Data")]
    public GameObject[] trophyObjArr;
    public Trophy_Data[] trophyDataArr
    {
        get
        {
            return m_TrophyDataArr;
        }
    }
    private Trophy_Data[] m_TrophyDataArr;

    [Header("Shell Data")]
    public GameObject[] shellObjArr;

    [Header("Effect Data")]
    public GameObject[] effectObjArr;

    [Header("Drink Data")]
    public GameObject[] drinkObjArr;
    public Drink_Data[] drinkDataArr
    {
        get
        {
            return m_DrinkDataArr;
        }
    }
    private Drink_Data[] m_DrinkDataArr;

    [Header("Reference Data")]
    public Sprite populationPointSprite;
    public Sprite[] wealthSpriteArr;
    public Sprite[] manaCostSpriteArr;
    public Sprite[] rewardCardSpriteArr;
    public Color textColor;
    public float[] foodGradePenaltyValue;

    // Init
    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield return InitHeroData_Cor();
        yield return InitUnitData_Cor();
        yield return InitMonsterData_Cor();
        yield return InitFoodData_Cor();
        yield return InitTrophyData_Cor();
        yield return InitSkillData_Cor();
        yield return InitDrinkData_Cor();

        yield break;
    }
    IEnumerator InitHeroData_Cor()
    {
        Player_Script _playerClass = heroObj.GetComponent<Player_Script>();
        heroData.SetData_Func(_playerClass);

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
        }

        unitClassDic = new Dictionary<int, Unit_Script>();

        yield break;
    }
    IEnumerator InitMonsterData_Cor()
    {
        int _monsterDataObjNum = monsterDataObjArr.Length;
        m_MonsterDataArr = new Unit_Data[_monsterDataObjNum];
        for (int i = 0; i < _monsterDataObjNum; i++)
        {
            Unit_Script _monsterClass = monsterDataObjArr[i].GetComponent<Unit_Script>();
            m_MonsterDataArr[i].SetData_Func(_monsterClass, i);
        }

        monsterClassDic = new Dictionary<int, Unit_Script>();

        yield break;
    }
    IEnumerator InitFoodData_Cor()
    {
        // Food
        int _foodDataObjNum = foodDataObjArr.Length;
        m_FoodDataArr = new Food_Data[_foodDataObjNum];
        for (int i = 0; i < _foodDataObjNum; i++)
        {
            Food_Script _foodClass = foodDataObjArr[i].GetComponent<Food_Script>();
            _foodClass.foodId = i;
            m_FoodDataArr[i].SetData_Func(_foodClass);
        }

        // Stone
        Food_Script _stoneClass = stoneDataObj.GetComponent<Food_Script>();
        m_StoneData.SetData_Func(_stoneClass);

        // Source
        int _sourceDataObjNum = sourceDataObjArr.Length;
        m_SourceDataArr = new Food_Data[_sourceDataObjNum];
        for (int i = 0; i < _sourceDataObjNum; i++)
        {
            Food_Script _sourceClass = sourceDataObjArr[i].GetComponent<Food_Script>();
            _sourceClass.foodId = i;
            m_SourceDataArr[i].SetData_Func(_sourceClass);
        }

        yield break;
    }
    IEnumerator InitTrophyData_Cor()
    {
        int _trophyDataObjNum = trophyObjArr.Length;
        m_TrophyDataArr = new Trophy_Data[_trophyDataObjNum];
        for (int i = 0; i < _trophyDataObjNum; i++)
        {
            Trophy_Script _trophyClass = trophyObjArr[i].GetComponent<Trophy_Script>();
            _trophyClass.trophyID = i;
            m_TrophyDataArr[i].SetData_Func(_trophyClass);
        }

        yield break;
    }
    IEnumerator InitSkillData_Cor()
    {
        int _skillDataObjNum = skillDataObjArr.Length;
        m_SkillDataArr = new Skill_Data[_skillDataObjNum];
        for (int i = 0; i < _skillDataObjNum; i++)
        {
            Skill_Parent _skillClass = skillDataObjArr[i].GetComponent<Skill_Parent>();
            m_SkillDataArr[i].SetData_Func(_skillClass);
        }

        yield break;
    }
    IEnumerator InitDrinkData_Cor()
    {
        int _drinkDataNum = drinkObjArr.Length;
        m_DrinkDataArr = new Drink_Data[_drinkDataNum];
        for (int i = 0; i < _drinkDataNum; i++)
        {
            Drink_Script _drinkClass = drinkObjArr[i].GetComponent<Drink_Script>();
            m_DrinkDataArr[i].SetData_Func(_drinkClass);
        }

        yield break;
    }

    // Unit
    public void SetUnitClass_Func(int _unitID, Unit_Script _unitClass)
    {
        unitClassDic.Add(_unitID, _unitClass);
    }
    public string GetUnitName_Func(int _unitID)
    {
        return m_UnitDataArr[_unitID].unitName;
    }
    public Unit_Script GetUnitClass_Func(int _charID)
    {
        Unit_Script _unitClass = null;

        if (unitClassDic.TryGetValue(_charID, out _unitClass) == false)
        {
            Debug.LogError("Bug : 유닛ID가 설정치를 벗어났슴다");
        }

        return _unitClass;
    }
    public int GetUnitMaxNum_Func()
    {
        return unitDataObjArr.Length;
    }
    public int GetCharLevelUpCost_Func(int _recentLevel)
    {
        int _initCost = 1000;
        int _cost = _initCost;

        for (int i = 1; i < _recentLevel; i++)
        {
            _cost += (_initCost * i);
        }

        return _cost;
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
