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
        if (SaveSystem_Manager.Instance.isContinuePlayer == true)
        {
            string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Unit_zzzUnitIDzzz_FoodHaveNum, _unitID);
            int _feedFoodNum = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType);
            
            for (int i = 0; i < _feedFoodNum; i++)
            {
                SaveSystem_Manager.SaveFoodDataStr _saveFoodDataStr =
                    SaveSystem_Manager.Instance.LoadDataUnitFood_Func(_unitID, i);

                PlayerFood_ClassData _playerFoodClass = new PlayerFood_ClassData(_saveFoodDataStr);

                playerFoodDataList.Add(_playerFoodClass);

                yield return null;
            }
        }
        else
        {
            int _feedFoodType = 2;
            int _feedFoodID = 0;
            int _feedFoodLevel = 999;
            float _feedFoodPosX = 0f;
            float _feedFoodPosY = 0f;
            float _feedFoodRotZ = 0f;

            string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Unit_zzzUnitIDzzz_FoodHaveNum, _unitID);
            SaveSystem_Manager.Instance.SaveData_Func(_saveType, 3);

            for (int i = 0; i < 3; i++)
            {
                PlayerFood_ClassData _playerFoodClass = new PlayerFood_ClassData((FoodType)_feedFoodType, _feedFoodID, _feedFoodLevel);

                if(i == 0)
                {
                    _feedFoodPosX = 1403.901f;
                    _feedFoodPosY = 282.4627f;
                    _feedFoodRotZ = 349.2703f;
                }
                else if(i == 1)
                {
                    _feedFoodPosX = 1565.891f;
                    _feedFoodPosY = 310.1227f;
                    _feedFoodRotZ = 85.3086f;
                }
                else if(i == 2)
                {
                    _feedFoodPosX = 1463.391f;
                    _feedFoodPosY = 432.4628f;
                    _feedFoodRotZ = 343.5417f;
                }

                _playerFoodClass.pos = new Vector2(_feedFoodPosX, _feedFoodPosY);
                _playerFoodClass.rot = Vector3.forward * _feedFoodRotZ;

                playerFoodDataList.Add(_playerFoodClass);

                SaveSystem_Manager.Instance.SaveData_UnitFood_Func(_unitID, i, _playerFoodClass);

                yield return null;
            }
        }

        yield break;
    }

    public void FeedFood_Func(Food_Script _foodClass)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData(_foodClass);
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
    public void RemoveStone_Func(Food_Script _stoneClass)
    {
        int _haveFoodID = Player_Data.Instance.GetHaveFoodID_Func(playerFoodDataList, _stoneClass);
        playerFoodDataList.Remove(playerFoodDataList[_haveFoodID]);

        ObjectPool_Manager.Instance.Free_Func(_stoneClass.gameObject);
    }
    public void SaveFeedData_Func()
    {
        int _playerFoodDataNum = GetPlayerFoodDataNum_Func();

        string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Unit_zzzUnitIDzzz_FoodHaveNum, unitID);
        SaveSystem_Manager.Instance.SaveData_Func(_saveType, _playerFoodDataNum); // Unit_zzzUnitIDzzz_FoodHaveNum

        for (int i = 0; i < _playerFoodDataNum; i++)
        {
            SaveSystem_Manager.Instance.SaveData_UnitFood_Func(unitID, i, playerFoodDataList[i]);
        }
    }
    public PlayerFood_ClassData[] GetPlayerFoodDataArr_Func()
    {
        return playerFoodDataList.ToArray();
    }
    public int GetPlayerFoodDataNum_Func()
    {
        return playerFoodDataList.Count;
    }

    public void SetLevel_Func(float _levelValue, bool _isInit = false)
    {
        SetLevel_InitUnitData_Func();
        SetLevel_Level_Func(_levelValue);
        SetLevel_Food_Func();
        SetLevel_Trophy_Func();
        
        if(_isInit == false)
            Lobby_Manager.Instance.partySettingClass.PrintInfoUI_Func();

        string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Unit_zzzUnitIDzzz_Level, unitID);
        SaveSystem_Manager.Instance.SaveData_Func(_saveType, _levelValue); // Unit_zzzUnitIDzzz_Level
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
