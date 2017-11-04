using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerUnit_ClassData
{
    public bool isHave;
    public int level;
    public Unit_Script unitClass;

    private List<PlayerFood_ClassData> playerFoodDataList;

    public IEnumerator Init_Cor(bool _isUnlock, Unit_Script _unitClass)
    {
        isHave = _isUnlock;
        unitClass = _unitClass;
        playerFoodDataList = new List<PlayerFood_ClassData>();

        // 유닛의 음식 정보 불러오기
        for (int i = 0; i < playerFoodDataList.Count; i++)
        {
            
        }

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
}
