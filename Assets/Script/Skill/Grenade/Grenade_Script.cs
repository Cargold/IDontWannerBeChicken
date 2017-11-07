using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grenade_Script : Skill_Parent
{
    [SerializeField]
    private SkillVar damageData;
    public GameObject grenadeObj;
    public GrenadeCol_Script grenadeColClass;
    public float firePivotY;
    public float throwPower;
    public float throwHeight;
    public float throwTime;
    public float bombRange;
    public Transform playerTrf;
    public bool isActive;

    public override void Init_Func()
    {
        grenadeColClass.Init_Func(this);

        playerTrf = Player_Data.Instance.playerClass.transform;
    }
    public override void BattleEnter_Func()
    {
        base.BattleEnter_Func();

        damageData = skillVarArr[0];

        isActive = true;
    }
    public override void UseSkill_Func()
    {
        grenadeColClass.Active_Func();

        grenadeObj.transform.position = new Vector3(playerTrf.position.x, firePivotY, 0f);

        Vector3 _throwPos = grenadeObj.transform.position;
        _throwPos = new Vector3(_throwPos.x + throwPower, 0f, 0f);

        grenadeObj.transform.DOJump(_throwPos, throwHeight, 1, throwTime).OnComplete(grenadeColClass.Bomb_Func);
    }
    public void SetTarget_Func(Character_Script[] _charClassArr)
    {
        for (int i = 0; i < _charClassArr.Length; i++)
        {
            Vector3 _targetPos = _charClassArr[i].transform.position;
            _targetPos = new Vector3(_targetPos.x, _targetPos.y, _targetPos.z);

            float _distanceValue = Vector3.Distance(grenadeObj.transform.position, _targetPos);

            if(_distanceValue < bombRange)
            {
                _charClassArr[i].Damaged_Func(damageData.recentValue);
            }
        }

        Deactive_Func();
    }
    void Deactive_Func()
    {
        isActive = false;
    }
}
