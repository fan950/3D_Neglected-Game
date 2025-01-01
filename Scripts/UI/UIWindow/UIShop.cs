using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShop : UIWindow
{
    public UIBtn gold_Btn;
    public UIBtn equip_Btn;

    public UIBtn pick_1Btn;
    public UIBtn pick_10Btn;
    public UIBtn pick_100Btn;

    [Header("Obj")]
    public GameObject goods_Obj;
    public GameObject lootBox_Obj;

    [Header("Gold")]
    public TextMeshProUGUI gold_Tmp;
    public TextMeshProUGUI crystal_Tmp;

    [Header("LootBox")]
    public GameObject box_Obj;
    public Image exp_Img;
    public TextMeshProUGUI exp_Tmp;
    public TextMeshProUGUI level_Tmp;
    public ScrollRect scrollRect;
    public Image box_Img;
    public TextMeshProUGUI box_Tmp;

    [Header("Slot")]
    public UIShop_Slot[] arrShop_Slot;

    private ObjcetPool<UIItem_Slot> objcetPool_ItemSlot = new ObjcetPool<UIItem_Slot>();
    private ObjcetPool<UIShop_Slot> objcetPool_ShopSlot = new ObjcetPool<UIShop_Slot>();
    private List<int> lisPercent = new List<int>();
    private int nPercent_Max = 100000;
    private WaitForSeconds waitForSeconds;
    private const int nExp_Value = 10;
    private const string sPath_ItemSlot = "UI/InGameScene/UIItemSlot";
    private const string sPath_ShopSlot = "UI/InGameScene/UIShopSlot";
    public override void Init()
    {
        base.Init();

        gold_Btn.Init(Show_Goods);
        equip_Btn.Init(Show_Equip);

        pick_1Btn.Init(delegate { StartCoroutine(Pick_Equip(1)); });
        pick_10Btn.Init(delegate { StartCoroutine(Pick_Equip(10)); });
        pick_100Btn.Init(delegate { StartCoroutine(Pick_Equip(100)); });

        objcetPool_ItemSlot.Init(sPath_ItemSlot, 100, scrollRect.content);
        objcetPool_ShopSlot.Init(sPath_ShopSlot, 10, goods_Obj.transform);

        List<ShopData> _lisData = TableManager.Instance.shopTable.Get_LisShopData();
        for (int i = 0; i < _lisData.Count; ++i)
        {
            if (_lisData[i].shop_Value == eShop_Value.Item)
                continue;

            UIShop_Slot _slot = objcetPool_ShopSlot.Get();
            _slot.Init(_lisData[i].nIndex);
        }
        waitForSeconds = new WaitForSeconds(0.001f);
    }
    public override void Open()
    {
        base.Open();
        box_Obj.SetActive(true);

        Show_Equip();
        Set_Gold();
    }

    public void Show_Equip()
    {
        Set_Exp(0);

        goods_Obj.SetActive(false);
        lootBox_Obj.SetActive(true);
        gold_Btn.Change_interactable(true);
        equip_Btn.Change_interactable(false);
    }
    public void Set_Gold()
    {
        gold_Tmp.text = GameManager.Instance.localGame_DB.Get_Gold().ToString();
        crystal_Tmp.text = GameManager.Instance.localGame_DB.Get_Crystal().ToString();
    }
    public void Show_Goods()
    {
        goods_Obj.SetActive(true);
        lootBox_Obj.SetActive(false);
        gold_Btn.Change_interactable(false);
        equip_Btn.Change_interactable(true);
    }
    public void Set_Exp(int nCount)
    {
        GameManager.Instance.localGame_DB.Set_ShopExp(nCount);
        int _nLevel = GameManager.Instance.localGame_DB.Get_ShopLevel();

        float _fMax = TableManager.Instance.shopExpTable.Get_ShopExpData(_nLevel).nLevel_Max;
        float _fPercent = GameManager.Instance.localGame_DB.Get_ShopExp() / _fMax;
        float _float2 = Mathf.Floor(_fPercent * 100f) / 100f;

        exp_Img.fillAmount = _fPercent;
        exp_Tmp.text = string.Format("{0}%", _float2 * 100);
        level_Tmp.text = GameManager.Instance.localGame_DB.Get_ShopLevel().ToString();
        lisPercent = TableManager.Instance.shopExpTable.Get_ListPercent(_nLevel, nPercent_Max);

        string _sBox = "Box_1";
        if (_nLevel >= 100 && _nLevel < 200)
            _sBox = "Box_2";
        else if (_nLevel >= 200)
            _sBox = "Box_3";

        box_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UIShop_Atlas, _sBox);
        box_Tmp.text = TableManager.Instance.stringTable.Get_String(_sBox);
    }
    public IEnumerator Pick_Equip(int nCount)
    {
        int _nValue = GameManager.Instance.localGame_DB.shop_Data.nCrystal - nCount;
        if (_nValue < 0)
        {
            UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
            _uiOk_Popup.Buy_OnShow("Shortage", eBuy_Type.Crystal.ToString());
        }
        else
        {
            GameManager.Instance.localGame_DB.Set_Crystal(_nValue);
            objcetPool_ItemSlot.Return_All();

            box_Obj.SetActive(false);
            Set_Exp(nCount);
            for (int i = 0; i < nCount; ++i)
            {
                SB_Item_Data _item_Data = new SB_Item_Data();
                int _nRank = Get_Percent_Rank(TableManager.Instance.shopExpTable.Get_ShopExpData(GameManager.Instance.localGame_DB.Get_ShopLevel()).nRank_Max);
                int _nPart = Random.Range(0, (int)ePart_Type.Max);
                switch (_nPart)
                {
                    case (int)ePart_Type.Head_1:
                    case (int)ePart_Type.Head_2:
                    case (int)ePart_Type.Back:
                    case (int)ePart_Type.Mask:
                    case (int)ePart_Type.Body:
                        DefendData _defendData = TableManager.Instance.defendTable.Get_RankRandom_Index(_nRank);
                        _item_Data.nIndex = _defendData.nIndex;
                        _item_Data.part_Type = _defendData.part_Type;
                        break;
                    case (int)ePart_Type.Weapon:
                        _item_Data.nIndex = TableManager.Instance.weaponTable.Get_RankRandom_Index(_nRank);
                        _item_Data.part_Type = ePart_Type.Weapon;
                        break;
                }
                _item_Data.nStar = 0;
                _item_Data.nLevel = 1;
                _item_Data.lisState_Data = new List<SB_Equip_Item_State_Data>();

                UIItem_Slot _uiItem_Slot = objcetPool_ItemSlot.Get();
                _uiItem_Slot.Init(_item_Data);

                List<SB_Item_Data> _lisItem_Data = GameManager.Instance.localGame_DB.lisItem_Data;
                bool _bItem = false;
                for (int k = 0; k < _lisItem_Data.Count; ++k)
                {
                    _bItem = false;
                    if (_lisItem_Data[k].nIndex == _item_Data.nIndex && _lisItem_Data[k].part_Type == _item_Data.part_Type)
                    {
                        int _nMax = _lisItem_Data[k].nLevel * nExp_Value;
                        _lisItem_Data[k].nExp += 1;
                        if (_lisItem_Data[k].nExp >= _nMax)
                        {
                            ++_lisItem_Data[k].nLevel;
                            _lisItem_Data[k].nExp -= _nMax;
                        }
                        _bItem = true;

                        SB_Item_Data equip = GameManager.Instance.localGame_DB.Get_Equip_ItemData(_item_Data.part_Type);
                        if (equip.nIndex != 0 && equip.nIndex == _item_Data.nIndex)
                        {
                            ModelManager.Instance.player.Apply_State(_item_Data.part_Type, _item_Data);
                        }
                        break;
                    }
                }
                if (!_bItem)
                {
                    Effect_ApplyItem(_item_Data);
                    _lisItem_Data.Add(_item_Data);
                }
                yield return waitForSeconds;
                scrollRect.normalizedPosition = new Vector2(1, 0);
            }
            GameManager.Instance.Save_DB();
            Set_Gold();
        }
    }
    public void Effect_ApplyItem(SB_Item_Data item_Data)
    {
        int _nEffect_Count = 0;
        List<int> _lisEff_Index = null;
        if (item_Data.part_Type == ePart_Type.Weapon)
        {
            _nEffect_Count = TableManager.Instance.weaponTable.Get_WeaponDate(item_Data.nIndex).nEffect_Count;
            _lisEff_Index = TableManager.Instance.weaponTable.Get_ListEffect_Index(item_Data.nIndex);
        }
        else
        {
            _nEffect_Count = TableManager.Instance.defendTable.Get_DefendData(item_Data.nIndex).nEffect_Count;
            _lisEff_Index = TableManager.Instance.defendTable.Get_ListEffect_Index(item_Data.nIndex);
        }
        for (int i = 0; i < _nEffect_Count; ++i)
        {
            int nRandom = Random.Range(0, _lisEff_Index.Count);
            StateData _effectData = TableManager.Instance.stateTable.Get_StateData(_lisEff_Index[nRandom]);
            SB_Equip_Item_State_Data _data = new SB_Equip_Item_State_Data();
            _data.nIndex = _effectData.nIndex;
            _data.fValue = Mathf.Floor(Random.Range(_effectData.fEquip_Value_Min, _effectData.fEquip_Value_Max) * 10f) / 10f;
            item_Data.lisState_Data.Add(_data);
        }
    }
    public int Get_Percent_Rank(int nRank)
    {
        int _nIndex = 1;
        int _nRandom = Random.Range(1, nPercent_Max);
        switch (nRank)
        {
            case 2:
                if (_nRandom <= lisPercent[0])
                    _nIndex = 1;
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    _nIndex = 2;
                break;
            case 3:
                if (_nRandom <= lisPercent[0])
                    _nIndex = 1;
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    _nIndex = 2;
                else if (_nRandom > lisPercent[0] + lisPercent[1] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2])
                    _nIndex = 3;
                break;
            case 4:
                if (_nRandom <= lisPercent[0])
                    _nIndex = 1;
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    _nIndex = 2;
                else if (_nRandom > lisPercent[0] + lisPercent[1] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2])
                    _nIndex = 3;
                else if (_nRandom > lisPercent[0] + lisPercent[1] + lisPercent[2] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3])
                    _nIndex = 4;
                break;
            case 5:
                if (_nRandom <= lisPercent[0])
                    _nIndex = 1;
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    _nIndex = 2;
                else if (_nRandom > lisPercent[0] + lisPercent[1] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2])
                    _nIndex = 3;
                else if (_nRandom > lisPercent[0] + lisPercent[1] + lisPercent[2] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3])
                    _nIndex = 4;
                else if (_nRandom > lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3] + lisPercent[4])
                    _nIndex = 5;
                break;
        }
        return _nIndex;
    }
    public override void Close()
    {
        base.Close();
        objcetPool_ItemSlot.Return_All();
    }
}
