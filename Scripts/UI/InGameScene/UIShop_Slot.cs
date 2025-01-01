using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShop_Slot : MonoBehaviour
{
    [Header("Item")]
    public Image item_Img;
    public TextMeshProUGUI item_Name_Tmp;
    [Header("Price")]
    public Image price_Img;
    public TextMeshProUGUI price_Tmp;
    [Header("UIBtn")]
    public UIBtn buy_Btn;

    private ShopData shopData;
    public void Init(int nIndex)
    {
        shopData = TableManager.Instance.shopTable.Get_ShopData(nIndex);
        item_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UIShop_Atlas, shopData.sPath);
        item_Name_Tmp.text = TableManager.Instance.stringTable.Get_String(shopData.sName);
        price_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UIShop_Atlas, shopData.sPath_Buy);
        string _sTemp = string.Empty;
        int _nCount = 0;
        string _sValue = shopData.nPrice.ToString();
        for (int i = _sValue.Length - 1; i >= 0; --i)
        {
            if (_nCount == 3)
            {
                _sTemp = "," + _sTemp;
                _nCount = 0;
            }
            _sTemp = _sValue[i] + _sTemp;
            ++_nCount;
        }
        price_Tmp.text = _sTemp;
        buy_Btn.Init(Buy);
    }

    public void Buy()
    {
        LocalGameData localGameData = GameManager.Instance.localGame_DB;
        UIBuy_Popup uiBuy_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIBuy_Popup) as UIBuy_Popup;
        uiBuy_Popup.OnShow(item_Img.sprite, item_Name_Tmp.text, delegate
          {
              uiBuy_Popup.Close();

              int _nValue = 0;
              switch (shopData.buyType)
              {
                  case eBuy_Type.Gold:
                      _nValue = localGameData.Get_Gold() - shopData.nPrice;
                      if (_nValue < 0)
                      {
                          UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
                          _uiOk_Popup.Buy_OnShow("Shortage", shopData.buyType.ToString());
                          return;
                      }
                      localGameData.Set_Gold(_nValue);
                      break;
                  case eBuy_Type.Crystal:
                      _nValue = localGameData.Get_Crystal() - shopData.nPrice;
                      if (_nValue < 0)
                      {
                          UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
                          _uiOk_Popup.Buy_OnShow("Shortage", shopData.buyType.ToString());
                          return;
                      }
                      localGameData.Set_Crystal(_nValue);
                      break;
              }

              switch (shopData.shop_Value)
              {
                  case eShop_Value.Gold:
                      localGameData.Set_Gold(localGameData.Get_Gold() + shopData.nValue);
                      break;
                  case eShop_Value.Crystal:
                      localGameData.Set_Crystal(localGameData.Get_Crystal() + shopData.nValue);
                      break;
              }

              UIManager.Instance.Set_Gold();
              UIShop _uiShop = UIManager.Instance.Get_UIWindow(eUIWindow_Type.UIShop) as UIShop;
              _uiShop.Set_Gold();
          });
    }

}
