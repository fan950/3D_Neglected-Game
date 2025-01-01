using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory_Player : MonoBehaviour
{
    private Animator animator;
    private Dictionary<ePart_Type, GameObject> dicPart_Obj = new Dictionary<ePart_Type, GameObject>();
    private Dictionary<string, GameObject> dicMatch_Obj = new Dictionary<string, GameObject>();
    [Header("Part")]
    public Transform weapon_Pos;
    public Transform head_Pos;
    public Transform body_Pos;

    private const int nLayer_Index = 5;

    public void Init()
    {
        animator = GetComponentInChildren<Animator>();
    }
    public void Open()
    {
        Player _player = ModelManager.Instance.player;
        List<SB_Item_Data> _lisData = GameManager.Instance.localGame_DB.player_Data.lisEquip_ItemData;
        for (int i = 0; i < _lisData.Count; ++i)
        {
            Change_Part(_lisData[i].part_Type);
        }
    }
    public void Change_Part(ePart_Type part_Type)
    {
        if (dicPart_Obj.ContainsKey(part_Type))
            dicPart_Obj[part_Type].SetActive(false);

        GameObject _obj = null;
        SB_Item_Data _item_Data = GameManager.Instance.localGame_DB.Get_Equip_ItemData(part_Type);
        string _sPath = string.Empty;

        switch (part_Type)
        {
            case ePart_Type.Head_1:
            case ePart_Type.Head_2:
            case ePart_Type.Mask:
                if (_item_Data.nIndex == 0)
                {
                    if (dicPart_Obj.ContainsKey(part_Type))
                        dicPart_Obj[part_Type].SetActive(false);
                    return;
                }
                _sPath = TableManager.Instance.defendTable.Get_Path_Obj(_item_Data.nIndex);
                _obj = Create_Obj(_sPath, head_Pos);
                break;
            case ePart_Type.Weapon:
                _sPath = TableManager.Instance.weaponTable.Get_Path_Obj(_item_Data.nIndex);
                _obj = Create_Obj(_sPath, weapon_Pos);
                switch (TableManager.Instance.weaponTable.Get_WeaponDate(ModelManager.Instance.player.weapon_Player.nIndex).eAttack_Type)
                {
                    case eAttack_Type.TH_Sword:
                    case eAttack_Type.TH_Axe:
                        animator.SetFloat("Idle_Type", 0);
                        break;
                    case eAttack_Type.Sword:
                    case eAttack_Type.Mace:
                    case eAttack_Type.Axe:
                    case eAttack_Type.Dagger:
                    case eAttack_Type.Spell:
                    case eAttack_Type.Scythe:
                        animator.SetFloat("Idle_Type", 2);
                        break;
                    case eAttack_Type.Spear:
                        animator.SetFloat("Idle_Type", 1);
                        break;
                }
                break;
            case ePart_Type.Back:
            case ePart_Type.Body:
                if (_item_Data.nIndex == 0)
                {
                    if (dicPart_Obj.ContainsKey(part_Type))
                        dicPart_Obj[part_Type].SetActive(false);
                    return;
                }
                _sPath = TableManager.Instance.defendTable.Get_Path_Obj(_item_Data.nIndex);
                _obj = Create_Obj(_sPath, body_Pos);
                break;
        }

        if (dicPart_Obj.ContainsKey(part_Type))
        {
            dicPart_Obj[part_Type].SetActive(false);
            dicPart_Obj[part_Type] = _obj;
        }
        else
            dicPart_Obj.Add(part_Type, _obj);
        dicPart_Obj[part_Type].SetActive(true);
    }
    private GameObject Create_Obj(string sPath, Transform parent)
    {
        GameObject _obj = null;
        if (dicMatch_Obj.ContainsKey(sPath))
        {
            _obj = dicMatch_Obj[sPath];
        }
        else
        {
            _obj = Instantiate(Resources.Load(sPath) as GameObject);
            _obj.transform.SetParent(parent);
            _obj.transform.localRotation = Quaternion.identity;
            _obj.transform.localPosition = Vector3.zero;
            _obj.transform.localScale = Vector3.one;

            dicMatch_Obj.Add(sPath, _obj);
        }
        if (_obj.layer != nLayer_Index)
        {
            _obj.layer = nLayer_Index;
            for (int i = 0; i < _obj.transform.childCount; ++i)
            {
                GameObject _child = _obj.transform.GetChild(i).gameObject;
                _child.layer = nLayer_Index;
                for (int j = 0; j < _child.transform.childCount; ++j)
                {
                    _child.transform.GetChild(j).gameObject.layer = nLayer_Index;
                }
            }
        }
        return _obj;
    }
}
