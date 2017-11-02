using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawn_Script : MonoBehaviour
{
    public Battle_Manager battleManagerClass;
    public GroupType spawnGroupType;
    [SerializeField]
    private ArrayList spawnUnitList = new ArrayList();
    [SerializeField]
    private bool isActive = false;
    [SerializeField]
    private Unit_Script unitClass;

    public void Init_Func(Battle_Manager _battleManagerClass, GroupType _groupType)
    {
        battleManagerClass = _battleManagerClass;

        spawnGroupType = _groupType;
    }

    public void ActiveSpawn_Func(Unit_Script _unitClass)
    {
        isActive = true;

        unitClass = _unitClass;

        if (spawnGroupType == GroupType.Enemy)
            OnSpawning_Func();

        StartCoroutine("CheckSpawnTimer_Cor");
    }

    IEnumerator CheckSpawnTimer_Cor()
    {
        float _spawnCalcTime = 0f;

        while(isActive == true)
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
                    OnSpawning_Func();
                }
                yield return null;
            }
        }

        yield break;
    }
    void OnSpawning_Func()
    {
        GameObject _charObj = ObjectPool_Manager.Instance.Get_Func(unitClass.charName);

        Vector3 _spawnPos = Vector3.zero;
        if (spawnGroupType == GroupType.Ally)
        {
            _spawnPos = new Vector3(Battle_Manager.Instance.spawnPos_Ally.position.x + Random.Range(-1.5f, 1.5f), 0f, Random.Range(-1f, 1f));
            _charObj.transform.SetParent(Battle_Manager.Instance.spawnTrf_Ally);

            _charObj.transform.localScale = Vector3.one;
        }
        else if(spawnGroupType == GroupType.Enemy)
        {
            _spawnPos = new Vector3(Battle_Manager.Instance.spawnPos_Enemy.position.x + Random.Range(-1.5f, 1.5f), 0f, Random.Range(-1f, 1f));
            _charObj.transform.SetParent(Battle_Manager.Instance.spawnTrf_Enemy);

            _charObj.transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        _charObj.transform.position = _spawnPos;
        _charObj.transform.localEulerAngles = Vector3.zero;

        Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
        _spawnUnitClass.Init_Func(spawnGroupType);
        _spawnUnitClass.SetDataByPlayerUnit_Func(unitClass);
        _spawnUnitClass.moveSpeed *= Random.Range(0.95f, 1.05f);

        spawnUnitList.Add(_spawnUnitClass);
    }
    public void DeactiveSpawn_Func()
    {
        isActive = false;

        unitClass = null;

        StopCoroutine("CheckSpawnTimer_Cor");
    }
    public Unit_Script[] GetUnitClassArr_Func()
    {
        return spawnUnitList.ToArray() as Unit_Script[];
    }
}
