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
    private ArrayList spawnUnitList = new ArrayList();
    public bool isActive = false;
    [SerializeField]
    private Unit_Script unitClass;
    public int spawnID;
    public int spawnCheckCount;
    public int spawnNumLimit;
    
    public void Init_Func(Battle_Manager _battleManagerClass, GroupType _groupType,  int _spawnID)
    {
        battleManagerClass = _battleManagerClass;

        spawnGroupType = _groupType;

        spawnID = _spawnID;
    }

    public void ActiveSpawn_Func(Unit_Script _unitClass, BattleType _battleType, int _battleID)
    {
        isActive = true;

        unitClass = _unitClass;

        battleType = _battleType;

        battleID = _battleID;

        spawnCheckCount = 0;

        spawnNumLimit = _unitClass.spawnNum_Limit;
            
        StartCoroutine("CheckSpawnTimer_Cor");
    }
    IEnumerator CheckSpawnTimer_Cor()
    {
        float _spawnCalcTime = 0f;

        if(spawnGroupType == GroupType.Ally)
        {
            if (battleManagerClass.isTestSpawnAlly[spawnID] == true)
            {
                OnSpawningAlly_Func();
            }
        }
        if (spawnGroupType == GroupType.Enemy)
        {
            _spawnCalcTime = unitClass.spawnInterval;
        }
        
        while (isActive == true)
        {
            if(_spawnCalcTime < unitClass.spawnInterval)
            {
                _spawnCalcTime += 0.02f;
                yield return new WaitForFixedUpdate();
            }
            else
            {
                _spawnCalcTime = 0f;

                for (int i = 0; i < unitClass.spawnNum; i++)
                {
                    if (spawnGroupType == GroupType.Ally)
                        OnSpawningAlly_Func();
                    else if (spawnGroupType == GroupType.Enemy)
                        OnSpawningEnemy_Func();

                    yield return new WaitForSeconds(battleManagerClass.spawnDelay);
                }

                while(spawnNumLimit <= spawnUnitList.Count)
                {
                    yield return null;
                }

                yield return null;
            }
        }

        yield break;
    }
    void OnSpawningAlly_Func()
    {
        GameObject _charObj = ObjectPool_Manager.Instance.Get_Func(unitClass.charName);

        _charObj.transform.SetParent(Battle_Manager.Instance.spawnTrf_Ally);
        _charObj.transform.localScale = Vector3.one;
        _charObj.transform.localEulerAngles = Vector3.zero;

        Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
        _spawnUnitClass.Init_Func(spawnGroupType);
        _spawnUnitClass.SetDataByPlayerUnit_Func(unitClass);
        _spawnUnitClass.moveSpeed *= Random.Range(0.95f, 1.05f);

        float spawnPosX_Calc = Random.Range(-battleManagerClass.spawnPosX_Range, battleManagerClass.spawnPosX_Range);
        float spawnPosY_Calc = Random.Range(-battleManagerClass.spawnPosY_Min, battleManagerClass.spawnPosY_Max);
        float spawnJumpPower_Calc = Random.Range(battleManagerClass.spawnJumpPower_Min, battleManagerClass.spawnJumpPower_Max);
        float spawnJumpTime_Calc = spawnJumpPower_Calc * battleManagerClass.jumpTime_Relative;

        _spawnUnitClass.transform.position = battleManagerClass.spawnPos_Ally.position;
        Vector3 _landingPos =
            new Vector3
            (
                _spawnUnitClass.transform.position.x + spawnPosX_Calc - 3f,
                _spawnUnitClass.transform.position.y + spawnPosY_Calc - 3f,
                0f
            );
        _spawnUnitClass.transform.DOLocalJump(_landingPos, spawnJumpPower_Calc, 1, spawnJumpTime_Calc)
            .OnComplete(_spawnUnitClass.OnLanding_Func);


        StartCoroutine(SpawnUnitRotate_Cor(_spawnUnitClass.transform, spawnJumpTime_Calc));

        _spawnUnitClass.transform.localScale = Vector3.zero;
        _spawnUnitClass.transform.DOScale(1f, spawnJumpTime_Calc);

        spawnUnitList.Add(_spawnUnitClass);
        spawnCheckCount++;
    }
    void OnSpawningEnemy_Func()
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

        _spawnUnitClass.OnLanding_Func();

        spawnUnitList.Add(_spawnUnitClass);
        spawnCheckCount++;
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
    public IEnumerator StopUnit_Cor()
    {
        for (int i = 0; i < spawnUnitList.Count; i++)
        {
            ((Unit_Script)spawnUnitList[i]).moveSpeed = 0f;
            yield return null;
        }
    }
    public void UnitDie_Func(Unit_Script _unitClass)
    {
        spawnUnitList.Remove(_unitClass);
    }
    public void DeactiveSpawn_Func()
    {
        isActive = false;

        unitClass = null;

        StopCoroutine("CheckSpawnTimer_Cor");
    }
    public IEnumerator KillUnitAll_Cor(bool _isUneffect)
    {
        while(0 < spawnUnitList.Count)
        {
            ((Unit_Script)spawnUnitList[0]).Die_Func(_isUneffect);
            spawnUnitList.RemoveAt(0);
            yield return null;
        }
    }

    public Unit_Script[] GetUnitClassArr_Func()
    {
        return spawnUnitList.ToArray() as Unit_Script[];
    }
}