using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopExpData
{
    public int nIndex;
    public int nLevel;
    public int nLevel_Max;
    public int nRank_Max;
    public string sArrProbability;
}

public class ShopExpTable
{
    private List<ShopExpData> lisShopExpData = new List<ShopExpData>();

    public void Add_ShopExpData(ShopExpData expData)
    {
        lisShopExpData.Add(expData);
    }

    public ShopExpData Get_ShopExpData(int nLevel)
    {
        return lisShopExpData.Find(_ => _.nLevel == nLevel);
    }
    public List<int> Get_ListPercent(int nLevel, int nPercent)
    {
        List<int> _lisPercent = new List<int>();
        ShopExpData _shopExp = lisShopExpData.Find(_ => _.nLevel == nLevel);
        if (_shopExp != null)
        {
            string _sNum = string.Empty;
            for (int i = 0; i < _shopExp.sArrProbability.Length; ++i)
            {
                string _cTemp = _shopExp.sArrProbability[i].ToString();
                int num = 0;

                switch (_cTemp)
                {
                    case ".":
                        _sNum += _cTemp;
                        break;
                    case ",":
                    case "]":
                        _lisPercent.Add((int)(float.Parse(_sNum) * nPercent));
                        _sNum = string.Empty;
                        break;
                    default:
                        bool _bCheck = int.TryParse(_cTemp, out num);
                        if (_bCheck)
                        {
                            _sNum += _cTemp;
                        }
                        break;
                }               
            }
        }
        return _lisPercent;
    }
}