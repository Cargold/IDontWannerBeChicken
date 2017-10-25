using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Manager : MonoBehaviour
{
    public static Battle_Manager Instance;

    public Player_Script playerClass;
    public Transform spawnPos_Enemy;
    public Unit_Script[] enemyUnitClassArr;
    public RectTransform battleUITrf;
    public BattleSpawn_Script[] spawnClassArr_Ally;
    public BattleSpawn_Script[] spawnClassArr_Enemy;

    public ArrayList spawnUnitList_Ally = new ArrayList();
    public ArrayList spawnUnitList_Enemy = new ArrayList();

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
            spawnClassArr_Ally[i].Init_Func(this, GroupType.Ally);
        }

        yield break;
    }

    #region Start State
    public void BattleEnter_Func(BattleType _battleType)
    {
        m_BattleState = BattleState.Start;

        battleType = _battleType;

        StartCoroutine(BattleEnter_Cor());
    }
    IEnumerator BattleEnter_Cor()
    {
        yield return DirectingStart_Cor();
        BattleStart_Func();

        yield break;
    }
    IEnumerator DirectingStart_Cor()
    {
        // 전투 시작 연출

        battleUITrf.sizeDelta = Vector2.zero;
        battleUITrf.anchoredPosition = Vector2.zero;

        yield break;
    }
    #endregion
    #region Play State
    void BattleStart_Func()
    {
        m_BattleState = BattleState.Play;

        playerClass.BattleEnter_Func();

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
    #endregion
    #region Result State
    public void GameClear_Func()
    {
        m_BattleState = BattleState.Result;

        OnResult_Func();
    }

    public void GameOver_Func()
    {
        m_BattleState = BattleState.Result;

        OnResult_Func();
    }

    void OnResult_Func()
    {
        battleUITrf.sizeDelta = new Vector2(0f, 300f);
        battleUITrf.anchoredPosition = new Vector2(0f, -150f);
    }
    #endregion
    #region Test Group
    public bool isTest = false;
    void Update()
    {
        if (isTest == false) return;
        isTest = false;
        GameObject _charObj = ObjectPoolManager.Instance.Get_Func("Goblin");

        Vector3 _spawnPos = new Vector3(spawnPos_Enemy.position.x + Random.Range(-0.5f, 0.5f), 0f, Random.Range(-1f, 1f));

        _charObj.transform.position = _spawnPos;
        _charObj.transform.localScale = Vector3.one;

        Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
        _spawnUnitClass.Init_Func(GroupType.Enemy);
    }
    #endregion
}
