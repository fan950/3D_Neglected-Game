using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory_Slot : MonoBehaviour
{
    [Header("Btn")]
    public UIBtn uiBtn;
    [Header("Img")]
    public Image item_Img;
    public Image exp_Img;
    [Header("Tmp")]
    public TextMeshProUGUI level_Tmp;
    public TextMeshProUGUI exp_Tmp;
    [Header("Obj")]
    public GameObject[] arrStar_Obj;
    private SB_Item_Data item_Data;
    public int nIndex { get { return item_Data.nIndex; } }
    public ePart_Type part_Type { get { return item_Data.part_Type; } }
    public void Init(SB_Item_Data item_Data, Action<SB_Item_Data> action = null)
    {
        if (uiBtn == null)
            uiBtn = GetComponent<UIBtn>();
        uiBtn.Init(delegate
        {
            if (action != null)
                action(this.item_Data);
        });

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
        Set_Exp();
    }

    public void Set_Exp()
    {
        float _fExp_Max = item_Data.nLevel * 10.0f;
        level_Tmp.text = TableManager.Instance.stringTable.Get_String("Lv.") + item_Data.nLevel.ToString();
        exp_Tmp.text = string.Format("{0}/{1}", item_Data.nExp, _fExp_Max);
        exp_Img.rectTransform.anchoredPosition = new Vector2(item_Data.nExp / _fExp_Max * exp_Img.rectTransform.sizeDelta.x - exp_Img.rectTransform.sizeDelta.x, exp_Img.rectTransform.anchoredPosition.y);
    }
}
