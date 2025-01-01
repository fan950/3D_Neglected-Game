using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StringData
{
    public int nIndex;
    public string sKr;
    public string sEn;
    public string sPath;
}

public class StringTable
{
    private List<StringData> lisStringData = new List<StringData>();

    public void Add_StringData(StringData stringData)
    {
        lisStringData.Add(stringData);
    }

    public string Get_String(int nIndex)
    {
        switch (GameManager.Instance.localize_Type)
        {
            case eLocalize_Type.Kr:
                return lisStringData[nIndex].sKr;
            default:
                return lisStringData[nIndex].sEn;
        }
    }
    public string Get_String(string sKey)
    {
        StringData _stringData = lisStringData.Find(_ => _.sPath.ToLower() == sKey.ToLower());

        if (_stringData == null)
            return string.Empty;

        switch (GameManager.Instance.localize_Type)
        {
            case eLocalize_Type.Kr:
                return _stringData.sKr;
            default:
                return _stringData.sEn;
        }
    }
}