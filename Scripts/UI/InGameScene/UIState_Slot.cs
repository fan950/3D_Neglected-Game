using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIState_Slot : MonoBehaviour
{
    public Image state_Img;
    [Header("CoolTime")]
    [HideInInspector] public SB_Skill_State_Data skill_State_Data;
    private UIBtn uiBtn;
    public void Init(int nSlot_Num, int nIndex, Action<SB_Skill_State_Data> action)
    {
        if (uiBtn == null)
            uiBtn = GetComponent<UIBtn>();

        StateData _stateData = TableManager.Instance.stateTable.Get_StateData(nIndex);
        skill_State_Data = GameManager.Instance.localGame_DB.Get_Skill_StateData(nIndex);
        if (skill_State_Data == null)
        {
            skill_State_Data = new SB_Skill_State_Data();
            skill_State_Data.stateData = _stateData;
            skill_State_Data.nLevel = 0;
        }

        Set_Slot(skill_State_Data);

        uiBtn.Init(delegate
        {
            if (action != null && skill_State_Data != null)
                action(skill_State_Data);
        });
    }
    public void Set_Slot(SB_Skill_State_Data stateData)
    {
        this.skill_State_Data = stateData;
        if (stateData == null || stateData.stateData.nIndex == 0)
        {
            state_Img.gameObject.SetActive(false);
        }
        else
        {
            if (stateData.nLevel == 0)
                state_Img.color = new Color(0.2830189f, 0.2830189f, 0.2830189f);
            else
                state_Img.color = new Color(1, 1, 1);

            state_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UISkill_Atlas, stateData.stateData.sPath);
            state_Img.gameObject.SetActive(true);
        }
    }
}
