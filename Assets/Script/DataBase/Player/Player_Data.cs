using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data Instance;

    // Wealth
    [SerializeField]
    private int goldValue;
    [SerializeField]
    private int mineralValue;
    [SerializeField]
    private PlayerWealth_Script playerWealthClass;

    // Party
    public int[] partyUnitIdArr;

    // Unit
    [SerializeField]
    public PlayerUnit_ClassData[] playerUnitDataArr;

    // Hero
    public int levelHero;
    public Player_Script playerClass;
    public List<PlayerFood_ClassData> heroFoodDataList;

    // Inventory
    public List<PlayerFood_ClassData> inventoryFoodDataList;

    // Trophy

    // Skill

    // Test
    public PlayerFood_DataTemp[] test_InventoryDataArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield return InitWealth_Cor();
        yield return InitCharacter_Cor();

        yield break;
    }
    IEnumerator InitWealth_Cor()
    {
        // 골드, 미네랄 데이터 불러오기

        playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
        playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);

        Test_InventoryFood_Func();

        yield break;
    }
    IEnumerator InitCharacter_Cor()
    {
        // 캐릭터 정보 불러오기

        int _unitDataNum = DataBase_Manager.Instance.unitDataObjArr.Length;
        playerUnitDataArr = new PlayerUnit_ClassData[_unitDataNum];

        for (int i = 0; i < playerUnitDataArr.Length; i++)
        {
            Unit_Script _unitClass = ObjectPoolManager.Instance.GetUnitClass_Func(i);

            playerUnitDataArr[i] = new PlayerUnit_ClassData();

            yield return playerUnitDataArr[i].Init_Cor(_unitClass);
        }

        yield break;
    }
    void Test_InventoryFood_Func()
    {
        inventoryFoodDataList = new List<PlayerFood_ClassData>();

        for (int i = 0; i < 15; i++)
        {
            PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

            _playerFoodData.level = Random.Range(1, 4);
            int _foodIDMax = DataBase_Manager.Instance.foodDataArr.Length;
            _playerFoodData.foodID = Random.Range(0, _foodIDMax);
            _playerFoodData.remainExp = Random.Range(0f, 99f);

            //_playerFoodData.level = 1;
            //_playerFoodData.foodID = 0;
            //_playerFoodData.remainExp = 0f;

            inventoryFoodDataList.Add(_playerFoodData);
        }
    }

    private void Update()
    {
        //Debug.Log("Test, Food Class : " + inventoryFoodDataList[0].foodClass);
    }

    #region Party Group
    public void JoinParty_Func(int _partySlotId, int _unitId)
    {
        partyUnitIdArr[_partySlotId] = _unitId;
    }
    public void DisbandParty_Func(int _partySlotId, int _unitId)
    {
        if(partyUnitIdArr[_partySlotId] == _unitId)
        {
            partyUnitIdArr[_partySlotId] = -1;
        }
        else
        {
            Debug.LogError("Bug : 파티해제하려는 유닛과 기존 파티 유닛이 서로 정보가 다릅니다.");
        }
    }
    #endregion
    #region Wealth Group
    public void AddWealth_Func(WealthType _wealthType, int _value)
    {
        if (_wealthType == WealthType.Gold)
        {
            goldValue += _value;
            playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
        }
        else if (_wealthType == WealthType.Mineral)
        {
            mineralValue += _value;
            playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);
        }
    }
    public bool PayWealth_Func(WealthType _wealthType, int _value, bool _isJustCheck = false)
    {
        if (_wealthType == WealthType.Gold)
        {
            // 지불

            if (_value <= goldValue)
            {
                if (_isJustCheck == false)
                {
                    goldValue -= _value;
                    playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
                }

                return true;
            }
            else
            {
                // 재화가 부족함

                return false;
            }
        }
        else if (_wealthType == WealthType.Mineral)
        {
            if (_value <= mineralValue)
            {
                if (_isJustCheck == false)
                {
                    mineralValue -= _value;
                    playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);
                }

                return true;
            }
            else
            {
                // 재화가 부족함

                return false;
            }
        }
        else
        {
            Debug.LogError("Bug : 재화 종류 선택이 잘못되었습니다.");
            return false;
        }
    }
    public void ActiveWealthUI_Func()
    {
        playerWealthClass.Active_Func();
    }
    public void DeactiveWealthUI_Func()
    {
        playerWealthClass.Deactive_Func();
    }
    #endregion
    #region Food Group
    public void AddFood_Func(Food_Script _foodClass)
    {
        // 보상 또는 구매를 통해 인벤토리로...
        // 유닛 위장에서 인벤토리로...

        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();
        _playerFoodData.SetData_Func(_foodClass);

        inventoryFoodDataList.Add(_playerFoodData);
    }
    public void RemoveFood_Func(Food_Script _foodClass, bool _isInventoryFood, int _haveFoodUnitID)
    {
        if(_isInventoryFood == true)
        {
            // 인벤토리의 음식이 삭제되는 경우

            int _inventoryFoodID = GetInventoryFoodID_Func(_foodClass);

            if (_inventoryFoodID == -1)
                Debug.LogError("Bug : 해당 음식을 인벤토리에 찾을 수 없습니다.");

            ObjectPoolManager.Instance.Free_Func(inventoryFoodDataList[_inventoryFoodID].GetFoodClass_Func().gameObject);
            inventoryFoodDataList.RemoveAt(_inventoryFoodID);
        }
        else
        {
            // 유닛이 가진 음식이 삭제되는 경우

            
        }
    }
    public void FeedFood_Func(int _unitID, Food_Script _foodClass)
    {
        if (_unitID == 999)
        {
            // Hero

            PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();
            _playerFoodData.SetData_Func(_foodClass);
            heroFoodDataList.Add(_playerFoodData);
        }
        else
        {
            // Unit

            int _inventoryFoodID = GetInventoryFoodID_Func(_foodClass);
            inventoryFoodDataList.RemoveAt(_inventoryFoodID);

            playerUnitDataArr[_unitID].FeedFood_Func(_foodClass);
        }
    }
    public void OutFood_Func(Food_Script _foodClass, int _unitID)
    {
        playerUnitDataArr[_unitID].OutFood_Func(_foodClass);

    }

    public void SetFoodData_Func(Food_Script _foodClass, bool _isInventoryFood, int _unitID = -1)
    {
        if(_isInventoryFood == true)
        {
            int _inventoryFoodID = GetInventoryFoodID_Func(_foodClass);
            inventoryFoodDataList[_inventoryFoodID].SetData_Func(_foodClass);
        }
        else if(_isInventoryFood == false)
        {
            playerUnitDataArr[_unitID].SetFoodData_Func(_foodClass);
        }
    }
    public void SetFoodClassInInventory_Func(Food_Script _foodClass, int _inventoryID)
    {
        inventoryFoodDataList[_inventoryID].SetData_Func(_foodClass);
    }
    public void SetFoodClassInUnit_Func(Food_Script _foodClass, int _unitID, int _haveFoodID)
    {
        playerUnitDataArr[_unitID].SetFoodData_Func(_foodClass, _haveFoodID);
    }

    public int GetInventoryFoodID_Func(Food_Script _foodClass)
    {
        int _inventoryFoodID = -1;

        for (int i = 0; i < inventoryFoodDataList.Count; i++)
        {
            if (_foodClass == inventoryFoodDataList[i].GetFoodClass_Func())
            {
                _inventoryFoodID = i;
                break;
            }
        }

        if (_inventoryFoodID == -1)
            Debug.LogError("Bug : 음식을 찾을 수 없습니다.");

        return _inventoryFoodID;
    }
    public PlayerFood_ClassData GetPlayerFoodData_Func(int _inventoryFoodID)
    {
        return inventoryFoodDataList[_inventoryFoodID];
    }
    public int GetInventoryFoodNum_Func()
    {
        return inventoryFoodDataList.Count;
    }

    public void SetUnitDataByFood_Func(int _unitID, Food_Script _foodClass, bool _isFeed)
    {
        SetUnitDataByFood_Func(playerUnitDataArr[_unitID].unitClass, _foodClass, _isFeed);
    }
    public void SetUnitDataByFood_Func(Unit_Script _unitClass, Food_Script _foodClass, bool _isFeed)
    {
        float _feedingCalc = 0f;
        if (_isFeed == true)
            _feedingCalc = 0.01f;
        else if (_isFeed == false)
            _feedingCalc = -0.01f;

        if(_foodClass.effectMain == FoodEffect_Main.AttackPower || _foodClass.effectMain == FoodEffect_Main.HealthPoint)
            SetUnitDataByFoodMainEffect_Func(_unitClass, _foodClass, _feedingCalc);

        if(FoodEffect_Sub.None < _foodClass.effectSub)
            SetUnitDataByFoodSubEffect_Func(_unitClass, _foodClass, _feedingCalc);

        Lobby_Manager.Instance.partySettingClass.PrintInfoUI_Func();
    }
    void SetUnitDataByFoodMainEffect_Func(Unit_Script _unitClass, Food_Script _foodClass, float _feedingCalc)
    {
        int _charID = _unitClass.charId;

        switch (_foodClass.effectMain)
        {
            case FoodEffect_Main.AttackPower:
                float _attackValue = DataBase_Manager.Instance.charDataArr[_charID].attackValue;
                _attackValue = _attackValue * _foodClass.GetMainEffectValue_Func() * _feedingCalc;
                _unitClass.attackValue += _attackValue;
                break;

            case FoodEffect_Main.HealthPoint:
                float _healthPoint = DataBase_Manager.Instance.charDataArr[_charID].healthPoint;
                _healthPoint = _healthPoint * _foodClass.GetMainEffectValue_Func() * _feedingCalc;
                _unitClass.healthPoint_Max += _healthPoint;
                break;
        }
    }
    void SetUnitDataByFoodSubEffect_Func(Unit_Script _unitClass, Food_Script _foodClass, float _feedingCalc)
    {
        int _charID = _unitClass.charId;

        switch (_foodClass.effectSub)
        {
            case FoodEffect_Sub.Critical:
                float _criticalPercent = DataBase_Manager.Instance.charDataArr[_charID].criticalPercent;
                _criticalPercent = _criticalPercent * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.criticalPercent += _criticalPercent;
                break;

            case FoodEffect_Sub.SpawnInterval:
                float _spawnInterval = DataBase_Manager.Instance.charDataArr[_charID].spawnInterval;
                _spawnInterval = _spawnInterval * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.spawnInterval += _spawnInterval;
                break;

            case FoodEffect_Sub.DecreaseHP:
                float _healthPoint = DataBase_Manager.Instance.charDataArr[_charID].healthPoint;
                _healthPoint = _healthPoint * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.healthPoint_Max -= _healthPoint;
                break;

            case FoodEffect_Sub.DefenceValue:
                float _defenceValue = DataBase_Manager.Instance.charDataArr[_charID].defenceValue;
                _defenceValue = _defenceValue * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.defenceValue += _defenceValue;
                break;

            case FoodEffect_Sub.DecreaseAttack:
                float _attackValue = DataBase_Manager.Instance.charDataArr[_charID].attackValue;
                _attackValue = _attackValue * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.attackValue -= _attackValue;
                break;
        }
    }
    #endregion
}