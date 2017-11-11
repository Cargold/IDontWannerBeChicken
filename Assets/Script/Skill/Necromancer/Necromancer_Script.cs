using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Necromancer_Script : Skill_Parent
{
    private Transform playerTrf;
    private SkillVar zombieSpawnData;
    private SkillVar ReaperSpawnData;
    private int zombieNum;
    private int reaperNum;
    private bool isZombieSpawnClear;
    private bool isReaperSpawnClear;
    public float spawnPosY;
    public float spawnInterval_Min;
    public float spawnInterval_Max;
    public float spawnPosX_Left;
    public float spawnPosX_Right;
    public float riseTime;
    public CharEffectData effectData_Spawn;

    public override void Init_Func()
    {
        playerTrf = Player_Data.Instance.playerHeroData.transform;
    }
    protected override void BattleEnterChild_Func()
    {
        zombieSpawnData = skillVarArr[0];
        ReaperSpawnData = skillVarArr[1];
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        isZombieSpawnClear = false;
        isReaperSpawnClear = false;

        zombieNum = 0;
        reaperNum = 0;

        StartCoroutine(Necromancing_Cor());
    }
    IEnumerator Necromancing_Cor()
    {
        while (isZombieSpawnClear == false || isReaperSpawnClear == false)
        {
            Unit_Script _spawnUnitClass = null;
            Vector3 _spawnPos = playerTrf.position;
            float _randPosX = Random.Range(-spawnPosX_Left, spawnPosX_Right);
            float spawnPosY_Calc = Random.Range(-Battle_Manager.Instance.spawnPosY_Min, Battle_Manager.Instance.spawnPosY_Max);
            _spawnPos = new Vector3
                (
                    _spawnPos.x + _randPosX,
                    -spawnPosY + spawnPosY_Calc,
                    0f
                );

            int _randValue = Random.Range(0, 2);
            if (isZombieSpawnClear == true)
                _randValue = 1;

            while(_spawnUnitClass == null)
            {
                if (_randValue == 0 && isZombieSpawnClear == false)
                {
                    zombieNum++;

                    _spawnUnitClass = Battle_Manager.Instance.OnSpawnAllyUnit_Func(2);

                    if (zombieSpawnData.recentValue <= zombieNum)
                    {
                        isZombieSpawnClear = true;
                    }
                }
                else if (_randValue == 1 && isReaperSpawnClear == false)
                {
                    reaperNum++;

                    _spawnUnitClass = Battle_Manager.Instance.OnSpawnAllyUnit_Func(3);

                    if (ReaperSpawnData.recentValue <= reaperNum)
                    {
                        isReaperSpawnClear = true;
                    }
                }

                _randValue++;
                if (2 <= _randValue)
                    _randValue = 0;
            }

            _spawnUnitClass.transform.position = _spawnPos;
            _spawnPos += Vector3.up * spawnPosY;
            _spawnUnitClass.transform
                .DOMove(_spawnPos, riseTime)
                .OnComplete(_spawnUnitClass.OnLanding_Func);

            effectData_Spawn.SetEffectPos(_spawnPos);
            effectData_Spawn.SetActiveDelayTime_Func(riseTime / 2f);
            effectData_Spawn.ActiveEffect_Func();

            float _randInterval = Random.Range(spawnInterval_Min, spawnInterval_Max);
            yield return new WaitForSeconds(_randInterval);
        }

        Deactive_Func();
    }
    protected override void Deactive_Func()
    {
        isActive = false;

        isZombieSpawnClear = false;
        isReaperSpawnClear = false;

        zombieNum = 0;
        reaperNum = 0;
    }
}
