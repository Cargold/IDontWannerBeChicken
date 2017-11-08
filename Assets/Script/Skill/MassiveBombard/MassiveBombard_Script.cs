using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MassiveBombard_Script : Skill_Parent
{
    private Transform playerTrf;
    private SkillVar damageData;
    private SkillVar damagePerData;
    private float damageValue;
    public float spawnPosY;
    public float fallingPosX;
    public float dropInterval;
    public float spawnPosX_Left;
    public float spawnPosX_Right;
    public float fallingTime;
    public float bombTime;
    public GameObject eggBombObj;
    public float bombRange;

    public override void Init_Func()
    {
        playerTrf = Player_Data.Instance.playerClass.transform;
    }
    protected override void BattleEnterChild_Func()
    {
        damageData = skillVarArr[0];
        damagePerData = RevisionValue_Func(skillVarArr[1], 0.01f);
        damagePerData.recentValue += 1f;

        damageValue
            = damageData.recentValue
            + Player_Data.Instance.playerClass.attackValue * damagePerData.recentValue;
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        StartCoroutine(Raiding_Cor());
    }
    IEnumerator Raiding_Cor()
    {
        float _bombTime = 0f;

        while (_bombTime < bombTime)
        {
            Vector3 _spawnPos = playerTrf.position;
            float _randPosX = Random.Range(-spawnPosX_Left, spawnPosX_Right);
            float spawnPosY_Calc = Random.Range(-Battle_Manager.Instance.spawnPosY_Min, Battle_Manager.Instance.spawnPosY_Max);
            _spawnPos = new Vector3
                (
                    _spawnPos.x + _randPosX,
                    spawnPosY + spawnPosY_Calc,
                    0f
                );

            GameObject _eggBombObj = ObjectPool_Manager.Instance.Get_Func(eggBombObj);
            MassiveBombardCol_Script _eggbombClass = _eggBombObj.GetComponent<MassiveBombardCol_Script>();
            _eggbombClass.Init_Func(this);
            _eggbombClass.Active_Func();
            
            _eggBombObj.transform.position = _spawnPos;
            _spawnPos += Vector3.right * fallingPosX;
            _spawnPos += Vector3.down * spawnPosY;
            _eggBombObj.transform
                .DOMove(_spawnPos, fallingTime)
                .SetEase(Ease.Linear)
                .OnComplete(_eggbombClass.Bomb_Func);

            yield return new WaitForSeconds(dropInterval);
            _bombTime += dropInterval;
        }

        Deactive_Func();
    }

    public void SetTarget_Func(Vector3 _eggBombPos, Character_Script[] _charClassArr)
    {
        for (int i = 0; i < _charClassArr.Length; i++)
        {
            Vector3 _targetPos = _charClassArr[i].transform.position;
            _targetPos = new Vector3(_targetPos.x, _targetPos.y, _targetPos.z);

            float _distanceValue = Vector3.Distance(_eggBombPos, _targetPos);

            if (_distanceValue < bombRange)
            {
                _charClassArr[i].Damaged_Func(damageValue);
            }
        }

        Deactive_Func();
    }

    protected override void Deactive_Func()
    {
        isActive = false;
    }
}
