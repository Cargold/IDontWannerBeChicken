using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerUnit_ClassData
{
    public bool isHave;
    public int unitID;
    public int unitLevel;
    public float healthPoint_RelativeLevel;
    public float attackValue_RelativeLevel;
    [SerializeField]
    public Unit_Script unitClass;
    [SerializeField]
    private List<PlayerFood_ClassData> playerFoodDataList;

    public IEnumerator Init_Cor(bool _isUnlock, int _unitID, int _unitLevel, Unit_Script _unitClass)
    {
        isHave = _isUnlock;
        unitID = _unitID;
        unitLevel = _unitLevel;
        unitClass = _unitClass;
        playerFoodDataList = new List<PlayerFood_ClassData>();

        SetLevel_Func(_unitLevel, true);

        // 유닛의 음식 정보 불러오기
        for (int i = 0; i < 3; i++)
        {
            //PlayerFood_ClassData _playerFoodClass = new PlayerFood_ClassData();

            //Food_Script _foodClass = 

            //_playerFoodClass.SetData_Func()
        }

        yield break;
    }

    public void FeedFood_Func(Food_Script _foodClass)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();
        _playerFoodData.SetData_Func(_foodClass);
        playerFoodDataList.Add(_playerFoodData);

        Player_Data.Instance.SetCharDataByFood_Func(unitClass, _foodClass, true);
    }
    public void SetFoodData_Func(Food_Script _foodClass, int _haveFoodID = -1)
    {
        if(_haveFoodID == -1)
            _haveFoodID = Player_Data.Instance.GetHaveFoodID_Func(playerFoodDataList, _foodClass);

        playerFoodDataList[_haveFoodID].SetData_Func(_foodClass);
    }
    public void OutFood_Func(Food_Script _foodClass)
    {
        int _haveFoodID = Player_Data.Instance.GetHaveFoodID_Func(playerFoodDataList, _foodClass);
        playerFoodDataList.Remove(playerFoodDataList[_haveFoodID]);

        Player_Data.Instance.SetCharDataByFood_Func(unitClass, _foodClass, false);
    }
    public PlayerFood_ClassData[] GetPlayerFoodDataArr_Func()
    {
        return playerFoodDataList.ToArray();
    }
    public void SetLevel_Func(float _levelValue, bool _isInit = false)
    {
        SetLevel_InitUnitData_Func();
        SetLevel_Level_Func(_levelValue);
        SetLevel_Food_Func();
        SetLevel_Trophy_Func();
        
        if(_isInit == false)
            Lobby_Manager.Instance.partySettingClass.PrintInfoUI_Func();
    }
    void SetLevel_InitUnitData_Func()
    {
        Unit_Data _unitData = DataBase_Manager.Instance.unitDataArr[unitID];
        unitClass.SetData_Func(_unitData);
    }
    void SetLevel_Level_Func(float _levelValue)
    {
        unitLevel = (int)_levelValue;
        _levelValue -= 1f;

        if (unitClass.groupType == GroupType.Ally)
        {
            float _levelPerBonus = DataBase_Manager.Instance.allyUnit_LevelPerBonus;
            _levelPerBonus *= 0.01f;

            float _healthPoint = DataBase_Manager.Instance.unitDataArr[unitID].healthPoint;
            healthPoint_RelativeLevel = ((_levelValue * _levelPerBonus) + 1f) * _healthPoint;
            unitClass.healthPoint_Max = healthPoint_RelativeLevel;
        
            float _attackValue = DataBase_Manager.Instance.unitDataArr[unitID].attackValue;
            attackValue_RelativeLevel = ((_levelValue * _levelPerBonus) + 1f) * _attackValue;
            unitClass.attackValue = attackValue_RelativeLevel;
        }
        else if(unitClass.groupType == GroupType.Enemy)
        {
            float _levelPerBonus = DataBase_Manager.Instance.enemyMonster_LevelPerBonus;
            _levelPerBonus *= 0.01f;

            float _healthPoint = DataBase_Manager.Instance.unitDataArr[unitID].healthPoint;
            healthPoint_RelativeLevel = ((_levelValue * _levelPerBonus) + 1f) * _healthPoint;
            unitClass.healthPoint_Max = healthPoint_RelativeLevel;

            float _attackValue = DataBase_Manager.Instance.unitDataArr[unitID].attackValue;
            attackValue_RelativeLevel = ((_levelValue * _levelPerBonus) + 1f) * _attackValue;
            unitClass.attackValue = attackValue_RelativeLevel;
        }
    }
    void SetLevel_Food_Func()
    {
        for (int i = 0; i < playerFoodDataList.Count; i++)
        {
            Food_Script _foodClass = playerFoodDataList[i].GetFoodClass_Func();
            Player_Data.Instance.SetCharDataByFood_Func(unitClass, _foodClass, true, false);
        }
    }
    void SetLevel_Trophy_Func()
    {
        float _hpTrophyEffectValue = Player_Data.Instance.GetCalcTrophyEffect_Func(TrophyType.HealthPoint_Unit, true);
        float _dmgTrophyEffectValue = Player_Data.Instance.GetCalcTrophyEffect_Func(TrophyType.AttackValue_Unit, true);

        _hpTrophyEffectValue *= healthPoint_RelativeLevel * 0.01f;
        _dmgTrophyEffectValue *= attackValue_RelativeLevel * 0.01f;
        
        unitClass.healthPoint_Max += _hpTrophyEffectValue;
        unitClass.attackValue += _dmgTrophyEffectValue;
    }
}
