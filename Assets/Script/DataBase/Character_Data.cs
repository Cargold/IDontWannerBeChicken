﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Character_Data
{
    public int charId;
    public string charName;
    public string charDesc;

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

    public int spawnNum;
    public float spawnInterval;
    public int populationValue;

    // Info Data
    public GroupType groupType;

    // Rendering Data
    public Sprite charSprite;

    public void SetData_Func(Unit_Script _unitClass)
    {
        charId          = _unitClass.charId;
        charName        = _unitClass.charName;
        charDesc        = _unitClass.charDesc;

        healthPoint     = _unitClass.healthPoint_Max;
        defenceValue    = _unitClass.defenceValue;
        attackValue     = _unitClass.attackValue;
        attackRate      = _unitClass.attackRate_Max;
        attackRange     = _unitClass.attackRange;
        moveSpeed       = _unitClass.moveSpeed;
        criticalPercent = _unitClass.criticalPercent;
        criticalBonus   = _unitClass.criticalBonus;
        shootType       = _unitClass.shootType;
        shootSpeed      = _unitClass.shootSpeed;
        shootHeight     = _unitClass.shootHeight;
        attackType      = _unitClass.attackType;

        spawnNum        = _unitClass.spawnNum;
        spawnInterval   = _unitClass.spawnInterval;
        populationValue = _unitClass.populationValue;

        groupType       = _unitClass.groupType;

        charSprite      = _unitClass.charSprite;
    }
}
