using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Joystick_Player : FSM
{
    private Player player;
    public override void Start_FSM(Model model)
    {
        if (player == null)
            player = model as Player;

        player.animator.SetTrigger("Run");
    }
    public override void Update_FSM(Model model)
    {
        if (UIManager.Instance.Is_Auto_Joystick())
        {
            player.Nav_Stop(true);
            player.Set_FSM(eFsm_State.Idle);
            return;
        }
        player.JoyStick_Move();
        player.JoyStick_Attack();
    }
    public override void End_FSM(Model model)
    {
        player.animator.SetLayerWeight(1, 0);
    }
}
