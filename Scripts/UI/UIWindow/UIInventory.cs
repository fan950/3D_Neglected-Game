using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class UIInventory : UIWindow
{
    public UIBtn weapon_Btn;
    public UIBtn defend_Btn;
    [Header("Scroll")]
    public ScrollRect scrollRect;

    [Header("State")]
    public TextMeshProUGUI level_Tmp;
    public TextMeshProUGUI hp_Tmp;
    public TextMeshProUGUI attack_Tmp;
    public TextMeshProUGUI attack_Speed_Tmp;
    public TextMeshProUGUI defend_Tmp;
    public TextMeshProUGUI move_Speed_Tmp;
    public TextMeshProUGUI critical_Damage_Tmp;
    public TextMeshProUGUI critical_Percent_Tmp;
    public TextMeshProUGUI absorb_blood_Tmp;

    [Header("Equipment")]
    public UIItem_Slot[] arrItemSlot;
    private UIInventory_Player inventory_Player;
    //List_Type
    private ObjcetPool<UIInventory_Slot> uiInvenItem_Pool;
    //Path
    private const string sItem_Path = "UI/InGameScene/UIInventroySlot";
    private int nBase_Max = 60;
    private bool bWeapon_Type;
    public override void Init()
    {
        base.Init();

        uiInvenItem_Pool = new ObjcetPool<UIInventory_Slot>();
        uiInvenItem_Pool.Init(sItem_Path, nBase_Max, scrollRect.content);

        inventory_Player = GetComponentInChildren<UIInventory_Player>();
        inventory_Player.Init();

        weapon_Btn.Init(delegate
        {
            scrollRect.normalizedPosition = Vector2.one;

            Set_PartBag(true);
        });
        defend_Btn.Init(delegate
        {
            scrollRect.normalizedPosition = Vector2.one;

            Set_PartBag(false);
        });
    }

    public override void Open()
    {
        base.Open();
        Set_State();

        SB_Player_Data player_Data = GameManager.Instance.localGame_DB.player_Data;

        for (int i = (int)ePart_Type.None + 1; i < (int)ePart_Type.Max; ++i)
        {
            SB_Item_Data _item_Data = player_Data.lisEquip_ItemData.Find(_ => (int)_.part_Type == i);
            if (_item_Data != null && _item_Data.nIndex != 0)
            {
                arrItemSlot[i].Init(_item_Data, Remove_Eqiup);
                arrItemSlot[(int)_item_Data.part_Type].gameObject.SetActive(true);
                continue;
            }

            arrItemSlot[i].gameObject.SetActive(false);
        }

        Set_PartBag(true);
        inventory_Player.Open();
    }
    public void Set_State()
    {
        Player _player = ModelManager.Instance.player;
        hp_Tmp.text = _player.nHp_Max.ToString();
        attack_Tmp.text = _player.nAttack.ToString();
        defend_Tmp.text = _player.nDefend.ToString();
        level_Tmp.text = string.Format("Lv.{0}", _player.nLevel);
        attack_Speed_Tmp.text = string.Format("{0}", Mathf.Floor(_player.weapon_Player.fWeapon_Speed * 100) / 100);
        move_Speed_Tmp.text = string.Format("{0}",_player.fMove_Speed);
        critical_Damage_Tmp.text = string.Format("{0}%", _player.fCritical_Damage);
        critical_Percent_Tmp.text = string.Format("{0}%", _player.fCritical_Percent);
        absorb_blood_Tmp.text = string.Format("{0}%", Mathf.Floor(_player.fAbsord_blood * 10000) / 100);
    }
    public void Remove_Eqiup(SB_Item_Data item_Data)
    {
        UIItem_Popup _uiItemPopup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIItem_Popup) as UIItem_Popup;
        _uiItemPopup.Open(item_Data, false, delegate
        {
            arrItemSlot[(int)item_Data.part_Type].gameObject.SetActive(false);
            GameManager.Instance.localGame_DB.Set_Remove_Equip(item_Data.part_Type);

            bool _bSlot = false;
            if ((item_Data.part_Type == ePart_Type.Weapon && bWeapon_Type) ||
            (item_Data.part_Type != ePart_Type.Weapon && !bWeapon_Type))
                _bSlot = true;

            if (_bSlot)
                Set_ItmeSlot(item_Data);

            inventory_Player.Change_Part(item_Data.part_Type);
            Set_State();
        }, Set_State);
    }
    public void Set_PartBag(bool bWeapon)
    {
        uiInvenItem_Pool.Return_All();
        var _lisItem_DB = GameManager.Instance.localGame_DB.lisItem_Data.OrderBy(_ => _.nIndex).ToList();
        for (int i = 0; i < _lisItem_DB.Count; ++i)
        {
            if (bWeapon)
            {
                if (_lisItem_DB[i].part_Type == ePart_Type.Weapon)
                    Set_ItmeSlot(_lisItem_DB[i]);
            }
            else
            {
                if (_lisItem_DB[i].part_Type != ePart_Type.Weapon)
                    Set_ItmeSlot(_lisItem_DB[i]);
            }
        }

        weapon_Btn.Change_interactable(!bWeapon);
        defend_Btn.Change_interactable(bWeapon);

        bWeapon_Type = bWeapon;
    }
    public void Set_ItmeSlot(SB_Item_Data itemData)
    {
        if (itemData.nIndex == 0)
            return;

        UIInventory_Slot uiItemSlot = uiInvenItem_Pool.Get();
        uiItemSlot.Init(itemData, delegate
         {
             UIItem_Popup _uiItemPopup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIItem_Popup) as UIItem_Popup;
             _uiItemPopup.Open(itemData, true, delegate
             {
                 arrItemSlot[(int)itemData.part_Type].gameObject.SetActive(true);
                 arrItemSlot[(int)itemData.part_Type].Init(itemData, Remove_Eqiup);
                 inventory_Player.Change_Part(itemData.part_Type);

                 Set_State();
             }, Set_State);
         });
    }

    public override void Close()
    {
        base.Close();
    }
}