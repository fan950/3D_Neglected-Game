using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBtn : MonoBehaviour
{
    private Button button;
    public virtual void Init(Action action = null)
    {
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        if (action != null)
        {
            button.onClick.AddListener(delegate
            {
                action();
            });
        }
    }

    public void Change_interactable(bool bActive)
    {
        if (button == null)
            button = GetComponent<Button>();

        button.interactable = bActive;
    }
    public RectTransform Btn_RectPos()
    {
        return button.image.rectTransform;
    }
}
