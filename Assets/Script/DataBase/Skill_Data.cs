using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Skill_Data
{
    public int skillID;
    public string skillName;
    public string skillDesc;
    public Sprite skillSprite;
    
    public int slotOrder;
    public float coolTime;
    public float manaCost;
    public int unlockLevel;
    public int upgradeInitCost;
    public SkillVar[] skillVarArr;

    public void SetData_Func(Skill_Parent _skillClass)
    {
        skillID     = _skillClass.skillID;
        skillName   = _skillClass.skillName;
        skillDesc   = _skillClass.skillDesc;
        skillSprite = _skillClass.skillSprite;
                                 
        slotOrder   = _skillClass.slotOrder   ;
        coolTime    = _skillClass.coolTime    ;
        manaCost    = _skillClass.manaCost    ;
        unlockLevel = _skillClass.unlockLevel ;
        upgradeInitCost = _skillClass.upgradeInitCost ;
        skillVarArr = _skillClass.skillVarArr;
    }
}
