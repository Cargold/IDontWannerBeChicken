﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodThirst_Script : Skill_Parent
{
    private SkillVar hasteData;
    private GameObject hasteObj;
    private BloodThirstCol_Script bloodThirstColClass;
    public float hasteRange;
    public float hasteTime;
    private Transform playerTrf;
    [SerializeField]
    private List<Character_Script> hasteCharList;

    public override void Init_Func()
    {
        hasteObj = this.transform.GetChild(0).gameObject;
        bloodThirstColClass = hasteObj.GetComponent<BloodThirstCol_Script>();
        bloodThirstColClass.Init_Func(this);

        playerTrf = Player_Data.Instance.heroClass.transform;

        hasteCharList = new List<Character_Script>();
    }
    protected override void BattleEnterChild_Func()
    {
        hasteData = RevisionValue_Func(skillVarArr[0], 0.01f);
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        hasteObj.transform.SetParent(playerTrf);
        hasteObj.transform.localPosition = Vector3.zero;
        bloodThirstColClass.Active_Func();

        bloodThirstColClass.Haste_Func();

        SoundSystem_Manager.Instance.PlaySFX_Func(sfxArr_Use);
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

        if (hasteCharList.Contains(Player_Data.Instance.heroClass) == false)
        {
            hasteCharList.Add(Player_Data.Instance.heroClass);
        }

        StartCoroutine(Hasting_Cor());
    }
    IEnumerator Hasting_Cor()
    {
        for (int i = 0; i < hasteCharList.Count; i++)
        {
            hasteCharList[i].SetAttackSpeed_Func(hasteData.recentValue);
        }

        yield return new WaitForSeconds(hasteTime);

        for (int i = 0; i < hasteCharList.Count; i++)
        {
            if(hasteCharList[i].isAlive == true)
                hasteCharList[i].SetAttackSpeed_Func(1f);
        }

        hasteCharList.Clear();

        hasteObj.transform.SetParent(this.transform);
        Deactive_Func();
    }
    protected override void Deactive_Func()
    {
        isActive = false;

        bloodThirstColClass.Deactive_Func();
    }
}
