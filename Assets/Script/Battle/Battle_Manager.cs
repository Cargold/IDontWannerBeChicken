using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Battle_Manager : MonoBehaviour
{
    public static Battle_Manager Instance;

    public Player_Script playerClass;
    public SkillSystem_Manager skillSystemManager;
    public RectTransform battleUITrf;
    public BattleSpawn_Script[] spawnClassArr_Ally;
    public List<BattleSpawn_Script> activeSapwnClassList_Ally;
    public Transform spawnPos_Ally;
    public Transform spawnTrf_Ally;
    public BattleSpawn_Script[] spawnClassArr_Enemy;
    public List<BattleSpawn_Script> activeSpawnClassList_Enemy;
    public Transform spawnTrf_Enemy;
    public Transform spawnPos_Enemy;
    public List<int> spawnEnemyIDList;

    public bool[] isSpawnWaveArr;
    public int[] spawnWave_BonusValue_Normal;
    public float[] spawnWave_BonusTime_Normal;
    public int[] spawnWave_BonusValue_Special;
    public float[] spawnWave_BonusTime_Special;
    public ChickenHouse_Script chickenHouseClass;
    public Sprite[] chickenHouseSpriteArr;

    public float spawnPosX_Range;
    public float spawnPosY_Min;
    public float spawnPosY_Max;
    public float spawnJumpPower_Min;
    public float spawnJumpPower_Max;
    public float spawnDelay;
    public float jumpTime_Relative;

    public Pause_Script pauseClass;
    public Result_Script resultClass;
    [SerializeField]
    private List<Reward_Data> rewardDataList;
    public int stageClearGold;
    public float battleTime;
    public int killCount;
    public float goldBonus;
    public float foodGetPer;
    public bool[] isTestSpawnAlly;

    public enum BattleState
    {
        None = -1,
        Start,
        Play,
        Result,
        Pause,
    }
    public BattleState battleState
    {
        get
        {
            return m_BattleState;
        }
    }
    private BattleState m_BattleState;

    public BattleType battleType;

    public int battleID;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        int _allyUnitNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
        spawnClassArr_Ally = new BattleSpawn_Script[_allyUnitNum];
        activeSapwnClassList_Ally = new List<BattleSpawn_Script>();
        for (int i = 0; i < _allyUnitNum; i++)
        {
            GameObject _spawnAllyObj = new GameObject();
            _spawnAllyObj.transform.parent = this.transform;
            _spawnAllyObj.name = "SpawnObjAlly_" + i + "_" + DataBase_Manager.Instance.GetUnitName_Func(i);

            Unit_Script _playerUnitClass = DataBase_Manager.Instance.GetUnitClass_Func(i);

            spawnClassArr_Ally[i] = _spawnAllyObj.AddComponent<BattleSpawn_Script>();
            spawnClassArr_Ally[i].Init_Func(this, GroupType.Ally, i, _playerUnitClass);
        }

        int _enemyMonsterNum = DataBase_Manager.Instance.GetMonsterMaxNum_Func();
        spawnClassArr_Enemy = new BattleSpawn_Script[_enemyMonsterNum];
        activeSpawnClassList_Enemy = new List<BattleSpawn_Script>();
        for (int i = 0; i < _enemyMonsterNum; i++)
        {
            GameObject _spawnAllyObj = new GameObject();
            _spawnAllyObj.transform.parent = this.transform;
            _spawnAllyObj.name = "SpawnObjEnemy_" + i + "_" + DataBase_Manager.Instance.GetMonsterName_Func(i);

            Unit_Script _monsterClass = DataBase_Manager.Instance.GetMonsterClass_Func(i);

            spawnClassArr_Enemy[i] = _spawnAllyObj.AddComponent<BattleSpawn_Script>();
            spawnClassArr_Enemy[i].Init_Func(this, GroupType.Enemy, i, _monsterClass);
        }

        isSpawnWaveArr = new bool[3];

        pauseClass.Init_Func();
        resultClass.Init_Func(this);

        yield return skillSystemManager.Init_Cor();

        yield break;
    }

    #region Start State
    public void BattleEnter_Func(BattleType _battleType, int _stageID)
    {
        m_BattleState = BattleState.Start;

        battleType = _battleType;

        battleID = _stageID;
        
        StartCoroutine(BattleEnter_Cor());
    }
    IEnumerator BattleEnter_Cor()
    {
        CheckSpawnMonsterID_Func();
        yield return DirectingStart_Cor();
        BattlePlay_Func();

        yield break;
    }
    void CheckSpawnMonsterID_Func()
    {
        int _monsterNum = DataBase_Manager.Instance.GetMonsterMaxNum_Func();
        bool[] _isMonsterUnlock = new bool[_monsterNum];
        int _monsterSpawnSetNum = 0;
        int[] _spawnMonsterIDArr = new int[5];

        // 현재 스테이지 중 해금된 몬스터ID 기록
        for (int i = 0; i < _monsterNum; i++)
        {
            if(DataBase_Manager.Instance.monsterClassDic[i].unlockLevel <= battleID)
            {
                _isMonsterUnlock[i] = true;
            }
            else
            {
                _isMonsterUnlock[i] = false;
            }
        }

        // 근접 몬스터 랜덤 1종 지정
        int _randMonsterID = Random.Range(0, _monsterNum);
        for (int i = 0; i < _monsterNum; i++)
        {
            if(_isMonsterUnlock[_randMonsterID] == true)
            {
                if(DataBase_Manager.Instance.GetMonsterType_Func(_randMonsterID) == MonsterType.Melee)
                {
                    _isMonsterUnlock[_randMonsterID] = false;

                    _spawnMonsterIDArr[_monsterSpawnSetNum] = _randMonsterID;

                    _monsterSpawnSetNum++;

                    break;
                }
            }

            _randMonsterID++;
            if (_monsterNum <= _randMonsterID)
                _randMonsterID = 0;
        }
        
        // 나머지 스폰 몬스터 4종 다 채우기
        while( _monsterSpawnSetNum < 5)
        {
            // 몬스터 중 랜덤ID 하나 지목
            _randMonsterID = Random.Range(0, _monsterNum);

            // 몬스터 전체를 뒤짐
            bool _isSearchAll = true;
            for (int i = 0; i < _monsterNum; i++)
            {
                if (_isMonsterUnlock[_randMonsterID] == true)
                {
                    _isMonsterUnlock[_randMonsterID] = false;

                    _spawnMonsterIDArr[_monsterSpawnSetNum] = _randMonsterID;

                    _monsterSpawnSetNum++;

                    _isSearchAll = false;

                    break;
                }

                // 반복하며 ID범위를 넘을 경우 처음부터
                _randMonsterID++;
                if (_monsterNum <= _randMonsterID)
                    _randMonsterID = 0;
            }

            // 없을 경우 또는 모든 스폰ID가 준비된 경우 세팅 마무리
            if(_isSearchAll == true || 5 <= _monsterSpawnSetNum)
            {
                for (int i = 0; i < _monsterSpawnSetNum; i++)
                {
                    spawnEnemyIDList.Add(_spawnMonsterIDArr[i]);
                }
                break;
            }
        }
    }
    IEnumerator DirectingStart_Cor()
    {
        // 전투 시작 연출

        battleUITrf.DOSizeDelta(Vector2.zero, 0.5f);

        yield break;
    }
    #endregion
    #region Play State
    void BattlePlay_Func()
    {
        m_BattleState = BattleState.Play;

        playerClass.BattleEnter_Func();

        chickenHouseClass.healthPoint_Max = ((battleID * 0.05f) + 1f) * DataBase_Manager.Instance.chickenHouseHp_Default;
        chickenHouseClass.Init_Func(GroupType.Enemy);
        chickenHouseClass.OnLanding_Func();

        if(battleType == BattleType.Normal)
            chickenHouseClass.unitRend.sprite = chickenHouseSpriteArr[0];
        else if (battleType == BattleType.Special)
            chickenHouseClass.unitRend.sprite = chickenHouseSpriteArr[1];

        OnSpawnAllyUnit_Func();
        OnSpawnEnemyUnit_Func();
        StartCoroutine(OnBattleTimer_Cor());
        killCount = 0;

        skillSystemManager.BattleStart_Func();
    }

    void OnSpawnAllyUnit_Func()
    {
        for (int i = 0; i < 5; i++)
        {
            int _partyUnitId = Player_Data.Instance.partyUnitIdArr[i];
            if (0 <= _partyUnitId)
            {
                activeSapwnClassList_Ally.Add(spawnClassArr_Ally[_partyUnitId]);

                int _activeSpawnAllyID = activeSapwnClassList_Ally.Count - 1;

                activeSapwnClassList_Ally[_activeSpawnAllyID]
                    .ActiveSpawn_Func(battleType, battleID);
            }
        }
    }
    public Unit_Script OnSpawnAllyUnit_Func(int _unitID)
    {
        // 스킬에 의한 호출

        if(spawnClassArr_Ally[_unitID].isActive == false)
        {
            spawnClassArr_Ally[_unitID].ActiveSpawn_Func(battleType, battleID, true);
        }

        return spawnClassArr_Ally[_unitID].OnSpawningAlly_Func(false);
    }

    void OnSpawnEnemyUnit_Func()
    {
        for (int i = 0; i < spawnEnemyIDList.Count; i++)
        {
            int _enemyID = spawnEnemyIDList[i];
            if(0 <= _enemyID)
            {
                activeSpawnClassList_Enemy.Add(spawnClassArr_Enemy[_enemyID]);

                int _activeSpawnEnemyID = activeSpawnClassList_Enemy.Count - 1;

                activeSpawnClassList_Enemy[_activeSpawnEnemyID]
                    .ActiveSpawn_Func(battleType, battleID);
            }
        }
    }

    public void SetMonsterSpawnBonus_Func(int _spawnWaveLevel)
    {
        if(isSpawnWaveArr[_spawnWaveLevel] == false)
        {
            isSpawnWaveArr[_spawnWaveLevel] = true;

            int _bonusValue = 0;
            float _bonusTime = 0f;

            if (battleType == BattleType.Normal)
            {
                _bonusValue = spawnWave_BonusValue_Normal[_spawnWaveLevel];
                _bonusTime = spawnWave_BonusTime_Normal[_spawnWaveLevel];
            }
            else if (battleType == BattleType.Special)
            {
                _bonusValue = spawnWave_BonusValue_Special[_spawnWaveLevel];
                _bonusTime = spawnWave_BonusTime_Special[_spawnWaveLevel];
            }

            for (int i = 0; i < activeSpawnClassList_Enemy.Count; i++)
            {
                activeSpawnClassList_Enemy[i].SetTimerBonus_Func(_bonusValue, _bonusTime);
            }
        }
    }

    IEnumerator OnBattleTimer_Cor()
    {
        battleTime = 0f;

        while (m_BattleState == BattleState.Play)
        {
            battleTime += 0.02f;

            yield return new WaitForFixedUpdate();
        }
    }
    public void CountKillMonster_Func(int _monsterID)
    {
        killCount++;
    }

    public void OnMoveLeft_Func(bool _isDown)
    {
        // Call : Btn Event

        if(m_BattleState == BattleState.Play)
        {
            if(_isDown == true)
            {
                playerClass.MoveLeft_Func();
            }
            else if(_isDown == false)
            {
                playerClass.MoveOver_Func();
            }
        }
    }
    public void OnMoveRight_Func(bool _isDown)
    {
        // Call : Btn Event

        if (m_BattleState == BattleState.Play)
        {
            if (_isDown == true)
            {
                playerClass.MoveRight_Func();
            }
            else if (_isDown == false)
            {
                playerClass.MoveOver_Func();
            }
        }
    }

    public void Pause_Func()
    {
        // Call : Btn Event

        pauseClass.Active_Func();
    }
    public void Resume_Func()
    {
        Time.timeScale = 1f;
    }
    #endregion
    #region Result State
    public void GameClear_Func()
    {
        OnResult_Func(true);
    }
    public void GameOver_Func(bool _isPause)
    {
        OnResult_Func(false);
    }
    void OnResult_Func(bool _isVictory)
    {
        // 결과창 연출

        if(_isVictory == true)
        {
            // 승리한 경우

            // 1. 치킨집 파괴 이미지 교체
            chickenHouseClass.unitRend.sprite = chickenHouseSpriteArr[2];

            // 2. 치킨집 파괴 이펙트 연출

            // 3. 적군 유닛 전원 사망
            for (int i = 0; i < spawnClassArr_Enemy.Length; i++)
            {
                spawnClassArr_Enemy[i].KillUnitAll_Func(false);
            }

            // 4. 플레이어 이동 불가
            playerClass.SetControlOut_Func(true);

            // 5. 아군 이동 불가
            for (int i = 0; i < spawnClassArr_Ally.Length; i++)
            {
                spawnClassArr_Ally[i].StopUnit_Func();
            }

            // 6. 스테이지 데이터 기록
            if (battleType == BattleType.Normal)
            {
                Player_Data.Instance.stageID_Normal = battleID;
            }
            else if (battleType == BattleType.Special)
            {
                Player_Data.Instance.stageID_Special = battleID;
            }
        }
        else if(_isVictory == false)
        {
            // 패배한 경우

            // 1. 플레이어 사망
            playerClass.SetControlOut_Func(true);

            // 2. 아군 사망
            for (int i = 0; i < spawnClassArr_Ally.Length; i++)
            {
                spawnClassArr_Ally[i].KillUnitAll_Func(false);
            }

            // 3. 적군 이동 불가
            for (int i = 0; i < spawnClassArr_Enemy.Length; i++)
            {
                spawnClassArr_Enemy[i].StopUnit_Func();
            }

            // 4. 스테이지 데이터 기록
            if (battleType == BattleType.Normal)
            {

            }
            else if (battleType == BattleType.Special)
            {
                int _penalty = Player_Data.Instance.stageID_Special % 5;

                Player_Data.Instance.stageID_Special -= _penalty;
            }

            // 5. 자연 파괴
            Enviroment_Manager.Instance.NatureReset_Func();
        }

        // 공통 처리

        // 1. 전투상태, 결과
        m_BattleState = BattleState.Result;

        // 2. 보상 계산
        rewardDataList = new List<Reward_Data>();
        GetRewardGold_Func(_isVictory);
        GetRewardMineral_Func(_isVictory);
        GetRewardFood_Func(_isVictory);
        GetRewardUnit_Func(_isVictory);
        GetRewardPopulationPoint_Func(_isVictory);
        GetRewardSkill_Func(_isVictory);
        GetRewardTrophy_Func(_isVictory);
        GetRewardFoodBox_Func(_isVictory);

        // 3. 스폰 비활성화
        for (int i = 0; i < spawnClassArr_Ally.Length; i++)
        {
            spawnClassArr_Ally[i].DeactiveSpawn_Func();
        }

        for (int i = 0; i < spawnClassArr_Enemy.Length; i++)
        {
            spawnClassArr_Enemy[i].DeactiveSpawn_Func();
        }

        // 4. 스킬 시스템 작동 중지
        skillSystemManager.Deactive_Func();

        // 5. UI 출력
        StartCoroutine(ResultUI_Cor(_isVictory));
    }
    IEnumerator ResultUI_Cor(bool _isVictory)
    {
        yield return new WaitForSeconds(1f);

        resultClass.Active_Func(battleType, _isVictory, rewardDataList.ToArray());
        yield break;
    }
    void GetRewardGold_Func(bool _isVictory)
    {
        int _wealthAmount = 0;
        stageClearGold = ((battleID - 1) * 200) + 1000;

        float _calcBonus = stageClearGold / 400f;
        goldBonus += battleTime * _calcBonus;
        goldBonus += killCount * _calcBonus;
        if (stageClearGold < goldBonus)
            goldBonus = stageClearGold;

        if (_isVictory == true)
        {
            _wealthAmount += stageClearGold;
            _wealthAmount += (int)goldBonus;
        }
        else if(_isVictory == false)
        {
            _wealthAmount = (int)(goldBonus / 2f);
        }

        Reward_Data _rewardData = new Reward_Data();
        _rewardData.SetData_Func(RewardType.Wealth, 0, _wealthAmount);

        rewardDataList.Add(_rewardData);
    }
    void GetRewardMineral_Func(bool _isVictory)
    {
        if(battleType == BattleType.Normal)
        {
            if(_isVictory == true)
            {
                Reward_Data _rewardData = new Reward_Data();
                _rewardData.SetData_Func(RewardType.Wealth, 1, 10);

                rewardDataList.Add(_rewardData);
            }
        }
    }
    void GetRewardFood_Func(bool _isVictory)
    {
        float _calcFoodGetPer = 0f;

        if(_isVictory == true)
        {
            _calcFoodGetPer = foodGetPer;
        }
        else if(_isVictory == false)
        {
            _calcFoodGetPer = (goldBonus / stageClearGold) * foodGetPer;
        }

        // 아이템 획득 확률 비교
        if (_calcFoodGetPer < Random.Range(0f, 100f)) return;

        FoodGrade _foodGrade = FoodGrade.None;
        int _randValue = Random.Range(0, 100);
        if(0 <= _randValue && _randValue < 70)
        {
            _foodGrade = FoodGrade.Common;
        }
        else if (70 <= _randValue && _randValue < 95)
        {
            _foodGrade = FoodGrade.Rare;
        }
        else
        {
            _foodGrade = FoodGrade.Legend;
        }

        int _foodNum = DataBase_Manager.Instance.foodDataArr.Length;
        int _randFoodID = Random.Range(0, _foodNum);
        while(true)
        {
            if(_foodGrade == DataBase_Manager.Instance.foodDataArr[_randFoodID].foodGrade)
            {
                break;
            }

            _randFoodID++;
            if (_foodNum <= _randFoodID)
                _randFoodID = 0;
        }

        Reward_Data _rewardData = new Reward_Data();
        _rewardData.SetData_Func(RewardType.Food, _randFoodID, 1);
        rewardDataList.Add(_rewardData);
    }
    void GetRewardUnit_Func(bool _isVictory)
    {
        if (_isVictory == false) return;

        if (battleType == BattleType.Normal)
        {
            int _unitNum = DataBase_Manager.Instance.GetUnitMaxNum_Func();
            int _unlockUnitID = -1;
            for (int i = 0; i < _unitNum; i++)
            {
                int _unlockStageLevel = DataBase_Manager.Instance.GetUnitClass_Func(i).unlockLevel;
                if(battleID == _unlockStageLevel)
                {
                    _unlockUnitID = i;
                    break;
                }
            }
            
            if(0 < _unlockUnitID)
            {
                Reward_Data _rewardData = new Reward_Data();
                _rewardData.SetData_Func(RewardType.Unit, _unlockUnitID, 1);
                rewardDataList.Add(_rewardData);
            }
        }
    }
    void GetRewardPopulationPoint_Func(bool _isVictory)
    {
        if (_isVictory == false) return;

        if(battleType == BattleType.Normal)
        {
            int _calcValue = -1;

            if (battleID <= 1) return;

            if(Player_Data.Instance.populationPoint < 8)
            {
                _calcValue = (battleID - 6) % 5;
            }
            else
            {
                _calcValue = (battleID - 1) % 10;
            }

            if(_calcValue == 0)
            {
                Reward_Data _rewardData = new Reward_Data();
                _rewardData.SetData_Func(RewardType.PopulationPoint, 0, 1);
                rewardDataList.Add(_rewardData);
            }
        }
    }
    void GetRewardSkill_Func(bool _isVictory)
    {
        if (_isVictory == false) return;
    } // Uncomplete
    void GetRewardTrophy_Func(bool _isVictory)
    {
        if (_isVictory == false) return;

        if (battleType == BattleType.Normal)
        {
            int _calcValue = -1;

            _calcValue = (battleID - 9) % 10;

            if (_calcValue == 0)
            {
                int _trophyDataNum = DataBase_Manager.Instance.trophyDataArr.Length;
                int _randTrophyID = Random.Range(0, _trophyDataNum);
                while (true)
                {
                    PlayerTrophy_Data _playerTrophyData = Player_Data.Instance.trophyDataArr[_randTrophyID];
                    if (_playerTrophyData.haveNumLimit == 0)
                    {
                        break;
                    }
                    else if (_playerTrophyData.haveNum < _playerTrophyData.haveNumLimit)
                    {
                        break;
                    }

                    _randTrophyID++;
                    if (_trophyDataNum <= _randTrophyID)
                        _randTrophyID = 0;
                }

                Reward_Data _rewardData = new Reward_Data();
                _rewardData.SetData_Func(RewardType.Trophy, _randTrophyID, 1);
                rewardDataList.Add(_rewardData);
            }
        }
    }
    void GetRewardFoodBox_Func(bool _isVictory)
    {
        if (_isVictory == false) return;

        if (battleType == BattleType.Special)
        {
            int _calcValue = -1;

            _calcValue = (battleID - 4) % 10;

            if (_calcValue == 0)
            {
                Reward_Data _rewardData = new Reward_Data();
                _rewardData.SetData_Func(RewardType.FoodBoxLevel, 0, 1);
                rewardDataList.Add(_rewardData);
            }
        }
    }
    public int GetGoldByWatchedAD_Func()
    {
        int _adValue = rewardDataList[0].rewardAmount * 2;
        rewardDataList[0].SetRewardAmount_Func(_adValue);
        
        // 광고 연출

        return _adValue;
    }

    public void NextStage_Func()
    {
        ClearBattleData_Func();

        Game_Manager.Instance.BattleEnter_Func(battleType);
    }
    public void Retry_Func()
    {
        ClearBattleData_Func();

        Game_Manager.Instance.BattleEnter_Func(battleType, battleID);
    }
    public void ExitBattle_Func()
    {
        // Reward Get

        ClearBattleData_Func();

        Game_Manager.Instance.LobbyEnter_Func();

        battleUITrf.DOSizeDelta(new Vector2(0f, 600f), 1f);
    }
    
    void ClearBattleData_Func()
    {
        for (int i = 0; i < 5; i++)
        {
            spawnClassArr_Ally[i].KillUnitAll_Func(true);
        }
        for (int i = 0; i < spawnClassArr_Enemy.Length; i++)
        {
            spawnClassArr_Enemy[i].KillUnitAll_Func(true);
        }

        Enviroment_Manager.Instance.NatureReset_Func();

        SetRewardOnPlayer_Func();
        
        goldBonus = 0f;
    }
    void SetRewardOnPlayer_Func()
    {
        for (int i = 0; i < rewardDataList.Count; i++)
        {
            int _rewardID = rewardDataList[i].rewardID;
            int _rewardAmount = rewardDataList[i].rewardAmount;

            switch (rewardDataList[i].rewardType)
            {
                case RewardType.Wealth:
                    SetRewardOnPlayerWealth_Func(_rewardID, _rewardAmount);
                    break;

                case RewardType.Food:
                    SetRewardOnPlayerFood_Func(_rewardID, _rewardAmount);
                    break;

                case RewardType.FoodBoxLevel:
                    SetRewardOnPlayerFoodBox_Func(_rewardID, _rewardAmount);
                    break;

                case RewardType.Unit:
                    SetRewardOnPlayerUnit_Func(_rewardID, _rewardAmount);
                    break;

                case RewardType.PopulationPoint:
                    SetRewardOnPlayerPopulationPoint_Func(_rewardID, _rewardAmount);
                    break;

                case RewardType.Skill:
                    SetRewardOnPlayerSkill_Func(_rewardID, _rewardAmount);
                    break;

                case RewardType.Trophy:
                    SetRewardOnPlayerTrophy_Func(_rewardID, _rewardAmount);
                    break;
            }
        }
    }
    void SetRewardOnPlayerWealth_Func(int _rewardID, int _rewardAmount)
    {
        Player_Data.Instance.AddWealth_Func((WealthType)_rewardID, _rewardAmount);
    }
    void SetRewardOnPlayerFood_Func(int _rewardID, int _rewardAmount)
    {
        Player_Data.Instance.AddFood_Func(_rewardID);
    }
    void SetRewardOnPlayerFoodBox_Func(int _rewardID, int _rewardAmount)
    {
        Player_Data.Instance.AddFoodBoxLevel_Func();
    }
    void SetRewardOnPlayerUnit_Func(int _rewardID, int _rewardAmount)
    {
        Player_Data.Instance.UnlockUnit_Func(_rewardID);
    }
    void SetRewardOnPlayerPopulationPoint_Func(int _rewardID, int _rewardAmount)
    {
        Player_Data.Instance.AddPopulationPoint_Func();
    }
    void SetRewardOnPlayerSkill_Func(int _rewardID, int _rewardAmount)
    {
        Player_Data.Instance.UnlockSkill_Func(_rewardID);
    }
    void SetRewardOnPlayerTrophy_Func(int _rewardID, int _rewardAmount)
    {
        Player_Data.Instance.AddTrophy_Func(_rewardID);
    }
    #endregion
}