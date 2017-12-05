using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealPack_Script : Skill_Parent
{
    private SkillVar healData;
    public GameObject healObj;
    public HealPackCol_Script healpackColClass;
    public int healCount;
    public float healInterval;
    public float healRange;
    public Transform playerTrf;
    
    public override void Init_Func()
    {
        healpackColClass.Init_Func(this);

        playerTrf = Player_Data.Instance.heroClass.transform;
    }
    protected override void BattleEnterChild_Func()
    {
        healData = skillVarArr[0];
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        healpackColClass.Active_Func();
        healObj.transform.SetParent(playerTrf);
        healObj.transform.localPosition = Vector3.zero;

        StartCoroutine(Healling_Cor());
    }
    IEnumerator Healling_Cor()
    {
        int _count = healCount;
        while(0 < _count)
        {
            healpackColClass.Heal_Func();

            yield return new WaitForSeconds(healInterval);
            _count--;
        }

        healObj.transform.SetParent(this.transform);
        Deactive_Func();
    }

    public void SetTarget_Func(Character_Script[] _charClassArr)
    {
        for (int i = 0; i < _charClassArr.Length; i++)
        {
            Vector3 _targetPos = _charClassArr[i].transform.position;
            _targetPos = new Vector3(_targetPos.x, _targetPos.y, _targetPos.z);

            float _distanceValue = Vector3.Distance(healObj.transform.position, _targetPos);

            if (_distanceValue < healRange)
            {
                _charClassArr[i].Heal_Func(healData.recentValue);
            }
        }

        Player_Data.Instance.heroClass.Heal_Func(healData.recentValue);
    }
    protected override void Deactive_Func()
    {
        isActive = false;

        healpackColClass.Deactive_Func();
    }
}
