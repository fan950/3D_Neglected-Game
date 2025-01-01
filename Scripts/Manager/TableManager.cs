using System;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    private static Dictionary<eTable_Type, TableManager> dicTable = new Dictionary<eTable_Type, TableManager>();
    private Dictionary<int, Dictionary<string, string>> dicData = new Dictionary<int, Dictionary<string, string>>();

    public static bool isAllTableLoaded = false;

    public MonsterTable monsterTable;
    public WeaponTable weaponTable;
    public DefendTable defendTable;
    public StageTable stageTable;
    public ExpTable expTable;
    public StringTable stringTable;
    public SkillTable skillTable;
    public ShopTable shopTable;
    public ShopExpTable shopExpTable;
    public StateTable stateTable;

    public void Load(eTable_Type eTableType, string sText, string sType)
    {
        string[] rows = sText.Split('\n');

        for (int i = 0; i < rows.Length; ++i)
        {
            if (rows[i].Contains("\r"))
            {
                rows[i] = rows[i].Remove(rows[i].Length - 1);
            }
        }

        int rowCount = rows.Length;

        if (string.IsNullOrEmpty(rows[rows.Length - 1]))
        {
            rowCount--;
        }

        string[] subject = rows[0].Split(',');
        for (int row = 1; row < rowCount; ++row)
        {
            string[] values = rows[row].Split(',');
            List<string> _lisTemp = new List<string>();
            bool _bArr = false;
            int _nCount = 0;
            for (int i = 0; i < values.Length; ++i)
            {
                if (_bArr)
                {
                    if (values[i].Contains("]"))
                    {
                        _bArr = false;
                    }
                    _lisTemp[_nCount] += "," + values[i];
                    continue;
                }

                if (!values[i].Contains("[") && !values[i].Contains("]"))
                {
                    _lisTemp.Add(values[i]);
                    _bArr = false;
                    continue;
                }

                if (values[i].Contains("[") && values[i].Contains("]"))
                {
                    _lisTemp.Add(values[i]);
                    _bArr = false;
                    continue;
                }
                _bArr = true;
                _lisTemp.Add(values[i]);

                _nCount = _lisTemp.Count - 1;
            }
            int val = -1;
            if (int.TryParse(_lisTemp[0], out val))
            {
                if (dicData.ContainsKey(val) == false)
                {
                    dicData.Add(val, new Dictionary<string, string>());
                }
            }

            for (int col = 0; col < subject.Length; ++col)
            {
                if (dicData[val].ContainsKey(subject[col]) == false)
                {
                    dicData[val].Add(subject[col], _lisTemp[col]);
                }
            }
        }
        switch (eTableType)
        {
            case eTable_Type.MonsterTable:
                {
                    monsterTable = new MonsterTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        MonsterData monster = new MonsterData();

                        monster.nIndex = int.Parse(date.Value["Index"]);
                        monster.sName = date.Value["Name"];

                        monster.nHp = int.Parse(date.Value["Hp"]);
                        monster.nExp = int.Parse(date.Value["Exp"]);

                        monster.nAttack = int.Parse(date.Value["Attack"]);
                        monster.fAttack_Speed = float.Parse(date.Value["Attack_Speed"]);
                        monster.eAni_Type =(eAni_Type)Enum.Parse(typeof(eAni_Type), date.Value["Ani_Type"]);
                        monster.eAttack_Type = (eAttack_Type)Enum.Parse(typeof(eAttack_Type), date.Value["Attack_Type"]);
                        monster.monster_Type = (eMonster_Type)Enum.Parse(typeof(eMonster_Type), date.Value["Monster_Type"]);
                        monster.fAttack_Distance = float.Parse(date.Value["Attack_Distance"]);

                        monster.fMove_Speed = float.Parse(date.Value["Move_Speed"]);
                        monster.nSkill_Count = int.Parse(date.Value["Skill_Count"]);

                        monster.sPath = date.Value["Path"];

                        monsterTable.Add_MonsterDate(monster);
                    }
                }
                break;
            case eTable_Type.WeaponTable:
                {
                    weaponTable = new WeaponTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        WeaponData weapon = new WeaponData();

                        weapon.nIndex = int.Parse(date.Value["Index"]);
                        weapon.sName = date.Value["Name"];
                        weapon.nGrade = int.Parse(date.Value["Grade"]);

                        weapon.nAttack = int.Parse(date.Value["Attack"]);
                        weapon.eAni_Type = (eAni_Type)Enum.Parse(typeof(eAni_Type), date.Value["Ani_Type"]);
                        weapon.fAttack_Speed = float.Parse(date.Value["Attack_Speed"]);
                        weapon.eAttack_Type = (eAttack_Type)Enum.Parse(typeof(eAttack_Type), date.Value["Attack_Type"]);
                        weapon.fAttack_Distance = float.Parse(date.Value["Attack_Distance"]);

                        weapon.sArrEffect = date.Value["Arr_Effect_Index"];
                        weapon.nEffect_Count = int.Parse(date.Value["Effect_Count"]);

                        weapon.sPath = date.Value["Path"];

                        weaponTable.Add_WeaponDate(weapon);
                    }
                }
                break;
            case eTable_Type.StageTable:
                {
                    stageTable = new StageTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        StageData stage = new StageData();

                        stage.nIndex = int.Parse(date.Value["Index"]);
                        stage.sArrMonster = date.Value["Arr_Monster"];
                        stage.sArrPercent = date.Value["Arr_Percent"];
                        stage.fCoolTime = float.Parse(date.Value["CoolTime"]);
                        stage.fReinforce = float.Parse(date.Value["Reinforce"]);
                        stage.sName = date.Value["Name"];
                        stage.fBossTime = int.Parse(date.Value["BossTime"]);
                        stage.nBossIndex = int.Parse(date.Value["BossIndex"]);
                        stage.sScene = date.Value["Scene"];

                        stageTable.Add_StageData(stage);
                    }
                }
                break;
            case eTable_Type.DefendTable:
                {
                    defendTable = new DefendTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        DefendData defend = new DefendData();

                        defend.nIndex = int.Parse(date.Value["Index"]);
                        defend.sName = date.Value["Name"];
                        defend.nGrade = int.Parse(date.Value["Grade"]);

                        defend.nHp = int.Parse(date.Value["Hp"]);
                        defend.nDefend = int.Parse(date.Value["Defend"]);
                        defend.part_Type = (ePart_Type)Enum.Parse(typeof(ePart_Type), date.Value["Defend_Type"]);

                        defend.sArrEffect = date.Value["Arr_Effect_Index"];
                        defend.nEffect_Count = int.Parse(date.Value["Effect_Count"]);

                        defend.sPath = date.Value["Path"];

                        defendTable.Add_DefendDate(defend);
                    }
                }
                break;
            case eTable_Type.ExpTable:
                {
                    expTable = new ExpTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        ExpData exp = new ExpData();

                        exp.nIndex = int.Parse(date.Value["Index"]);
                        exp.nLevel = int.Parse(date.Value["Level"]);
                        exp.nExp_Max = int.Parse(date.Value["Level_Max"]);
                        exp.nHp = int.Parse(date.Value["Hp"]);

                        expTable.Add_StageData(exp);
                    }
                }
                break;
            case eTable_Type.StringTable:
                {
                    stringTable = new StringTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        StringData stringData = new StringData();

                        stringData.nIndex = int.Parse(date.Value["Index"]);
                        stringData.sKr = date.Value["Kr"];
                        stringData.sEn = date.Value["En"];
                        stringData.sPath = date.Value["Path"];

                        stringTable.Add_StringData(stringData);
                    }
                }
                break;
            case eTable_Type.SkillTable:
                {
                    skillTable = new SkillTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        SkillData skillData = new SkillData();
                        skillData.nIndex = int.Parse(date.Value["Index"]);
                        skillData.sName = date.Value["Name"];
                        skillData.skill_Type = (eSkill_Type)Enum.Parse(typeof(eSkill_Type), date.Value["Type"]);
                        skillData.fValue = float.Parse(date.Value["Value"]);
                        skillData.nMax_Level = int.Parse(date.Value["Max_Level"]);
                        skillData.nAni = int.Parse(date.Value["Ani"]);
                        skillData.nCount = int.Parse(date.Value["Count"]);
                        skillData.nUpgrade_Gold = int.Parse(date.Value["Upgrade_Gold"]);
                        skillData.fUpgrade_Value = float.Parse(date.Value["Upgrade_Value"]);
                        skillData.fCoolTime = float.Parse(date.Value["CoolTime"]);
                        skillData.sDes = date.Value["Des"];
                        skillData.sPath = date.Value["Path"];

                        skillTable.Add_SkillData(skillData);
                    }
                }
                break;
            case eTable_Type.ShopTable:
                {
                    shopTable = new ShopTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        ShopData shopData = new ShopData();
                        shopData.nIndex = int.Parse(date.Value["Index"]);
                        shopData.sName = date.Value["Name"];
                        shopData.buyType = (eBuy_Type)Enum.Parse(typeof(eBuy_Type), date.Value["BuyType"]);
                        shopData.nPrice = int.Parse(date.Value["Price"]);
                        shopData.shop_Value = (eShop_Value)Enum.Parse(typeof(eShop_Value), date.Value["Value_Item"]);
                        shopData.nValue = int.Parse(date.Value["Value"]);
                        shopData.sPath = date.Value["Path"];
                        shopData.sPath_Buy = date.Value["Path_Buy"];

                        shopTable.Add_ShopData(shopData);
                    }
                }
                break;
            case eTable_Type.ShopExpTable:
                {
                    shopExpTable = new ShopExpTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        ShopExpData shopExpData = new ShopExpData();
                        shopExpData.nIndex = int.Parse(date.Value["Index"]);
                        shopExpData.nLevel = int.Parse(date.Value["Level"]);
                        shopExpData.nLevel_Max = int.Parse(date.Value["Level_Max"]);
                        shopExpData.nRank_Max = int.Parse(date.Value["Rank_Max"]);
                        shopExpData.sArrProbability = date.Value["Arr_Probability"];

                        shopExpTable.Add_ShopExpData(shopExpData);
                    }
                }
                break;
            case eTable_Type.StateTable:
                {
                    stateTable = new StateTable();
                    foreach (KeyValuePair<int, Dictionary<string, string>> date in dicData)
                    {
                        StateData stateData = new StateData();
                        stateData.nIndex = int.Parse(date.Value["Index"]);
                        stateData.sName = date.Value["Name"];
                        stateData.state_Type = (eState_Type)Enum.Parse(typeof(eState_Type), date.Value["Type"]);
                        stateData.fEquip_Value_Min = float.Parse(date.Value["Equip_Value_Min"]);
                        stateData.fEquip_Value_Max = float.Parse(date.Value["Equip_Value_Max"]);
                        stateData.fUpgrade_Value = float.Parse(date.Value["Upgrade_Value"]);
                        stateData.nUpgrade_Gold = int.Parse(date.Value["Upgrade_Gold"]);
                        stateData.nUpgrade_Max = int.Parse(date.Value["Upgrade_Max"]);
                        stateData.sDes = date.Value["Des"];
                        stateData.sPath = date.Value["Path"];

                        stateTable.Add_StateData(stateData);
                    }
                }
                break;
            default:
                break;
        }
        dicData.Clear();
    }

    public void Add_Table(eTable_Type table)
    {
        if (dicTable.ContainsKey(table) == false)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Table/" + table.ToString());

            Load(table, textAsset.text, table.ToString());
        }
    }

    public void All_TableLoading()
    {
        eTable_Type _tableType = eTable_Type.MonsterTable;
        for (int i = 0; i < (int)eTable_Type.Max; ++i)
        {
            Add_Table(_tableType);
            ++_tableType;
        }

        isAllTableLoaded = true;
    }
}
