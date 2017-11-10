﻿using System.Collections;
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
    private Unit_Script unitClass;

    private List<PlayerFood_ClassData> playerFoodDataList;

    public IEnumerator Init_Cor(bool _isUnlock, int _unitID, int _unitLevel, Unit_Script _unitClass)
    {
        isHave = _isUnlock;
        unitID = _unitID;
        unitLevel = _unitLevel;
        unitClass = _unitClass;
        playerFoodDataList = new List<PlayerFood_ClassData>();

        // 유닛의 음식 정보 불러오기
        for (int i = 0; i < playerFoodDataList.Count; i++)
        {
            
        }

        SetLevel_Func(_unitLevel, true);

        yield break;
    }

    public void FeedFood_Func(Food_Script _foodClass)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();
        _playerFoodData.SetData_Func(_foodClass);
        playerFoodDataList.Add(_playerFoodData);

        Player_Data.Instance.SetUnitDataByFood_Func(unitClass, _foodClass, true);
    }
    public void SetFoodData_Func(Food_Script _foodClass, int _haveFoodID = -1)
    {
        if(_haveFoodID == -1)
            _haveFoodID = GetHaveFoodID_Func(_foodClass);

        playerFoodDataList[_haveFoodID].SetData_Func(_foodClass);
    }
    public void OutFood_Func(Food_Script _foodClass)
    {
        int _haveFoodID = GetHaveFoodID_Func(_foodClass);
        playerFoodDataList.Remove(playerFoodDataList[_haveFoodID]);

        Player_Data.Instance.SetUnitDataByFood_Func(unitClass, _foodClass, false);
    }
    public int GetHaveFoodID_Func(Food_Script _foodClass)
    {
        int _inventoryFoodID = -1;

        for (int i = 0; i < playerFoodDataList.Count; i++)
        {
            if (_foodClass == playerFoodDataList[i].GetFoodClass_Func())
            {
                _inventoryFoodID = i;
                break;
            }
        }

        if (_inventoryFoodID == -1)
            Debug.LogError("Bug : 음식을 찾을 수 없습니다.");

        return _inventoryFoodID;
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

        // 유닛 레벨업은 파티창에서만 가능하니까?
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

        float _healthPoint = DataBase_Manager.Instance.unitDataArr[unitID].healthPoint;
        healthPoint_RelativeLevel = ((_levelValue * 0.05f) + 1f) * _healthPoint;
        unitClass.healthPoint_Max = healthPoint_RelativeLevel;
        
        float _attackValue = DataBase_Manager.Instance.unitDataArr[unitID].attackValue;
        attackValue_RelativeLevel = ((_levelValue * 0.05f) + 1f) * _attackValue;
        unitClass.attackValue = attackValue_RelativeLevel;
    }
    void SetLevel_Food_Func()
    {
        for (int i = 0; i < playerFoodDataList.Count; i++)
        {
            Food_Script _foodClass = playerFoodDataList[i].foodClass;
            Player_Data.Instance.SetUnitDataByFood_Func(unitClass, _foodClass, true, false);
        }
    }
    void SetLevel_Trophy_Func()
    {

    }
}
