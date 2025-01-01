using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fireball : Skill_Player
{
    protected const float fRotate_Speed = 1000;
    private const float fSpeed_Move = 10;

    private int nDamage;
    public override void Init(int nIndex)
    {
        base.Init(nIndex);

        Vector3 _direction = ModelManager.Instance.player.Get_TargetObj.transform.position - ModelManager.Instance.player.transform.position;
        Quaternion _targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, fRotate_Speed);
        transform.position = ModelManager.Instance.player.transform.position;

        nDamage = (int)(ModelManager.Instance.player.nAttack + ModelManager.Instance.player.nAttack * ((skill_Data.skillData.fValue * (skill_Data.nLevel * skill_Data.skillData.fUpgrade_Value)) * 0.0001f));
    }
    public override void Update_Skil()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * fSpeed_Move));
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ModelManager.Instance.Play_Calculate_Damage(other.gameObject, nDamage);
            Die_Skil();
        }
    }
}
