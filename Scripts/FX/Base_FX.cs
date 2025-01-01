using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_FX : MonoBehaviour
{
    [HideInInspector] public string sKey;

    public ParticleSystem particleSystem;

    public virtual void Init(string sKey)
    {
        this.sKey = sKey;
    }
    public virtual void Update_FX()
    {
        FX_Update_Action();

        if (particleSystem.isStopped)
        {
            Die_FX();
            return;
        }
    }
    public virtual void Die_FX()
    {
        FXManager.Instance.Die_FX(this);
    }

    public virtual void FX_Update_Action() { }
}
