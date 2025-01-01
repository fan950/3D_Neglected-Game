using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HitGround : Skill_Monster
{
    public override void Init(Monster monster, Transform pos)
    {
        GameManager.Instance.inGameScene.Set_ShakeCamera();

        base.Init(monster, pos);
        transform.localRotation = Quaternion.identity;
        transform.position = pos.position;

    }
}
