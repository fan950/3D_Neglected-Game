using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Attack_Player : FSM
{
    protected Player player;
    public override void Start_FSM(Model model)
    {
        if (player == null)
            player = model as Player;
        player.bAttack = false;
        player.fAttack_Time = player.fAttack_Speed;
    }
    public override void Update_FSM(Model model)
    {
        AnimatorStateInfo stateInfo = model.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1.0f)
        {
            player.Set_FSM(eFsm_State.Idle);
            return;
        }

        player.Attack();
    }
    public override void End_FSM(Model model)
    {
        player.fAttack_Time = 0;
        player.bAttack = false;
    }
}
