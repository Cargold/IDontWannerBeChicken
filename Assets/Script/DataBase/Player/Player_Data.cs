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
    public int populationPoint;

    // Unit
    [SerializeField]
    public PlayerUnit_ClassData[] playerUnitDataArr;

    // Hero
    public int heroLevel;
    public Player_Script playerClass;
    public List<PlayerFood_ClassData> heroFoodDataList;

    // Inventory
    public List<PlayerFood_ClassData> inventoryFoodDataList;
    public int foodBoxLevel;

    // Trophy
    public PlayerTrophy_Data[] trophyDataArr;

    // Skill
    public PlayerSkill_Data[] skillDataArr;
    public int[] selectSkillIDArr;
    public int[] test_SkillLevel;

    // Stage
    public int stageID_Normal;
    public int stageID_Special;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        Debug.Log("Cargold : 데이터 불러오기, 미작업");
        yield return LoadWealth_Cor();
        yield return LoadParty_Cor();
        yield return LoadUnit_Cor();
        yield return LoadHero_Cor();
        yield return LoadInventory_Cor();
        yield return LoadTrophy_Cor();
        yield return LoadSkill_Cor();
        yield return LoadStage_Cor();

        yield break;
    }
    IEnumerator LoadWealth_Cor()
    {
        // Cargold : 골드, 미네랄 데이터 불러오기

        playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
        playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);

        yield break;
    }
    IEnumerator LoadParty_Cor()
    {
        //for (int i = 0; i < 5; i++)
        //{
        //    partyUnitIdArr[i] = 0;
        //}

        yield break;
    }
    IEnumerator LoadUnit_Cor()
    {
        for (int i = 0; i < playerUnitDataArr.Length; i++)
        {
            // Cargold : 캐릭터 정보 불러오기
            bool _isUnlock = true;
            _isUnlock = playerUnitDataArr[i].isHave; // Test
            Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(i);

            playerUnitDataArr[i] = new PlayerUnit_ClassData();

            yield return playerUnitDataArr[i].Init_Cor(_isUnlock, _unitClass);
        }

        yield break;
    }
    IEnumerator LoadHero_Cor()
    {
        //heroLevel = 0;


        // 영웅의 음식 정보 불러오기
        //heroFoodDataList = new List<PlayerFood_ClassData>();
        //for (int i = 0; i < heroFoodDataList.Count; i++)
        //{

        //}

        DataBase_Manager.Instance.heroAttackRate = playerClass.GetAttackSpeedMax_Func();

        yield break;
    }
    IEnumerator LoadInventory_Cor()
    {
        inventoryFoodDataList = new List<PlayerFood_ClassData>();
        for (int i = 0; i < 1; i++)
        {
            PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

            //_playerFoodData.level = Random.Range(1, 4);
            //int _foodIDMax = DataBase_Manager.Instance.foodDataArr.Length;
            //_playerFoodData.foodID = Random.Range(0, _foodIDMax);
            //_playerFoodData.remainExp = Random.Range(0f, 99f);

            _playerFoodData.level = 1;
            _playerFoodData.foodID = 1;
            _playerFoodData.remainExp = 0f;

            inventoryFoodDataList.Add(_playerFoodData);
        }

        yield break;
    } // UnComplete
    IEnumerator LoadTrophy_Cor()
    {
        yield break;
    } // UnComplete
    IEnumerator LoadSkill_Cor()
    {
        // Test
        skillDataArr = new PlayerSkill_Data[10];
        for (int i = 0; i < 10; i++)
        {
            skillDataArr[i].Init_Func(i);
        }

        for (int i = 0; i < 10; i++)
        {
            if(0 < test_SkillLevel[i])
            {
                UnlockSkill_Func(i);
                LevelUpSkill_Func(i, test_SkillLevel[i]);
            }
        }

        yield break;
    } // UnComplete
    IEnumerator LoadStage_Cor()
    {
        // Cargold : 스테이지 데이터 불러오기
        
        yield break;
    } // UnComplete
    
    #region Party Group
    public void UnlockUnit_Func(int _unitID)
    {
        playerUnitDataArr[_unitID].isHave = true;
        Lobby_Manager.Instance.partySettingClass.UnlockCard_Func(_unitID);
        //Debug.Log("Cargold : 유닛 해금 보상은 미구현됨, Unit ID : " + _unitID);
    }
    public void JoinParty_Func(int _partySlotId, int _unitId)
    {
        partyUnitIdArr[_partySlotId] = _unitId;
    }
    public void DisbandParty_Func(int _partySlotId, int _unitId)
    {
        if(partyUnitIdArr[_partySlotId] < 0)
        {

        }
        else if(partyUnitIdArr[_partySlotId] == _unitId)
        {
            partyUnitIdArr[_partySlotId] = -1;
        }
        else
        {
            Debug.LogError("Bug : 파티해제하려는 유닛과 기존 파티 유닛이 서로 정보가 다릅니다.");
        }
    }
    public void AddPopulationPoint_Func()
    {
        populationPoint++;
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
    public void AddFood_Func(int _foodID, int _foodLevel = -1)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

        if (_foodLevel == -1)
            _foodLevel = foodBoxLevel;
        _playerFoodData.level = _foodLevel;
        _playerFoodData.foodID = _foodID;
        _playerFoodData.remainExp = 0f;

        inventoryFoodDataList.Add(_playerFoodData);
    }
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

            ObjectPool_Manager.Instance.Free_Func(inventoryFoodDataList[_inventoryFoodID].GetFoodClass_Func().gameObject);
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
        int _charID = _unitClass.unitID;

        switch (_foodClass.effectMain)
        {
            case FoodEffect_Main.AttackPower:
                float _attackValue = DataBase_Manager.Instance.unitDataArr[_charID].attackValue;
                _attackValue = _attackValue * _foodClass.GetMainEffectValue_Func() * _feedingCalc;
                _unitClass.attackValue += _attackValue;
                break;

            case FoodEffect_Main.HealthPoint:
                float _healthPoint = DataBase_Manager.Instance.unitDataArr[_charID].healthPoint;
                _healthPoint = _healthPoint * _foodClass.GetMainEffectValue_Func() * _feedingCalc;
                _unitClass.healthPoint_Max += _healthPoint;
                break;
        }
    }
    void SetUnitDataByFoodSubEffect_Func(Unit_Script _unitClass, Food_Script _foodClass, float _feedingCalc)
    {
        int _charID = _unitClass.unitID;
        float _value = 0f;

        switch (_foodClass.effectSub)
        {
            case FoodEffect_Sub.Critical:
                _value = _foodClass.GetSubEffectValue_Func() * _feedingCalc * 100f;
                _unitClass.criticalPercent += _value;
                break;

            case FoodEffect_Sub.SpawnInterval:
                _value = DataBase_Manager.Instance.unitDataArr[_charID].spawnInterval;
                if (_value == 0f) _value = 1f;
                _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.spawnInterval -= _value;
                break;

            case FoodEffect_Sub.DecreaseHP:
                _value = DataBase_Manager.Instance.unitDataArr[_charID].healthPoint;
                if (_value == 0f) _value = 1f;
                _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.healthPoint_Max -= _value;
                break;

            case FoodEffect_Sub.DefenceValue:
                _value = _foodClass.GetSubEffectValue_Func() * _feedingCalc * 100f;
                _unitClass.defenceValue += _value;
                break;

            case FoodEffect_Sub.DecreaseAttack:
                _value = DataBase_Manager.Instance.unitDataArr[_charID].attackValue;
                if (_value == 0f) _value = 1f;
                _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.attackValue -= _value;
                break;
        }
    }

    public void AddFoodBoxLevel_Func()
    {
        foodBoxLevel++;
    }
    #endregion
    #region Skill Group
    public void UnlockSkill_Func(int _skillID)
    {
        skillDataArr[_skillID].UnlockSkill_Func(_skillID);
    }
    public void LevelUpSkill_Func(int _skillID, int _fixedLevel = -1)
    {
        skillDataArr[_skillID].LevelUpSkill_Func(_fixedLevel);
    }
    public void SelectSkill_Func(int _grade, bool _isUpSkill)
    {
        selectSkillIDArr[_grade] = _grade * 2;

        if (_isUpSkill == false)
        {
            selectSkillIDArr[_grade]++;
        }
    }
    #endregion
    #region Trophy Group
    public void AddTrophy_Func(int _trophyID)
    {
        trophyDataArr[_trophyID].AddTrophy_Func();
    }
    #endregion
}