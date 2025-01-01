using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItem_Slot : UIBtn
{
    private SB_Item_Data item_Data;
    public Image item_Img;
    public GameObject[] arrStar_Obj;
    public ePart_Type part_Type { get { return item_Data.part_Type; } }
    public void Init(SB_Item_Data item_Data, Action<SB_Item_Data> action = null)
    {
        base.Init(delegate
        {
            if (action != null)
                action(this.item_Data);
        });
        Btn_RectPos().anchoredPosition3D = Vector3.zero;
        if (item_Data.part_Type == ePart_Type.Weapon)
        {
            item_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.Weapon_Atlas, TableManager.Instance.weaponTable.Get_Path(item_Data.nIndex));
        }
        else
        {
            item_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.Defend_Atlas, TableManager.Instance.defendTable.Get_Path(item_Data.nIndex));
        }
        this.item_Data = item_Data;
        for (int i = 0; i < arrStar_Obj.Length; ++i)
        {
            if (item_Data.nStar > i)
                arrStar_Obj[i].SetActive(true);
            else
                arrStar_Obj[i].SetActive(false);
        }
    }
}
