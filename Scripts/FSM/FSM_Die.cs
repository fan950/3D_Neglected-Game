using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Die : FSM
{
    private const string sDie_Name = "Die";
    public override void Start_FSM(Model model)
    {
        model.Nav_Stop(true);
        model.Play_AniTrigger(sDie_Name); 
        model.Die();
    }
}
