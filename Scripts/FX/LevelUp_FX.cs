using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp_FX : Base_FX
{
    public override void FX_Update_Action() 
    {
        transform.position = ModelManager.Instance.player.transform.position;
    }

}
