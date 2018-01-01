using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour
{
    public static Player_Data Instance;
    
    [Header("Wealth")]
    [SerializeField] private bool isTest_Wealth;
    [SerializeField] private int goldValue;
    [SerializeField] private int mineralValue;
    [SerializeField] private PlayerWealth_Script playerWealthClass;

    [Header("Stage")]
    [SerializeField] private bool isTest_Stage;
    public int stageID_Normal
    {
        get
        {
            return m_stageID_Normal;
        }
    }
    private int m_stageID_Normal;
    public int stageID_Special
    {
        get
        {
            return m_stageID_Special;
        }
    }
    private int m_stageID_Special;

    [Header("Party")]
    [SerializeField] private bool isTest_Party;
    public int[] partyUnitIdArr;

    [Header("Unit")]
    [SerializeField] private bool isTest_Unit;
    [SerializeField] public PlayerUnit_ClassData[] playerUnitDataArr;

    [Header("Hero")]
    [SerializeField] private bool isTest_Hero;
    public int heroLevel
    {
        get
        {
            return m_heroLevel;
        }
    }
    private int m_heroLevel;
    public float hero_healthPoint_RelativeLevel;
    public float hero_attackValue_RelativeLevel;
    public Player_Script heroClass;
    public List<PlayerFood_ClassData> heroFoodDataList;

    [Header("Inventory")]
    [SerializeField] private bool isTest_Inventory = false;
    public List<PlayerFood_ClassData> inventoryFoodDataList;
    
    [Header("Trophy")]
    [SerializeField] private bool isTest_Trophy = false;
    public PlayerTrophy_Data[] trophyDataArr;

    [Header("Skill")]
    [SerializeField] private bool isTest_Skill = false;
    public PlayerSkill_Data[] skillDataArr;
    public int[] selectSkillIDArr;
    public int[] test_SkillLevel;

    [Header("Drink")]
    [SerializeField] private bool isTest_Drink = false;
    public PlayerDrink_Data[] drinkDataArr;

    [Header("Store")]
    [SerializeField] private bool isTest_Store = false;
    public int foodBoxLevel;
    public bool[] isPackageAlreadyBuyArr;
    
    [Header("PlayerData")]
    public bool isBgmOn = false;
    public bool isSfxOn = false;

    public IEnumerator Init_Cor()
    {
        Instance = this;
        
        yield return LoadWealth_Cor();
        yield return LoadStage_Cor();
        yield return LoadParty_Cor();
        yield return LoadUnit_Cor();
        yield return LoadHero_Cor();
        yield return LoadInventory_Cor();
        yield return LoadTrophy_Cor();
        yield return LoadSkill_Cor();
        yield return LoadDrink_Cor();
        yield return LoadStore_Cor();
        yield return LoadPlayData_Cor();

        yield break;
    }
    IEnumerator LoadWealth_Cor()
    {
        // Cargold : 골드, 미네랄 데이터 불러오기

        if(isTest_Wealth == false)
        {
            if(SaveSystem_Manager.Instance.isContinuePlayer == true)
            {
                goldValue = SaveSystem_Manager.Instance.LoadDataInt_Func(SaveType.Wealth_Gold);
                mineralValue = SaveSystem_Manager.Instance.LoadDataInt_Func(SaveType.Wealth_Mineral);
            }
            else
            {
                goldValue = 0;
                mineralValue = 0;

                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Wealth_Gold, 0);
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Wealth_Mineral, 0);
            }
        }
        else
        {

        }

        playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);
        playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);

        yield break;
    }
    IEnumerator LoadStage_Cor()
    {
        // Cargold : 스테이지 데이터 불러오기
        
        if(isTest_Stage == false)
        {
            if(SaveSystem_Manager.Instance.isContinuePlayer == true)
            {
                m_stageID_Normal = SaveSystem_Manager.Instance.LoadDataInt_Func(SaveType.Stage_Normal);
                m_stageID_Special = SaveSystem_Manager.Instance.LoadDataInt_Func(SaveType.Stage_Special);
            }
            else
            {
                m_stageID_Normal = 0;
                m_stageID_Special = 0;

                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Stage_Normal, 0);
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Stage_Special, 0);
            }
        }
        else
        {

        }

        yield break;
    }
    IEnumerator LoadParty_Cor()
    {
        if(isTest_Party == false)
        {
            partyUnitIdArr = new int[5];

            for (int i = 0; i < 5; i++)
            {
                if (SaveSystem_Manager.Instance.isContinuePlayer == true)
                {
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Party_Member_zzzSlotIDzzz_UnitID, i);
                    partyUnitIdArr[i] = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType); // Party_Member_zzzSlotIDzzz_UnitID
                }
                else
                {
                    partyUnitIdArr[i] = -1;

                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Party_Member_zzzSlotIDzzz_UnitID, i);
                    SaveSystem_Manager.Instance.SaveData_Func(_loadType, -1); // Party_Member_zzzSlotIDzzz_UnitID
                }
            }
        }
        else
        {

        }

        yield break;
    }
    IEnumerator LoadUnit_Cor()
    {
        if(isTest_Unit == false)
        {
            int _unitNum = DataBase_Manager.Instance.unitDataObjArr.Length;
            playerUnitDataArr = new PlayerUnit_ClassData[_unitNum];

            for (int i = 0; i < _unitNum; i++)
            {
                bool _isUnlock = false;
                int _unitLevel = 1;

                if (SaveSystem_Manager.Instance.isContinuePlayer == true)
                {
                    // 캐릭터 획득 여부 판단
                    if (DataBase_Manager.Instance.GetUnitClass_Func(i).unlockLevel <= (m_stageID_Normal + 1))
                    {
                        _isUnlock = true;
                    }

                    // 캐릭터 레벨 불러오기
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Unit_zzzUnitIDzzz_Level, i);
                    _unitLevel = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType);
                }
                else
                {
                    // 첫 캐릭터 해금
                    if (i == 0)
                    {
                        _isUnlock = true;
                    }

                    // 캐릭터 초기 레벨 저장
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Unit_zzzUnitIDzzz_Level, i);
                    SaveSystem_Manager.Instance.SaveData_Func(_loadType, 1); // Unit_zzzUnitIDzzz_Level
                }

                Unit_Script _unitClass = DataBase_Manager.Instance.GetUnitClass_Func(i);

                playerUnitDataArr[i] = new PlayerUnit_ClassData();
                yield return playerUnitDataArr[i].Init_Cor(_isUnlock, i, _unitLevel, _unitClass);
            }
        }
        else
        {

        }
        
        yield break;
    }
    IEnumerator LoadHero_Cor()
    {
        DataBase_Manager.Instance.heroAttackRate = heroClass.GetAttackSpeedMax_Func();
        heroFoodDataList = new List<PlayerFood_ClassData>();

        if(isTest_Hero == false)
        {
            if (SaveSystem_Manager.Instance.isContinuePlayer == true)
            {
                // 영웅 레벨 불러오기
                m_heroLevel = SaveSystem_Manager.Instance.LoadDataInt_Func(SaveType.Hero_Level);
                Hero_SetLevel_Func(m_heroLevel, true);

                // 영웅의 음식 정보 불러오기
                int _feedFoodNum = SaveSystem_Manager.Instance.LoadDataInt_Func(SaveType.Hero_FoodHaveNum);
                for (int i = 0; i < _feedFoodNum; i++)
                {
                    SaveSystem_Manager.SaveFoodDataStr _saveFoodDataStr =
                        SaveSystem_Manager.Instance.LoadDataHeroFood_Func(i);

                    PlayerFood_ClassData _playerFoodClass = new PlayerFood_ClassData(_saveFoodDataStr);

                    yield return null;
                }
            }
            else
            {
                // 영웅 초기 레벨 저장
                m_heroLevel = 1;
                Hero_SetLevel_Func(m_heroLevel, true);
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Hero_Level, 1);

                // 영웅 초기 모래주머니
                SetHeroStone_Func();
            }
        }
        else
        {

        }

        yield break;
    }
    void SetHeroStone_Func()
    {
        // 영웅의 음식 정보 불러오기
        float _feedFoodPosX = 0f;
        float _feedFoodPosY = 0f;
        float _feedFoodRotZ = 0f;

        for (int i = 0; i < 3; i++)
        {
            PlayerFood_ClassData _playerFoodClass = new PlayerFood_ClassData(FoodType.Stone, 0, 1);

            if (i == 0)
            {
                _feedFoodPosX = 1403.901f;
                _feedFoodPosY = 282.4627f;
                _feedFoodRotZ = 349.2703f;
            }
            else if (i == 1)
            {
                _feedFoodPosX = 1565.891f;
                _feedFoodPosY = 310.1227f;
                _feedFoodRotZ = 85.3086f;
            }
            else if (i == 2)
            {
                _feedFoodPosX = 1463.391f;
                _feedFoodPosY = 432.4628f;
                _feedFoodRotZ = 343.5417f;
            }

            _playerFoodClass.pos = new Vector2(_feedFoodPosX, _feedFoodPosY);
            _playerFoodClass.rot = Vector3.forward * _feedFoodRotZ;

            heroFoodDataList.Add(_playerFoodClass);

            SaveSystem_Manager.Instance.SaveData_HeroFood_Func(i, _playerFoodClass);
        }

        SaveSystem_Manager.Instance.SaveData_Func(SaveType.Hero_FoodHaveNum, 3);
    }
    IEnumerator LoadInventory_Cor()
    {
        if (isTest_Inventory == false)
        {
            inventoryFoodDataList = new List<PlayerFood_ClassData>();

            if (SaveSystem_Manager.Instance.isContinuePlayer == true)
            {
                // 음식 정보 불러오기
                int _foodHaveNum = SaveSystem_Manager.Instance.LoadDataInt_Func(SaveType.Inventory_FoodHaveNum);
                for (int i = 0; i < _foodHaveNum; i++)
                {
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Inventory_zzzFoodHaveIDzzz_FoodType, i);
                    int _foodTypeID = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType);
                    FoodType _foodType = (FoodType)_foodTypeID;

                    _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Inventory_zzzFoodHaveIDzzz_FoodID, i);
                    int _foodID = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType);

                    _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Inventory_zzzFoodHaveIDzzz_FoodLevel, i);
                    int _level = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType);

                    _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Inventory_zzzFoodHaveIDzzz_FoodExp, i);
                    float _remainExp = SaveSystem_Manager.Instance.LoadDataFloat_Func(_loadType);

                    PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData(_foodType, _foodID, _level, _remainExp);

                    inventoryFoodDataList.Add(_playerFoodData);
                }
            }
            else
            {
                SaveSystem_Manager.Instance.SaveData_Func(SaveType.Inventory_FoodHaveNum, 0);
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                int _foodIDMax = DataBase_Manager.Instance.foodDataArr.Length;
                int _foodID = Random.Range(0, _foodIDMax);
                int _level = Random.Range(1, 4);
                float _remainExp = Random.Range(0f, 99f);
                PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData(FoodType.Normal, _foodID, _level, _remainExp);

                inventoryFoodDataList.Add(_playerFoodData);
            }
        }

        yield break;
    }
    IEnumerator LoadTrophy_Cor()
    {
        if(isTest_Trophy == false)
        {
            int _trophyNum = DataBase_Manager.Instance.trophyObjArr.Length;
            trophyDataArr = new PlayerTrophy_Data[_trophyNum];

            for (int i = 0; i < _trophyNum; i++)
            {
                trophyDataArr[i].trophyID = i;
                trophyDataArr[i].trophyType = (TrophyType)i;

                if(SaveSystem_Manager.Instance.isContinuePlayer == true)
                {
                    // 트로피 보유량 불러오기 기능
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Trophy_zzzTrophyIDzzz_HaveNum, i);
                    int _haveNum = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType);
                    trophyDataArr[i].haveNum = _haveNum;

                    SetTrophyEffect_Func(i, trophyDataArr[i].haveNum);
                }
                else
                {
                    trophyDataArr[i].haveNum = 0;

                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Trophy_zzzTrophyIDzzz_HaveNum, i);
                    SaveSystem_Manager.Instance.SaveData_Func(_loadType, 0); // Trophy_zzzTrophyIDzzz_HaveNum
                }
            }
        }
        else
        {

        }
        
        yield break;
    }
    IEnumerator LoadSkill_Cor()
    {
        if (isTest_Skill == false)
        {
            int _skillNum = DataBase_Manager.Instance.skillDataObjArr.Length;
            skillDataArr = new PlayerSkill_Data[_skillNum];

            // 스킬 데이터 관리
            for (int skillID = 0; skillID < _skillNum; skillID++)
            {
                // 스킬 데이터 초기화
                skillDataArr[skillID].Init_Func(skillID);

                if (SaveSystem_Manager.Instance.isContinuePlayer == true)
                {
                    // 스킬 해금
                    if (skillDataArr[skillID].skillParentClass.unlockLevel <= (m_stageID_Normal + 1))
                    {
                        skillDataArr[skillID].UnlockSkill_Func();
                    }

                    // 스킬 레벨 불러오기
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Skill_zzzSkillIDzzz_Level, skillID);
                    int _skillLevel = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType); // Skill_zzzSkillIDzzz_Level
                    LevelUpSkill_Func(skillID, _skillLevel);
                }
                else
                {
                    // 첫 스킬 해금
                    if (skillID == 0)
                    {
                        skillDataArr[0].UnlockSkill_Func();

                        selectSkillIDArr[0] = 0;
                    }

                    // 스킬 초기 레벨
                    LevelUpSkill_Func(skillID, 1);
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Skill_zzzSkillIDzzz_Level, skillID);
                    SaveSystem_Manager.Instance.SaveData_Func(_loadType, 1); // Skill_zzzSkillIDzzz_Level
                }
            }

            selectSkillIDArr = new int[5];
            for (int slotID = 0; slotID < 5; slotID++)
            {
                string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Skill_zzzSlotIDzzz_SkillID, slotID);

                if (SaveSystem_Manager.Instance.isContinuePlayer == true)
                {
                    // 선택한 스킬 슬롯 불러오기

                    int _selectSkillID = SaveSystem_Manager.Instance.LoadDataInt_Func(_saveType); // Skill_zzzSlotIDzzz_SkillID
                    selectSkillIDArr[slotID] = _selectSkillID;
                }
                else
                {
                    // 초기 선택 스킬 슬롯

                    if (0 < slotID)
                    {
                        selectSkillIDArr[slotID] = -1;
                    }
                    else
                    {
                        selectSkillIDArr[slotID] = 0;
                    }

                    SaveSystem_Manager.Instance.SaveData_Func(_saveType, selectSkillIDArr[slotID]); // Skill_zzzSlotIDzzz_SkillID
                }
            }
        }
        else
        {
            //if (0 < test_SkillLevel[i])
            //{
            //    UnlockSkill_Func(i);
            //    LevelUpSkill_Func(i, test_SkillLevel[i]);
            //}
        }
            
        yield break;
    }
    IEnumerator LoadDrink_Cor()
    {
        if(isTest_Drink == false)
        {
            int _drinkNum = DataBase_Manager.Instance.drinkDataArr.Length;
            drinkDataArr = new PlayerDrink_Data[_drinkNum];
            
            for (int i = 0; i < _drinkNum; i++)
            {
                drinkDataArr[i].drinkID = i;
                drinkDataArr[i].drinkType = (DrinkType)i;
                drinkDataArr[i].isUse = false;

                if (SaveSystem_Manager.Instance.isContinuePlayer == true)
                {
                    // 드링크 개수 불러오기
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Drink_zzzDrinkIDzzz_HaveNum, i);
                    drinkDataArr[i].haveNum = SaveSystem_Manager.Instance.LoadDataInt_Func(_loadType); // Drink_zzzDrinkIDzzz_HaveNum
                }
                else
                {
                    // 초기 드링크 개수 저장
                    drinkDataArr[i].haveNum = 0;
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Drink_zzzDrinkIDzzz_HaveNum, i);
                    SaveSystem_Manager.Instance.SaveData_Func(_loadType, 0); // Drink_zzzDrinkIDzzz_HaveNum
                }
            }
        }
        else
        {

        }
        
        yield break;
    }
    IEnumerator LoadStore_Cor()
    {
        if(isTest_Store == false)
        {
            if(SaveSystem_Manager.Instance.isContinuePlayer == true)
            {
                // 가차박스 레벨 갱신
                foodBoxLevel = ((int)((m_stageID_Special + 1) / 5)) + 1;
            }
            else
            {
                // 가차박스 초기 레벨
                foodBoxLevel = 1;
            }

            // 패키지 구매 기록 불러오기
            isPackageAlreadyBuyArr = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                if (SaveSystem_Manager.Instance.isContinuePlayer == true)
                {
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Store_zzzPackageIDzzz_IsBuyRecord, i);
                    isPackageAlreadyBuyArr[i] = SaveSystem_Manager.Instance.LoadDataBool_Func(_loadType); // Store_zzzPackageIDzzz_IsBuyRecord
                }
                else
                {
                    isPackageAlreadyBuyArr[i] = false;
                    string _loadType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Store_zzzPackageIDzzz_IsBuyRecord, i);
                    SaveSystem_Manager.Instance.SaveData_Func(_loadType, false); // Store_zzzPackageIDzzz_IsBuyRecord
                }
            }
        }
        else
        {

        }

        yield break;
    }
    IEnumerator LoadPlayData_Cor()
    {
        if(SaveSystem_Manager.Instance.isContinuePlayer == true)
        {
            isBgmOn = SaveSystem_Manager.Instance.LoadDataBool_Func(SaveType.Player_BGM);
            isSfxOn = SaveSystem_Manager.Instance.LoadDataBool_Func(SaveType.Player_SFX);
        }
        else
        {
            isBgmOn = true;
            isSfxOn = true;

            SaveSystem_Manager.Instance.SaveData_Func(SaveType.Player_BGM, true);
            SaveSystem_Manager.Instance.SaveData_Func(SaveType.Player_SFX, true);

            SaveSystem_Manager.Instance.isContinuePlayer = true;
        }
        
        yield break;
    }
    
    #region Wealth Group
    public void AddWealth_Func(WealthType _wealthType, int _value)
    {
        if (_wealthType == WealthType.Gold)
        {
            goldValue += _value;
            playerWealthClass.PrintWealth_Func(WealthType.Gold, goldValue);

            SaveSystem_Manager.Instance.SaveData_Func(SaveType.Wealth_Gold, goldValue);
        }
        else if (_wealthType == WealthType.Mineral)
        {
            mineralValue += _value;
            playerWealthClass.PrintWealth_Func(WealthType.Mineral, mineralValue);

            SaveSystem_Manager.Instance.SaveData_Func(SaveType.Wealth_Mineral, mineralValue);
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

                    SaveSystem_Manager.Instance.SaveData_Func(SaveType.Wealth_Gold, goldValue);
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

                    SaveSystem_Manager.Instance.SaveData_Func(SaveType.Wealth_Mineral, mineralValue);
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
    #region Stage Group
    public void SetStageID_Func(BattleType _battleType, int _stageID)
    {
        if(_battleType == BattleType.Normal)
        {
            m_stageID_Normal = _stageID;

            SaveSystem_Manager.Instance.SaveData_Func(SaveType.Stage_Normal, _stageID);
        }
        else if(_battleType == BattleType.Special)
        {
            m_stageID_Special = _stageID;

            SaveSystem_Manager.Instance.SaveData_Func(SaveType.Stage_Special, _stageID);
        }
    }
    #endregion
    #region Party Group
    public void UnlockUnit_Func(int _unitID)
    {
        playerUnitDataArr[_unitID].isHave = true;
        Lobby_Manager.Instance.partySettingClass.UnlockCard_Func(_unitID);
    }
    public void JoinParty_Func(int _partySlotId, int _unitId)
    {
        partyUnitIdArr[_partySlotId] = _unitId;

        SavePartyData_Func(_partySlotId, _unitId);
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

        SavePartyData_Func(_partySlotId, _unitId);
    }
    void SavePartyData_Func(int _partySlotId, int _unitId)
    {
        string _saveType =
            SaveSystem_Manager.Instance.SetRename_Func(SaveType.Party_Member_zzzSlotIDzzz_UnitID, _partySlotId);
        SaveSystem_Manager.Instance.SaveData_Func(_saveType, _unitId); // Party_Member_zzzSlotIDzzz_UnitID
    }
    #endregion
    #region Unit Group
    public void SaveFeedData_Func(int _unitID)
    {
        playerUnitDataArr[_unitID].SaveFeedData_Func();
    }
    #endregion
    #region Hero Group
    void Hero_FeedFood_Func(Food_Script _foodClass)
    {
        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData(_foodClass);
        heroFoodDataList.Add(_playerFoodData);

        SetCharDataByFood_Func(heroClass, _foodClass, true);
    }
    void Hero_OutFood_Func(Food_Script _foodClass)
    {
        int _haveFoodID = GetHaveFoodID_Func(heroFoodDataList, _foodClass);
        heroFoodDataList.RemoveAt (_haveFoodID);

        SetCharDataByFood_Func(heroClass, _foodClass, false);
    }
    void Hero_SetFoodData_Func(Food_Script _foodClass, int _haveFoodID = -1)
    {
        if (_haveFoodID == -1)
            _haveFoodID = GetHaveFoodID_Func(heroFoodDataList, _foodClass);

        heroFoodDataList[_haveFoodID].SetData_Func(_foodClass);
    }

    public void Hero_SetLevel_Func(float _levelValue = -1f, bool _isInit = false)
    {
        if (_levelValue == -1f)
            _levelValue = m_heroLevel;

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
        m_heroLevel = (int)_levelValue;
        SaveSystem_Manager.Instance.SaveData_Func(SaveType.Hero_Level, m_heroLevel);
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
            Food_Script _foodClass = heroFoodDataList[i].GetFoodClass_Func();
            SetCharDataByFood_Func(heroClass, _foodClass, true, false);
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

    public void Hero_SaveFeedData_Func()
    {
        int _heroFoodDataNum = heroFoodDataList.Count;
        string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Hero_FoodHaveNum, _heroFoodDataNum);
        SaveSystem_Manager.Instance.SaveData_Func(_saveType, _heroFoodDataNum); // Hero_FoodHaveNum

        for (int i = 0; i < _heroFoodDataNum; i++)
        {
            SaveSystem_Manager.Instance.SaveData_HeroFood_Func(i, heroFoodDataList[i]);
        }
    }
    #endregion
    #region Inventory Group
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
        {
            Debug.LogError("Bug : 음식을 찾을 수 없습니다.");
            Debug.LogError("Bug : _charFoodDataList.ToString()");
        }

        return _inventoryFoodID;
    }

    public void AddFood_Func(int _foodID, int _foodLevel = -1, FoodType _foodType = FoodType.Normal)
    {
        // 획득 경로1 : 전투 보상
        // 획득 경로2 : 상점 구매

        if (_foodLevel == -1)
            _foodLevel = foodBoxLevel;

        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData(_foodType, _foodID, _foodLevel);
        int _invenFoodNum = inventoryFoodDataList.Count;

        SaveSystem_Manager.Instance.SaveData_Func(SaveType.Inventory_FoodHaveNum, _invenFoodNum);
        SaveSystem_Manager.Instance.SaveData_InvenFood_Func(_invenFoodNum, _playerFoodData);
        
        inventoryFoodDataList.Add(_playerFoodData);
    }
    public void AddFoodInInventory_Func(Food_Script _foodClass)
    {
        // 유닛 위장에서 인벤토리로...

        PlayerFood_ClassData _playerFoodData = new PlayerFood_ClassData(_foodClass);
        
        inventoryFoodDataList.Add(_playerFoodData);
    }
    public void SaveInvenFoodData_Func()
    {
        int _invenFoodNum = inventoryFoodDataList.Count;

        SaveSystem_Manager.Instance.SaveData_Func(SaveType.Inventory_FoodHaveNum, _invenFoodNum);

        for (int i = 0; i < _invenFoodNum; i++)
        {
            SaveSystem_Manager.Instance.SaveData_InvenFood_Func(i, inventoryFoodDataList[i]);
        }
    }
    public void UseMaterialFood_Func(Food_Script _foodClass)
    {
        int _inventoryFoodID = GetHaveFoodID_Func(inventoryFoodDataList, _foodClass);

        if (_inventoryFoodID == -1)
            Debug.LogError("Bug : 해당 음식을 인벤토리에 찾을 수 없습니다.");

        inventoryFoodDataList.RemoveAt(_inventoryFoodID);
    }
    public void FeedFood_Func(int _charID, Food_Script _foodClass)
    {
        int _inventoryFoodID = GetHaveFoodID_Func(inventoryFoodDataList, _foodClass);
        inventoryFoodDataList.RemoveAt(_inventoryFoodID);

        if (_charID == 999)
        {
            // Hero
            Hero_FeedFood_Func(_foodClass);
        }
        else
        {
            // Unit
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
            int _inventoryFoodID = GetHaveFoodID_Func(inventoryFoodDataList, _foodClass);
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

                string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Trophy_zzzTrophyIDzzz_HaveNum, _trophyID);
                SaveSystem_Manager.Instance.SaveData_Func(_saveType, trophyDataArr[_trophyID].haveNum);

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
        int _selectSkillID = _grade * 2;
        if (_isUpSkill == false) _selectSkillID++;
        selectSkillIDArr[_grade] = _selectSkillID;

        string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Skill_zzzSlotIDzzz_SkillID, _grade);
        SaveSystem_Manager.Instance.SaveData_Func(_saveType, _selectSkillID);
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

        string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Drink_zzzDrinkIDzzz_HaveNum, _drinkID);
        SaveSystem_Manager.Instance.SaveData_Func(_saveType, drinkDataArr[_drinkID].haveNum);
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
        {
            Debug.LogError("Bug : 다음 드링크의 재고가 없습니다." + (DrinkType)_drinkID);
        }
        else
        {
            drinkDataArr[_drinkID].haveNum--;

            string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Drink_zzzDrinkIDzzz_HaveNum, _drinkID);
            SaveSystem_Manager.Instance.SaveData_Func(_saveType, drinkDataArr[_drinkID].haveNum);

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
    #region Store Group
    public void BuyPackage_Func(int _packageID)
    {
        if (isPackageAlreadyBuyArr[_packageID] == false)
        {
            isPackageAlreadyBuyArr[_packageID] = true;

            string _saveType = SaveSystem_Manager.Instance.SetRename_Func(SaveType.Store_zzzPackageIDzzz_IsBuyRecord, _packageID);
            SaveSystem_Manager.Instance.SaveData_Func(_saveType, true);
        }
        else if(isPackageAlreadyBuyArr[_packageID] == true)
        {
            Debug.LogError("Bug : 패키지 구매 이력이 이미 있습니다.");
        }
    }
    #endregion
}