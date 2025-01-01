using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHp_Bar : MonoBehaviour
{
    public Image hp_Img;

    [HideInInspector] public Monster monster;
    [HideInInspector] public RectTransform rectTransform;
    public Transform target_Pos { get { return monster.offset_HeadPos; } }

    [HideInInspector] public float fLive_Time;
    public bool bCheck_Time { get { return fLive_Time >= fLive_Max; } }
    private const float fLive_Max = 10;

    public float fHp { get { return monster.nHp; } }
    public void Init(Monster monster)
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        fLive_Time = 0;
        this.monster = monster;

        Set_Hp();
        gameObject.SetActive(true);
    }
    public void Set_Hp()
    {
        hp_Img.fillAmount = monster.nHp / (monster.nHp_Max * 1.0f);
        UIManager.Instance.Set_ObjPos(rectTransform, target_Pos.gameObject);
    }
}
