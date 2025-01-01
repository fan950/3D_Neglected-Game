using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shouting : Skill_Monster
{
    private BoxCollider boxCollider;

    private float fTime = 0;
    private float fDamageTime = 0;
    private bool bMeet;

    private const float fDamageStand = 0.3f;
    private const float fPosY = 3;

    public override void Init(Monster monster, Transform pos)
    {
        base.Init(monster, pos);
        fTime = 0;
        transform.position = new Vector3(pos.position.x, pos.position.y + fPosY, pos.position.z);
        transform.localRotation = monster.transform.localRotation;

        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }
        boxCollider.enabled = false;
        fDamageTime = fDamageStand;
    }

    public override void Update()
    {
        fTime += Time.deltaTime;
        if (fTime >= 0.7f && !boxCollider.enabled)
        {
            boxCollider.enabled = true;
        }

        base.Update();

        if (bMeet)
        {
            fDamageTime += Time.deltaTime;
            if (fDamageTime >= fDamageStand)
            {
                ModelManager.Instance.Monster_Calculate_Damage(monster);
                fDamageTime = 0;
            }
        }
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bMeet = true;
            fDamageTime = fDamageStand;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            bMeet = false;
        }
    }
}
