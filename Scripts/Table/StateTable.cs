using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class StateData
{
    public int nIndex;
    public string sName;
    public eState_Type state_Type;
    public float fEquip_Value_Min;
    public float fEquip_Value_Max;
    public float fUpgrade_Value;
    public int nUpgrade_Gold;
    public int nUpgrade_Max;
    public string sDes;
    public string sPath;
}
public class StateTable
{
    private List<StateData> lisStateData = new List<StateData>();

    public void Add_StateData(StateData skillData)
    {
        lisStateData.Add(skillData);
    }
    public StateData Get_StateData(int nIndex)
    {
        return lisStateData.Find(_ => _.nIndex == nIndex);
    }
    public List<StateData> Get_StateList()
    {
        return lisStateData;
    }
    public eState_Type Get_State_Type(int nIndex)
    {
        StateData _stateData = lisStateData.Find(_ => _.nIndex == nIndex);
        if (_stateData == null)
        {
            return eState_Type.None;
        }
        return _stateData.state_Type;
    }
}