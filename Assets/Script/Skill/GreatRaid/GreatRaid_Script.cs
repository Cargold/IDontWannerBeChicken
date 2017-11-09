using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GreatRaid_Script : Skill_Parent
{
    private Transform playerTrf;
    private SkillVar RaptorSpawnData;
    private SkillVar fullMetalSpawnData;
    private int raptorNum;
    private int fullMetalNum;
    private bool isRaptorSpawnClear;
    private bool isFullMetalSpawnClear;
    public float spawnPosY;
    public float fallingPosX;
    public float spawnInterval_Min;
    public float spawnInterval_Max;
    public float spawnPosX_Left;
    public float spawnPosX_Right;
    public float fallingTime;
    public CharEffectData effectData_Spawn;

    public override void Init_Func()
    {
        playerTrf = Player_Data.Instance.playerClass.transform;
    }
    protected override void BattleEnterChild_Func()
    {
        RaptorSpawnData = skillVarArr[0];
        fullMetalSpawnData = skillVarArr[1];
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        isRaptorSpawnClear = false;
        isFullMetalSpawnClear = false;

        raptorNum = 0;
        fullMetalNum = 0;

        StartCoroutine(Raiding_Cor());
    }
    IEnumerator Raiding_Cor()
    {
        while (isRaptorSpawnClear == false || isFullMetalSpawnClear == false)
        {
            Unit_Script _spawnUnitClass = null;
            Vector3 _spawnPos = playerTrf.position;
            float _randPosX = Random.Range(-spawnPosX_Left, spawnPosX_Right);
            float spawnPosY_Calc = Random.Range(-Battle_Manager.Instance.spawnPosY_Min, Battle_Manager.Instance.spawnPosY_Max);
            _spawnPos = new Vector3
                (
                    _spawnPos.x + _randPosX,
                    spawnPosY + spawnPosY_Calc,
                    0f
                );

            int _randValue = Random.Range(0, 2);
            if (isRaptorSpawnClear == true)
                _randValue = 1;

            while (_spawnUnitClass == null)
            {
                if (_randValue == 0 && isRaptorSpawnClear == false)
                {
                    raptorNum++;

                    _spawnUnitClass = Battle_Manager.Instance.OnSpawnAllyUnit_Func(0);

                    if (RaptorSpawnData.recentValue <= raptorNum)
                    {
                        isRaptorSpawnClear = true;
                    }
                }
                else if (_randValue == 1 && isFullMetalSpawnClear == false)
                {
                    fullMetalNum++;

                    _spawnUnitClass = Battle_Manager.Instance.OnSpawnAllyUnit_Func(1);

                    if (fullMetalSpawnData.recentValue <= fullMetalNum)
                    {
                        isFullMetalSpawnClear = true;
                    }
                }

                _randValue++;
                if (2 <= _randValue)
                    _randValue = 0;
            }

            StartCoroutine(SpawnUnitRotate_Cor(_spawnUnitClass.transform, fallingTime));

            _spawnUnitClass.transform.position = _spawnPos;
            _spawnPos += Vector3.right * fallingPosX;
            _spawnPos += Vector3.down * spawnPosY;
            _spawnUnitClass.transform
                .DOMove(_spawnPos, fallingTime)
                .OnComplete(_spawnUnitClass.OnLanding_Func);

            effectData_Spawn.SetEffectPos(_spawnPos);
            effectData_Spawn.SetActiveDelayTime_Func(fallingTime / 2f);
            effectData_Spawn.ActiveEffect_Func();

            float _randInterval = Random.Range(spawnInterval_Min, spawnInterval_Max);
            yield return new WaitForSeconds(_randInterval);
        }

        Deactive_Func();
    }
    private IEnumerator SpawnUnitRotate_Cor(Transform _targetTrf, float _rotateTime)
    {
        _rotateTime *= 0.8f;

        float _calcTime = 0f;
        float _rotateValue = 360f / _rotateTime;
        while (_calcTime <= _rotateTime)
        {
            _targetTrf.localEulerAngles = Vector3.forward * _rotateValue * _calcTime;

            _calcTime += 0.02f;
            yield return new WaitForFixedUpdate();
        }

        _targetTrf.localEulerAngles = Vector3.zero;
    }

    protected override void Deactive_Func()
    {
        isActive = false;

        isRaptorSpawnClear = false;
        isFullMetalSpawnClear = false;

        raptorNum = 0;
        fullMetalNum = 0;
    }
}
