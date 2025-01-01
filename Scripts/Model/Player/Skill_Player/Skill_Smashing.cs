using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Smashing : Skill_Player
{
    public Smashing_Player collider_Player;
    protected const float fRotate_Speed = 1000;

    private int nDamage;
    public override void Init(int nIndex)
    {
        base.Init(nIndex);

        Vector3 _direction = ModelManager.Instance.player.Get_TargetObj.transform.position - ModelManager.Instance.player.transform.position;
        Quaternion _targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, fRotate_Speed);
        transform.position = ModelManager.Instance.player.transform.position;

        nDamage = (int)(ModelManager.Instance.player.nAttack + ModelManager.Instance.player.nAttack * ((skill_Data.skillData.fValue * (skill_Data.nLevel * skill_Data.skillData.fUpgrade_Value)) * 0.0001f));
        collider_Player.Init(nDamage);
    }
    public override void Update_Skil()
    {
        if (particleSystem.isStopped)
        {
            Die_Skil();
            return;
        }
        collider_Player.Update_Skil();
    }
}
