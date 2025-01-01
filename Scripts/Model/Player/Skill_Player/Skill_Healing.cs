using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Healing : Skill_Player
{
    private int nHealing;
    public override void Init(int nIndex)
    {
        base.Init(nIndex);

        transform.position = ModelManager.Instance.player.transform.position;
        nHealing = (int)((skill_Data.skillData.fValue * (skill_Data.nLevel * skill_Data.skillData.fUpgrade_Value)));
        ModelManager.Instance.Player_Healing(nHealing);
    }
    public override void Update_Skil()
    {
        transform.position = ModelManager.Instance.player.transform.position;

        if (particleSystem.isStopped)
        {
            Die_Skil();
            return;
        }
    }
}
