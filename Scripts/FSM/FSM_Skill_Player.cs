using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Skill_Player : FSM_Skill
{
    private Player player;

    private Skill_Sword_Spin skill_Sword_Spin;

    public override void Start_FSM(Model model)
    {
        if (player == null)
        {
            player = model as Player;
        }

        switch (player.skillData.nIndex)
        {
            case (int)eSkill_Name.Sword_Spin:
                if (skill_Sword_Spin == null)
                {
                    skill_Sword_Spin = new Skill_Sword_Spin();
                    skill_Sword_Spin.Init(player.skillData.nIndex);
                }
                player.Nav_Stop(false);

                skill_Sword_Spin.Start_Skill();
                player.Play_AniTrigger(skill_Sword_Spin.sAni_Name);
                action_Update = () =>
                 {
                     skill_Sword_Spin.Update_Skil();

                     if (!UIManager.Instance.Is_Auto_Joystick())
                     {
                         player.JoyStick_Move();
                     }
                 };
                break;
        }
    }
}
