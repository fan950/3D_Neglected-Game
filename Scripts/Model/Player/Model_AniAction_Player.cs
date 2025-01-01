using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_AniAction_Player : Model_AniAction
{
    private Player player;
    protected override void Start()
    {
        player = GetComponentInParent<Player>();
    }
    public override void On_Collider()
    {
        player.weapon_Player.Switch_Collider(true);
    }
    public override void Off_Collider()
    {
        player.weapon_Player.Switch_Collider(false);
    }

    public override void End_Die()
    {
    }
    public override void Move()
    {
        player.Nav_Stop(false);
    }
}
