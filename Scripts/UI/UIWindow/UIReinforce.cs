using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIReinforce : UIWindow
{
    public UIBtn weapon_Btn;
    public UIBtn defend_Btn;
    public UIBtn black_Btn;
    public UIBtn reinforce_Btn;

    private SB_Item_Data select_Item;
    [Header("Reinforce")]
    public UIReinforce_Slot reinforce_Slot;
    public UIReinforce_Slot[] arrMaterial_Slot;
    public Image[] arrLine_Img;
    [Header("Scroll")]
    public ScrollRect scrollRect;

    private ObjcetPool<UIInventory_Slot> uiInvenItem_Pool;
    private Coroutine Coro_Reinforce;

    private const string sItem_Path = "UI/InGameScene/UIInventroySlot";
    private int nBase_Max = 60;
    private float[] arrPercent = { 50, 40, 30, 20, 10 };
    private bool bWeapon_Type;
    public override void Init()
    {
        base.Init();

        uiInvenItem_Pool = new ObjcetPool<UIInventory_Slot>();
        uiInvenItem_Pool.Init(sItem_Path, nBase_Max, scrollRect.content);

        weapon_Btn.Init(delegate
        {
            for (int i = 0; i < arrLine_Img.Length; ++i)
            {
                arrLine_Img[i].fillAmount = 0;
            }
            scrollRect.normalizedPosition = Vector2.one;
            Set_PartBag(true);
        });
        defend_Btn.Init(delegate
        {
            for (int i = 0; i < arrLine_Img.Length; ++i)
            {
                arrLine_Img[i].fillAmount = 0;
            }
            scrollRect.normalizedPosition = Vector2.one;
            Set_PartBag(false);
        });
        black_Btn.Init(delegate
        {
            black_Btn.gameObject.SetActive(false);
            select_Item = null;
        });
        reinforce_Btn.Init(delegate
        {
            for (int i = 0; i < arrLine_Img.Length; ++i)
            {
                arrLine_Img[i].fillAmount = 0;
            }
            if (Coro_Reinforce != null)
                StopCoroutine(Coro_Reinforce);

            Coro_Reinforce = StartCoroutine(Reinforce_Coro(Reinforce));
        });

        reinforce_Slot.Init(delegate
        {
            for (int i = 0; i < arrLine_Img.Length; ++i)
            {
                arrLine_Img[i].fillAmount = 0;
            }
            if (!black_Btn.gameObject.activeSelf)
            {
                reinforce_Slot.Set_Remove();
                select_Item = null;
            }
            else
            {
                Check_ItemData();
                reinforce_Slot.Set_Install(select_Item);
                black_Btn.gameObject.SetActive(false);
                select_Item = null;
            }

            if (reinforce_Slot.item_Data != null && arrMaterial_Slot[0].item_Data != null && arrMaterial_Slot[1].item_Data != null)
            {
                reinforce_Btn.Change_interactable(true);
            }
            else
            {
                reinforce_Btn.Change_interactable(false);
            }
        });
        for (int i = 0; i < arrMaterial_Slot.Length; ++i)
        {
            UIReinforce_Slot uiReinforce_Slot = arrMaterial_Slot[i];
            uiReinforce_Slot.Init(delegate
            {
                for (int i = 0; i < arrLine_Img.Length; ++i)
                {
                    arrLine_Img[i].fillAmount = 0;
                }
                if (!black_Btn.gameObject.activeSelf)
                {
                    uiReinforce_Slot.Set_Remove();
                    select_Item = null;
                }
                else
                {
                    Check_ItemData();
                    uiReinforce_Slot.Set_Install(select_Item);
                    black_Btn.gameObject.SetActive(false);
                    select_Item = null;
                }

                if (reinforce_Slot.item_Data != null && arrMaterial_Slot[0].item_Data != null && arrMaterial_Slot[1].item_Data != null)
                {
                    reinforce_Btn.Change_interactable(true);
                }
                else
                {
                    reinforce_Btn.Change_interactable(false);
                }
            });
        }
    }
    public override void Open()
    {
        base.Open();

        reinforce_Btn.Change_interactable(false);
        for (int i = 0; i < arrLine_Img.Length; ++i)
        {
            arrLine_Img[i].fillAmount = 0;
        }

        black_Btn.gameObject.SetActive(false);
        Set_PartBag(true);
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
            black_Btn.gameObject.SetActive(true);
            select_Item = itemData;
        });
    }
    public void Check_ItemData()
    {
        if (reinforce_Slot.item_Data == select_Item)
        {
            reinforce_Slot.Set_Remove();
        }
        for (int i = 0; i < arrMaterial_Slot.Length; ++i)
        {
            if (arrMaterial_Slot[i].item_Data == select_Item)
            {
                arrMaterial_Slot[i].Set_Remove();
            }
        }
    }

    public void Reinforce()
    {
        string _sResult = "Fail";
        SB_Item_Data _reinforce = GameManager.Instance.localGame_DB.Get_ItemData(reinforce_Slot.item_Data.part_Type, reinforce_Slot.item_Data.nIndex);
        for (int i = 0; i < arrMaterial_Slot.Length; ++i)
        {
            GameManager.Instance.localGame_DB.Remove_ItemData(arrMaterial_Slot[i].item_Data.part_Type, arrMaterial_Slot[i].item_Data.nIndex);
            UIInventory_Slot _uiInventroySlot = uiInvenItem_Pool.Get_ListActive().Find(_ => _.nIndex == arrMaterial_Slot[i].item_Data.nIndex && _.part_Type == arrMaterial_Slot[i].item_Data.part_Type);
            uiInvenItem_Pool.Return(_uiInventroySlot);
            arrMaterial_Slot[i].Set_Remove();
        }
        int _nRandom = UnityEngine.Random.Range(1, 101);

        if (_nRandom <= arrPercent[_reinforce.nStar])
        {
            _sResult = "Success";
            ++_reinforce.nStar;

            UIInventory_Slot _uiInventroySlot = uiInvenItem_Pool.Get_ListActive().Find(_ => _.nIndex == _reinforce.nIndex && _.part_Type == _reinforce.part_Type);

            if (_uiInventroySlot != null)
            {
                for (int i = 0; i < _uiInventroySlot.arrStar_Obj.Length; ++i)
                {
                    if (_reinforce.nStar > i)
                        _uiInventroySlot.arrStar_Obj[i].SetActive(true);
                    else
                        _uiInventroySlot.arrStar_Obj[i].SetActive(false);
                }
            }
            for (int i = 0; i < reinforce_Slot.arrStar_Obj.Length; ++i)
            {
                if (_reinforce.nStar > i)
                    reinforce_Slot.arrStar_Obj[i].SetActive(true);
                else
                    reinforce_Slot.arrStar_Obj[i].SetActive(false);
            }
            GameManager.Instance.Save_DB();
        }
        UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
        _uiOk_Popup.OnShow(TableManager.Instance.stringTable.Get_String("Reinforce") + " " + TableManager.Instance.stringTable.Get_String(_sResult));

        if (reinforce_Slot.item_Data != null && arrMaterial_Slot[0].item_Data != null && arrMaterial_Slot[1].item_Data != null)
        {
            reinforce_Btn.Change_interactable(true);
        }
        else
        {
            reinforce_Btn.Change_interactable(false);
        }
    }
    public IEnumerator Reinforce_Coro(Action action)
    {
        float _fGage_Time = 0;
        while (true)
        {
            yield return null;

            for (int i = 0; i < arrLine_Img.Length; ++i)
            {
                arrLine_Img[i].fillAmount = _fGage_Time;
            }

            _fGage_Time += Time.deltaTime;

            if (_fGage_Time >= 1.0f)
            {
                if (action != null)
                    action();
                break;
            }
        }
    }
}
