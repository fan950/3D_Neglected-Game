using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smashing_Player : MonoBehaviour
{
    private int nDamage;
    private const float fSpeed_Move = 16.5f;
    public void Init(int nDamage)
    {
        this.nDamage = nDamage;
        transform.position = ModelManager.Instance.player.transform.position;
    }
    public void Update_Skil()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * fSpeed_Move));
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            ModelManager.Instance.Play_Calculate_Damage(other.gameObject,nDamage);
        }
    }
}
