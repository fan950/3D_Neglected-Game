using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blink : Skill_Player
{
    public Skill_Blink_Missile skill_Blink_Missile;
    public Skill_Blink_Explosion skill_Blink_Explosion;

    private int nDamage;
    public override void Init(int nIndex)
    {
        base.Init(nIndex);

        Vector3 _direction = ModelManager.Instance.player.Get_TargetObj.transform.position - ModelManager.Instance.player.transform.position;
        Quaternion _targetRotation = Quaternion.LookRotation(_direction);
        transform.localScale = Vector3.one;
        transform.position = ModelManager.Instance.player.transform.position;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

        nDamage = (int)(ModelManager.Instance.player.nAttack + ModelManager.Instance.player.nAttack * ((skill_Data.skillData.fValue * (skill_Data.nLevel * skill_Data.skillData.fUpgrade_Value)) * 0.0001f));

        skill_Blink_Missile.Init(delegate
        {
            skill_Blink_Explosion.transform.position = skill_Blink_Missile.transform.position;
            skill_Blink_Missile.gameObject.SetActive(false);
            skill_Blink_Explosion.gameObject.SetActive(true);
        });
        skill_Blink_Explosion.Init(nDamage, Die_Skil);
    }
    public override void Update_Skil()
    {
        if (skill_Blink_Missile.gameObject.activeSelf)
        {
            skill_Blink_Missile.Update_Skil();
        }
        else if (skill_Blink_Explosion.gameObject.activeSelf)
        {
            skill_Blink_Explosion.Update_Skil();
        }
    }

    public override void Die_Skil()
    {
        base.Die_Skil();
    }
}
