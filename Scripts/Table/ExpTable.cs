using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExpData
{
    public int nIndex;
    public int nLevel;
    public int nExp_Max;
    public int nHp;
}

public class ExpTable
{
    private List<ExpData> lisExpData = new List<ExpData>();

    public void Add_StageData(ExpData expData)
    {
        lisExpData.Add(expData);
    }

    public int Get_ExpMax(int nLevel)
    {
        return lisExpData.Find(_ => _.nLevel == nLevel).nExp_Max;
    }
    public int Get_Hp(int nLevel)
    {
        return lisExpData.Find(_ => _.nLevel == nLevel).nHp;
    }
}