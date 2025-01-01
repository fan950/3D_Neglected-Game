using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UICommon_Popup : UIPopup
{
    public UIBtn ok_Btn;
    public UIBtn cancel_Btn;

    [Header("Content")]
    public TextMeshProUGUI Content_Tmp;

    private Action action;
    public override void Init()
    {
        base.Init();

        ok_Btn.Init(delegate
        {
            if (action != null)
                action();
            Close();
        });
        cancel_Btn.Init(Close);
    }
    public void OnShow(string sContent, Action action = null)
    {
        Content_Tmp.text = sContent;
        this.action = action;
    }
}
