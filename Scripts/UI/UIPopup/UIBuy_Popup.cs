using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIBuy_Popup : UIPopup
{
    public UIBtn ok_Btn;
    public UIBtn cancel_Btn;

    [Header("Content")]
    public Image item_Img;
    public TextMeshProUGUI item_Tmp;

    private Action action;
    public override void Init()
    {
        base.Init();

        ok_Btn.Init(delegate 
        {
            if(action!=null)
                action();
        });
        cancel_Btn.Init(Close);
    }

    public void OnShow(Sprite sprite,string sName,Action action=null) 
    {
        item_Img.sprite = sprite;
        item_Tmp.text = sName;
        this.action = action;
    }
}
