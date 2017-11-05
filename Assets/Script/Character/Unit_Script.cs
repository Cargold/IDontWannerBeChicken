using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Script : Character_Script
{
    public MutantType mutantType;

    [SerializeField]
    private Vector3 moveDir;

    public int spawnNum;
    public float spawnInterval;
    public int populationValue;
    public int unlockLevel;
    
    public Sprite cardSprite;
    public Vector2 cardPortraitPos;
    public float cardImageSize;

    public Vector2 feedImagePos;
    public float feedImageSize;

    public void SetData_Func(Unit_Data _charData)
    {
        unitID = _charData.charId;
        charName = _charData.charName;
        charDesc = _charData.charDesc;

        healthPoint_Max = _charData.healthPoint;
        healthPoint_Recent = _charData.healthPoint;
        defenceValue = _charData.defenceValue;
        attackValue = _charData.attackValue;
        attackRate_Max = _charData.attackRate;
        attackRange = _charData.attackRange + 1f;
        moveSpeed = _charData.moveSpeed;
        criticalPercent = _charData.criticalPercent;
        criticalBonus = _charData.criticalBonus;
        shootType = _charData.shootType;
        shootTime = _charData.shootSpeed;
        shootHeight = _charData.shootHeight;
        attackType = _charData.attackType;

        spawnNum = _charData.spawnNum;
        spawnInterval = _charData.spawnInterval;
        populationValue = _charData.populationValue;

        groupType = _charData.groupType;
        unitSprite = _charData.unitSprite;

        unitRend = this.transform.Find("Pivot").Find("Image").GetComponent<SpriteRenderer>();
        unitRend.color = Color.white;
        unitRend.sprite = _charData.unitSprite;
        if (groupType == GroupType.Enemy)
            unitRend.flipX = true;
        imagePivotAxisY = _charData.imagePivotAxisY;
        shadowSize = _charData.shadowSize;
        
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

        attackRate_Speed = DataBase_Manager.Instance.unitDataArr[unitID].attackRate / _unitClass.attackRate_Max;
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
}
