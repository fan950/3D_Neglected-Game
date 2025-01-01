using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILevel_Tmp : MonoBehaviour
{
    private Action<UILevel_Tmp> action;

    private TextMeshProUGUI level_Tmp;
    [HideInInspector] public RectTransform rectTransform;
    public void Init(Action<UILevel_Tmp> action)
    {
        if (level_Tmp == null)
            level_Tmp = GetComponent<TextMeshProUGUI>();

        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        this.action = action;

        level_Tmp.text = TableManager.Instance.stringTable.Get_String("LevelUp");
    }

    public void Die()
    {
        if (action != null)
            action(this);
    }
}
