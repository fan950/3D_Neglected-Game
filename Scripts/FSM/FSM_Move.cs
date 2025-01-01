using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FSM_Move : FSM
{
    private const string sRun_Name = "Run";
    public override void Start_FSM(Model model)
    {
        model.Play_AniTrigger(sRun_Name);
    }
    public override void Update_FSM(Model model)
    {
        model.Move();
    }
}