using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIItem_Popup : UIPopup
{
    public UIBtn equip_UIBtn;
    public UIBtn effect_UIBtn;
    public TextMeshProUGUI type_Tmp;
    public TextMeshProUGUI equip_Tmp;

    [Header("Item")]
    public Image icon_img;
    public GameObject[] arrStar_Obj;
    public TextMeshProUGUI name_Tmp;

    [Header("State")]
    public TextMeshProUGUI[] arrState_Tmp;
    public TextMeshProUGUI[] arrState_Value_Tmp;
    public Image[] arrArrow_Img;

    [Header("Effect")]
    public TextMeshProUGUI[] arrEffect_Tmp;
    public TextMeshProUGUI[] arrEffect_Value_Tmp;

    private bool bEquip;
    private SB_Item_Data item_Data;

    private Action action;
    private Action StateAction;
    public override void Init()
    {
        base.Init();
        equip_UIBtn.Init(delegate
        {
            if (!bEquip)
            {
                UIManager.Instance.Set_Equipment(item_Data.part_Type, GameManager.Instance.lisItemData.Find(_ => _.part_Type == item_Data.part_Type));
            }
            else
                UIManager.Instance.Set_Equipment(item_Data.part_Type, item_Data);

            if (action != null)
                action();

            Close();
        });
        effect_UIBtn.Init(delegate
        {
            UICommon_Popup uiCommon = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UICommon_Popup) as UICommon_Popup;
            string _sTemp = TableManager.Instance.stringTable.Get_String("Effect Change Popup").Replace("\\n", "\n");
            uiCommon.OnShow(string.Format("{0}", _sTemp), delegate { Effect_ApplyItem(item_Data.part_Type, item_Data.nIndex); });
        });
    }
    public override void Open()
    {
        for (int i = 0; i < arrEffect_Tmp.Length; ++i)
        {
            arrEffect_Tmp[i].gameObject.SetActive(false);
            arrEffect_Value_Tmp[i].gameObject.SetActive(false);
        }
    }
    public void Open(SB_Item_Data item_Data, bool bEquip, Action action, Action StateAction)
    {
        this.action = action;
        this.StateAction = StateAction;
        this.bEquip = bEquip;
        this.item_Data = item_Data;

        for (int i = 0; i < arrStar_Obj.Length; ++i)
        {
            if (item_Data.nStar > i)
                arrStar_Obj[i].SetActive(true);
            else
                arrStar_Obj[i].SetActive(false);
        }

        Show_ItemData();

        string _sEquip = "Equip";
        if (!bEquip)
            _sEquip = "Rmove";
        equip_Tmp.text = TableManager.Instance.stringTable.Get_String(_sEquip);

        base.Open();
    }
    public void Show_ItemData()
    {
        int _nColor;

        Player _player = ModelManager.Instance.player;

        for (int i = 0; i < item_Data.lisState_Data.Count; ++i)
        {
            string _sKey = TableManager.Instance.stateTable.Get_StateData(item_Data.lisState_Data[i].nIndex).sName;
            arrEffect_Tmp[i].text = TableManager.Instance.stringTable.Get_String(_sKey);
            arrEffect_Value_Tmp[i].text = string.Format("{0}%", item_Data.lisState_Data[i].fValue);

            arrEffect_Tmp[i].gameObject.SetActive(true);
            arrEffect_Value_Tmp[i].gameObject.SetActive(true);
        }

        if (item_Data.part_Type == ePart_Type.Weapon)
        {
            WeaponData _weaponData_After = TableManager.Instance.weaponTable.Get_WeaponDate(item_Data.nIndex);

            if (_weaponData_After.nEffect_Count == 0)
                effect_UIBtn.gameObject.SetActive(false);
            else
                effect_UIBtn.gameObject.SetActive(true);

            arrState_Tmp[0].text = string.Format("{0}", TableManager.Instance.stringTable.Get_String("Attack"));

            arrState_Value_Tmp[0].text = string.Format("{0}", _weaponData_After.Get_Attack(item_Data));
            Set_Arrow(0, _player.weapon_Player.nWeapon_Attack, _weaponData_After.Get_Attack(item_Data));

            arrState_Tmp[1].text = string.Format("{0}", TableManager.Instance.stringTable.Get_String("Attack Speed"));
            arrState_Value_Tmp[1].text = string.Format("{0}", _weaponData_After.fAttack_Speed);
            Set_Arrow(1, _player.weapon_Player.fWeapon_Speed, _weaponData_After.fAttack_Speed);

            name_Tmp.text = TableManager.Instance.stringTable.Get_String(_weaponData_After.sName);
            type_Tmp.text = TableManager.Instance.stringTable.Get_String(_weaponData_After.eAttack_Type.ToString());
            icon_img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.Weapon_Atlas, _weaponData_After.sPath);
            _nColor = _weaponData_After.nGrade;
        }
        else
        {
            int _nValue_1 = 0;
            int _nValue_2 = 0;

            DefendData _defendData_After = TableManager.Instance.defendTable.Get_DefendData(item_Data.nIndex);
            SB_Item_Data _data = GameManager.Instance.localGame_DB.Get_Equip_ItemData(item_Data.part_Type);
            if (_data != null)
            {
                _nValue_1 = TableManager.Instance.defendTable.Get_DefendData(_data.nIndex).Get_Defend(_data);
                _nValue_2 = TableManager.Instance.defendTable.Get_DefendData(_data.nIndex).nHp;
            }

            if (_defendData_After.nEffect_Count == 0)
                effect_UIBtn.gameObject.SetActive(false);
            else
                effect_UIBtn.gameObject.SetActive(true);

            arrState_Tmp[0].text = string.Format("{0}", TableManager.Instance.stringTable.Get_String("Defend"));
            arrState_Value_Tmp[0].text = string.Format("{0}", _defendData_After.Get_Defend(item_Data));
            Set_Arrow(0, _nValue_1, _defendData_After.Get_Defend(item_Data));

            arrState_Tmp[1].text = string.Format("{0}", TableManager.Instance.stringTable.Get_String("Hp"));
            arrState_Value_Tmp[1].text = string.Format("{0}", _defendData_After.nHp);
            Set_Arrow(1, _nValue_2, _defendData_After.nHp);

            name_Tmp.text = TableManager.Instance.stringTable.Get_String(_defendData_After.sName);
            type_Tmp.text = TableManager.Instance.stringTable.Get_String(item_Data.part_Type.ToString());
            icon_img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.Defend_Atlas, _defendData_After.sPath);
            _nColor = _defendData_After.nGrade;
        }
        switch (_nColor)
        {
            case 0:
            case 1:
                name_Tmp.color = Color.white;
                break;
            case 2:
                name_Tmp.color = Color.green;
                break;
            case 3:
                name_Tmp.color = Color.yellow;
                break;
            case 4:
                name_Tmp.color = new Color(253f, 151f, 0);
                break;
            case 5:
                name_Tmp.color = Color.red;
                break;
        }
    }
    public void Set_Arrow(int nIndex, float nBefore, float nAfter)
    {
        if (nBefore < nAfter)
        {
            arrArrow_Img[nIndex].transform.localRotation = Quaternion.Euler(0, 0, -90);
            arrArrow_Img[nIndex].color = Color.blue;
            arrArrow_Img[nIndex].gameObject.SetActive(true);
        }
        else if (nBefore > nAfter)
        {
            arrArrow_Img[nIndex].transform.localRotation = Quaternion.Euler(0, 0, 90);
            arrArrow_Img[nIndex].color = Color.red;
            arrArrow_Img[nIndex].gameObject.SetActive(true);
        }
        else
            arrArrow_Img[nIndex].gameObject.SetActive(false);
    }

    public void Effect_ApplyItem(ePart_Type part_Type, int nIndex)
    {
        int _nCrystal = GameManager.Instance.localGame_DB.shop_Data.nCrystal - TableManager.Instance.shopTable.Get_ShopData("Effect Change").nPrice;
        if (_nCrystal < 0)
        {
            UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
            _uiOk_Popup.Buy_OnShow("Shortage", eBuy_Type.Crystal.ToString());
            return;
        }

        GameManager.Instance.localGame_DB.Set_Crystal(_nCrystal);

        SB_Item_Data _item_Data = GameManager.Instance.localGame_DB.Get_ItemData(part_Type, nIndex);

        if (_item_Data == null)
            return;

        _item_Data.lisState_Data.Clear();

        int _nEffect_Count = 0;
        List<int> _lisEff_Index = null;
        if (_item_Data.part_Type == ePart_Type.Weapon)
        {
            _nEffect_Count = TableManager.Instance.weaponTable.Get_WeaponDate(_item_Data.nIndex).nEffect_Count;
            _lisEff_Index = TableManager.Instance.weaponTable.Get_ListEffect_Index(_item_Data.nIndex);
        }
        else
        {
            _nEffect_Count = TableManager.Instance.defendTable.Get_DefendData(_item_Data.nIndex).nEffect_Count;
            _lisEff_Index = TableManager.Instance.defendTable.Get_ListEffect_Index(_item_Data.nIndex);
        }

        for (int i = 0; i < _nEffect_Count; ++i)
        {
            int nRandom = UnityEngine.Random.Range(0, _lisEff_Index.Count);
            StateData _stateData = TableManager.Instance.stateTable.Get_StateData(_lisEff_Index[nRandom]);
            SB_Equip_Item_State_Data _data = new SB_Equip_Item_State_Data();
            _data.nIndex = _stateData.nIndex;
            _data.fValue = Mathf.Floor(UnityEngine.Random.Range(_stateData.fEquip_Value_Min, _stateData.fEquip_Value_Max) * 10f) / 10f;
            _item_Data.lisState_Data.Add(_data);
        }

        item_Data = _item_Data;
        SB_Item_Data sb_Item_Data = GameManager.Instance.localGame_DB.Get_Equip_ItemData(_item_Data.part_Type);
        if (sb_Item_Data.nIndex == _item_Data.nIndex)
        {
            ModelManager.Instance.player.Apply_State(_item_Data.part_Type, _item_Data);
        }
        Show_ItemData();

        if (StateAction != null)
            StateAction();

        GameManager.Instance.Save_DB();
    }
}
