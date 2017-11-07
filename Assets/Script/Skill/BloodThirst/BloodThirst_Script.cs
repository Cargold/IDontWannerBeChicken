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
    public bool isActive;
    public List<Character_Script> hasteCharList;
    public List<float> hasteValueList;

    public override void Init_Func()
    {
        bloodThirstColClass.Init_Func(this);

        playerTrf = Player_Data.Instance.playerClass.transform;

        hasteCharList = new List<Character_Script>();
        hasteValueList = new List<float>();
    }
    public override void BattleEnter_Func()
    {
        base.BattleEnter_Func();

        hasteData = skillVarArr[0];

        hasteData.initValue *= 0.01f;
        hasteData.recentValue *= 0.01f;
        hasteData.recentValue += 1f;
        hasteData.upgradeValue *= 0.01f;

        isActive = true;
    }
    public override void UseSkill_Func()
    {
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

        StartCoroutine(Healling_Cor());
    }
    IEnumerator Healling_Cor()
    {
        for (int i = 0; i < hasteCharList.Count; i++)
        {
            hasteValueList.Add(hasteCharList[i].attackRate_Max);
            hasteCharList[i].attackRate_Max /= hasteData.recentValue;
        }

        yield return new WaitForSeconds(hasteTime);

        for (int i = 0; i < hasteCharList.Count; i++)
        {
            if(hasteCharList[i].isAlive == true)
                hasteCharList[i].attackRate_Max = hasteValueList[i];
        }

        hasteCharList.Clear();
        hasteValueList.Clear();

        Deactive_Func();
    }
    void Deactive_Func()
    {
        isActive = false;

        bloodThirstColClass.Deactive_Func();
    }
}
