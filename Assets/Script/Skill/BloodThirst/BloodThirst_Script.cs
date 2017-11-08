using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodThirst_Script : Skill_Parent
{
    [SerializeField]
    private SkillVar hasteData;
    public GameObject hasteObj;
    public BloodThirstCol_Script bloodThirstColClass;
    public float hasteRange;
    public float hasteTime;
    public Transform playerTrf;
    public List<Character_Script> hasteCharList;
    public List<float> hasteValueList;

    public override void Init_Func()
    {
        bloodThirstColClass.Init_Func(this);

        playerTrf = Player_Data.Instance.playerClass.transform;

        hasteCharList = new List<Character_Script>();
        hasteValueList = new List<float>();
    }
    protected override void BattleEnterChild_Func()
    {
        hasteData = RevisionValue_Func(skillVarArr[0], 0.01f);
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        bloodThirstColClass.Active_Func();

        hasteObj.transform.position = new Vector3(playerTrf.position.x, 0f, 0f);
        bloodThirstColClass.Haste_Func();
    }
    public void SetTarget_Func(Character_Script[] _charClassArr)
    {
        for (int i = 0; i < _charClassArr.Length; i++)
        {
            Vector3 _targetPos = _charClassArr[i].transform.position;
            _targetPos = new Vector3(_targetPos.x, _targetPos.y, _targetPos.z);

            float _distanceValue = Vector3.Distance(hasteObj.transform.position, _targetPos);

            if (_distanceValue < hasteRange)
            {
                hasteCharList.Add(_charClassArr[i]);
            }
        }

        StartCoroutine(Hasting_Cor());
    }
    IEnumerator Hasting_Cor()
    {
        for (int i = 0; i < hasteCharList.Count; i++)
        {
            hasteValueList.Add(hasteCharList[i].GetAttackSpeedMax_Func());
            float _attackSpeed = hasteCharList[i].GetAttackSpeedMax_Func() / hasteData.recentValue;
            hasteCharList[i].SetAttackSpeed_Func(_attackSpeed);
        }

        yield return new WaitForSeconds(hasteTime);

        for (int i = 0; i < hasteCharList.Count; i++)
        {
            if(hasteCharList[i].isAlive == true)
                hasteCharList[i].SetAttackSpeed_Func(hasteValueList[i]);
        }

        hasteCharList.Clear();
        hasteValueList.Clear();

        Deactive_Func();
    }
    protected override void Deactive_Func()
    {
        isActive = false;

        bloodThirstColClass.Deactive_Func();
    }
}
