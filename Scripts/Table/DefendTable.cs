using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefendData
{
    public int nIndex;
    public string sName;

    public int nHp;
    public int nGrade;
    public int nDefend;
    public ePart_Type part_Type;

    public string sArrEffect;
    public int nEffect_Count;

    public string sPath;

    public int Get_Defend(SB_Item_Data _item_Data)
    {
        int _nLevel = _item_Data.nLevel - 1;
        if (_nLevel < 0)
            _nLevel = 0;

        float _fStar = (_item_Data.nStar * 0.1f) + 1.0f;

        return (int)((nDefend + (nDefend * (_nLevel * 0.1f))) * _fStar);
    }
}

public class DefendTable
{
    private List<DefendData> lisDefendData = new List<DefendData>();
    private Dictionary<int, List<DefendData>> dicLevel_Item = new Dictionary<int, List<DefendData>>();

    public void Add_DefendDate(DefendData defendData)
    {
        lisDefendData.Add(defendData);

        if (!dicLevel_Item.ContainsKey(defendData.nGrade))
        {
            dicLevel_Item.Add(defendData.nGrade, new List<DefendData>());
        }
        dicLevel_Item[defendData.nGrade].Add(defendData);
    }

    public DefendData Get_DefendData(int nIndex)
    {
        return lisDefendData.Find(_ => _.nIndex == nIndex);
    }
    public string Get_Path(int nIndex)
    {
        DefendData _defendData = lisDefendData.Find(_ => _.nIndex == nIndex);
        if (_defendData == null)
            return null;

        return _defendData.sPath;
    }
    public string Get_Path_Obj(int nIndex)
    {
        DefendData _defendData = lisDefendData.Find(_ => _.nIndex == nIndex);
        if (_defendData == null)
            return null;

        return "Defend/" + _defendData.sPath;
    }
    public List<int> Get_ListEffect_Index(int nIndex)
    {
        List<int> _lisEffect_Index = new List<int>();

        DefendData _defendData = lisDefendData.Find(_ => _.nIndex == nIndex);
        if (_defendData != null)
        {
            for (int i = 0; i < _defendData.sArrEffect.Length; ++i)
            {
                string _cTemp = _defendData.sArrEffect[i].ToString();
                int num = 0;
                bool _bCheck = int.TryParse(_cTemp, out num);
                if (_bCheck)
                {
                    _lisEffect_Index.Add(num);
                }
            }
        }
        return _lisEffect_Index;
    }

    public DefendData Get_RankRandom_Index(int nIndex)
    {
        int _nItem_Index = Random.Range(0, dicLevel_Item[nIndex].Count);

        return dicLevel_Item[nIndex][_nItem_Index];
    }
}