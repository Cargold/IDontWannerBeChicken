using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data Instance;

    [Header("Wealth")]
    [SerializeField]
    private int goldValue;
    [SerializeField]
    private int mineralValue;
    [SerializeField]
    private PlayerWealth_Script playerWealthClass;

    [Header("Trophy")]
    public PlayerTrophy_Data[] trophyDataArr;

    [Header("Party")]
    public int[] partyUnitIdArr;
    public int populationPoint;

    [Header("Unit")]
    [SerializeField]
    public PlayerUnit_ClassData[] playerUnitDataArr;

    [Header("Hero")]
    public int heroLevel;
    public float hero_healthPoint_RelativeLevel;
    public float hero_attackValue_RelativeLevel;
    public Player_Script heroClass;
    public List<PlayerFood_ClassData> heroFoodDataList;

    [Header("Inventory")]
    public List<PlayerFood_ClassData> inventoryFoodDataList;
    public int foodBoxLevel;

    [Header("Stage")]
    public int stageID_Normal;
    public int stageID_Special;

    [Header("Skill")]
    public PlayerSkill_Data[] skillDataArr;
    public int[] selectSkillIDArr;
    public int[] test_SkillLevel;

    [Header("Drink")]
    public PlayerDrink_Data[] drinkDataArr;

    [Header("Package")]
    public bool[] isPackageAlreadyBuyArr;

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
        yield return LoadStage_Cor();
        yield return LoadSkill_Cor();
        yield return LoadDrink_Cor();
        yield return LoadPackage_Cor();

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

        Hero_SetLevel_Func(heroLevel, true);

        DataBase_Manager.Instance.heroAttackRate = heroClass.GetAttackSpeedMax_Func();

        yield break;
    }
    IEnumerator LoadInventory_Cor()
    {
        inventoryFoodDataList = new List<PlayerFood_ClassData>();

        LoadInventory_Test_Func();

        //for (int i = 0; i < 10; i++)
        //{
        //    PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

        //    _playerFoodData.foodType = FoodType.Stone;
        //    _playerFoodData.foodID = 0;
        //    _playerFoodData.level = 1;
        //    _playerFoodData.remainExp = 0;

        //    inventoryFoodDataList.Add(_playerFoodData);
        //}

        yield break;
    }
    void LoadInventory_Test_Func()
    {
        for (int i = 0; i < 20; i++)
        {
            PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

            int _foodIDMax = DataBase_Manager.Instance.foodDataArr.Length;
            _playerFoodData.foodID = Random.Range(0, _foodIDMax);
            _playerFoodData.level = Random.Range(1, 4);
            _playerFoodData.remainExp = Random.Range(0f, 99f);

            inventoryFoodDataList.Add(_playerFoodData);
        }
    }
    IEnumerator LoadTrophy_Cor()
    {
        for (int i = 0; i < trophyDataArr.Length; i++)
        {
            trophyDataArr[i].trophyID = i;
            trophyDataArr[i].trophyType = (TrophyType)i;

            // 트로피 보유량 불러오기 기능

            SetTrophyEffect_Func(i, trophyDataArr[i].haveNum);
        }

        yield break;
    } // UnComplete
    IEnumerator LoadStage_Cor()
    {
        // Cargold : 스테이지 데이터 불러오기
        
        yield break;
    } // UnComplete
    IEnumerator LoadSkill_Cor()
    {
        skillDataArr = new PlayerSkill_Data[10];
        for (int i = 0; i < 10; i++)
        {
            skillDataArr[i].Init_Func(i);
        }

        for (int i = 0; i < skillDataArr.Length; i++)
        {
            if(skillDataArr[i].skillParentClass.unlockLevel <= stageID_Normal)
            {
                skillDataArr[i].UnlockSkill_Func();
            }
        }

        // Test
        for (int i = 0; i < test_SkillLevel.Length; i++)
        {
            if(0 < test_SkillLevel[i])
            {
                UnlockSkill_Func(i);
                LevelUpSkill_Func(i, test_SkillLevel[i]);
            }
        }

        yield break;
    } // UnComplete
    IEnumerator LoadDrink_Cor()
    {
        yield break;
    }
    IEnumerator LoadPackage_Cor()
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
    public void OnLobbyWealthUI_Func()
    {
        playerWealthClass.OnLobby_Func();
    }
    public void OnBattleWealthUI_Func()
    {
        playerWealthClass.OnBattle_Func();
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
    #region Hero Group
    void Hero_FeedFood_Func(Food_Script _foodClass)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();
        _playerFoodData.SetData_Func(_foodClass);
        heroFoodDataList.Add(_playerFoodData);

        SetCharDataByFood_Func(heroClass, _foodClass, true);
    }
    void Hero_OutFood_Func(Food_Script _foodClass)
    {
        //int _haveFoodID = GetHaveFoodID_Func(heroFoodDataList, _foodClass);
        int _haveFoodID = _foodClass.placeID;
        heroFoodDataList.Remove(heroFoodDataList[_haveFoodID]);

        SetCharDataByFood_Func(heroClass, _foodClass, false);
    }
    void Hero_SetFoodData_Func(Food_Script _foodClass, int _haveFoodID = -1)
    {
        if (_haveFoodID == -1)
            //_haveFoodID = GetHaveFoodID_Func(heroFoodDataList, _foodClass);
            _haveFoodID = _foodClass.placeID;

        heroFoodDataList[_haveFoodID].SetData_Func(_foodClass);
    }

    public void Hero_SetLevel_Func(float _levelValue = -1f, bool _isInit = false)
    {
        if (_levelValue == -1f)
            _levelValue = heroLevel;

        Hero_SetLevel_InitUnitData_Func();
        Hero_SetLevel_Level_Func(_levelValue);
        Hero_SetLevel_Food_Func();
        Hero_SetLevel_Trophy_Func();

        if (_isInit == false)
            Lobby_Manager.Instance.heroManagementClass.PrintInfoUI_Func();
    }
    void Hero_SetLevel_InitUnitData_Func()
    {
        Hero_Data _heroData = DataBase_Manager.Instance.heroData;
        heroClass.SetData_Func(_heroData);
    }
    void Hero_SetLevel_Level_Func(float _levelValue)
    {
        heroLevel = (int)_levelValue;
        _levelValue -= 1f;

        float _levelPerBonus = DataBase_Manager.Instance.hero_LevelPerBonus;
        _levelPerBonus *= 0.01f;

        float _healthPoint = DataBase_Manager.Instance.heroData.healthPoint;
        hero_healthPoint_RelativeLevel = ((_levelValue * _levelPerBonus) + 1f) * _healthPoint;
        heroClass.healthPoint_Max = hero_healthPoint_RelativeLevel;

        float _attackValue = DataBase_Manager.Instance.heroData.attackValue;
        hero_attackValue_RelativeLevel = ((_levelValue * _levelPerBonus) + 1f) * _attackValue;
        heroClass.attackValue = hero_attackValue_RelativeLevel;
    }
    void Hero_SetLevel_Food_Func()
    {
        for (int i = 0; i < heroFoodDataList.Count; i++)
        {
            Food_Script _foodClass = heroFoodDataList[i].foodClass;
            Player_Data.Instance.SetCharDataByFood_Func(heroClass, _foodClass, true, false);
        }
    }
    void Hero_SetLevel_Trophy_Func()
    {
        float _hpTrophyEffectValue = GetCalcTrophyEffect_Func(TrophyType.HealthPoint_Hero, true);
        float _dmgTrophyEffectValue = GetCalcTrophyEffect_Func(TrophyType.AttackValue_Hero, true);
        float _critPerTrophyEffectValue = GetCalcTrophyEffect_Func(TrophyType.CriticalPercent_Hero, true);
        float _critBonusTrophyEffectValue = GetCalcTrophyEffect_Func(TrophyType.CriticalBonus_Hero, true);
        float _manaStart = GetCalcTrophyEffect_Func(TrophyType.ManaStart, true);
        float _manaRegen = GetCalcTrophyEffect_Func(TrophyType.ManaRegen, true);

        _hpTrophyEffectValue *= hero_healthPoint_RelativeLevel * 0.01f;
        _dmgTrophyEffectValue *= hero_attackValue_RelativeLevel * 0.01f;
        _critPerTrophyEffectValue *= DataBase_Manager.Instance.heroData.criticalPercent;
        _critBonusTrophyEffectValue *= DataBase_Manager.Instance.heroData.criticalBonus * 0.01f;
        _manaStart *= DataBase_Manager.Instance.heroData.manaStart * 0.01f;
        _manaRegen *= DataBase_Manager.Instance.heroData.manaRegen * 0.01f;

        heroClass.healthPoint_Max += _hpTrophyEffectValue;
        heroClass.attackValue += _dmgTrophyEffectValue;
        heroClass.criticalPercent += _critPerTrophyEffectValue;
        heroClass.criticalBonus += _critBonusTrophyEffectValue;
        heroClass.manaStart += _manaStart;
        heroClass.manaRegen += _manaRegen;
    }
    #endregion
    #region Food Group
    public int GetHaveFoodID_Func(List<PlayerFood_ClassData> _charFoodDataList, Food_Script _foodClass)
    {
        int _inventoryFoodID = -1;

        for (int i = 0; i < _charFoodDataList.Count; i++)
        {
            if (_foodClass == _charFoodDataList[i].GetFoodClass_Func())
            {
                _inventoryFoodID = i;
                break;
            }
        }

        if (_inventoryFoodID == -1)
            Debug.LogError("Bug : 음식을 찾을 수 없습니다.");

        return _inventoryFoodID;
    }

    public void AddFood_Func(int _foodID, int _foodLevel = -1)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

        if (_foodLevel == -1)
            _foodLevel = foodBoxLevel;
        _playerFoodData.foodType = FoodType.Normal;
        _playerFoodData.foodID = _foodID;
        _playerFoodData.level = _foodLevel;

        inventoryFoodDataList.Add(_playerFoodData);
    }
    public void AddSource_Func(int _sourceID, int _sourceLevel = -1)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();

        if (_sourceLevel == -1)
            _sourceLevel = foodBoxLevel;
        _playerFoodData.foodType = FoodType.Source;
        _playerFoodData.foodID = _sourceID;

        inventoryFoodDataList.Add(_playerFoodData);
    }
    public void AddFoodInInventory_Func(Food_Script _foodClass)
    {
        // 보상 또는 구매를 통해 인벤토리로...
        // 유닛 위장에서 인벤토리로...

        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData();
        _playerFoodData.SetData_Func(_foodClass);
        
        inventoryFoodDataList.Add(_playerFoodData);
    }
    public void OutFoodInInventory_Func(Food_Script _foodClass)
    {
        if (_foodClass.placeID == -1)
            Debug.LogError("Bug : 해당 음식을 인벤토리에 찾을 수 없습니다.");

        inventoryFoodDataList.RemoveAt(_foodClass.placeID);
        ObjectPool_Manager.Instance.Free_Func(_foodClass.gameObject);
    }
    public void FeedFood_Func(int _charID, Food_Script _foodClass)
    {
        if (_charID == 999)
        {
            // Hero

            //int _inventoryFoodID = GetInventoryFoodID_Func(_foodClass);
            int _inventoryFoodID = _foodClass.placeID;
            inventoryFoodDataList.RemoveAt(_inventoryFoodID);

            Hero_FeedFood_Func(_foodClass);
        }
        else
        {
            // Unit

            //int _inventoryFoodID = GetInventoryFoodID_Func(_foodClass);
            int _inventoryFoodID = _foodClass.placeID;
            inventoryFoodDataList.RemoveAt(_inventoryFoodID);

            playerUnitDataArr[_charID].FeedFood_Func(_foodClass);
        }
    }
    public void OutFoodInChar_Func(int _charID, Food_Script _foodClass)
    {
        if(_charID == 999)
        {
            Hero_OutFood_Func(_foodClass);
        }
        else
        {
            playerUnitDataArr[_charID].OutFood_Func(_foodClass);
        }
    }

    public void SetFoodData_Func(Food_Script _foodClass, bool _isInventoryFood, int _charID = -1)
    {
        if(_isInventoryFood == true)
        {
            //int _inventoryFoodID = GetInventoryFoodID_Func(_foodClass);
            int _inventoryFoodID = _foodClass.placeID;
            inventoryFoodDataList[_inventoryFoodID].SetData_Func(_foodClass);
        }
        else if(_isInventoryFood == false)
        {
            if(_charID == 999)
            {
                Hero_SetFoodData_Func(_foodClass);
            }
            else
            {
                playerUnitDataArr[_charID].SetFoodData_Func(_foodClass);
            }
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

    public void SetCharDataByFood_Func(int _charID, Food_Script _foodClass, bool _isFeed)
    {
        Character_Script _charClass = null;

        if(_charID == 999)
        {
            _charClass = heroClass;
        }
        else
        {
            _charClass = DataBase_Manager.Instance.GetUnitClass_Func(_charID);
        }
        
        SetCharDataByFood_Func(_charClass, _foodClass, _isFeed);
    }
    public void SetCharDataByFood_Func(Character_Script _charClass, Food_Script _foodClass, bool _isFeed, bool _isPrintUI = true)
    {
        float _feedingCalc = 0f;
        if (_isFeed == true)
            _feedingCalc = 0.01f;
        else if (_isFeed == false)
            _feedingCalc = -0.01f;

        if(_foodClass.effectMain == FoodEffect_Main.AttackPower || _foodClass.effectMain == FoodEffect_Main.HealthPoint)
            SetCharDataByFoodMainEffect_Func(_charClass, _foodClass, _feedingCalc);

        if(FoodEffect_Sub.None < _foodClass.effectSub)
            SetCharDataByFoodSubEffect_Func(_charClass, _foodClass, _feedingCalc);

        if(_isPrintUI == true)
        {
            if(_charClass.unitID == 999)
            {
                Lobby_Manager.Instance.heroManagementClass.PrintInfoUI_Func();
            }
            else
            {
                Lobby_Manager.Instance.partySettingClass.PrintInfoUI_Func();
            }
        }
    }
    void SetCharDataByFoodMainEffect_Func(Character_Script _charClass, Food_Script _foodClass, float _feedingCalc)
    {
        int _charID = _charClass.unitID;
        float _value = 0f;

        switch (_foodClass.effectMain)
        {
            case FoodEffect_Main.AttackPower:
                if (_charID == 999)
                {
                    _value = hero_attackValue_RelativeLevel;
                }
                else
                {
                    _value = playerUnitDataArr[_charID].attackValue_RelativeLevel;
                }
                _value = _value * _foodClass.GetMainEffectValue_Func() * _feedingCalc;
                _charClass.attackValue += _value;
                break;

            case FoodEffect_Main.HealthPoint:
                if (_charID == 999)
                {
                    _value = hero_healthPoint_RelativeLevel;
                }
                else
                {
                    _value = playerUnitDataArr[_charID].healthPoint_RelativeLevel;
                }
                _value = _value * _foodClass.GetMainEffectValue_Func() * _feedingCalc;
                _charClass.healthPoint_Max += _value;
                break;
        }
    }
    void SetCharDataByFoodSubEffect_Func(Character_Script _charClass, Food_Script _foodClass, float _feedingCalc)
    {
        int _charID = _charClass.unitID;
        float _value = 0f;

        switch (_foodClass.effectSub)
        {
            case FoodEffect_Sub.Critical:
                _value = _foodClass.GetSubEffectValue_Func() * _feedingCalc * 100f;
                _charClass.criticalPercent += _value;
                break;

            case FoodEffect_Sub.SpawnInterval:
                if(_charClass.unitID == 999)
                {
                    Debug.Log("System : 영웅 캐릭터가 생산속도 관련 음식을 습득했습니다.");
                }
                else
                {
                    Unit_Script _unitClass = _charClass as Unit_Script;
                    _value = _unitClass.spawnInterval;
                    if (_value == 0f) _value = 1f;
                    _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                    _unitClass.spawnInterval -= _value;
                }
                break;

            case FoodEffect_Sub.DecreaseHP:
                if (_charID == 999)
                {
                    _value = hero_healthPoint_RelativeLevel;
                }
                else
                {
                    _value = playerUnitDataArr[_charID].healthPoint_RelativeLevel;
                }
                if (_value == 0f) _value = 1f;
                _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _charClass.healthPoint_Max -= _value;
                break;

            case FoodEffect_Sub.DefenceValue:
                _value = _foodClass.GetSubEffectValue_Func() * _feedingCalc * 100f;
                _charClass.defenceValue += _value;
                break;

            case FoodEffect_Sub.DecreaseAttack:
                if (_charID == 999)
                {
                    _value = hero_attackValue_RelativeLevel;
                }
                else
                {
                    _value = playerUnitDataArr[_charID].attackValue_RelativeLevel;
                }
                if (_value == 0f) _value = 1f;
                _value = _value * _foodClass.GetSubEffectValue_Func() * _feedingCalc;
                _charClass.attackValue -= _value;
                break;
        }
    }

    public void AddFoodBoxLevel_Func()
    {
        foodBoxLevel++;
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
    //public void 
    public int GetTrophyRandID_Func()
    {
        return Random.Range(0, trophyDataArr.Length);
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
        if(_trophyID < 0)
            _trophyID = Random.Range(0, trophyDataArr.Length);

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
        float _calcValue = GetCalcTrophyEffect_Func(_trophyID);
        int _charNum = 0;

        switch (_trophyType)
        {
            case TrophyType.HealthPoint_Hero:
                _calcValue *= 0.01f;
                _calcValue *= hero_healthPoint_RelativeLevel;
                heroClass.healthPoint_Max += _calcValue * _calcNum;
                break;
            case TrophyType.AttackValue_Hero:
                _calcValue *= 0.01f;
                _calcValue *= hero_attackValue_RelativeLevel;
                heroClass.attackValue += _calcValue * _calcNum;
                break;
            case TrophyType.CriticalPercent_Hero:
                //_calcValue *= 0.01f;
                _calcValue *= DataBase_Manager.Instance.heroData.criticalPercent;
                heroClass.criticalPercent += _calcValue * _calcNum;
                break;
            case TrophyType.CriticalBonus_Hero:
                _calcValue *= 0.01f;
                _calcValue *= DataBase_Manager.Instance.heroData.criticalBonus;
                heroClass.criticalBonus += _calcValue * _calcNum;
                break;
            case TrophyType.ManaRegen:
                _calcValue *= 0.01f;
                _calcValue *= DataBase_Manager.Instance.heroData.manaRegen;
                heroClass.manaRegen += _calcValue * _calcNum;
                break;
            case TrophyType.ManaStart:
                _calcValue *= 0.01f;
                _calcValue *= DataBase_Manager.Instance.heroData.manaStart;
                heroClass.manaStart += _calcValue * _calcNum;
                break;
            case TrophyType.HealthPoint_Unit:
                _charNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                _calcValue *= 0.01f;
                for (int i = 0; i < _charNum; i++)
                {
                    float _calcValue_Unit = _calcValue;
                    _calcValue_Unit *= playerUnitDataArr[i].healthPoint_RelativeLevel;
                    playerUnitDataArr[i].unitClass.healthPoint_Max += _calcValue_Unit * _calcNum;
                }
                break;
            case TrophyType.AttackValue_Unit:
                _charNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                _calcValue *= 0.01f;
                for (int i = 0; i < _charNum; i++)
                {
                    float _calcValue_Unit = _calcValue;
                    _calcValue_Unit *= playerUnitDataArr[i].attackValue_RelativeLevel * 0.01f;
                    playerUnitDataArr[i].unitClass.attackValue += _calcValue_Unit * _calcNum;
                }
                break;
            case TrophyType.CriticalPercent_Unit:
                _charNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                for (int i = 0; i < _charNum; i++)
                {
                    float _calcValue_Unit = _calcValue;
                    _calcValue_Unit *= DataBase_Manager.Instance.unitDataArr[i].criticalPercent;
                    playerUnitDataArr[i].unitClass.criticalPercent += _calcValue_Unit * _calcNum;
                }
                break;
            case TrophyType.CriticalBonus_Unit:
                _charNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
                _calcValue *= 0.01f;
                for (int i = 0; i < _charNum; i++)
                {
                    float _calcValue_Unit = _calcValue;
                    _calcValue_Unit *= DataBase_Manager.Instance.unitDataArr[i].criticalBonus;
                    playerUnitDataArr[i].unitClass.criticalBonus += _calcValue_Unit * _calcNum;
                }
                break;
            case TrophyType.HealthPoint_Monster:
                _charNum = DataBase_Manager.Instance.GetMonsterMaxNum_Func();
                _calcValue *= 0.01f;
                for (int i = 0; i < _charNum; i++)
                {
                    float _calcValue_Unit = _calcValue;
                    _calcValue_Unit *= DataBase_Manager.Instance.monsterDataArr[i].healthPoint;
                    DataBase_Manager.Instance.monsterClassDic[i].healthPoint_Max -= _calcValue_Unit * _calcNum;
                }
                break;
            case TrophyType.AttackValue_Monster:
                _charNum = DataBase_Manager.Instance.GetMonsterMaxNum_Func();
                _calcValue *= 0.01f;
                for (int i = 0; i < _charNum; i++)
                {
                    float _calcValue_Unit = _calcValue;
                    _calcValue_Unit *= DataBase_Manager.Instance.monsterDataArr[i].attackValue;
                    DataBase_Manager.Instance.monsterClassDic[i].attackValue -= _calcValue_Unit * _calcNum;
                }
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
    #region Skill Group
    public void UnlockSkill_Func(int _skillID)
    {
        skillDataArr[_skillID].UnlockSkill_Func();
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
    public int GetSkillLevel_Func(int _skillID)
    {
        return skillDataArr[_skillID].skillLevel;
    }
    public int GetSkillUpCost_Func(int _skillID)
    {
        int _skillLevel = GetSkillLevel_Func(_skillID);
        
        int _skillUpCost = ((_skillLevel - 1) * 2) + skillDataArr[_skillID].skillParentClass.upgradeInitCost;

        if (60 < _skillUpCost)
            _skillUpCost = 60;

        return _skillUpCost;
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
    public void BuyPackage_Func(int _packageID)
    {
        if (isPackageAlreadyBuyArr[_packageID] == false)
        {
            isPackageAlreadyBuyArr[_packageID] = true;
        }
        else if(isPackageAlreadyBuyArr[_packageID] == true)
        {
            Debug.LogError("Bug : 패키지 구매 이력이 이미 있습니다.");
        }
    }
}