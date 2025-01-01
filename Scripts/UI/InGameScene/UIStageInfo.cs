using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIStageInfo : UIBtn
{
    public TextMeshProUGUI name_Tmp;

    private Action<StageData> action;
    private StageData stageData;
    public override void Init(Action action = null)
    {
        base.Init(delegate { this.action(stageData); });
    }

    public void Set_State(StageData stageData, Action<StageData> action, bool bClear)
    {
        Change_interactable(bClear);

        this.action = action;
        this.stageData = stageData;

        name_Tmp.text = string.Format(TableManager.Instance.stringTable.Get_String("Stage"), stageData.nIndex);

        if (bClear)
        {
            name_Tmp.color = Color.white;
        }
        else
        {
            name_Tmp.color = new Color(0.8773585f, 0.536737f, 0);
        }
    }

}
