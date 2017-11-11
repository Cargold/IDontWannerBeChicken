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

    // Trophy
    public PlayerTrophy_Data[] trophyDataArr;

    // Party
    public int[] partyUnitIdArr;
    public int populationPoint;

    // Unit
    [SerializeField]
    public PlayerUnit_ClassData[] playerUnitDataArr;

    // Hero
    public int heroLevel;
    public Player_Script playerHeroData;
    public float heroHealthPoint_RelativeLevel;
    public float heroAttackValue_RelativeLevel;
    public List<PlayerFood_ClassData> heroFoodDataList;

    // Inventory
    public List<PlayerFood_ClassData> inventoryFoodDataList;
    public int foodBoxLevel;

    // Skill
    public PlayerSkill_Data[] skillDataArr;
    public int[] selectSkillIDArr;
    public int[] test_SkillLevel;

    // Stage
    public int stageID_Normal;
    public int stageID_Special;

    [Header("Drink")]
    public PlayerDrink_Data[] drinkDataArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        Debug.Log("Cargold : 데이터 불러오기, 미작업");
        yield return LoadWealth_Cor();
        yield return LoadTrophy_Cor();
        yield return LoadParty_Cor();
        yield return LoadUnit_Cor();
        yield return LoadHero_Cor();
        yield return LoadInventory_Cor();
        yield return LoadSkill_Cor();
        yield return LoadStage_Cor();
        yield return LoadDrink_Cor();

        yield break;
    }
    IEnumerator LoadWealth_Cor()
    {
        // Cargold : 골드, 미네랄 데이터 불러오기

        playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
        playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);

        yield break;
    }
    IEnumerator LoadTrophy_Cor()
    {
        for (int i = 0; i < trophyDataArr.Length; i++)
        {
            trophyDataArr[i].trophyID = i;
            trophyDataArr[i].trophyType = (TrophyType)i;
            //trophyDataArr[i].haveNum = 0;
        }

        yield break;
    } // UnComplete
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
            bool _isUnlock = playerUnitDataArr[i].isHave; // Test

            int _unitLevel = 1; // Test

            Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(i);

            playerUnitDataArr[i] = new PlayerUnit_ClassData();
            yield return playerUnitDataArr[i].Init_Cor(_isUnlock, i, _unitLevel, _unitClass);
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

        DataBase_Manager.Instance.heroAttackRate = playerHeroData.GetAttackSpeedMax_Func();

        yield break;
    }
    IEnumerator LoadInventory_Cor()
    {
        inventoryFoodDataList = new List<PlayerFood_ClassData>();
        for (int i = 0; i < 10; i++)
        {
            PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

            int _foodIDMax = DataBase_Manager.Instance.foodDataArr.Length;
            _playerFoodData.foodID = Random.Range(0, _foodIDMax);
            _playerFoodData.level = Random.Range(1, 4);
            _playerFoodData.remainExp = Random.Range(0f, 99f);

            //_playerFoodData.foodID = 1;

            inventoryFoodDataList.Add(_playerFoodData);
        }

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
    IEnumerator LoadDrink_Cor()
    {
        yield break;
    }
    
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
        else if(_wealthType == WealthType.Real)
        {
            return true;
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
    #region Trophy Group
    public float GetCalcTrophyEffect_Func(TrophyType _trophyType, bool _isAll = false)
    {
        int _trophyID = (int)_trophyType;

        return GetCalcTrophyEffect_Func(_trophyID, _isAll);
    }
    public float GetCalcTrophyEffect_Func(int _trophyID, bool _isAll = false)
    {
        int _haveNum = trophyDataArr[_trophyID].haveNum;
        float _effectValue = DataBase_Manager.Instance.trophyDataArr[_trophyID].effectValue;

        if (_isAll == true)
        {
            float _calcValue = _effectValue * _haveNum;
            return _calcValue;
        }
        else
        {

            return _effectValue;
        }
    }
    public bool AddTrophy_Func(TrophyType _trophyType, bool _isAdd = false)
    {
        int _trophyID = -1;
        if (_trophyType == TrophyType.Random)
            _trophyID = Random.Range(0, trophyDataArr.Length);
        else 
            _trophyID = (int)_trophyType;

        return AddTrophy_Func(_trophyID, _isAdd);
    }
    public bool AddTrophy_Func(int _trophyID, bool _isAdd = false)
    {
        int _haveNum = trophyDataArr[_trophyID].haveNum;
        bool _isReturnValue = false;

        if (DataBase_Manager.Instance.trophyDataArr[_trophyID].amountLimit == 0)
        {
            _isReturnValue = true;
        }
        else if (_haveNum + 1 <= DataBase_Manager.Instance.trophyDataArr[_trophyID].amountLimit)
        {
            _isReturnValue = true;
        }
        else
        {
            _isReturnValue = false;
        }

        if(_isAdd == true)
        {
            if(_isReturnValue == true)
            {
                trophyDataArr[_trophyID].haveNum++;
                SetTrophyEffect_Func(_trophyID);
            }
        }

        return _isReturnValue;
    }
    public int GetTrophyNum_Func(TrophyType _trophyType)
    {
        int _trophyID = (int)_trophyType;

        return GetTrophyNum_Func(_trophyID);
    }
    public int GetTrophyNum_Func(int _trophyID)
    {
        return trophyDataArr[_trophyID].haveNum;
    }
    public void SetTrophyEffect_Func(TrophyType _trophyType, int _calcNum = 1)
    {
        int _trophyID = (int)_trophyType;

        SetTrophyEffect_Func(_trophyID, _calcNum);
    }
    public void SetTrophyEffect_Func(int _trophyID, int _calcNum = 1)
    {
        TrophyType _trophyType = (TrophyType)_trophyID;
        float _effectValue = DataBase_Manager.Instance.trophyDataArr[_trophyID].effectValue * _calcNum;
        float _calcValue = 0f;
        int _unitNum = 0;

        switch (_trophyType)
        {
            case TrophyType.HealthPoint_Hero:
                _calcValue = heroHealthPoint_RelativeLevel * _effectValue;
                playerHeroData.healthPoint_Max += _calcValue;
                break;
            case TrophyType.AttackValue_Hero:
                _calcValue = heroAttackValue_RelativeLevel * _effectValue;
                playerHeroData.healthPoint_Max += _calcValue;
                break;
            case TrophyType.CriticalPercent_Hero:
                playerHeroData.criticalPercent += _effectValue;
                break;
            case TrophyType.CriticalBonus_Hero:
                playerHeroData.criticalBonus += _effectValue;
                break;
            case TrophyType.ManaRegen:
                playerHeroData.manaRegen += _effectValue;
                break;
            case TrophyType.ManaStart:
                playerHeroData.manaStart += _effectValue;
                break;
            case TrophyType.HealthPoint_Unit:
                _unitNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                for (int i = 0; i < _unitNum; i++)
                {
                    float _hp = playerUnitDataArr[i].healthPoint_RelativeLevel;
                    _calcValue = _hp * _effectValue;

                    playerUnitDataArr[i].unitClass.healthPoint_Max += _calcValue;
                }
                break;
            case TrophyType.AttackValue_Unit:
                _unitNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                for (int i = 0; i < _unitNum; i++)
                {
                    float _dmg = Player_Data.Instance.playerUnitDataArr[i].attackValue_RelativeLevel;
                    _calcValue = _dmg * _effectValue;

                    Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(i);
                    _unitClass.attackValue += _calcValue;
                }
                break;
            case TrophyType.CriticalPercent_Unit:
                _unitNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                for (int i = 0; i < _unitNum; i++)
                {
                    Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(i);
                    _unitClass.criticalPercent += _effectValue;
                }
                break;
            case TrophyType.CriticalBonus_Unit:
                _unitNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                for (int i = 0; i < _unitNum; i++)
                {
                    Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(i);
                    _unitClass.criticalBonus += _effectValue;
                }
                break;
            case TrophyType.HealthPoint_Monster:
                break;
            case TrophyType.AttackValue_Monster:
                break;
            case TrophyType.HealthPoint_House:
                break;
            case TrophyType.GoldBonus:
                break;
            case TrophyType.ItemDropPer:
                break;
            case TrophyType.UpgradeExp:
                break;
        }
    }
    #endregion
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
    #region Unit Group
    #endregion
    #region Food Group
    public void AddFood_Func(int _foodID, int _foodLevel = -1)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

        if (_foodLevel == -1)
            _foodLevel = foodBoxLevel;
        _playerFoodData.foodID = _foodID;

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
        Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(_unitID);
        SetUnitDataByFood_Func(_unitClass, _foodClass, _isFeed);
    }
    public void SetUnitDataByFood_Func(Unit_Script _unitClass, Food_Script _foodClass, bool _isFeed, bool _isPrintUI = true)
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

        if(_isPrintUI == true)
            Lobby_Manager.Instance.partySettingClass.PrintInfoUI_Func();
    }
    void SetUnitDataByFoodMainEffect_Func(Unit_Script _unitClass, Food_Script _foodClass, float _feedingCalc)
    {
        int _charID = _unitClass.unitID;

        switch (_foodClass.effectMain)
        {
            case FoodEffect_Main.AttackPower:
                float _attackValue = playerUnitDataArr[_charID].attackValue_RelativeLevel;
                _attackValue = _attackValue * _foodClass.GetMainEffectValue_Func() * _feedingCalc;
                _unitClass.attackValue += _attackValue;
                break;

            case FoodEffect_Main.HealthPoint:
                float _healthPoint = playerUnitDataArr[_charID].healthPoint_RelativeLevel;
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
                _value = _unitClass.spawnInterval;
                if (_value == 0f) _value = 1f;
                _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.spawnInterval -= _value;
                break;

            case FoodEffect_Sub.DecreaseHP:
                _value = _unitClass.healthPoint_Max;
                if (_value == 0f) _value = 1f;
                _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _unitClass.healthPoint_Max -= _value;
                break;

            case FoodEffect_Sub.DefenceValue:
                _value = _foodClass.GetSubEffectValue_Func() * _feedingCalc * 100f;
                _unitClass.defenceValue += _value;
                break;

            case FoodEffect_Sub.DecreaseAttack:
                _value = _unitClass.attackValue;
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
    #region Drink Group
    public void AddDrink_Func(DrinkType _drinkType, int _addNum = 1)
    {
        int _drinkID = (int)_drinkType;

        AddDrink_Func(_drinkID, _addNum);
    }
    public void AddDrink_Func(int _drinkID, int _addNum = 1)
    {
        drinkDataArr[_drinkID].haveNum += _addNum;
    }
    public bool SetDrinkUse_Func(DrinkType _drinkType, bool _isOn)
    {
        int _drinkID = (int)_drinkType;

        return SetDrinkUse_Func(_drinkID, _isOn);
    }
    public bool SetDrinkUse_Func(int _drinkID, bool _isOn)
    {
        if(_isOn == true)
        {
            if (0 < drinkDataArr[_drinkID].haveNum)
            {
                drinkDataArr[_drinkID].isUse = true;
                return true;
            }
            else
            {
                drinkDataArr[_drinkID].isUse = false;
                return false;
            }
        }
        else
        {
            drinkDataArr[_drinkID].isUse = false;
            return true;
        }
    }
    public void UseDrink_Func(DrinkType _drinkType)
    {
        int _drinkID = (int)_drinkType;

        UseDrink_Func(_drinkID);
    }
    public void UseDrink_Func(int _drinkID)
    {
        if (drinkDataArr[_drinkID].haveNum <= 0)
            Debug.LogError("Bug : 다음 드링크의 재고가 없습니다." + (DrinkType)_drinkID);
        else
        {
            drinkDataArr[_drinkID].haveNum--;

            if (drinkDataArr[_drinkID].haveNum <= 0)
            {
                drinkDataArr[_drinkID].isUse = false;
            }
        }
    }
    public bool CheckDrinkUse_Func(DrinkType _drinkType)
    {
        int _drinkID = (int)_drinkType;

        return CheckDrinkUse_Func(_drinkID);
    }
    public bool CheckDrinkUse_Func(int _drinkID)
    {
        return drinkDataArr[_drinkID].isUse;
    }
    public int GetDrinkNum_Func(DrinkType _drinkType)
    {
        int _drinkID = (int)_drinkType;

        return GetDrinkNum_Func(_drinkID);
    }
    public int GetDrinkNum_Func(int _drinkID)
    {
        return drinkDataArr[_drinkID].haveNum;
    }
    #endregion
}