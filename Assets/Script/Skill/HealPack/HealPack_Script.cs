using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack_Script : Skill_Parent
{
    [SerializeField]
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

        playerTrf = Player_Data.Instance.playerClass.transform;
    }
    protected override void BattleEnterChild_Func()
    {
        healData = skillVarArr[0];
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        healpackColClass.Active_Func();

        StartCoroutine(Healling_Cor());
    }
    IEnumerator Healling_Cor()
    {
        int _count = healCount;
        while(0 < _count)
        {
            healObj.transform.position = new Vector3(playerTrf.position.x, 0f, 0f);
            healpackColClass.Heal_Func();
            yield return new WaitForSeconds(healInterval);
            _count--;
        }

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

        Player_Data.Instance.playerClass.Heal_Func(healData.recentValue);
    }
    protected override void Deactive_Func()
    {
        isActive = false;

        healpackColClass.Deactive_Func();
    }
}
