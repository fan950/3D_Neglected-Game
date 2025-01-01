using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_GoblinKing : Monster
{
    [SerializeField] private Skill_MoveType shouting_Skill;
    [SerializeField] private Skill_MoveType downHit_Skill;

    private const string sShouting = "Shouting";
    private const string sDownHit = "HitGround";

    [HideInInspector] public int nCollider_Type;
    public override void Init(int nIndex)
    {
        base.Init(nIndex);
        dicFsm.Add(eFsm_State.Skill, new FSM_Skill_Monster());
        shouting_Skill.Init(delegate
        {
            nCollider_Type = 1;
            Play_AniTrigger(sShouting);
            shouting_Skill.gameObject.SetActive(false);
        });
        downHit_Skill.Init(delegate
        {
            nCollider_Type = 2;
            Play_AniTrigger(sDownHit);
            downHit_Skill.gameObject.SetActive(false);
        });
    }

    public override void Idle()
    {
        if (targetObj != null)
        {
            Distance_In_Target(delegate { Set_FSM(eFsm_State.Move); }, delegate { Set_FSM(eFsm_State.Attack); });
        }
    }
    public void Skill_Shouting()
    {
        shouting_Skill.Play_SkillPos();
    }
    public void Skill_HitGround()
    {
        downHit_Skill.Play_SkillScale();
    }
    public override void Attack_Action(string sName = "Attack")
    {
        fAttack_Time += Time.deltaTime;
        if (!bAttack && model_State_Common.fAttack_Speed <= fAttack_Time)
        {
            fAttack_Time = 0;
            nCollider_Type = 0;
            bAttack = true;

            int _nAttack = Random.Range(0, 100);
            if (_nAttack <= 35)
            {
                Set_FSM(eFsm_State.Skill);
                return;
            }

            Play_AniTrigger(sName);
        }
    }

    public override void On_Collider()
    {
        switch (nCollider_Type)
        {
            case 0:
                base.On_Collider();
                break;
            case 1:
                if (shouting_Skill.skill == null)
                {
                    Skill_Shouting _shouting = ModelManager.Instance.Monster_Skill(sShouting) as Skill_Shouting;
                    shouting_Skill.skill = _shouting;
                }
                shouting_Skill.skill.Init(this, transform);
                break;
            case 2:
                if (downHit_Skill.skill == null)
                {
                    Skill_HitGround _downHit = ModelManager.Instance.Monster_Skill(sDownHit) as Skill_HitGround;
                    downHit_Skill.skill = _downHit;
                }
                downHit_Skill.skill.Init(this, downHit_Skill.range_Obj.transform);
                break;
        }
    }
    public override void Off_Collider()
    {
        switch (nCollider_Type)
        {
            case 0:
                base.Off_Collider();
                break;
            case 1:
                bSkilling = true;
                break;
            case 2:
                bSkilling = true;
                break;
        }
    }
    public override void Die()
    {
        shouting_Skill.End_Skill();
        downHit_Skill.End_Skill();

        GameManager.Instance.inGameScene.Clear_Stage();
        ModelManager.Instance.player.Active_Box(false);
    }
}
