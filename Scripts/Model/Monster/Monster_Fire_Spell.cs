using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Fire_Spell : Monster
{
    private GameObject launcherObj;
    private const string sBullet_Name = "Fire_Bullet";
    public override void Init(int nIndex)
    {
        base.Init(nIndex);

        Weapon_LongDistance_Monster _weapon_LongDistance = weapon_Monster as Weapon_LongDistance_Monster;
        launcherObj = _weapon_LongDistance.launcherPos.gameObject;
    }
    public override void Attack()
    {
        LookAt_Target(targetObj);
        base.Attack();
    }
    public override void On_Collider()
    {
        BulletManager.Instance.Create_Bullet(this, launcherObj, nIndex, sBullet_Name);
    }
}
