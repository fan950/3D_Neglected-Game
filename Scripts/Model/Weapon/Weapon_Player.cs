using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Player : Weapon
{
    private WeaponData weaponData;
    private Weapon_Collider weapon_Collider;
    public int nIndex { get { return weaponData.nIndex; } }
    public int nWeapon_Attack { get { return nAttack; } }
    public float fWeapon_Speed { get { return weaponData.fAttack_Speed; } }
    public void Init(WeaponData weaponData, SB_Item_Data item_Data)
    {
        this.weaponData = weaponData;
        nAttack = weaponData.Get_Attack(item_Data);

        if (boxCollider == null)
        {
            boxCollider = GetComponentInChildren<BoxCollider>();
            if (weapon_Collider == null)
                weapon_Collider = boxCollider.gameObject.AddComponent<Weapon_Collider>();
            weapon_Collider.action = Calculate_Damage;
        }
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
    }

    protected override void Calculate_Damage(GameObject obj)
    {
        if (obj.tag == "Monster")
        {
            ModelManager.Instance.Play_Calculate_Damage(obj, nAttack);
        }
    }
}
