using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Skill : FSM
{
    protected Action action_Update;

    public override void Start_FSM(Model model){}
    public override void Update_FSM(Model model)
    {
        if (action_Update != null)
            action_Update();
    }
    public override void End_FSM(Model model)
    {
        action_Update = null;
    }
}
