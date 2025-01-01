using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Weapon_Collider : MonoBehaviour
{
    public Action<GameObject> action;
    private void OnTriggerEnter(Collider other)
    {
        action(other.gameObject);
    }
}
