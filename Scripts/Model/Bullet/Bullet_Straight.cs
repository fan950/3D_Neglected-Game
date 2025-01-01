using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Straight : Bullet
{
    protected float fLife_Time;

    protected const float fLife_Limit_Time = 10f;
    protected const float fRotate_Speed = 1000;
    public override void Init(string sKey_Name, Model model, int nAttack, float fSpeed_Move = 12)
    {
        base.Init(sKey_Name, model, nAttack, fSpeed_Move);

        Vector3 _direction = ModelManager.Instance.player.transform.position - model.transform.position;
        Quaternion _targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, fRotate_Speed);
        fLife_Time = 0;
    }
    public override void Update_Bullet()
    {
        if (bRemove)
            return;

        fLife_Time += Time.deltaTime;
        if (fLife_Time >= fLife_Limit_Time)
        {
            Die_Bullet();
            return;
        }
        transform.Translate(Vector3.forward * (Time.deltaTime * fSpeed_Move));
    }
}
