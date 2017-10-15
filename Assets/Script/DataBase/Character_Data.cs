using System.Collections;
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
}
