using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClearEnd : UIWindow
{
    public UIBtn next_Btn;
    public UIBtn return_Btn;
    public override void Init()
    {
        base.Init();

        next_Btn.Init(delegate { Next_Scene(1); });
        return_Btn.Init(delegate { Next_Scene(0); });
    }

    public void Next_Scene(int _nPlus)
    {
        int _nIndex = GameManager.Instance.localGame_DB.Get_Stage() + _nPlus;

        if (_nPlus == 1)
            GameManager.Instance.localGame_DB.Set_Clear_Stage(_nIndex);
        GameManager.Instance.Next_Scene(_nIndex);
        Close();
    }
}