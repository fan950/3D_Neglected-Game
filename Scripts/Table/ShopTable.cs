using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopData
{
    public int nIndex;
    public string sName;
    public eBuy_Type buyType;
    public int nPrice;
    public eShop_Value shop_Value;
    public int nValue;
    public string sPath;
    public string sPath_Buy;
}

public class ShopTable
{
    private List<ShopData> lisShopData = new List<ShopData>();

    public void Add_ShopData(ShopData expData)
    {
        lisShopData.Add(expData);
    }
    public List<ShopData> Get_LisShopData()
    {
        return lisShopData;
    }
    public ShopData Get_ShopData(int nIndex)
    {
        return lisShopData.Find(_ => _.nIndex == nIndex);
    }
    public ShopData Get_ShopData(string sName)
    {
        return lisShopData.Find(_ => _.sName == sName);
    }
}