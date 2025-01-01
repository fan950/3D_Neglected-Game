using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill_MoveType : MonoBehaviour
{
    [HideInInspector] public Skill_Monster skill;
    public eSkill_MoveType skill_MoveType;
    public GameObject range_Obj;

    private Vector3 vecMove_Pos;

    private Action action;
    private const float fMove_Speed = 5f;
    private const float fScale_Speed = 0.7f;

    private Coroutine skill_Coro;
    public void Init(Action action)
    {
        this.action = action;
        gameObject.SetActive(false);
    }
    public void Play_SkillPos()
    {
        gameObject.SetActive(true);

        if (skill_Coro != null)
            StopCoroutine(skill_Coro);

        skill_Coro = StartCoroutine(Skill_Pos());
    }
    public void Play_SkillScale()
    {
        gameObject.SetActive(true);

        if (skill_Coro != null)
            StopCoroutine(skill_Coro);

        skill_Coro = StartCoroutine(Skill_Scale());
    }

    public IEnumerator Skill_Pos()
    {
        range_Obj.transform.localPosition = new Vector3(5, 0, 0);

        while (true)
        {
            yield return null;

            range_Obj.transform.localPosition = Vector3.MoveTowards(range_Obj.transform.localPosition, Vector3.zero, Time.deltaTime * fMove_Speed);

            if (Vector3.Distance(range_Obj.transform.localPosition, Vector3.zero) <= 0.1f)
            {
                range_Obj.transform.localPosition = Vector3.zero;
                action();
                break;
            }
        }
    }

    public IEnumerator Skill_Scale()
    {
        range_Obj.transform.localScale = Vector3.zero;

        float _fTemp = 0;
        while (true)
        {
            yield return null;

            _fTemp += Time.deltaTime * fScale_Speed;
            range_Obj.transform.localScale = new Vector3(_fTemp, _fTemp, _fTemp);

            if (range_Obj.transform.localScale.x >= 1)
            {
                range_Obj.transform.localScale = Vector3.one;
                action();
                break;
            }
        }
    }

    public void End_Skill()
    {
        if (!gameObject.activeSelf)
            return;

        if (skill_Coro != null)
            StopCoroutine(skill_Coro);
        skill_Coro = null;
        gameObject.SetActive(false);
    }
}
