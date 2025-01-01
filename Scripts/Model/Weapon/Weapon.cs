using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected int nAttack;

    protected BoxCollider boxCollider;

    protected abstract void Calculate_Damage(GameObject obj);

    public virtual void Init(Model model)
    {
        this.nAttack = model.nAttack;

        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
    }
    public void Switch_Collider(bool bSwitch)
    {
        boxCollider.enabled = bSwitch;
    }
}
