using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill_Player : MonoBehaviour
{
    public ParticleSystem particleSystem;
    [HideInInspector] public int nIndex;
    [HideInInspector] public SB_Skill_Data skill_Data;

    public virtual void Init(int nIndex)
    {
        this.nIndex = nIndex;
        skill_Data = GameManager.Instance.localGame_DB.Get_SkillData(nIndex);
    }
    public virtual void Update_Skil()
    {

    }
    public virtual void Die_Skil()
    {
        ModelManager.Instance.Die_Skill(this);
    }
}
