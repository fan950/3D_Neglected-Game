using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UISkill : UIWindow
{
    private bool bSkill;
    public UIBtn black_Btn;
    public UIBtn equip_Btn;
    public UIBtn skill_Btn;
    public UIBtn state_Btn;
    public UIBtn upgrade_Btn;

    [Header("Scroll")]
    public ScrollRect scrollRect_Skill;
    public ScrollRect scrollRect_State;

    [Header("Explanation")]
    public Image icon_Img;
    public TextMeshProUGUI name_Tmp;
    public TextMeshProUGUI level_Tmp;
    public TextMeshProUGUI damage_Tmp;
    public TextMeshProUGUI next_damage_Tmp;
    public TextMeshProUGUI explanation_Tmp;
    public TextMeshProUGUI gold_Tmp;
    public TextMeshProUGUI coolTime_Tmp;

    [Header("Slot")]
    public UISkill_Slot[] arrSkill_Slot;

    private List<UISkill_Slot> lisSkill_Slot = new List<UISkill_Slot>();
    private List<UIState_Slot> lisState_Slot = new List<UIState_Slot>();

    private SB_Skill_Data select_SkillData;
    private SB_Skill_State_Data select_StateData;

    private const string sPath_Skill = "UI/InGameScene/UISkillSlot";
    private const string sPath_State = "UI/InGameScene/UIStateSlot";

    public override void Init()
    {
        base.Init();

        black_Btn.Init(delegate {
            if (select_SkillData == null)
                return;
            black_Btn.gameObject.SetActive(false);
        });
        equip_Btn.Init(delegate {
            if (select_SkillData == null)
                return;
            black_Btn.gameObject.SetActive(true); 
        });
        skill_Btn.Init(delegate { Type_Btn(true); });
        state_Btn.Init(delegate { Type_Btn(false); });

        upgrade_Btn.Init(delegate
        {
            if (bSkill)
                Upgrade_Skill();
            else
                Upgrade_State();
        });
        GameObject _Obj = Resources.Load<GameObject>(sPath_Skill);
        for (int i = 1; i < TableManager.Instance.skillTable.Get_SkillList().Count; ++i)
        {
            GameObject _slot_Obj = Instantiate(_Obj);
            _slot_Obj.transform.SetParent(scrollRect_Skill.content);
            _slot_Obj.transform.localRotation = Quaternion.identity;
            _slot_Obj.transform.localScale = Vector3.one;
            _slot_Obj.transform.localPosition = Vector3.zero;

            UISkill_Slot _skillSlot = _slot_Obj.GetComponent<UISkill_Slot>();
            int _nIndex = TableManager.Instance.skillTable.Get_SkillList()[i].nIndex;
            _skillSlot.Init(i, _nIndex, Set_SelectEquip);
            lisSkill_Slot.Add(_skillSlot);
        }

        _Obj = Resources.Load<GameObject>(sPath_State);
        for (int i = 0; i < TableManager.Instance.stateTable.Get_StateList().Count; ++i)
        {
            GameObject _slot_Obj = Instantiate(_Obj);
            _slot_Obj.transform.SetParent(scrollRect_State.content);
            _slot_Obj.transform.localRotation = Quaternion.identity;
            _slot_Obj.transform.localScale = Vector3.one;
            _slot_Obj.transform.localPosition = Vector3.zero;

            UIState_Slot _stateSlot = _slot_Obj.GetComponent<UIState_Slot>();
            int _nIndex = TableManager.Instance.stateTable.Get_StateList()[i].nIndex;
            _stateSlot.Init(i, _nIndex, Show_Info_State);
            lisState_Slot.Add(_stateSlot);
        }

        for (int i = 0; i < arrSkill_Slot.Length; ++i)
        {
            SB_Skill_Data skill_Data = GameManager.Instance.localGame_DB.player_Data.lisEquip_SkillData[i];
            arrSkill_Slot[i].Init(i, skill_Data.skillData.nIndex, Set_EquipSkill);
        }
    }
    public override void Open()
    {
        base.Open();

        icon_Img.gameObject.SetActive(false);
        name_Tmp.text = string.Empty;
        level_Tmp.text = string.Empty;
        damage_Tmp.text = string.Empty;
        next_damage_Tmp.gameObject.SetActive(false);
        explanation_Tmp.text = string.Empty;
        gold_Tmp.text = string.Empty;
        coolTime_Tmp.text = string.Empty;

        Type_Btn(true);
    }

    public override void Close()
    {
        base.Close();
        UIManager.Instance.Set_Skill();
    }
    public void Type_Btn(bool bSkill)
    {
        select_SkillData = null;
        select_StateData = null;

        if (bSkill)
        {
            this.bSkill = true;
            skill_Btn.Change_interactable(false);
            state_Btn.Change_interactable(true);
            scrollRect_Skill.gameObject.SetActive(true);
            scrollRect_State.gameObject.SetActive(false);
        }
        else
        {
            this.bSkill = false;
            state_Btn.Change_interactable(false);
            skill_Btn.Change_interactable(true);
            scrollRect_Skill.gameObject.SetActive(false);
            scrollRect_State.gameObject.SetActive(true);
        }
    }
    public void Set_SelectEquip(UISkill_Slot uiSkill_Slot, SB_Skill_Data select_SkillData)
    {
        this.select_SkillData = select_SkillData;
        Show_Info_Skill();
    }
    public void Set_EquipSkill(UISkill_Slot uISkill_Slot, SB_Skill_Data equip_SkillData)
    {
        if (black_Btn.gameObject.activeSelf)
        {
            black_Btn.gameObject.SetActive(false);
            List<SB_Skill_Data> _lisEquipSkill = GameManager.Instance.localGame_DB.Get_Equip_ListSkillData();
            for (int i = 0; i < _lisEquipSkill.Count; ++i)
            {
                if (_lisEquipSkill[i].skillData.nIndex == select_SkillData.skillData.nIndex)
                {
                    SB_Skill_Data _skill_Data = new SB_Skill_Data();
                    _skill_Data.skillData = TableManager.Instance.skillTable.Get_SkillData(0);
                    _skill_Data.nLevel = 0;
                    arrSkill_Slot[i].Set_Slot(_skill_Data);

                    GameManager.Instance.localGame_DB.Set_EquipSkill(i, null);
                    break;
                }
            }

            GameManager.Instance.localGame_DB.Set_EquipSkill(uISkill_Slot.nSlot_Num, select_SkillData);
            uISkill_Slot.Set_Slot(select_SkillData);
        }
        else
        {
            if (uISkill_Slot.skill_Data.skillData.nIndex == 0)
                return;

            SB_Skill_Data _skill_Data = new SB_Skill_Data();
            _skill_Data.skillData = TableManager.Instance.skillTable.Get_SkillData(0);
            _skill_Data.nLevel = 0;

            uISkill_Slot.Set_Slot(_skill_Data);
            GameManager.Instance.localGame_DB.Set_EquipSkill(uISkill_Slot.nSlot_Num, null);
        }
    }
    public void Upgrade_Skill()
    {
        if (select_SkillData == null)
            return;

        int _nGold = GameManager.Instance.localGame_DB.Get_Gold() - (select_SkillData.skillData.nUpgrade_Gold * (select_SkillData.nLevel + 1));
        if (_nGold < 0)
        {
            UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
            _uiOk_Popup.Buy_OnShow("Shortage", eBuy_Type.Gold.ToString());
            return;
        }

        GameManager.Instance.localGame_DB.Set_Gold(_nGold);

        ++select_SkillData.nLevel;

        for (int i = 0; i < lisSkill_Slot.Count; ++i)
        {
            if (lisSkill_Slot[i].skill_Data.skillData.nIndex == select_SkillData.skillData.nIndex)
            {
                lisSkill_Slot[i].Set_Slot(select_SkillData);
                break;
            }
        }

        GameManager.Instance.localGame_DB.Set_SkillData(select_SkillData);

        Show_Info_Skill();
    }
    public void Upgrade_State()
    {
        int _nGold = GameManager.Instance.localGame_DB.Get_Gold() - (select_StateData.stateData.nUpgrade_Gold * (select_StateData.nLevel + 1));
        if (_nGold < 0)
        {
            UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
            _uiOk_Popup.Buy_OnShow("Shortage", eBuy_Type.Gold.ToString());
            return;
        }

        GameManager.Instance.localGame_DB.Set_Gold(_nGold);

        ++select_StateData.nLevel;

        for (int i = 0; i < lisState_Slot.Count; ++i)
        {
            if (lisState_Slot[i].skill_State_Data.stateData.nIndex == select_StateData.stateData.nIndex)
            {
                lisState_Slot[i].Set_Slot(select_StateData);
                break;
            }
        }

        GameManager.Instance.localGame_DB.Set_StateData(select_StateData);

        if (select_StateData.stateData.state_Type == eState_Type.Hp)
        {
            ModelManager.Instance.player.Apply_State_Hp();
            UIManager.Instance.Set_Hp();
        }
        Show_Info_State(select_StateData);
    }
    public void Show_Info_Skill()
    {
        icon_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UISkill_Atlas, select_SkillData.skillData.sPath);
        name_Tmp.text = select_SkillData.skillData.sName;
        level_Tmp.text = TableManager.Instance.stringTable.Get_String("Lv.") + " : " + select_SkillData.nLevel;
        damage_Tmp.text = string.Format("{0} : {1}%", TableManager.Instance.stringTable.Get_String("Current Ability"), (select_SkillData.skillData.fValue * (select_SkillData.nLevel * select_SkillData.skillData.fUpgrade_Value)) * 0.01f);

        if (select_SkillData.nLevel == select_SkillData.skillData.nMax_Level)
        {
            next_damage_Tmp.gameObject.SetActive(false);
        }
        else
        {
            next_damage_Tmp.text = string.Format("{0} : {1}%", TableManager.Instance.stringTable.Get_String("Next Ability"), (select_SkillData.skillData.fValue * ((select_SkillData.nLevel + 1) * select_SkillData.skillData.fUpgrade_Value)) * 0.01f);
            next_damage_Tmp.gameObject.SetActive(true);
        }
        coolTime_Tmp.text = string.Format(TableManager.Instance.stringTable.Get_String("CoolTime_Des"), (int)select_SkillData.skillData.fCoolTime);
        explanation_Tmp.text = TableManager.Instance.stringTable.Get_String(select_SkillData.skillData.sDes);
        if (select_SkillData.nLevel == 10)
        {
            gold_Tmp.gameObject.SetActive(false);
            upgrade_Btn.Change_interactable(false);
        }
        else
        {
            gold_Tmp.text = (select_SkillData.skillData.nUpgrade_Gold * (select_SkillData.nLevel + 1)).ToString();
            gold_Tmp.gameObject.SetActive(true);
            upgrade_Btn.Change_interactable(true);
        }

        if (select_SkillData.nLevel == 0)
            equip_Btn.Change_interactable(false);
        else
            equip_Btn.Change_interactable(true);

        upgrade_Btn.gameObject.SetActive(true);
        coolTime_Tmp.gameObject.SetActive(true);
        equip_Btn.gameObject.SetActive(true);
        icon_Img.gameObject.SetActive(true);
    }
    public void Show_Info_State(SB_Skill_State_Data sb_state_Data)
    {
        select_StateData = sb_state_Data;

        icon_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UISkill_Atlas, sb_state_Data.stateData.sPath);
        name_Tmp.text = sb_state_Data.stateData.sName;
        level_Tmp.text = TableManager.Instance.stringTable.Get_String("Lv.") + " : " + sb_state_Data.nLevel;

        damage_Tmp.text = string.Format("{0} : {1}%", TableManager.Instance.stringTable.Get_String("Current Ability"), (sb_state_Data.nLevel * sb_state_Data.stateData.fUpgrade_Value));
        next_damage_Tmp.text = string.Format("{0} : {1}%", TableManager.Instance.stringTable.Get_String("Next Ability"), ((sb_state_Data.nLevel + 1) * sb_state_Data.stateData.fUpgrade_Value));

        explanation_Tmp.text = TableManager.Instance.stringTable.Get_String(sb_state_Data.stateData.sDes);
        gold_Tmp.text = (sb_state_Data.stateData.nUpgrade_Gold * (sb_state_Data.nLevel + 1)).ToString();

        if (sb_state_Data.stateData.nUpgrade_Max == -1)
        {
            upgrade_Btn.gameObject.SetActive(true);
            next_damage_Tmp.gameObject.SetActive(true);
        }
        else
        {
            if (sb_state_Data.nLevel >= sb_state_Data.stateData.nUpgrade_Max)
            {
                upgrade_Btn.gameObject.SetActive(false);
                next_damage_Tmp.gameObject.SetActive(false);
            }
            else
            {
                upgrade_Btn.gameObject.SetActive(true);
                next_damage_Tmp.gameObject.SetActive(true);
            }
        }

        icon_Img.gameObject.SetActive(true);
        gold_Tmp.gameObject.SetActive(true);
        coolTime_Tmp.gameObject.SetActive(false);
        equip_Btn.gameObject.SetActive(false);
    }
}
