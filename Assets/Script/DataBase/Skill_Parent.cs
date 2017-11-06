using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill_Parent : MonoBehaviour
{
    public int skillID;
    public string skillName;
    public string skillDesc;
    public Sprite skillSprite;

    public int skillLevel;
    public int slotOrder;
    public float coolTime;
    public float manaCost;
    public int unlockLevel;
    public float upgradeCost;
    public skillVar[] skillVarArr;

    public abstract void Init_Func();

    protected skillVar GetSetVar_Func(skillVar _var)
    {
        _var.recentValue = _var.initValue + (_var.upgradeValue * (skillLevel - 1));

        return _var;
    }

    
}

[System.Serializable]
public struct skillVar
{
    public float initValue;
    public float upgradeValue;
    [System.NonSerialized]
    public float recentValue;
}

