using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Monster : MonoBehaviour
{
    public ParticleSystem particleSystem;
    protected Monster monster;
    public virtual void Init(Monster monster, Transform pos)
    {
        gameObject.SetActive(true);
        this.monster = monster;
    }

    protected virtual void Calculate_Damage(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            ModelManager.Instance.Monster_Calculate_Damage(monster);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Calculate_Damage(other.gameObject);
    }
    public virtual void Update()
    {
        if (particleSystem.isStopped)
        {
            gameObject.SetActive(false);
        }
    }
}
