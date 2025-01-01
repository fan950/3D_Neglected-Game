using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Model
{
    protected struct Model_State_Player
    {
        public Dictionary<ePart_Type, int> dicHp;
        public Dictionary<ePart_Type, int> dicDefend;

        public Dictionary<ePart_Type, List<SB_Equip_Item_State_Data>> dicEquip_State_Data;
        public void State_Clear(ePart_Type part_Type)
        {
            if (dicEquip_State_Data.ContainsKey(part_Type))
                dicEquip_State_Data[part_Type].Clear();
        }
        public void Set_Part_StateData(ePart_Type part_Type, SB_Equip_Item_State_Data stateData)
        {
            if (!dicEquip_State_Data.ContainsKey(part_Type))
            {
                dicEquip_State_Data.Add(part_Type, new List<SB_Equip_Item_State_Data>());
                return;
            }
            dicEquip_State_Data[part_Type].Add(stateData);
        }
        public void Set_Part_Hp(ePart_Type part_Type, int nHp)
        {
            if (!dicHp.ContainsKey(part_Type))
            {
                dicHp.Add(part_Type, nHp);
                return;
            }
            dicHp[part_Type] = nHp;
        }
        public void Set_Part_Defend(ePart_Type part_Type, int nDefend)
        {
            if (!dicDefend.ContainsKey(part_Type))
            {
                dicDefend.Add(part_Type, nDefend);
                return;
            }
            dicDefend[part_Type] = nDefend;
        }
        public float Get_State(eState_Type state_Type, float fPercent = 0.01f)
        {
            float _fSum = 0;
            foreach (KeyValuePair<ePart_Type, List<SB_Equip_Item_State_Data>> state in dicEquip_State_Data)
            {
                for (int i = 0; i < state.Value.Count; ++i)
                {
                    eState_Type _state_Data = TableManager.Instance.stateTable.Get_State_Type(state.Value[i].nIndex);
                    if (_state_Data == state_Type)
                    {
                        _fSum += state.Value[i].fValue;
                    }
                }
            }
            return _fSum * fPercent;
        }

        public int nDefend()
        {
            int _nSum = 0;
            foreach (KeyValuePair<ePart_Type, int> defend in dicDefend)
            {
                _nSum += defend.Value;
            }
            _nSum += (int)(_nSum * Get_State(eState_Type.Defend));
            _nSum += (int)GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Defend, _nSum);
            return _nSum;
        }
        public int nHp_Max(int nLevel_Hp)
        {
            int _nSum = nLevel_Hp;
            foreach (KeyValuePair<ePart_Type, int> nHp in dicHp)
            {
                _nSum += nHp.Value;
            }
            _nSum += (int)(_nSum * Get_State(eState_Type.Hp));
            _nSum += (int)GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Hp, _nSum);
            return _nSum;
        }
    }
    [HideInInspector] public Weapon_Player weapon_Player;
    [HideInInspector] public SkillData skillData;
    private BoxCollider boxCollider;
    private Monster monster;
    public int nMonster_Hp { get { return monster.nHp; } }
    private const float fBase_Move_Speed = 2.5f;
    private const float fBase_Critical_Damage = 100f;
    private const float fBase_Value = 1;
    [Header("Part")]
    public Transform weapon_Pos;
    public Transform head_Pos;
    public Transform body_Pos;

    public bool bAniMask;

    //State
    protected Model_State_Player model_State_Player;
    #region Property
    public int nLevel { get { return GameManager.Instance.localGame_DB.player_Data.nLevel; } set { GameManager.Instance.localGame_DB.player_Data.nLevel = value; } }
    public override int nHp
    {
        get
        {
            return GameManager.Instance.localGame_DB.player_Data.nHp;
        }
        set
        {
            if (value >= nHp_Max)
                value = nHp_Max;
            else if (value < 0)
                value = 0;

            GameManager.Instance.localGame_DB.player_Data.nHp = value;
        }
    }
    public float fExp { get { return GameManager.Instance.localGame_DB.player_Data.fExp; } set { GameManager.Instance.localGame_DB.player_Data.fExp = value; } }
    public int nDefend { get { return model_State_Player.nDefend(); } }
    public override int nAttack
    {
        get
        {
            int _nAttack = model_State_Common.nAttack + ((int)(model_State_Common.nAttack * model_State_Player.Get_State(eState_Type.Attack)));
            return _nAttack + (int)GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Attack, _nAttack);
        }
    }
    public float fMove_Speed
    {
        get
        {
            float _fMove = fBase_Move_Speed + ((fBase_Move_Speed * model_State_Player.Get_State(eState_Type.Move_Speed, 1)));
            return _fMove + GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Move_Speed, _fMove);
        }
    }
    public float fExp_Increment
    {
        get
        {
            return model_State_Player.Get_State(eState_Type.EXP_Increment) + GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.EXP_Increment, fBase_Value);
        }
    }
    public float fGold_Increment
    {
        get
        {
            return model_State_Player.Get_State(eState_Type.Gold_Increment) + GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Gold_Increment, fBase_Value);
        }
    }
    public float fFull_Item
    {
        get
        {
            return model_State_Player.Get_State(eState_Type.Pull_Item) + GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Pull_Item, fBase_Value, 0.5f);
        }
    }
    public float fCritical_Percent
    {
        get
        {
            float fCri = model_State_Player.Get_State(eState_Type.Critical_Percent, 1) + GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Critical_Percent, fBase_Value, fBase_Value);
            if (fCri > 100)
                return 100;
            else
                return fCri;
        }
    }
    public float fCritical_Damage
    {
        get
        {
            float _fCri = fBase_Critical_Damage + model_State_Player.Get_State(eState_Type.Critical_Damage, 1);
            return _fCri + GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Critical_Damage, fBase_Value, fBase_Value);
        }
    }
    public float fAbsord_blood
    {
        get
        {
            return model_State_Player.Get_State(eState_Type.Absorb_blood) + GameManager.Instance.localGame_DB.Get_Skill_State(eState_Type.Absorb_blood, fBase_Value);
        }
    }
    public int nAttack_Rank
    {
        get
        {
            if (monster != null)
                return monster.nWeapon_Rank;
            else
                return 99;
        }
    }
    #endregion
    public override void Init()
    {
        base.Init();
        boxCollider = GetComponent<BoxCollider>();
        Active_Box(true);
        LocalGameData localGame_DB = GameManager.Instance.localGame_DB;
        model_State_Player.dicHp = new Dictionary<ePart_Type, int>();
        model_State_Player.dicDefend = new Dictionary<ePart_Type, int>();
        model_State_Player.dicEquip_State_Data = new Dictionary<ePart_Type, List<SB_Equip_Item_State_Data>>();
        for (int i = 0; i < (int)ePart_Type.Max; ++i)
        {
            switch (i)
            {
                case (int)ePart_Type.Weapon:
                    if (localGame_DB.Get_Equip_ItemData(ePart_Type.Weapon) == null)
                        continue;
                    SB_Item_Data _item_Data_Weapon = localGame_DB.Get_Equip_ItemData(ePart_Type.Weapon);
                    WeaponData _weaponData = TableManager.Instance.weaponTable.Get_WeaponDate(_item_Data_Weapon.nIndex);

                    model_State_Common.nAttack = _weaponData.Get_Attack(_item_Data_Weapon);
                    model_State_Common.fAttack_Speed = _weaponData.fAttack_Speed;
                    model_State_Common.eAttack_Type = _weaponData.eAttack_Type;
                    model_State_Common.fAttack_Distance = _weaponData.fAttack_Distance;
                    fAttack_Speed = _weaponData.fAttack_Speed;
                    model_State_Player.State_Clear(ePart_Type.Weapon);
                    if (_item_Data_Weapon.lisState_Data != null)
                    {
                        for (int s = 0; s < _item_Data_Weapon.lisState_Data.Count; ++s)
                        {
                            model_State_Player.Set_Part_StateData(ePart_Type.Weapon, _item_Data_Weapon.lisState_Data[s]);
                        }
                    }
                    break;
                default:
                    if (localGame_DB.Get_Equip_ItemData((ePart_Type)i) == null)
                        continue;
                    SB_Item_Data _item_Data_Defend = localGame_DB.Get_Equip_ItemData((ePart_Type)i);

                    DefendData _defendData = TableManager.Instance.defendTable.Get_DefendData(_item_Data_Defend.nIndex);
                    model_State_Player.Set_Part_Defend((ePart_Type)i, _defendData.Get_Defend(_item_Data_Defend));
                    model_State_Player.Set_Part_Hp((ePart_Type)i, _defendData.nHp);

                    model_State_Player.State_Clear((ePart_Type)i);
                    if (_item_Data_Defend.lisState_Data != null)
                    {
                        for (int s = 0; s < _item_Data_Defend.lisState_Data.Count; ++s)
                        {
                            model_State_Player.Set_Part_StateData((ePart_Type)i, _item_Data_Defend.lisState_Data[s]);
                        }
                    }
                    break;
            }
        }
        Apply_State_Hp();
        model_State_Common.nHp = localGame_DB.player_Data.nHp;
        if (model_State_Common.nHp > nHp_Max)
        {
            model_State_Common.nHp = nHp_Max;
            localGame_DB.player_Data.nHp = nHp_Max;
        }

        dicFsm.Clear();
        dicFsm.Add(eFsm_State.Idle, new FSM_Idle(false));
        dicFsm.Add(eFsm_State.Move, new FSM_Move());
        dicFsm.Add(eFsm_State.Attack, new FSM_Attack_Player());
        dicFsm.Add(eFsm_State.Die, new FSM_Die());
        dicFsm.Add(eFsm_State.Skill, new FSM_Skill_Player());
        dicFsm.Add(eFsm_State.Joystick, new FSM_Joystick_Player());
        Set_FSM(eFsm_State.Idle);
    }
    public override void Idle()
    {
        if (UIManager.Instance.Is_Auto_Joystick())
        {
            if (animator.GetLayerWeight(1) != 0)
                animator.SetLayerWeight(1, 0);

            base.Idle();
        }
        else
        {
            Vector2 _dir = UIManager.Instance.Get_Joystick_Dir();
            if (_dir.x != 0 || _dir.y != 0)
            {
                Set_FSM(eFsm_State.Joystick);
            }
        }
    }
    public override void Set_Target(GameObject obj)
    {
        if (obj == null)
            return;

        base.Set_Target(obj);
        monster = ModelManager.Instance.Find_Monster(targetObj);
    }
    public void Active_Box(bool bActive)
    {
        boxCollider.enabled = bActive;
    }
    public override void Attack_Action(string sName = "Attack")
    {
        fAttack_Time += Time.deltaTime;
        if (!bAttack && model_State_Common.fAttack_Speed <= fAttack_Time)
        {
            fAttack_Time = 0;
            animator.SetFloat("Attack_Type", TableManager.Instance.weaponTable.Get_AniType(weapon_Player.nIndex));
            Play_AniTrigger(sName);
            bAttack = true;
        }
    }
    public void Set_Weapon(Weapon_Player weapon_Player)
    {
        this.weapon_Player = weapon_Player;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        switch (TableManager.Instance.weaponTable.Get_WeaponDate(weapon_Player.nIndex).eAttack_Type)
        {
            case eAttack_Type.TH_Sword:
            case eAttack_Type.TH_Axe:
                animator.SetFloat("Idle_Type", 0);
                break;
            case eAttack_Type.Sword:
            case eAttack_Type.Mace:
            case eAttack_Type.Axe:
            case eAttack_Type.Dagger:
            case eAttack_Type.Spell:
            case eAttack_Type.Scythe:
                animator.SetFloat("Idle_Type", 2);
                break;
            case eAttack_Type.Spear:
                animator.SetFloat("Idle_Type", 1);
                break;
        }
    }
    public override void Nav_Stop(bool bStop)
    {
        if (navMeshAgent.isStopped == bStop)
            return;

        if (navMeshAgent.isOnNavMesh == false)
        {
            nHp = 0;
            return;
        }

        if (bStop)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
        }
        else
        {
            navMeshAgent.ResetPath();
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = true;
        }
        navMeshAgent.isStopped = bStop;
    }
    public void Level_Up()
    {
        ++nLevel;
        Apply_State_Hp();
        nHp = model_State_Player.nHp_Max(TableManager.Instance.expTable.Get_Hp(nLevel));
    }
    public void Apply_State_Hp()
    {
        model_State_Common.nHp_Max = model_State_Player.nHp_Max(TableManager.Instance.expTable.Get_Hp(nLevel));
    }
    public void Apply_State(ePart_Type part_Type, SB_Item_Data item_Data)
    {
        switch (part_Type)
        {
            case ePart_Type.Weapon:
                WeaponData _weaponData = TableManager.Instance.weaponTable.Get_WeaponDate(item_Data.nIndex);
                model_State_Common.nAttack = _weaponData.Get_Attack(item_Data);
                model_State_Common.fAttack_Speed = _weaponData.fAttack_Speed;
                model_State_Common.eAttack_Type = _weaponData.eAttack_Type;
                model_State_Common.fAttack_Distance = _weaponData.fAttack_Distance;
                fAttack_Speed = _weaponData.fAttack_Speed;
                break;
            default:
                DefendData _defendData = TableManager.Instance.defendTable.Get_DefendData(item_Data.nIndex);
                model_State_Player.Set_Part_Defend(part_Type, _defendData.Get_Defend(item_Data));
                model_State_Player.Set_Part_Hp(part_Type, _defendData.nHp);
                model_State_Common.nHp_Max = model_State_Player.nHp_Max(TableManager.Instance.expTable.Get_Hp(nLevel));
                break;
        }
        if (nHp > nHp_Max)
            model_State_Common.nHp = model_State_Player.nHp_Max(TableManager.Instance.expTable.Get_Hp(nLevel));

        model_State_Player.State_Clear(part_Type);
        if (item_Data.lisState_Data != null)
        {
            for (int s = 0; s < item_Data.lisState_Data.Count; ++s)
            {
                model_State_Player.Set_Part_StateData(part_Type, item_Data.lisState_Data[s]);
            }
        }

        UIManager.Instance.Set_Hp();
    }
    public override void Move()
    {
        if (!UIManager.Instance.Is_Auto_Joystick())
        {
            Nav_Stop(true);
            Set_FSM(eFsm_State.Idle);
            return;
        }
        if (targetObj == null)
        {
            Set_FSM(eFsm_State.Idle);
            return;
        }
        Distance_In_Target(delegate
        {
            if (navMeshAgent.isOnNavMesh == false)
            {
                nHp = 0;
                return;
            }
            navMeshAgent.speed = fMove_Speed;
            NavMeshPath path = new NavMeshPath();
            navMeshAgent.CalculatePath(targetObj.transform.position, path);
            navMeshAgent.SetPath(path);
        },
        delegate
        {
            fAttack_Time = fAttack_Speed;
            Set_FSM(eFsm_State.Attack);
            Nav_Stop(true);
        }
        );
    }
    public void JoyStick_Move()
    {
        Vector2 _dir = UIManager.Instance.Get_Joystick_Dir();
        if (_dir.x != 0 || _dir.y != 0)
        {
            if (current_State != eFsm_State.Skill)
                Play_AniTrigger("Run");

            Vector3 _worldDir = new Vector3(_dir.x, 0, _dir.y);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _worldDir, Time.deltaTime * fMove_Speed);

            Quaternion _targetRotation = Quaternion.LookRotation(_worldDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, fRotate_Speed * Time.deltaTime);
        }
        else
        {
            if (current_State != eFsm_State.Skill)
            {
                Nav_Stop(true);
                Play_AniTrigger("Idle");
            }
        }
    }
    public void JoyStick_Attack()
    {
        if (!bAttack)
        {
            Attack_Action("Attack_Mask");
            bAniMask = false;
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                if (!bAniMask)
                    Play_AniTrigger("Idle_Mask");
                bAniMask = true;
                bAttack = false;
                fAttack_Time = 0;
            }
        }
    }
}
