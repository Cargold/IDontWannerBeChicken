﻿public enum ShootType
{
    None = -1,
    Melee,
    Range,
}

public enum AttackType
{
    None = -1,
    Single,
    Plural,
}

public enum GroupType
{
    None = -1,
    Ally,
    Enemy,
    Neutral,
}

public enum PoolingType
{
    None = -1,
    AllyUnit_Melee,
    AllyUnit_Range,
    EnemyUnit_Melee,
    EnemyUnit_Range,
}

public enum WealthType
{
    None = -1,
    Gold,
    Mineral,
}

public enum FoodGrade
{
    None = -1,
    Common,
    Rare,
    Legend,
}

public enum FoodEffect_Main
{
    None = -1,
    AttackPower,
    HealthPoint,
    Gizzard,
}

public enum FoodEffect_Sub
{
    None = -1,
    Critical,
    SpawnInterval,
    DecreaseHP,
    DefenceValue,
    DecreaseAttack,
    Bonus_Apple,
    Bounus_Fish,
}

public enum FoodPlaceState
{
    None = -1,
    Stomach,
    Inventory,
}

public enum FoodState
{
    Inventory = -1,
    Stomach = 0,
    FeedingByChain = 1,
    FeedingByInner = 2,
}

public enum UnitInfoUIType
{
    None = -1,
    AttackValue,
    HealthPoint,
    DefenceValue,
    CriticalPercent,
    SpawnNum,
    SpawnInterval,
}