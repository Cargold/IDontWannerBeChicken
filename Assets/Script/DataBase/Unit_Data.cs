using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Unit_Data
{
    public int unitId;
    public string unitName;
    public string[] charNameArr;
    public string[] charDescArr;

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
    public int spawnNum_Limit;

    // Info Data
    public GroupType groupType;

    // Rendering Data
    public Sprite unitSprite;
    public float imagePivotAxisY;
    public Vector2 shadowSize;

    // Card Data
    public Sprite cardSprite;
    public Vector2 cardPortraitPos;
    public float cardImageSize;

    public void SetData_Func(Unit_Script _unitClass, int _unitID)
    {
        unitId          = _unitID;
        charNameArr     = _unitClass.charNameArr;
        charDescArr     = _unitClass.charDescArr;

        healthPoint     = _unitClass.healthPoint_Max;
        defenceValue    = _unitClass.defenceValue;
        attackValue     = _unitClass.attackValue;
        attackRate      = _unitClass.GetAttackSpeedMax_Func();
        attackRange     = _unitClass.attackRange;
        moveSpeed       = _unitClass.moveSpeed;
        criticalPercent = _unitClass.criticalPercent;
        criticalBonus   = _unitClass.criticalBonus;
        shootType       = _unitClass.shootType;
        shootSpeed      = _unitClass.shootTime;
        shootHeight     = _unitClass.shootHeight;
        attackType      = _unitClass.attackType;

        spawnNum        = _unitClass.spawnNum;
        spawnInterval   = _unitClass.spawnInterval;
        spawnNum_Limit  = _unitClass.spawnNum_Limit;

        groupType       = _unitClass.groupType;

        unitSprite      = _unitClass.unitSprite;

        if (_unitClass.imagePivotAxisY == 0f)
            imagePivotAxisY = _unitClass.transform.Find("Pivot").Find("Image").transform.localPosition.y;
        else
            imagePivotAxisY = _unitClass.imagePivotAxisY;

        shadowSize = _unitClass.transform.Find("Pivot").Find("Shadow").localScale;

        cardSprite      = _unitClass.cardSprite;
        cardPortraitPos = _unitClass.cardPortraitPos;
        cardImageSize   = _unitClass.cardImageSize;
    }
}
