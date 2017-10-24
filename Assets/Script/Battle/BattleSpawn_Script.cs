using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawn_Script : MonoBehaviour
{
    public Battle_Manager battleManagerClass;
    public GroupType spawnGroupType;
    public ArrayList spawnUnitList = new ArrayList();
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
        GameObject _charObj = ObjectPoolManager.Instance.Get_Func(unitClass.charName);

        Vector3 _spawnPos = Vector3.zero;
        if (spawnGroupType == GroupType.Ally)
            _spawnPos = new Vector3(Player_Script.Instance.spawnPos.position.x + Random.Range(-0.5f, 0.5f), 0f, Random.Range(-1f, 1f));
        else if(spawnGroupType == GroupType.Enemy)
            _spawnPos = new Vector3(Battle_Manager.Instance.spawnPos_Enemy.position.x + Random.Range(-0.5f, 0.5f), 0f, Random.Range(-1f, 1f));

        _charObj.transform.position = _spawnPos;
        _charObj.transform.localScale = Vector3.one;

        Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
        _spawnUnitClass.Init_Func(spawnGroupType);
        _spawnUnitClass.SetDataByPlayerUnit_Func(unitClass);

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
