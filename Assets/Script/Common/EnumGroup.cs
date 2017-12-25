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
    Real,
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
}

public enum FoodPlaceState
{
    None = -1,
    Stomach,
    Inventory,
    Dragging_Inven
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
    Random = -2,
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

public enum AttackTiming
{
    None = -1,
    Closer,
    Contact,
}

public enum CharacterCondition
{
    None = -1,
    Normal,
    Stun,
    KnockBack,
}

public enum MonsterType
{
    None = -1,
    Melee,
    Range,
    Shield,
    Bomb,
    Spread,
    Boss,
}

public enum DrinkType
{
    None = -1,
    Gold,
    Mana,
    Health,
    Critical,
}

public enum StoreState
{
    None = -1,
    Mineral,
    Gold,
    Box,
    Drink,
    Package,
}

public enum StoreGoodsType
{
    None = -1,
    Wealth,
    FoodBox, // 0 : 일반, 1 : 고급
    Trophy,
    Drink,
    Package,
}

public enum LanguageType
{
    None = -1,
    Korea,
    English,
}

public enum GachaType
{
    None = -1,
    NormalBox,
    SpeicalBox,
}

public enum FoodType
{
    None = -1,
    Normal,
    Source,
    Stone,
}

public enum SoundType
{
    None = -1,
    BGM_Lobby,
    BGM_Store,
    BGM_Battle_0,
    BGM_Battle_1,
    BGM_Battle_2,
    SFX_baseExplosion,
    SFX_box_open,
    SFX_btn_card,
    SFX_btn_press,
    SFX_btn_start1,
    SFX_explosion0,
    SFX_explosion1,
    SFX_fire_machinegun0,
    SFX_fire_machinegun1,
    SFX_fire_missile0,
    SFX_fire_missile1,
    SFX_fire_pistol0,
    SFX_fire_pistol1,
    SFX_fire_rifle0,
    SFX_gameover_unitKilled,
    SFX_gamestart_crashdoor,
    SFX_hit_barricade,
    SFX_hit_flesh0,
    SFX_hit_flesh1,
    SFX_hit_flesh2,
    SFX_item_used,
    SFX_skill_Chicken0,
    SFX_skill_Chicken1,
    SFX_skill_lazer,
    SFX_UI_ItemAdded,
    SFX_weapon_chainsaw,
    SFX_weapon_magicboy,
}

public enum SaveType
{
    Wealth_Gold,
    Wealth_Mineral,

    Stage_Normal,
    Stage_Special,

    Party_Member_zzzSlotIDzzz_UnitID,

    Unit_zzzUnitIDzzz_Level,
    Unit_zzzUnitIDzzz_FoodHaveNum,
    Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodType,
    Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodID,
    Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodLevel,
    Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_FoodExp,
    Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosX,
    Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_PosY,
    Unit_zzzUnitIDzzz_zzzFoodHaveIDzzz_RotZ,

    Hero_Level,
    Hero_FoodHaveNum,
    Hero_zzzFoodHaveIDzzz_FoodType,
    Hero_zzzFoodHaveIDzzz_FoodID,
    Hero_zzzFoodHaveIDzzz_FoodLevel,
    Hero_zzzFoodHaveIDzzz_FoodExp,
    Hero_zzzFoodHaveIDzzz_PosX,
    Hero_zzzFoodHaveIDzzz_PosY,
    Hero_zzzFoodHaveIDzzz_RotZ,

    Inventory_FoodHaveNum,
    Inventory_zzzFoodHaveIDzzz_FoodType,
    Inventory_zzzFoodHaveIDzzz_FoodID,
    Inventory_zzzFoodHaveIDzzz_FoodLevel,
    Inventory_zzzFoodHaveIDzzz_FoodExp,

    Trophy_zzzTrophyIDzzz_HaveNum,

    Skill_zzzSkillIDzzz_Level,
    Skill_zzzSlotIDzzz_SkillID,

    Drink_zzzDrinkIDzzz_HaveNum,

    Store_zzzPackageIDzzz_IsBuyRecord,

    Player_IsContinuePlayer,
    Player_zzzTutorialIDzzz_IsClear,
    Player_BGM,
    Player_SFX,
}