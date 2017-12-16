using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill_Parent : MonoBehaviour
{
    public int skillID;
    public string[] skillNameArr;
    public string[] skillDescArr;
    public Sprite skillSprite;
    
    public int slotOrder;
    public float coolTime;
    public float manaCost;
    public int unlockLevel;
    public int upgradeInitCost;
    public SkillVar[] skillVarArr;
    public bool isActive;

    public abstract void Init_Func();
    public void BattleEnter_Func()
    {
        for (int i = 0; i < skillVarArr.Length; i++)
        {
            skillVarArr[i] = GetSetVar_Func(skillVarArr[i]);
        }

        BattleEnterChild_Func();
    }
    protected abstract void BattleEnterChild_Func();
    public abstract void UseSkill_Func();
    protected abstract void Deactive_Func();

    protected SkillVar GetSetVar_Func(SkillVar _var)
    {
        int _skillLevel = Player_Data.Instance.skillDataArr[skillID].skillLevel;

        _var.recentValue = _var.initValue + (_var.upgradeValue * (_skillLevel - 1));

        return _var;
    }
    protected SkillVar RevisionValue_Func(SkillVar _var, float _value)
    {
        _var.initValue *= _value;
        _var.recentValue *= _value;
        _var.upgradeValue *= _value;

        return _var;
    }
}

[System.Serializable]
public struct SkillVar
{
    public float initValue;
    public float upgradeValue;
    [System.NonSerialized]
    public float recentValue;
}

