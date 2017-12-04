using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleSpawn_Script : MonoBehaviour
{
    public Battle_Manager battleManagerClass;
    public GroupType spawnGroupType;
    public BattleType battleType;
    public int battleID;
    [SerializeField]
    public ArrayList spawnUnitList = new ArrayList();
    public bool isActive = false;
    [SerializeField]
    private Unit_Script unitClass;
    public int spawnID;
    public int spawnCheckAllCount;
    public int spawnActiveUnitCount;
    public int spawnNumLimit;
    public float spawnTimerBonus;

    public bool isDrinkHpOn = false;
    
    public void Init_Func(Battle_Manager _battleManagerClass, GroupType _groupType,  int _spawnID, Unit_Script _unitClass)
    {
        battleManagerClass = _battleManagerClass;

        spawnGroupType = _groupType;

        spawnID = _spawnID;

        unitClass = _unitClass;

        spawnNumLimit = _unitClass.spawnNum_Limit;
    }

    #region Spawn Group
    public void ActiveSpawn_Func(BattleType _battleType, int _battleID, bool _isDrinkHpOn = false, bool _isTemporarySpawn = false)
    {
        isActive = true;
        
        battleType = _battleType;

        battleID = _battleID;

        spawnCheckAllCount = 0;

        spawnTimerBonus = 1f;

        isDrinkHpOn = _isDrinkHpOn;

        if (spawnGroupType == GroupType.Enemy)
        {
            int _monsterID = unitClass.unitID;

            float _hpValue = DataBase_Manager.Instance.monsterDataArr[_monsterID].healthPoint;
            unitClass.healthPoint_Max = _hpValue * (((_battleID - 1) * 0.05f) + 1f);

            float _attackValue = DataBase_Manager.Instance.monsterDataArr[_monsterID].attackValue;
            unitClass.attackValue = _attackValue * (((_battleID - 1) * 0.05f) + 1f);
        }
        
        if(_isTemporarySpawn == false)
            StartCoroutine("CheckSpawnTimer_Cor");
    }
    private IEnumerator CheckSpawnTimer_Cor()
    {
        float _spawnCalcTime = 0f;

        if(spawnGroupType == GroupType.Ally)
        {
            for (int i = 0; i < 5; i++)
            {
                if (battleManagerClass.isTestSpawnAlly[i] == true)
                {
                    // 테스트를 위해 시작부터 아군 유닛이 나오길 원하는 경우

                    _spawnCalcTime = unitClass.spawnInterval;
                }
            }
        }
        else if (spawnGroupType == GroupType.Enemy)
        {
            // 적은 시작부터 등장
            
            _spawnCalcTime = unitClass.spawnInterval;
        }
        
        while (isActive == true)
        {
            while (spawnNumLimit <= spawnActiveUnitCount)
            {
                // 활성화 유닛 최대치면은 잠시 대기

                yield return null;
            }

            if (_spawnCalcTime < unitClass.spawnInterval)
            {
                _spawnCalcTime += (spawnTimerBonus * 0.02f);
                yield return new WaitForFixedUpdate();
            }
            else
            {
                _spawnCalcTime -= unitClass.spawnInterval;

                for (int i = 0; i < unitClass.spawnNum; i++)
                {
                    if (spawnGroupType == GroupType.Ally)
                    {
                        OnSpawningAlly_Func();
                    }
                    else if (spawnGroupType == GroupType.Enemy)
                    {
                        OnSpawningEnemy_Func();
                    }

                    if (spawnNumLimit <= spawnActiveUnitCount)
                    {
                        break;
                    }
                    else
                    {
                        yield return new WaitForSeconds(battleManagerClass.spawnDelay);
                    }
                }
                
                yield return null;
            }
        }

        yield break;
    }
    public void SetTimerBonus_Func(int _bonusValue, float _time)
    {
        StartCoroutine(CheckBonus_Cor(_bonusValue, _time));
    }
    private IEnumerator CheckBonus_Cor(int _bonusValue, float _time)
    {
        spawnTimerBonus += _bonusValue;
        yield return new WaitForSeconds(_time);
        spawnTimerBonus -= _bonusValue;
    }

    public Unit_Script OnSpawningAlly_Func(bool _isDefaultDirection = true)
    {
        GameObject _charObj = ObjectPool_Manager.Instance.Get_Func(unitClass.charName);

        _charObj.transform.SetParent(Battle_Manager.Instance.spawnTrf_Ally);
        _charObj.transform.localScale = Vector3.one;
        _charObj.transform.localEulerAngles = Vector3.zero;

        Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
        _spawnUnitClass.Init_Func(spawnGroupType);
        _spawnUnitClass.SetDataByPlayerUnit_Func(unitClass);
        if(isDrinkHpOn == true)
            _spawnUnitClass.SetDrinkBonus_Func(DrinkType.Health, true);
        _spawnUnitClass.SetSpawner_Func(this);
        _spawnUnitClass.moveSpeed *= Random.Range(0.95f, 1.05f);
        spawnUnitList.Add(_spawnUnitClass);
        spawnCheckAllCount++;
        
        if (_isDefaultDirection == true)
        {
            spawnActiveUnitCount++;
            OnSpawnAllyDirection_Func(_spawnUnitClass);
        }

        return _spawnUnitClass;
    }
    private void OnSpawnAllyDirection_Func(Unit_Script _spawnUnitClass)
    {
        Transform _spawnUnitTrf = _spawnUnitClass.transform;

        float spawnPosX_Calc = Random.Range(-battleManagerClass.spawnPosX_Range, battleManagerClass.spawnPosX_Range);
        float spawnPosY_Calc = Random.Range(-battleManagerClass.spawnPosY_Min, battleManagerClass.spawnPosY_Max);
        float spawnJumpPower_Calc = Random.Range(battleManagerClass.spawnJumpPower_Min, battleManagerClass.spawnJumpPower_Max);
        float spawnJumpTime_Calc = spawnJumpPower_Calc * battleManagerClass.jumpTime_Relative;

        _spawnUnitTrf.position = battleManagerClass.spawnPos_Ally.position;
        Vector3 _landingPos =
            new Vector3
            (
                _spawnUnitTrf.position.x + spawnPosX_Calc - 3f,
                _spawnUnitTrf.position.y + spawnPosY_Calc - 3f,
                0f
            );
        _spawnUnitTrf.DOLocalJump(_landingPos, spawnJumpPower_Calc, 1, spawnJumpTime_Calc)
            .OnComplete(_spawnUnitClass.OnLanding_Func);
        
        StartCoroutine(SpawnUnitRotate_Cor(_spawnUnitTrf, spawnJumpTime_Calc));

        _spawnUnitTrf.localScale = Vector3.zero;
        _spawnUnitTrf.DOScale(1f, spawnJumpTime_Calc);
    }
    private IEnumerator SpawnUnitRotate_Cor(Transform _targetTrf, float _spawnJumpTime_Calc)
    {
        _spawnJumpTime_Calc *= 0.8f;

        float _calcTime = 0f;
        float _rotateValue = 720f / _spawnJumpTime_Calc;
        while (_calcTime <= _spawnJumpTime_Calc)
        {
            _targetTrf.localEulerAngles = Vector3.forward * _rotateValue * _calcTime;

            _calcTime += 0.02f;
            yield return new WaitForFixedUpdate();
        }

        _targetTrf.localEulerAngles = Vector3.zero;
    }

    private void OnSpawningEnemy_Func()
    {
        MutantType _mutantType = CheckMutant_Func();

        GameObject _charObj = ObjectPool_Manager.Instance.Get_Func(unitClass.charName);

        _charObj.transform.SetParent(Battle_Manager.Instance.spawnTrf_Enemy);
        _charObj.transform.localScale = new Vector3(-1f, 1f, 1f);
        _charObj.transform.localEulerAngles = Vector3.zero;

        Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
        _spawnUnitClass.Init_Func(spawnGroupType);
        _spawnUnitClass.SetDataByPlayerUnit_Func(unitClass);
        _spawnUnitClass.SetMutant_Func(_mutantType);
        _spawnUnitClass.SetSpawner_Func(this);
        _spawnUnitClass.moveSpeed *= Random.Range(0.95f, 1.05f);

        float spawnPosX_Calc = Random.Range(-battleManagerClass.spawnPosX_Range, battleManagerClass.spawnPosX_Range);
        float spawnPosY_Calc = Random.Range(-battleManagerClass.spawnPosY_Min, battleManagerClass.spawnPosY_Max);
        _spawnUnitClass.transform.position =
            new Vector3
            (
                battleManagerClass.spawnPos_Enemy.position.x + spawnPosX_Calc,
                battleManagerClass.spawnPos_Enemy.position.y + spawnPosY_Calc,
                0f
            );

        spawnUnitList.Add(_spawnUnitClass);
        spawnCheckAllCount++;
        spawnActiveUnitCount++;

        _spawnUnitClass.OnLanding_Func();
    }
    private MutantType CheckMutant_Func()
    {
        MutantType _mutantType = MutantType.None;
        int _mutantPer = 0;

        if (battleType == BattleType.Normal)
        {
            if (20 < battleID)
            {
                _mutantPer = battleID % 5;
                if (_mutantPer == 0)
                    _mutantPer = 4;
                else
                    _mutantPer = 2;
            }
        }
        else if (battleType == BattleType.Special)
        {
            _mutantPer = battleID % 5;
            if (_mutantPer == 0)
                _mutantPer = 10;
            else
            {
                _mutantPer *= 2;
            }
        }

        if (0 < _mutantPer)
        {
            if (Random.Range(0, 100) < _mutantPer)
            {
                _mutantType = (MutantType)Random.Range(0, 2);
            }
        }

        return _mutantType;
    }
    #endregion

    public void StopUnit_Func()
    {
        StartCoroutine(StopUnit_Cor());
    }
    private IEnumerator StopUnit_Cor()
    {
        for (int i = 0; i < spawnUnitList.Count; i++)
        {
            ((Unit_Script)spawnUnitList[i]).moveSpeed = 0f;

            yield return null;
        }
    }

    public void UnitDie_Func(Unit_Script _unitClass)
    {
        spawnActiveUnitCount--;
        spawnUnitList.Remove(_unitClass);
    }
    public void KillUnitAll_Func(bool _isUneffect)
    {
        StartCoroutine(KillUnitAll_Cor(_isUneffect));
    }
    private IEnumerator KillUnitAll_Cor(bool _isUneffect)
    {
        while(0 < spawnUnitList.Count)
        {
            Unit_Script _unitClass = (Unit_Script)spawnUnitList[0];
            spawnUnitList.RemoveAt(0);
            _unitClass.Die_Func(_isUneffect);

            yield return null;
        }
    }

    public void DeactiveSpawn_Func()
    {
        isActive = false;

        StopCoroutine("CheckSpawnTimer_Cor");
    }

    public Unit_Script[] GetUnitClassArr_Func()
    {
        return spawnUnitList.ToArray() as Unit_Script[];
    }
}