using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fire_Spirit : Skill_Player
{
    private int nDamage;
    private const float fDamage_Time = 1.0f;

    private Dictionary<GameObject, float> dicDamageTime = new Dictionary<GameObject, float>();
    private List<GameObject> lisLive_Obj = new List<GameObject>();
    private List<GameObject> lisDie_Obj = new List<GameObject>();

    public override void Init(int nIndex)
    {
        base.Init(nIndex);

        nDamage = (int)(ModelManager.Instance.player.nAttack + ModelManager.Instance.player.nAttack * ((skill_Data.skillData.fValue * (skill_Data.nLevel * skill_Data.skillData.fUpgrade_Value)) * 0.0001f));
        transform.position = ModelManager.Instance.player.transform.position;
    }
    public override void Update_Skil()
    {
        transform.position = ModelManager.Instance.player.transform.position;

        if (particleSystem.isStopped)
        {
            Die_Skil();
            return;
        }

        if (lisLive_Obj.Count > 0)
        {
            for (int i = 0; i < lisLive_Obj.Count; ++i)
            {
                dicDamageTime[lisLive_Obj[i]] += Time.deltaTime;

                if (dicDamageTime[lisLive_Obj[i]] >= fDamage_Time)
                {
                    Monster _mob = ModelManager.Instance.Play_Calculate_Damage(lisLive_Obj[i], nDamage);

                    if (_mob == null || _mob.nHp <= 0)
                    {
                        lisDie_Obj.Add(lisLive_Obj[i]);
                    }
                    dicDamageTime[lisLive_Obj[i]] = 0;
                }
            }
        }
        if (lisDie_Obj.Count > 0)
        {
            for (int i = 0; i < lisDie_Obj.Count; ++i)
            {
                lisLive_Obj.Remove(lisDie_Obj[i]);
                dicDamageTime.Remove(lisDie_Obj[i]);
            }
            lisDie_Obj.Clear();
        }
    }
    public override void Die_Skil()
    {
        lisLive_Obj.Clear();
        dicDamageTime.Clear();
        lisDie_Obj.Clear();

        ModelManager.Instance.Die_Skill(this);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            if (!lisLive_Obj.Contains(other.gameObject))
            {
                lisLive_Obj.Add(other.gameObject);
                dicDamageTime.Add(other.gameObject, skill_Data.skillData.fCoolTime);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            if (dicDamageTime.ContainsKey(other.gameObject))
            {
                lisDie_Obj.Add(other.gameObject);
            }
        }
    }
}
