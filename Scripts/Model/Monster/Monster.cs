using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Model
{
    public Weapon_Monster weapon_Monster;

    public Transform offset_HeadPos;
    public Transform offset_HitPos;

    [HideInInspector] public eMonster_Type monster_Type;

    [HideInInspector] public int nWeapon_Rank;
    [HideInInspector] public bool bSkilling = false;
    public virtual void Init(int nIndex)
    {
        this.nIndex = nIndex;

        Init();
        float _fReinforce = TableManager.Instance.stageTable.Get_Reinforce(GameManager.Instance.localGame_DB.Get_Stage());
        MonsterData _monsterData = TableManager.Instance.monsterTable.Get_MonsterDate(nIndex);
        model_State_Common.nHp = (int)(_monsterData.nHp * _fReinforce);
        model_State_Common.nHp_Max = (int)(_monsterData.nHp * _fReinforce);
        model_State_Common.nAttack = (int)(_monsterData.nAttack * _fReinforce);
        model_State_Common.fAttack_Speed = _monsterData.fAttack_Speed * _fReinforce;
        model_State_Common.eAttack_Type = _monsterData.eAttack_Type;
        model_State_Common.fAttack_Distance = _monsterData.fAttack_Distance;
        monster_Type = _monsterData.monster_Type;
        animator.SetInteger("Attack_Type", TableManager.Instance.monsterTable.Get_AniType(nIndex));

        fAttack_Speed = model_State_Common.fAttack_Speed;

        switch (eAttack_Type)
        {
            case eAttack_Type.Spell:
                nWeapon_Rank = 1;
                break;
            default:
                nWeapon_Rank = 2;
                break;
        }

        weapon_Monster.Init(this);

        dicFsm.Clear();
        dicFsm.Add(eFsm_State.Idle, new FSM_Idle(false));
        dicFsm.Add(eFsm_State.Move, new FSM_Move());
        dicFsm.Add(eFsm_State.Attack, new FSM_Attack_Monster());
        dicFsm.Add(eFsm_State.Die, new FSM_Die());

        Set_FSM(eFsm_State.Idle);
        navMeshAgent.speed = _monsterData.fMove_Speed;
    }
    public override void Attack()
    {
        if (current_State != eFsm_State.Attack)
        {
            return;
        }
        if (targetObj == null || nHp <= 0)
        {
            Set_FSM(eFsm_State.Idle);
            fAttack_Time = 0;
            bAttack = false;
            return;
        }

        Attack_Action();
    }
    public override void Attack_Action(string sName = "Attack")
    {
        fAttack_Time += Time.deltaTime;
        if (!bAttack && model_State_Common.fAttack_Speed <= fAttack_Time)
        {
            fAttack_Time = 0;

            Play_AniTrigger(sName);
            bAttack = true;
        }
    }
    public virtual void On_Collider()
    {
        weapon_Monster.Switch_Collider(true);
    }
    public virtual void Off_Collider()
    {
        weapon_Monster.Switch_Collider(false);
    }
}
