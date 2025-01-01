using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterData
{
    public int nIndex;
    public string sName;
    public eMonster_Type monster_Type;

    public int nHp;
    public int nExp;

    public int nAttack;
    public float fAttack_Speed;
    public eAni_Type eAni_Type;
    public eAttack_Type eAttack_Type;
    public float fAttack_Distance;
    public int nSkill_Count;

    public float fMove_Speed;

    public string sPath;
}

public class MonsterTable
{
    private List<MonsterData> lisMonsterData = new List<MonsterData>();

    public void Add_MonsterDate(MonsterData monsterData)
    {
        lisMonsterData.Add(monsterData);
    }
    public MonsterData Get_MonsterDate(int nIndex)
    {
        return lisMonsterData.Find(_ => _.nIndex == nIndex);
    }

    public int Get_Exp(int nIndex)
    {
        return lisMonsterData.Find(_ => _.nIndex == nIndex).nExp;
    }
    public int Get_SkillCount(int nIndex)
    {
        return lisMonsterData.Find(_ => _.nIndex == nIndex).nSkill_Count;
    }
    public int Get_AniType(int nIndex)
    {
        int _nAni = 0;

        switch (lisMonsterData[nIndex-1].eAni_Type)
        {
            case eAni_Type.Stab:
                _nAni = 1;
                break;
            default:
                _nAni = 0;
                break;
        }
        return _nAni;
    }
}