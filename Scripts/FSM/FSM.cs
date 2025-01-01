using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM
{
    public abstract void Start_FSM(Model model);

    public virtual void Update_FSM(Model model) { }

    public virtual void End_FSM(Model model) { }

}
