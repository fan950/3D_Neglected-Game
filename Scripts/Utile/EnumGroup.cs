public enum eLocalize_Type
{
    Kr,
    En,
}
public enum eTable_Type
{
    None = -1,
    MonsterTable,
    WeaponTable,
    StageTable,
    DefendTable,
    ExpTable,
    StringTable,
    SkillTable,
    ShopTable,
    ShopExpTable,
    StateTable,
    Max,
}

public enum eFsm_State
{
    Idle,
    Attack,
    Move,
    Skill,
    Die,
    Joystick,
}

public enum eAttack_Type
{
    Sword,
    TH_Sword,
    Mace,
    Axe,
    TH_Axe,
    Spear,
    Dagger,
    Spell,
    Scythe,
}
public enum eAni_Type
{
    Normal,
    Melee,
    Sword,
    Spear,
    OneHanded,
    Stab,
}
public enum eSkill_Name
{
    Sword_Spin = 4,
}
public enum eDefend_Type
{
    None,
    Head_1,
    Head_2,
    Mask,
    Back,
    Body,
    Max,
}
public enum eSkill_Type
{
    Damage,
    Buff,
    Heal,
}

public enum eState_Type
{
    None = -1,
    Attack,
    Defend,
    Hp,
    Move_Speed,
    Critical_Percent,
    Critical_Damage,
    EXP_Increment,
    Gold_Increment,
    Absorb_blood,
    Pull_Item,
    Max,
}

public enum ePart_Type
{
    None = -1,
    Head_1,
    Head_2,
    Weapon,
    Back,
    Mask,
    Body,
    Max,
}
public enum eAtlas_Type
{
    None = -1,
    Weapon_Atlas,
    Defend_Atlas,
    UISkill_Atlas,
    UIShop_Atlas,
    UICommon_Atlas,
    Max,
}

public enum eUIWindow_Type
{
    UIScreen,
    UIInventory,
    UIHUD,
    UIShop,
    UIReinforce,
    UISkill,
    UIFade,
    UIClearEnd,
    UIStart,
}
public enum eUIPopup_Type
{
    UIItem_Popup,
    UIBuy_Popup,
    UIOk_Popup,
    UICommon_Popup,
    UIStage_Popup,
}
public enum eBuy_Type
{
    Gold,
    Crystal,
}
public enum eShop_Value
{
    Gold,
    Crystal,
    Item,
}

public enum eSkill_MoveType
{
    Pos,
    Scale,
}

public enum eMonster_Type
{
    Normal,
    Boss,
}