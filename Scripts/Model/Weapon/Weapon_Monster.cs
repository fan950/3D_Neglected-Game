using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Monster : Weapon
{
    protected Monster monster;
    public override void Init(Model model)
    {
        base.Init(model);
        monster = model as Monster;
    }
    protected override void Calculate_Damage(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            ModelManager.Instance.Monster_Calculate_Damage(monster);
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        Calculate_Damage(other.gameObject);
    }
}
