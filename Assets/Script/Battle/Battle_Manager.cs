using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Manager : MonoBehaviour
{
    public static Battle_Manager Instance;

    public Player_Script playerClass;

    public Transform spawnPos;

    public Unit_Script[] enemyUnitClassArr;

    public IEnumerator Init_Cor()
    {
        Instance = this;

        yield break;
    }

    public void BattleEnter_Func()
    {
        playerClass.BattleEnter_Func();
        InitEnemy_Func();
    }

    void InitEnemy_Func()
    {
        for (int i = 0; i < enemyUnitClassArr.Length; i++)
        {
            StartCoroutine(CheckSpawn_Cor(enemyUnitClassArr[i]));
        }
    }

    IEnumerator CheckSpawn_Cor(Unit_Script _unitClass)
    {
        while (true)
        {
            yield return new WaitForSeconds(_unitClass.spawnInterval);

            for (int i = 0; i < _unitClass.spawnNum; i++)
            {
                GameObject _charObj = ObjectPoolManager.Instance.Get_Func(_unitClass.charType.ToString());

                Vector3 _spawnPos = new Vector3(spawnPos.position.x + Random.Range(-0.5f, 0.5f), 0f, Random.Range(-1f, 1f));

                _charObj.transform.position = _spawnPos;
                _charObj.transform.localScale = Vector3.one;

                Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
                _spawnUnitClass.Init_Func(GroupType.Enemy);
            }
        }
    }

    public bool isTest = false;
    void Update()
    {
        if (isTest == false) return;
        isTest = false;
        GameObject _charObj = ObjectPoolManager.Instance.Get_Func("Goblin");

        Vector3 _spawnPos = new Vector3(spawnPos.position.x + Random.Range(-0.5f, 0.5f), 0f, Random.Range(-1f, 1f));

        _charObj.transform.position = _spawnPos;
        _charObj.transform.localScale = Vector3.one;

        Unit_Script _spawnUnitClass = _charObj.GetComponent<Unit_Script>();
        _spawnUnitClass.Init_Func(GroupType.Enemy);
    }
}
