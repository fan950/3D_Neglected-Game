using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Lightning_Strike : Skill_Player
{
    private bool bDamage = false;
    private float fDamage_Max = 0.2f;
    private float fDamage_Time;

    private Monster monster;
    private int nDamage;
    public override void Init(int nIndex)
    {
        base.Init(nIndex);

        nDamage = (int)(ModelManager.Instance.player.nAttack + ModelManager.Instance.player.nAttack * ((skill_Data.skillData.fValue * (skill_Data.nLevel * skill_Data.skillData.fUpgrade_Value)) * 0.0001f));

        monster = ModelManager.Instance.Random_Monster();

        if (monster == null)
            return;

        transform.position = monster.transform.position;

        bDamage = false;
    }
    public override void Update_Skil()
    {
        if (particleSystem.isStopped)
        {
            Die_Skil();
            monster = null;
            return;
        }

        if (monster == null)
            return;

        if (!bDamage)
        {
            fDamage_Time += Time.deltaTime;

            if (fDamage_Time >= fDamage_Max)
            {
                bDamage = true;

                ModelManager.Instance.Play_Calculate_Damage(monster.gameObject, nDamage);
            }
        }

        transform.position = monster.transform.position;
    }
}
