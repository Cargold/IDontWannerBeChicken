using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Script : Character_Script
{
    public MutantType mutantType;

    [SerializeField]
    private Vector3 moveDir;

    private BattleSpawn_Script spawnCalss;
    public int spawnNum;
    public float spawnInterval;
    public int spawnNum_Limit;
    public int populationValue;
    public int unlockLevel;
    
    public Sprite cardSprite;
    public Vector2 cardPortraitPos;
    public float cardImageSize;

    public Vector2 feedImagePos;
    public float feedImageSize;

    public void SetData_Func(Unit_Data _unitData)
    {
        unitID = _unitData.unitId;
        charName = _unitData.unitName;
        charDesc = _unitData.unitDesc;

        healthPoint_Max = _unitData.healthPoint;
        healthPoint_Recent = _unitData.healthPoint;
        defenceValue = _unitData.defenceValue;
        attackValue = _unitData.attackValue;
        attackRate_Max = _unitData.attackRate;
        attackRange = _unitData.attackRange + 1f;
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
        populationValue = _unitData.populationValue;

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

    public void Init_Func(GroupType _groupType)
    {
        base.Init_Func(_groupType);

        if (unitRend == null)
            unitRend = this.transform.Find("Pivot").Find("Image").GetComponent<SpriteRenderer>();
        unitRend.sprite = unitSprite;
        unitRend.color = Color.white;

        unitRend.sortingOrder = -6;
        shadowRend.sortingOrder = -7;

        InitMove_Func();
    }
    public void SetDataByPlayerUnit_Func(Unit_Script _unitClass)
    {
        healthPoint_Max = _unitClass.healthPoint_Max;
        healthPoint_Recent = _unitClass.healthPoint_Max;
        defenceValue = _unitClass.defenceValue;
        attackValue = _unitClass.attackValue;
        attackRate_Max = _unitClass.attackRate_Max;
        attackRange = _unitClass.attackRange + 1f;
        moveSpeed = _unitClass.moveSpeed;
        criticalPercent = _unitClass.criticalPercent;
        
        spawnInterval = _unitClass.spawnInterval;

        if(groupType == GroupType.Ally)
        {
            attackRate_Speed = DataBase_Manager.Instance.unitDataArr[unitID].attackRate / _unitClass.attackRate_Max;
        }
        else if (groupType == GroupType.Enemy)
        {
            attackRate_Speed = DataBase_Manager.Instance.monsterDataArr[unitID].attackRate / _unitClass.attackRate_Max;
        }
    }
    public void SetMutant_Func(MutantType _mutantType)
    {
        mutantType = _mutantType;

        if (_mutantType == MutantType.Attack)
        {
            attackRate_Max *= 0.5f;
            attackRate_Speed *= 0.5f;
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
        spawnCalss = _spawnClass;
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
            StopCoroutine("Move_Cor");
            StartCoroutine("Move_Cor");
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
        isAlive = false;
        charState = CharacterState.Die;
        contactCharClassList.Clear();

        StopCoroutine("Move_Cor");

        hpRend_Group.gameObject.SetActive(false);

        if (_isImmediate == false)
        {
            if (effectData_Die.isEffectOn == true)
            {
                GameObject _effectObj = ObjectPool_Manager.Instance.Get_Func(effectData_Die.effectObj);
                _effectObj.transform.position = effectData_Die.effectPos.position;
                _effectObj.transform.eulerAngles = new Vector3(270f, 0f, 0f);
            }
        }

        spawnCalss.UnitDie_Func(this);

        ObjectPool_Manager.Instance.Free_Func(this.gameObject);

        if (groupType == GroupType.Enemy)
            Battle_Manager.Instance.CountKillMonster_Func(unitID);
    }
}
