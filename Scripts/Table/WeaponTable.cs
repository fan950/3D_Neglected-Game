using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponData
{
    public int nIndex;
    public string sName;

    public int nGrade;
    public int nAttack;
    public float fAttack_Speed;
    public eAttack_Type eAttack_Type;
    public eAni_Type eAni_Type;

    public float fAttack_Distance;

    public string sArrEffect;
    public int nEffect_Count;

    public string sPath;

    public int Get_Attack(SB_Item_Data _item_Data)
    {
        int _nLevel = _item_Data.nLevel - 1;
        if (_nLevel < 0)
            _nLevel = 0;

        float _fStar = (_item_Data.nStar * 0.1f) + 1.0f;

        return (int)((nAttack + (nAttack * (_nLevel * 0.1f))) * _fStar);
    }
}

public class WeaponTable
{
    private List<WeaponData> lisWeaponData = new List<WeaponData>();
    private Dictionary<int, List<int>> dicLevel_Item = new Dictionary<int, List<int>>();

    public void Add_WeaponDate(WeaponData weaponData)
    {
        lisWeaponData.Add(weaponData);

        if (!dicLevel_Item.ContainsKey(weaponData.nGrade))
        {
            dicLevel_Item.Add(weaponData.nGrade, new List<int>());
        }
        dicLevel_Item[weaponData.nGrade].Add(weaponData.nIndex);
    }
    public WeaponData Get_WeaponDate(int nIndex)
    {
        return lisWeaponData.Find(_ => _.nIndex == nIndex);
    }
    public List<int> Get_ListEffect_Index(int nIndex)
    {
        List<int> _lisEffect_Index = new List<int>();

        WeaponData _weaponData = lisWeaponData.Find(_ => _.nIndex == nIndex);
        if (_weaponData != null)
        {
            for (int i = 0; i < _weaponData.sArrEffect.Length; ++i)
            {
                string _cTemp = _weaponData.sArrEffect[i].ToString();
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
    public string Get_Path(int nIndex)
    {
        WeaponData _weaponData = lisWeaponData.Find(_ => _.nIndex == nIndex);
        if (_weaponData == null)
            return null;

        return _weaponData.sPath;
    }
    public string Get_Path_Obj(int nIndex)
    {
        WeaponData _weaponData = lisWeaponData.Find(_ => _.nIndex == nIndex);
        if (_weaponData == null)
            return null;

        return "Weapon/" + _weaponData.sPath;
    }

    public int Get_RankRandom_Index(int nIndex)
    {
        int _nItem_Index = Random.Range(0, dicLevel_Item[nIndex].Count);

        return dicLevel_Item[nIndex][_nItem_Index];
    }

    public float Get_AniType(int nIndex)
    {
        float _fAni = 0;

        switch (lisWeaponData[nIndex].eAni_Type)
        {
            case eAni_Type.Melee:
                switch (Random.Range(0, 3))
                {
                    case 0:
                        _fAni = 0;
                        break;
                    case 1:
                        _fAni = 0.166666f;
                        break;
                    case 2:
                        _fAni = 0.333333f;
                        break;
                }
                break;
            case eAni_Type.Sword:
                switch (Random.Range(0, 2))
                {
                    case 0:
                        _fAni = 0.5f;
                        break;
                    case 1:
                        _fAni = 0.666666f;
                        break;
                }
                break;
            case eAni_Type.Spear:
                _fAni = 1f;
                break;
        }
        return _fAni;
    }
}