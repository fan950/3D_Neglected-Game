using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Model_AniAction : MonoBehaviour
{
    protected abstract void Start();
    public abstract void On_Collider();
    public abstract void Off_Collider();
    public abstract void End_Die();

    public abstract void Move();
}
