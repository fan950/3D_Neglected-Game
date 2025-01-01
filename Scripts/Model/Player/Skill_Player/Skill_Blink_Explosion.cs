using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill_Blink_Explosion : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private int nDamage;
    private Action die_Action;

    public void Init(int nDamage, Action die_Action)
    {
        this.nDamage = nDamage;
        this.die_Action = die_Action;

        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();

        gameObject.SetActive(false);
    }
    public void Update_Skil()
    {
        if (particleSystem.isStopped)
        {
            die_Action();
            return;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ModelManager.Instance.Play_Calculate_Damage(other.gameObject, nDamage);
        }
    }

}
