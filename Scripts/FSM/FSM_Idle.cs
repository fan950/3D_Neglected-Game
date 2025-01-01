using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Idle : FSM
{
    private string sIdle_Name = "Idle";
    public FSM_Idle(bool bFast)
    {
        if (bFast)
            sIdle_Name = "Fast_Idle";
        else
            sIdle_Name = "Idle";
    }
    public override void Start_FSM(Model model)
    {
        model.Nav_Stop(true);
        model.Play_AniTrigger(sIdle_Name);
    }
    public override void Update_FSM(Model model)
    {
        model.Idle();
    }
}
