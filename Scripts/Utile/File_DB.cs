using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;

public class File_DB
{
    public static void Save(LocalGameData localGameData)
    {
        string jsonString = DataToJson(localGameData);
        SaveFile(jsonString);
    }

    public static LocalGameData Load()
    {
        if (!File.Exists(GetPath()))
        {
            LocalGameData localGameData = new LocalGameData();
            localGameData.Init();

            return localGameData;
        }

        string encryptData = LoadFile(GetPath());
        LocalGameData data = JsonToData(encryptData);
        return data;
    }

    static string DataToJson(LocalGameData sd)
    {
        string jsonData = JsonUtility.ToJson(sd);
        return jsonData;
    }

    static LocalGameData JsonToData(string jsonData)
    {
        LocalGameData data = JsonUtility.FromJson<LocalGameData>(jsonData);
        return data;
    }

    static void SaveFile(string jsonData)
    {
        using (FileStream fs = new FileStream(GetPath(), FileMode.Create, FileAccess.Write))
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);

            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }

    static string LoadFile(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            fs.Close();
            return jsonString;
        }
    }
    static string GetPath()
    {
        string filePath = string.Empty;
        string gameDataFileName = "save.txt";

        filePath = Application.persistentDataPath + "/" + gameDataFileName;

        return Path.Combine(filePath);
    }

}
public class LocalGameData
{
    public SB_Player_Data player_Data;
    public SB_Shop_Data shop_Data;
    public List<SB_Item_Data> lisItem_Data;
    public List<SB_Skill_Data> lisSkill_Data;
    public List<SB_Skill_State_Data> lisState_Data;

    public void Init()
    {
        player_Data = new SB_Player_Data();
        player_Data.Init();

        shop_Data = new SB_Shop_Data();
        shop_Data.Init();

        lisItem_Data = new List<SB_Item_Data>();
        lisSkill_Data = new List<SB_Skill_Data>();
        lisState_Data = new List<SB_Skill_State_Data>();
    }
    public int Get_Stage()
    {
        return player_Data.nCurrentStage;
    }
    public void Set_Stage(int nStage)
    {
        player_Data.nCurrentStage = nStage;
    }
    public int Get_Clear_Stage()
    {
        return player_Data.nClearStage;
    }
    public void Set_Clear_Stage(int nStage)
    {
        if (player_Data.nClearStage < nStage)
        {
            player_Data.nClearStage = nStage;
        }
    }
    #region ItemData
    public SB_Item_Data Get_ItemData(ePart_Type part_Type, int nIndex)
    {
        return lisItem_Data.Find(_ => _.nIndex == nIndex && _.part_Type == part_Type);
    }
    public void Remove_ItemData(ePart_Type part_Type, int nIndex)
    {
        SB_Item_Data _item_Data = lisItem_Data.Find(_ => _.nIndex == nIndex && _.part_Type == part_Type);
        lisItem_Data.Remove(_item_Data);

        if (Get_Equip_ItemData(part_Type).nIndex == nIndex)
        {
            Set_Remove_Equip(part_Type);
        }
    }
    public void Set_ItemData(SB_Item_Data item_Data)
    {
        bool _bGet = false;
        for (int i = 0; i < lisItem_Data.Count; ++i)
        {
            if (lisItem_Data[i].nIndex == item_Data.nIndex)
            {
                lisItem_Data[i] = item_Data;
                _bGet = true;
                break;
            }
        }
        if (!_bGet)
            lisItem_Data.Add(item_Data);

        GameManager.Instance.Save_DB();
    }
    public void Set_Equip_ItemData(ePart_Type part_Type, SB_Item_Data item_Data)
    {
        bool bEquip = false;
        for (int i = 0; i < player_Data.lisEquip_ItemData.Count; ++i)
        {
            if (player_Data.lisEquip_ItemData[i].part_Type == part_Type)
            {
                player_Data.lisEquip_ItemData[i] = item_Data;
                bEquip = true;
                break;
            }
        }
        if (!bEquip)
            player_Data.lisEquip_ItemData.Add(item_Data);
        GameManager.Instance.Save_DB();
    }
    public void Set_Change_Equip(SB_Item_Data item_Equip, SB_Item_Data item_Remove)
    {
        Set_Equip_ItemData(item_Equip.part_Type, item_Equip);
    }
    public void Set_Remove_Equip(ePart_Type part_Type)
    {
        for (int i = 0; i < player_Data.lisEquip_ItemData.Count; ++i)
        {
            if (player_Data.lisEquip_ItemData[i].part_Type == part_Type)
            {
                player_Data.lisEquip_ItemData[i] = GameManager.Instance.lisItemData.Find(_ => _.part_Type == part_Type);
                break;
            }
        }

        GameManager.Instance.Save_DB();
    }

    public SB_Item_Data Get_Equip_ItemData(ePart_Type part_Type)
    {
        SB_Item_Data _item_Data = player_Data.lisEquip_ItemData.Find(_ => _.part_Type == part_Type);
        if (_item_Data == null)
        {
            return GameManager.Instance.lisItemData.Find(_ => _.part_Type == part_Type);
        }
        return _item_Data;
    }
    #endregion

    #region Skill
    public void Set_EquipSkill(int nSlot_Num, SB_Skill_Data skill_Data)
    {
        if (skill_Data == null)
        {
            skill_Data = new SB_Skill_Data();
            skill_Data.skillData = TableManager.Instance.skillTable.Get_SkillData(0);
            skill_Data.nLevel = 0;
        }
        player_Data.lisEquip_SkillData[nSlot_Num] = skill_Data;
        GameManager.Instance.Save_DB();
    }
    public void Set_SkillData(SB_Skill_Data skill_Data)
    {
        if (skill_Data == null)
            return;

        bool isGet = false;
        for (int i = 0; i < lisSkill_Data.Count; ++i)
        {
            if (lisSkill_Data[i].skillData.nIndex == skill_Data.skillData.nIndex)
            {
                lisSkill_Data[i] = skill_Data;
                isGet = true;
                break;
            }

        }
        if (!isGet)
            lisSkill_Data.Add(skill_Data);
        GameManager.Instance.Save_DB();
    }
    public SB_Skill_Data Get_Equip_SkillData(int nIndex)
    {
        return player_Data.lisEquip_SkillData.Find(_ => _.skillData.nIndex == nIndex);
    }
    public List<SB_Skill_Data> Get_Equip_ListSkillData()
    {
        return player_Data.lisEquip_SkillData;
    }
    public SB_Skill_Data Get_SkillData(int nIndex)
    {
        return lisSkill_Data.Find(_ => _.skillData.nIndex == nIndex);
    }

    public void Set_StateData(SB_Skill_State_Data state_Data)
    {
        if (state_Data == null)
            return;

        bool isGet = false;
        for (int i = 0; i < lisState_Data.Count; ++i)
        {
            if (lisState_Data[i].stateData.nIndex == state_Data.stateData.nIndex)
            {
                lisState_Data[i] = state_Data;
                isGet = true;
                break;
            }

        }
        if (!isGet)
            lisState_Data.Add(state_Data);
        GameManager.Instance.Save_DB();
    }
    public SB_Skill_State_Data Get_Skill_StateData(int nIndex)
    {
        return lisState_Data.Find(_ => _.stateData.nIndex == nIndex);
    }
    public float Get_Skill_State(eState_Type state_Type, float fValue, float fPercent = 0.01f)
    {
        SB_Skill_State_Data sb_Skill_State_Data = lisState_Data.Find(_ => _.stateData.state_Type == state_Type);
        if (sb_Skill_State_Data == null)
            return 0;

        return fValue * (sb_Skill_State_Data.nLevel * sb_Skill_State_Data.stateData.fUpgrade_Value * fPercent);
    }
    #endregion

    #region Shop
    public void Set_Crystal(int nCrystal)
    {
        shop_Data.nCrystal = nCrystal;

        UIManager.Instance.Set_Gold();
        GameManager.Instance.Save_DB();
    }
    public void Set_Gold(int nGold)
    {
        shop_Data.nGold = nGold;

        UIManager.Instance.Set_Gold();
        GameManager.Instance.Save_DB();
    }
    public void Set_PlusGold(int nGold)
    {
        shop_Data.nGold += nGold;

        UIManager.Instance.Set_Gold();
        GameManager.Instance.Save_DB();
    }
    public void Set_PlusCrystal(int nGold)
    {
        shop_Data.nGold += nGold;

        UIManager.Instance.Set_Gold();
        GameManager.Instance.Save_DB();
    }
    public void Set_ShopExp(int nExp)
    {
        if (shop_Data.nLevel >= 300)
            return;

        shop_Data.nExp += nExp;

        while (true)
        {
            int _nMax = TableManager.Instance.shopExpTable.Get_ShopExpData(shop_Data.nLevel).nLevel_Max;
            if (shop_Data.nExp >= _nMax)
            {
                shop_Data.nExp -= _nMax;
                ++shop_Data.nLevel;
            }

            if (shop_Data.nExp < _nMax)
                break;
        }

        GameManager.Instance.Save_DB();
    }
    public int Get_ShopExp()
    {
        return shop_Data.nExp;
    }
    public int Get_ShopLevel()
    {
        return shop_Data.nLevel;
    }
    public int Get_Gold()
    {
        return shop_Data.nGold;
    }
    public int Get_Crystal()
    {
        return shop_Data.nCrystal;
    }
    #endregion
}
[Serializable]
public class SB_Player_Data
{
    public int nHp;
    public int nLevel;
    public float fExp;

    public int nCurrentStage;
    public int nClearStage;
    public List<SB_Item_Data> lisEquip_ItemData;
    public List<SB_Skill_Data> lisEquip_SkillData;
    public void Init()
    {
        nLevel = 1;
        fExp = 0;
        nCurrentStage = 1;
        nClearStage = 1;
        nHp = TableManager.Instance.expTable.Get_Hp(1);
        lisEquip_SkillData = new List<SB_Skill_Data>();
        lisEquip_ItemData = new List<SB_Item_Data>();

        for (int i = 0; i < (int)ePart_Type.Max; ++i)
        {
            SB_Item_Data _item_Data = new SB_Item_Data();
            _item_Data.nIndex = 0;
            _item_Data.nStar = 0;
            _item_Data.part_Type = (ePart_Type)i;
            _item_Data.lisState_Data = new List<SB_Equip_Item_State_Data>();
            lisEquip_ItemData.Add(_item_Data);
        }
        int _nMax = 5;
        for (int i = 0; i < _nMax; ++i)
        {
            SB_Skill_Data skill_Data = new SB_Skill_Data();
            skill_Data.skillData = new SkillData();
            skill_Data.skillData = TableManager.Instance.skillTable.Get_SkillData(0);
            skill_Data.nLevel = 0;
            lisEquip_SkillData.Add(skill_Data);
        }
    }
}
[Serializable]
public class SB_Item_Data
{
    public int nIndex;
    public int nStar;
    public ePart_Type part_Type;
    public List<SB_Equip_Item_State_Data> lisState_Data;

    public int nLevel = 1;
    public int nExp;
}
[Serializable]
public class SB_Skill_Data
{
    public SkillData skillData;
    public int nLevel;
}

[Serializable]
public class SB_Skill_State_Data
{
    public StateData stateData;
    public int nLevel;
}
[Serializable]
public class SB_Equip_Item_State_Data
{
    public int nIndex;
    public float fValue;
}

[Serializable]
public class SB_Shop_Data
{
    public int nGold;
    public int nCrystal;

    public int nLevel;
    public int nExp;
    public void Init()
    {
        nGold = 100000;
        nCrystal = 1000000;

        nLevel = 280;
        nExp = 0;
    }

}
