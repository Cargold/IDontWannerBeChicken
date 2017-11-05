public enum ShootType
{
    None = -1,
    RelativeAnimation,
    Projectile,
    FixedRange,
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

public enum BattleType
{
    None = -1,
    Normal,
    Special,
}

public enum GameState
{
    None = -1,
    Battle,
    Lobby,
}

public enum LobbyState
{
    None = -1,
    MainLobby,
    StageSelect,
    HeroManagement,
    PartySetting,
    StoreRoom,
    QuestRoom,
    TrophyRoom,

    FeedingRoom,
}

public enum RewardType
{
    None = -1,
    Wealth,
    Food,
    FoodBoxLevel,
    Unit,
    PopulationPoint,
    Skill,
    Trophy,
}

public enum NatureType
{
    None = -1,
    Mountain,
    Tree,
}

public enum NatureWorkType
{
    None = -1,
    Devastated,
    Woody,
}

public enum TrophyType
{
    None = -1,
    HealthPoint_Hero,
    AttackValue_Hero,
    CriticalPercent_Hero,
    CriticalBonus_Hero,
    ManaRegen,
    ManaStart,
    HealthPoint_Unit,
    AttackValue_Unit,
    CriticalPercent_Unit,
    CriticalBonus_Unit,
    HealthPoint_Monster,
    AttackValue_Monster,
    HealthPoint_House,
    GoldBonus,
    ItemDropPer,
    UpgradeExp,
}

public enum MutantType
{
    None = -1,
    Attack,
    Health,
}