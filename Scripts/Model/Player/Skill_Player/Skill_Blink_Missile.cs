using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill_Blink_Missile : MonoBehaviour
{
    private BoxCollider boxCollider;

    public float fSpeed = 20f;
    public float fAngle = 45f;
    private float fGravity = 4.9f;

    private float fElapsedTime;

    private Vector3 startPosition_Vec;

    private Action explosion_Action;

    public void Init(Action explosion_Action)
    {
        this.explosion_Action = explosion_Action;

        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        boxCollider.enabled = false;
        transform.localPosition = Vector3.zero;
        startPosition_Vec = transform.localPosition;

        fElapsedTime = 0;

        fGravity = UnityEngine.Random.Range(4.9f, 9.8f);

        gameObject.SetActive(true);
    }
    public void Update_Skil()
    {
        if (fElapsedTime >= 0.3f)
            boxCollider.enabled = true;

        fElapsedTime += Time.deltaTime;
        float _fvX = fSpeed * Mathf.Cos(fAngle * Mathf.Deg2Rad);
        float _fvY = fSpeed * Mathf.Sin(fAngle * Mathf.Deg2Rad) - fGravity * fElapsedTime;

        float _fPosX = startPosition_Vec.x + _fvX * fElapsedTime;
        float _fPosY = startPosition_Vec.y + _fvY * fElapsedTime - 0.5f * fGravity * Mathf.Pow(fElapsedTime, 2);

        transform.localPosition = new Vector3(_fPosX, _fPosY, startPosition_Vec.z);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Field")
        {
            explosion_Action();
        }
    }
}
