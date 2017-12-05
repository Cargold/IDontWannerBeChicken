using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PurifyingLay_Script : Skill_Parent
{
    private SkillVar damageData;
    private SkillVar damagePerData;
    private float damageValue;
    public PurifyingLayCol_Script layColClass;
    public float layStartPosX;
    public float layMoveDistance;
    public float layRange;
    public float layTime;
    public float layInterval;
    public Transform playerTrf;

    public override void Init_Func()
    {
        layColClass.Init_Func(this);

        playerTrf = Player_Data.Instance.heroClass.transform;
    }
    protected override void BattleEnterChild_Func()
    {
        damageData = skillVarArr[0];
        damagePerData = RevisionValue_Func(skillVarArr[1], 0.01f);

        damageValue
            = damageData.recentValue
            + Player_Data.Instance.heroLevel * damagePerData.recentValue;
    }
    public override void UseSkill_Func()
    {
        isActive = true;
        
        layColClass.Active_Func(layInterval);

        Transform _layTrf = layColClass.transform;

        _layTrf.transform.position = new Vector3(playerTrf.position.x + layStartPosX, 0f, 0f);

        Vector3 _layPos = _layTrf.transform.position + (Vector3.right * layMoveDistance);

        _layTrf.transform
            .DOMove(_layPos, layTime)
            .SetEase(Ease.Linear)
            .OnComplete(Deactive_Func);
    }
    public void SetTarget_Func(Vector3 _layPos, Character_Script[] _charClassArr)
    {
        for (int i = 0; i < _charClassArr.Length; i++)
        {
            Vector3 _targetPos = _charClassArr[i].transform.position;
            _targetPos = new Vector3(_targetPos.x, 0f, 0f);

            float _distanceValue = Vector3.Distance(_layPos, _targetPos);

            if (_distanceValue < layRange)
            {
                _charClassArr[i].Damaged_Func(damageValue);
            }
        }
    }
    protected override void Deactive_Func()
    {
        isActive = false;

        layColClass.Deactive_Func();
    }
}
