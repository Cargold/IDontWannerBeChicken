﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Script : Character_Script
{
    [SerializeField]
    private Vector3 moveDir;

    public int spawnNum;
    public float spawnInterval;
    public int populationValue;
    public Sprite charSprite;

    public void SetData_Func(Character_Data _charData)
    {
        charId = _charData.charId;
        charName = _charData.charName;
        charDesc = _charData.charDesc;

        healthPoint_Max = _charData.healthPoint;
        healthPoint_Recent = _charData.healthPoint;
        defenceValue = _charData.defenceValue;
        attackValue = _charData.attackValue;
        attackRate_Max = _charData.attackRate;
        attackRange = _charData.attackRange;
        moveSpeed = _charData.moveSpeed;
        criticalPercent = _charData.criticalPercent;
        criticalBonus = _charData.criticalBonus;
        shootType = _charData.shootType;
        shootSpeed = _charData.shootSpeed;
        shootHeight = _charData.shootHeight;
        attackType = _charData.attackType;

        spawnNum = _charData.spawnNum;
        spawnInterval = _charData.spawnInterval;
        populationValue = _charData.populationValue;

        groupType = _charData.groupType;
        charSprite = _charData.charSprite;
    }

    public void Init_Func(GroupType _groupType)
    {
        base.Init_Func(_groupType);

        InitMove_Func();
    }

    void InitMove_Func()
    {
        moveDir = Vector3.zero;
        if (groupType == GroupType.Ally)
        {
            moveDir = Vector3.right;
        }
        else if (groupType == GroupType.Enemy)
        {
            moveDir = Vector3.left;
        }
    }

    protected override void Move_Func()
    {
        if(charState != CharacterState.Move)
        {
            StopCoroutine("Move_Cor");
            StartCoroutine("Move_Cor");
        }
    }

    IEnumerator Move_Cor()
    {
        charState = CharacterState.Move;

        while (charState == CharacterState.Move)
        {
            this.transform.position += moveDir * moveSpeed * 0.01f;

            yield return new WaitForFixedUpdate();
        }
    }
}
