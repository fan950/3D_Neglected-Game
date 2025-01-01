using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UISkill_Slot : MonoBehaviour
{
    [HideInInspector] public int nSlot_Num;
    public Image skill_Img;
    [Header("CoolTime")]
    public Image coolTime_Img;
    public TextMeshProUGUI coolTime_Tmp;
    [HideInInspector] public bool bCoolTime = false;
    [HideInInspector] public SB_Skill_Data skill_Data;
    private UIBtn uiBtn;
    private Action<UISkill_Slot, SB_Skill_Data> action;
    private float fSkill_Time;
    public void Init(int nSlot_Num, int nIndex, Action<UISkill_Slot, SB_Skill_Data> action)
    {
        if (uiBtn == null)
            uiBtn = GetComponent<UIBtn>();
        this.nSlot_Num = nSlot_Num;

        SkillData _skillData = TableManager.Instance.skillTable.Get_SkillData(nIndex);
        skill_Data = GameManager.Instance.localGame_DB.Get_SkillData(nIndex);
        if (skill_Data == null)
        {
            skill_Data = new SB_Skill_Data();
            skill_Data.skillData = _skillData;
            skill_Data.nLevel = 0;
        }

        Set_Slot(skill_Data);

        uiBtn.Init(delegate
        {
            Call_Skill();
        });
        this.action = action;
        if (skill_Data.skillData.nIndex == 0)
        {
            End_CoolTime();
        }
        coolTime_Img.gameObject.SetActive(bCoolTime);
    }

    public void Call_Skill()
    {
        if (action != null && skill_Data != null)
            action(this, skill_Data);
    }
    public void Set_Slot(SB_Skill_Data skill_Data)
    {
        this.skill_Data = skill_Data;
        if (skill_Data == null || skill_Data.skillData.nIndex == 0)
        {
            skill_Img.gameObject.SetActive(false);
        }
        else
        {
            if (skill_Data.nLevel == 0)
                skill_Img.color = new Color(0.2830189f, 0.2830189f, 0.2830189f);
            else
                skill_Img.color = new Color(1, 1, 1);

            skill_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UISkill_Atlas, skill_Data.skillData.sPath);
            skill_Img.gameObject.SetActive(true);
        }
    }

    public void Start_CoolTime()
    {
        bCoolTime = true;

        fSkill_Time = skill_Data.skillData.fCoolTime;
        coolTime_Tmp.text = ((int)fSkill_Time + 1).ToString();
        coolTime_Img.gameObject.SetActive(true);
    }
    public void End_CoolTime()
    {
        bCoolTime = false;

        coolTime_Img.gameObject.SetActive(false);
    }
    public void Update_SkillSlot()
    {
        if (coolTime_Img.gameObject.activeSelf) 
        {
            coolTime_Img.fillAmount = fSkill_Time / skill_Data.skillData.fCoolTime;

            fSkill_Time -= Time.deltaTime;
            coolTime_Tmp.text = ((int)fSkill_Time + 1).ToString();

            if (fSkill_Time <= 0)
            {
                End_CoolTime();
            }
        }
    }
}
