using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIOk_Popup : UIPopup
{
    public UIBtn ok_Btn;
    public TextMeshProUGUI content_Tmp;
    private Action action;
    public override void Init()
    {
        base.Init();

        ok_Btn.Init(delegate
        {
            Close();
        });
    }
    public override void Close()
    {
        if (action != null)
            action();

        base.Close();
    }
    public void Buy_OnShow(string sKey, string sGold)
    {
        content_Tmp.text = string.Format(TableManager.Instance.stringTable.Get_String(sKey), sGold);
    }

    public void OnShow(string sContext, Action action = null)
    {
        content_Tmp.text = string.Format(sContext);
        this.action = action;
    }
}
