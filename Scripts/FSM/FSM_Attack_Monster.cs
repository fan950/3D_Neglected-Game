using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Attack_Monster : FSM
{
    protected Monster monster;
    public override void Start_FSM(Model model)
    {
        model.bAttack = false;

        if (monster == null)
            monster = model as Monster;

        UIManager.Instance.Set_HpBar(monster);
        monster.fAttack_Time = monster.fAttack_Speed;
    }
    public override void Update_FSM(Model model)
    {
        AnimatorStateInfo stateInfo = model.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f)
        {
            monster.Set_FSM(eFsm_State.Idle);
            return;
        }

        monster.Attack();
    }
    public override void End_FSM(Model model)
    {
        monster.fAttack_Time = 0;
        monster.bAttack = false;
    }
}
