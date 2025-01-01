using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDamage_Tmp : MonoBehaviour
{
    private Action<UIDamage_Tmp> action;

    [HideInInspector] public RectTransform rectTransform;
    public Image critical_Img;
    public TextMeshProUGUI damage_Tmp;
    public void Init(float fDamage, bool bCri, Action<UIDamage_Tmp> action)
    {
        if (damage_Tmp == null)
            damage_Tmp = GetComponent<TextMeshProUGUI>();

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        this.action = action;

        damage_Tmp.text = fDamage.ToString();
        critical_Img.gameObject.SetActive(bCri);
    }

    public void Die()
    {
        if (action != null)
            action(this);
    }
}
