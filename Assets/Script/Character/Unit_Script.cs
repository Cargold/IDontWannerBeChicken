﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Script : Character_Script
{
    [Header("Unit Data")]
    public MutantType mutantType;
    public MonsterType monsterType;

    [SerializeField]
    private Vector3 moveDir;

    private BattleSpawn_Script spawnClass;
    public int spawnNum;
    public float spawnInterval;
    public int spawnNum_Limit;
    public int unlockLevel;
    
    public Sprite cardSprite;
    public Vector2 cardPortraitPos;
    public float cardImageSize;

    public Vector2 feedImagePos;
    public float feedImageSize;

    public void SetData_Func(Unit_Data _unitData)
    {
        // 풀링 될 때의 첫 데이터 세팅

        unitID = _unitData.unitId;
        charNameArr = _unitData.charNameArr;
        charDescArr = _unitData.charDescArr;

        healthPoint_Max = _unitData.healthPoint;
        healthPoint_Recent = _unitData.healthPoint;
        defenceValue = _unitData.defenceValue;
        attackValue = _unitData.attackValue;
        attackRate_Max = _unitData.attackRate;
        attackRange = _unitData.attackRange;
        moveSpeed = _unitData.moveSpeed;
        criticalPercent = _unitData.criticalPercent;
        criticalBonus = _unitData.criticalBonus;
        shootType = _unitData.shootType;
        shootTime = _unitData.shootSpeed;
        shootHeight = _unitData.shootHeight;
        attackType = _unitData.attackType;

        spawnNum = _unitData.spawnNum;
        spawnInterval = _unitData.spawnInterval;
        spawnNum_Limit = _unitData.spawnNum_Limit;

        groupType = _unitData.groupType;
        unitSprite = _unitData.unitSprite;

        unitRend = this.transform.Find("Pivot").Find("Image").GetComponent<SpriteRenderer>();
        unitRend.color = Color.white;
        unitRend.sprite = _unitData.unitSprite;
        if (groupType == GroupType.Enemy)
            unitRend.flipX = true;
        imagePivotAxisY = _unitData.imagePivotAxisY;
        shadowSize = _unitData.shadowSize;
        
        if (cardSprite == null)
            cardSprite = unitSprite;
    }

    public void Init_Func(GroupType _groupType, bool _isLobbyPrint = false)
    {
        base.Init_Func(_groupType);

        if (unitRend == null)
            unitRend = this.transform.Find("Pivot").Find("Image").GetComponent<SpriteRenderer>();
        unitRend.sprite = unitSprite;
        unitRend.color = Color.white;

        if (isHouse == false && isPlayer == false)
            unitRend.sortingOrder = (int)(this.transform.position.y * -100f) + 210;
        if (isHouse == false && isPlayer == false)
            shadowRend.sortingOrder = (int)(this.transform.position.y * -100f) + 200;

        hpRend.sortingOrder = (int)(this.transform.position.y * -100f) + 210;

        InitMove_Func();

        if(_isLobbyPrint == true)
        {
            OnPrintLobby_Func();
        }
    }
    public void SetDataByPlayerUnit_Func(Unit_Script _unitClass)
    {
        healthPoint_Max = _unitClass.healthPoint_Max;
        healthPoint_Recent = _unitClass.healthPoint_Max;
        defenceValue = _unitClass.defenceValue;
        attackValue = _unitClass.attackValue;
        attackRate_Max = _unitClass.attackRate_Max;
        attackRange = _unitClass.attackRange;
        moveSpeed = _unitClass.moveSpeed;
        criticalPercent = _unitClass.criticalPercent;
        
        spawnInterval = _unitClass.spawnInterval;

        if(groupType == GroupType.Ally)
        {
            attackRate_AniSpeed = DataBase_Manager.Instance.unitDataArr[unitID].attackRate / _unitClass.attackRate_Max;
        }
        else if (groupType == GroupType.Enemy)
        {
            attackRate_AniSpeed = DataBase_Manager.Instance.monsterDataArr[unitID].attackRate / _unitClass.attackRate_Max;
        }
    }
    public void SetDataByPlayerUnit_Func(Unit_Data _unitData)
    {
        healthPoint_Max = _unitData.healthPoint;
        healthPoint_Recent = _unitData.healthPoint;
        defenceValue = _unitData.defenceValue;
        attackValue = _unitData.attackValue;
        attackRate_Max = _unitData.attackRate;
        attackRange = _unitData.attackRange;
        moveSpeed = _unitData.moveSpeed;
        criticalPercent = _unitData.criticalPercent;

        spawnInterval = _unitData.spawnInterval;

        if (groupType == GroupType.Ally)
        {
            attackRate_AniSpeed = DataBase_Manager.Instance.unitDataArr[unitID].attackRate / _unitData.attackRate;
        }
        else if (groupType == GroupType.Enemy)
        {
            attackRate_AniSpeed = DataBase_Manager.Instance.monsterDataArr[unitID].attackRate / _unitData.attackRate;
        }
    }
    public void SetMutant_Func(MutantType _mutantType)
    {
        mutantType = _mutantType;

        if (_mutantType == MutantType.Attack)
        {
            attackRate_Max *= 0.5f;
            attackRate_AniSpeed *= 0.5f;
            attackValue *= 2f;

            unitRend.color = new Color(1f, 0.5f, 0.5f, 1f);
        }
        else if(_mutantType == MutantType.Health)
        {
            healthPoint_Max *= 4f;
            healthPoint_Recent *= 4f;

            this.transform.localScale *= 2f;
        }
        else
        {
            unitRend.color = Color.white;
        }
    }
    public void SetSpawner_Func(BattleSpawn_Script _spawnClass)
    {
        spawnClass = _spawnClass;
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
        if(charState != CharacterState.Move && isAlive == true)
        {
            animator.speed = 1f;
            StopCoroutine("Move_Cor");
            StartCoroutine("Move_Cor");

            if (unitSprite != null)
                unitRend.sprite = unitSprite;
        }
    }
    IEnumerator Move_Cor()
    {
        animator.SetBool("OnContact", false);
        charState = CharacterState.Move;

        while (charState == CharacterState.Move)
        {
            this.transform.position += moveDir * moveSpeed * 0.01f;

            yield return new WaitForFixedUpdate();
        }
    }

    public override void Die_Func(bool _isImmediate = false)
    {
        sphereCol.enabled = false;

        if (spawnClass != null)
            spawnClass.UnitDie_Func(this);

        base.Die_Func(_isImmediate);
    }
}
