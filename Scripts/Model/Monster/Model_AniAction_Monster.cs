using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_AniAction_Monster : Model_AniAction
{
    private Monster monster;
    protected override void Start()
    {
        monster = GetComponent<Monster>();
        if (monster == null)
            monster = GetComponentInParent<Monster>();
    }
    public override void On_Collider()
    {
        monster.On_Collider();
    }
    public override void Off_Collider()
    {
        monster.Off_Collider();
    }

    public override void End_Die()
    {
        ModelManager.Instance.Remove_Monster(monster);
    }

    public override void Move()
    {
        monster.Nav_Stop(false);
    }
}
