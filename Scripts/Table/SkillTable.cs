using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SkillData
{
    public int nIndex;
    public string sName;
    public eSkill_Type skill_Type;
    public float fValue;
    public float fDuration;
    public int nCount;
    public int nAni;
    public int nMax_Level;
    public float fUpgrade_Value;
    public int nUpgrade_Gold;
    public float fCoolTime;
    public string sDes;
    public string sPath;
}
public class SkillTable
{
    private List<SkillData> lisSkillData = new List<SkillData>();

    public void Add_SkillData(SkillData skillData)
    {
        lisSkillData.Add(skillData);
    }
    public SkillData Get_SkillData(int nIndex)
    {
        return lisSkillData.Find(_ => _.nIndex == nIndex);
    }
    public List<SkillData> Get_SkillList()
    {
        return lisSkillData;
    }
}