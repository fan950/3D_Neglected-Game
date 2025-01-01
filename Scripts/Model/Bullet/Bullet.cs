using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public string sKey_Name;
    protected float fSpeed_Move;
    [HideInInspector] public bool bRemove;
    protected int nAttack;

    protected Model model;
    public virtual void Init(string sKey_Name, Model model, int nAttack, float fSpeed_Move = 12)
    {
        this.sKey_Name = sKey_Name;
        this.model = model;
        this.nAttack = nAttack;
        this.fSpeed_Move = fSpeed_Move;

        bRemove = false;
    }
    public virtual void Update_Bullet() { }
    public virtual void Die_Bullet()
    {
        bRemove = true;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (model.tag == "Player" && other.tag == "Monster")
        {
            bRemove = true;

            ModelManager.Instance.Play_Calculate_Damage(other.gameObject, nAttack);
        }
        else if (model.tag == "Monster" && other.tag == "Player")
        {
            bRemove = true;

            ModelManager.Instance.Monster_Calculate_Damage(model as Monster);
        }
    }
}
