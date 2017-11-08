using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverDrive_Script : Skill_Parent
{
    public Player_Script playerClass;
    public SkillVar damageData;
    public float overdriveTime;
    
    public override void Init_Func()
    {
        playerClass = Player_Data.Instance.playerClass;
    }
    protected override void BattleEnterChild_Func()
    {
        damageData = RevisionValue_Func(skillVarArr[0], 0.01f);
    }
    public override void UseSkill_Func()
    {
        isActive = true;

        StartCoroutine(Overdrive_Cor());
    }
    IEnumerator Overdrive_Cor()
    {
        float _damageUpValue = 0f;
        float _playerDamage = playerClass.attackValue;
        _damageUpValue = _playerDamage * damageData.recentValue;
        _damageUpValue -= _playerDamage;

        playerClass.attackValue += _damageUpValue;

        yield return new WaitForSeconds(overdriveTime);

        playerClass.attackValue -= _damageUpValue;

        Deactive_Func();
    }

    protected override void Deactive_Func()
    {
        isActive = false;
    }
}
