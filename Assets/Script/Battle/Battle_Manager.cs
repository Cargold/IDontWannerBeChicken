using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnData
{
    public int enemyID;
    public int enemyLevel;
}

public class Battle_Manager : MonoBehaviour
{
    public static Battle_Manager Instance;

    public Player_Script playerClass;
    public RectTransform battleUITrf;
    public BattleSpawn_Script[] spawnClassArr_Ally;
    public Transform spawnPos_Ally;
    public Transform spawnTrf_Ally;
    public BattleSpawn_Script[] spawnClassArr_Enemy;
    public Transform spawnTrf_Enemy;
    public Transform spawnPos_Enemy;
    [SerializeField]
    public EnemySpawnData[] enemySpawnDataArr;
    public ChickenHouse_Script chickenHouseClass;

    public float spawnPosX_Range;
    public float spawnPosY_Min;
    public float spawnPosY_Max;
    public float spawnJumpPower_Min;
    public float spawnJumpPower_Max;
    public float spawnDelay;
    public float jumpTime_Relative;

    public ArrayList spawnUnitList_Ally = new ArrayList();
    public ArrayList spawnUnitList_Enemy = new ArrayList();

    public Pause_Script pauseClass;
    public Result_Script resultClass;

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

    private int stageID;
    public RewardData[] rewardDataArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        spawnClassArr_Ally = new BattleSpawn_Script[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject _spawnAllyObj = new GameObject();
            _spawnAllyObj.transform.parent = this.transform;
            _spawnAllyObj.name = "SpawnObjAlly_" + i;

            spawnClassArr_Ally[i] = _spawnAllyObj.AddComponent<BattleSpawn_Script>();
            spawnClassArr_Ally[i].Init_Func(this, GroupType.Ally, i);
        }

        spawnClassArr_Enemy = new BattleSpawn_Script[10];
        for (int i = 0; i < 10; i++)
        {
            GameObject _spawnAllyObj = new GameObject();
            _spawnAllyObj.transform.parent = this.transform;
            _spawnAllyObj.name = "SpawnObjEnemy_" + i;

            spawnClassArr_Enemy[i] = _spawnAllyObj.AddComponent<BattleSpawn_Script>();
            spawnClassArr_Enemy[i].Init_Func(this, GroupType.Enemy, i);
        }

        pauseClass.Init_Func();
        resultClass.Init_Func(this);

        yield break;
    }

    #region Start State
    public void BattleEnter_Func(BattleType _battleType, int _stageID)
    {
        m_BattleState = BattleState.Start;

        battleType = _battleType;

        stageID = _stageID;

        CalcRewardData_Func();

        StartCoroutine(BattleEnter_Cor());
    }
    IEnumerator BattleEnter_Cor()
    {
        yield return DirectingStart_Cor();
        BattlePlay_Func();

        yield break;
    }
    IEnumerator DirectingStart_Cor()
    {
        // 전투 시작 연출

        battleUITrf.sizeDelta = Vector2.zero;

        yield break;
    }
    #endregion
    #region Play State
    void BattlePlay_Func()
    {
        m_BattleState = BattleState.Play;

        playerClass.BattleEnter_Func();

        chickenHouseClass.Init_Func(GroupType.Enemy);

        OnSpawnAllyUnit_Func();
        OnSpawnEnemyUnit_Func();
    }
    void OnSpawnAllyUnit_Func()
    {
        for (int i = 0; i < 5; i++)
        {
            int _partyUnitId = Player_Data.Instance.partyUnitIdArr[i];
            if (0 <= _partyUnitId)
            {
                PlayerUnit_ClassData _playerUnitData = Player_Data.Instance.playerUnitDataArr[_partyUnitId];
                spawnClassArr_Ally[i].ActiveSpawn_Func(_playerUnitData.unitClass);
            }
        }
    }
    void OnSpawnEnemyUnit_Func()
    {
        for (int i = 0; i < enemySpawnDataArr.Length; i++)
        {
            Unit_Script _monsterClass = DataBase_Manager.Instance.GetMonsterClass_Func(enemySpawnDataArr[i].enemyID);
            spawnClassArr_Enemy[i].ActiveSpawn_Func(_monsterClass);
        }
    }

    public void OnMoveLeft_Func(bool _isDown)
    {
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

    public void SetSpawnAllyUnit_Func(Unit_Script _unitClass)
    {
        spawnUnitList_Ally.Add(_unitClass);
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
        
        m_BattleState = BattleState.Result;
        
        resultClass.Active_Func(battleType, _isVictory, rewardDataArr);

        if(battleType == BattleType.Normal)
        {
            Player_Data.Instance.stageID_Normal = stageID;
        }
        else if(battleType == BattleType.Special)
        {
            Player_Data.Instance.stageID_Special = stageID;
        }
    }

    public void NextStage_Func()
    {
        StartCoroutine(NextStage_Cor());
    }
    IEnumerator NextStage_Cor()
    {
        yield return ClearBattleData_Cor();

        Game_Manager.Instance.BattleEnter_Func(battleType);
    }

    public void Retry_Func()
    {
        StartCoroutine(Retry_Cor());
    }
    IEnumerator Retry_Cor()
    {
        yield return ClearBattleData_Cor();

        Game_Manager.Instance.BattleEnter_Func(battleType, stageID);
    }

    public void ExitBattle_Func()
    {
        // Reward Get

        StartCoroutine(ExitBattle_Cor());
    }
    IEnumerator ExitBattle_Cor()
    {
        yield return ClearBattleData_Cor();

        Game_Manager.Instance.LobbyEnter_Func();

        battleUITrf.sizeDelta = new Vector2(0f, 600f);
    }

    public void WatchAD_Func()
    {
        rewardDataArr[0].rewardAmount *= 2;
    }

    void CalcRewardData_Func()
    {
        rewardDataArr = new RewardData[3];

        rewardDataArr[0].SetData_Func(RewardType.Wealth, 0, 1000 * stageID);
        rewardDataArr[1].SetData_Func(RewardType.Wealth, 1, 10);
        rewardDataArr[2].SetData_Func(RewardType.Food, 0, 0);
    }
    #endregion

    IEnumerator ClearBattleData_Cor()
    {
        Game_Manager.Instance.Loading_Func();

        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(spawnClassArr_Ally[i].DeactiveSpawn_Cor());
        }

        for (int i = 0; i < spawnClassArr_Enemy.Length; i++)
        {
            if (spawnClassArr_Enemy[i].isActive == true)
                StartCoroutine(spawnClassArr_Enemy[i].DeactiveSpawn_Cor());
            else
                break;
        }

        Enviroment_Manager.Instance.NatureReset_Func();

        GetRewardData_Func();

        yield return new WaitForSeconds(0.5f);

        Game_Manager.Instance.LoadingClear_Func();
    }
    void GetRewardData_Func()
    {
        for (int i = 0; i < rewardDataArr.Length; i++)
        {
            int _rewardID = rewardDataArr[i].rewardID;
            int _rewardAmount = rewardDataArr[i].rewardAmount;

            switch (rewardDataArr[i].rewardType)
            {
                case RewardType.Wealth:
                    Player_Data.Instance.AddWealth_Func((WealthType)_rewardID, _rewardAmount);
                    break;

                case RewardType.Food:
                    //Food_Script _foodClass = new Food_Script();
                    //Food_Data _foodData = DataBase_Manager.Instance.foodDataArr[_rewardID];
                    ////_foodClass.SetData_Func(_foodData);
                    //_foodClass.level = Player_Data.Instance.playerBoxLevel;
                    Player_Data.Instance.AddFood_Func(_rewardID);
                    break;

                case RewardType.Unit:
                    break;
                case RewardType.PopulationPoint:
                    break;
                case RewardType.Skill:
                    break;
            }
        }
    }
}

public struct RewardData
{
    public RewardType rewardType;
    public int rewardID;
    public int rewardAmount;

    public void SetData_Func(RewardType _rewardType, int _rewardID, int _rewardAmount)
    {
        rewardType = _rewardType;
        rewardID = _rewardID;
        rewardAmount = _rewardAmount;
    }
}