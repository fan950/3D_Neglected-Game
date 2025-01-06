using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Skill_Monster : FSM_Skill
{
    private Monster monster;
    private const string sAni_Idle = "Idle";
    public override void Start_FSM(Model model)
    {
        monster = model as Monster;
        switch (model.nIndex)
        {
            case 9:
                model.Play_AniTrigger(sAni_Idle);
                Monster_GoblinKing _monster_GoblinKing = model as Monster_GoblinKing;
                int _nSkill = Random.Range(0, TableManager.Instance.monsterTable.Get_SkillCount(model.nIndex));
                switch (_nSkill)
                {
                    case 0:
                        _monster_GoblinKing.Skill_Shouting();
                        break;
                    case 1:
                        _monster_GoblinKing.Skill_HitGround();
                        break;
                }
                break;
        }
    }
    public override void Update_FSM(Model model)
    {
        if (monster.bSkilling)
        {
            model.LookAt_Target(ModelManager.Instance.player.gameObject);

            AnimatorStateInfo stateInfo = model.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                monster.Set_FSM(eFsm_State.Idle);
                monster.bSkilling = false;
                return;
            }
        }
        if (action_Update != null)
            action_Update();
    }
    public override void End_FSM(Model model)
    {
        action_Update = null;

        monster.fAttack_Time = 0;
        monster.bAttack = false;
    }
}