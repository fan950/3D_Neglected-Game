using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageData
{
    public int nIndex;
    public string sArrMonster;
    public string sArrPercent;
    public float fCoolTime;
    public float fReinforce;
    public string sName;
    public string sScene;
    public int nBossIndex;
    public float fBossTime;
}

public class StageTable
{
    private List<StageData> lisStageData = new List<StageData>();

    public void Add_StageData(StageData stageData)
    {
        lisStageData.Add(stageData);
    }

    public List<int> Get_ListMonster(int nIndex)
    {
        List<int> _lisMonster = new List<int>();

        StageData _stageData = lisStageData.Find(_ => _.nIndex == nIndex);
        if (_stageData != null)
        {
            for (int i = 0; i < _stageData.sArrMonster.Length; ++i)
            {
                string _cTemp = _stageData.sArrMonster[i].ToString();
                int num = 0;
                bool _bCheck = int.TryParse(_cTemp, out num);
                if (_bCheck)
                {
                    _lisMonster.Add(num);
                }
            }
        }
        return _lisMonster;
    }
    public StageData Get_StageData(int nIndex)
    {
        StageData _stageData = lisStageData.Find(_ => _.nIndex == nIndex);
        return _stageData;
    }
    public List<StageData> Get_ListStageData()
    {
        return lisStageData;
    }
    public float Get_BossTime(int nIndex)
    {
        return lisStageData.Find(_ => _.nIndex == nIndex).fBossTime;
    }
    public float Get_Reinforce(int nIndex)
    {
        return lisStageData.Find(_ => _.nIndex == nIndex).fReinforce;
    }
    public int Get_BossIndex(int nIndex)
    {
        return lisStageData.Find(_ => _.nIndex == nIndex).nBossIndex;
    }
    public List<int> Get_ListPercent(int nIndex, int nPercent)
    {
        List<int> _lisPercent = new List<int>();
        StageData _stageData = lisStageData.Find(_ => _.nIndex == nIndex);
        if (_stageData != null)
        {
            string _sNum = string.Empty;
            for (int i = 0; i < _stageData.sArrPercent.Length; ++i)
            {
                string _cTemp = _stageData.sArrPercent[i].ToString();
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