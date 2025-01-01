using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReinforce_Slot : MonoBehaviour
{
    private UIBtn uiBtn;
    [Header("ItemData")]
    public Image item_Img;
    public GameObject[] arrStar_Obj;
    public SB_Item_Data item_Data;
    public void Init(Action action)
    {
        if (uiBtn == null)
            uiBtn = GetComponent<UIBtn>();

        uiBtn.Init(action);
        item_Img.sprite = null;
        item_Img.gameObject.SetActive(false);
        for (int i = 0; i < arrStar_Obj.Length; ++i)
        {
            arrStar_Obj[i].SetActive(false);
        }
        item_Data = null;
    }
    public void Set_Install(SB_Item_Data item_Data)
    {
        this.item_Data = item_Data;
        if (item_Data.part_Type == ePart_Type.Weapon)
        {
            item_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.Weapon_Atlas, TableManager.Instance.weaponTable.Get_Path(item_Data.nIndex));
        }
        else
        {
            item_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.Defend_Atlas, TableManager.Instance.defendTable.Get_Path(item_Data.nIndex));
        }
        item_Img.gameObject.SetActive(true);

        for (int i = 0; i < arrStar_Obj.Length; ++i)
        {
            if (item_Data.nStar > i)
                arrStar_Obj[i].SetActive(true);
            else
                arrStar_Obj[i].SetActive(false);
        }
    }
    public void Set_Remove()
    {
        this.item_Data = null;
        item_Img.gameObject.SetActive(false);
        item_Img.sprite = null;
        for (int i = 0; i < arrStar_Obj.Length; ++i)
        {
            arrStar_Obj[i].SetActive(false);
        }
    }
}
