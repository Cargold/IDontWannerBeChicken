using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Hero_Data
{
    public int unitId;
    public string unitName;
    public string unitDesc;

    public float healthPoint;
    public float defenceValue;
    public float attackValue;
    public float attackRate;
    public float attackRange;
    public float moveSpeed;
    public float criticalPercent;
    public float criticalBonus;
    public ShootType shootType;
    public float shootSpeed;
    public float shootHeight;
    public AttackType attackType;

    public float manaRegen;

    // Info Data
    public GroupType groupType;

    public void SetData_Func(Player_Script _playerClass)
    {
        unitId = 999;
        unitName = _playerClass.charName;
        unitDesc = _playerClass.charDesc;

        healthPoint = _playerClass.healthPoint_Max;
        defenceValue = _playerClass.defenceValue;
        attackValue = _playerClass.attackValue;
        attackRate = _playerClass.GetAttackSpeedMax_Func();
        attackRange = _playerClass.attackRange;
        moveSpeed = _playerClass.moveSpeed;
        criticalPercent = _playerClass.criticalPercent;
        criticalBonus = _playerClass.criticalBonus;
        shootType = _playerClass.shootType;
        shootSpeed = _playerClass.shootTime;
        shootHeight = _playerClass.shootHeight;
        attackType = _playerClass.attackType;
        
        groupType = GroupType.Ally;

        manaRegen = _playerClass.manaRegen;
    }
}
