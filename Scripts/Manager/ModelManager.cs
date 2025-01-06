using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ModelManager : Singleton<ModelManager>
{
    private Dictionary<Model, bool> dicStop_Move = new Dictionary<Model, bool>();

    [HideInInspector] public Player player;
    private Dictionary<SB_Item_Data, Weapon_Player> dicWeapon_Player = new Dictionary<SB_Item_Data, Weapon_Player>();
    private Dictionary<SB_Item_Data, GameObject> dicDefend_Player = new Dictionary<SB_Item_Data, GameObject>();

    List<int> lisPercent = new List<int>();
    private List<int> lisMonster_Index;
    private Dictionary<int, ObjcetPool<Monster>> dicMonster = new Dictionary<int, ObjcetPool<Monster>>();
    private Dictionary<GameObject, Monster> dicLive_Monster = new Dictionary<GameObject, Monster>();
    private List<Monster> lisDie_Monster = new List<Monster>();
    private float fCreate_Time;
    private float fRespawn_Time;
    [HideInInspector] public bool isBoss;

    private Dictionary<eShop_Value, ObjcetPool<Gold_Obj>> dicGold = new Dictionary<eShop_Value, ObjcetPool<Gold_Obj>>();
    private List<Gold_Obj> lisLive_Gold_Obj = new List<Gold_Obj>();
    private List<Gold_Obj> lisDie_Gold_Obj = new List<Gold_Obj>();

    private Dictionary<int, ObjcetPool<Skill_Player>> dicSkill_Player = new Dictionary<int, ObjcetPool<Skill_Player>>();
    private Dictionary<string, Skill_Monster> dicSkill_Monster = new Dictionary<string, Skill_Monster>();
    private List<Skill_Player> lisLive_Skill_Player = new List<Skill_Player>();
    private List<Skill_Player> lisDie_Skill_Player = new List<Skill_Player>();

    private Vector3[] arrSpawnPos;
    private GameObject target_Obj;
    private float fTarget_Distance;

    private const string sMonster_Path = "Monster/";
    private const string sSkill_Path = "Skill/";
    private const string sLevel_Key = "LevelUp_FX";
    private const string sLevel_Path = "FX/LevelUp_FX";
    private const string sHit_Key = "Hit_FX";
    private const string sHit_Path = "FX/Hit_FX";

    private const string sGold_Path = "Gold";
    private const string sCrystal_Path = "Crystal";

    private int nPercent_Max = 100000;
    protected override void Init()
    {
        GameObject _play_Obj = GameObject.FindGameObjectWithTag("Player");

        if (_play_Obj == null)
        {
            _play_Obj = Instantiate(Resources.Load("Player") as GameObject);
            _play_Obj.transform.SetParent(transform);
            _play_Obj.transform.localRotation = Quaternion.identity;
            _play_Obj.transform.transform.localScale = Vector3.one;
        }

        player = _play_Obj.GetComponent<Player>();
        player.Active_Nav(false);
        player.transform.localPosition = GameObject.FindGameObjectWithTag("Player_Pos").transform.position;
        player.Init();

        for (int i = 0; i < (int)ePart_Type.Max; ++i)
        {
            Set_Player_Equipment((ePart_Type)i, GameManager.Instance.localGame_DB.Get_Equip_ItemData((ePart_Type)i));
        }
        Init_Gold_Pool();
        isBoss = false;

        UIManager.Instance.Set_Hp();
        UIManager.Instance.Set_Exp();
        UIManager.Instance.Set_Level();
    }

    public void Init_Monster_Pool(Transform[] arrPos)
    {
        arrSpawnPos = new Vector3[arrPos.Length];
        for (int i = 0; i < arrSpawnPos.Length; ++i)
        {
            arrSpawnPos[i] = arrPos[i].transform.position;
        }
        lisPercent = TableManager.Instance.stageTable.Get_ListPercent(GameManager.Instance.localGame_DB.player_Data.nCurrentStage, nPercent_Max);
        fTarget_Distance = 9999;
        int _nIndex = GameManager.Instance.localGame_DB.Get_Stage();
        lisMonster_Index = TableManager.Instance.stageTable.Get_ListMonster(_nIndex);

        int _nStart_Count = 100;
        for (int i = 0; i < lisMonster_Index.Count; ++i)
        {
            if (!dicMonster.ContainsKey(lisMonster_Index[i]))
            {
                MonsterData monsterTable = TableManager.Instance.monsterTable.Get_MonsterDate(lisMonster_Index[i]);
                ObjcetPool<Monster> _obj = new ObjcetPool<Monster>();
                _obj.Init(sMonster_Path + monsterTable.sPath, _nStart_Count * 3, transform);
                dicMonster.Add(lisMonster_Index[i], _obj);
            }
        }
        StageData _stageData = TableManager.Instance.stageTable.Get_StageData(_nIndex);
        fCreate_Time = _stageData.fCoolTime;
        fRespawn_Time = _stageData.fCoolTime;
        for (int i = 0; i < _nStart_Count; ++i)
        {
            Create_Monster();
        }
    }
    public void Init_Gold_Pool()
    {
        int _nStart_Count = 100;
        ObjcetPool<Gold_Obj> _gold = new ObjcetPool<Gold_Obj>();
        _gold.Init(sGold_Path, _nStart_Count, transform);
        dicGold.Add(eShop_Value.Gold, _gold);

        ObjcetPool<Gold_Obj> _crystal = new ObjcetPool<Gold_Obj>();
        _crystal.Init(sCrystal_Path, _nStart_Count, transform);
        dicGold.Add(eShop_Value.Crystal, _crystal);
    }

    public Gold_Obj Get_Gold(eShop_Value shop_Value)
    {
        return dicGold[shop_Value].Get();
    }
    public void Return_Gold(eShop_Value shop_Value, Gold_Obj obj)
    {
        dicGold[shop_Value].Return(obj);
    }
    public void AllNav_Stop(bool bStop)
    {
        if (bStop)
        {
            if (dicStop_Move.Count != 0)
                return;

            if (player != null)
            {
                player.Nav_Stop(true);
                dicStop_Move.Add(player, player.bMove_Stop);
            }
            foreach (KeyValuePair<GameObject, Monster> mob in dicLive_Monster)
            {
                mob.Value.Nav_Stop(true);
                dicStop_Move.Add(mob.Value, mob.Value.bMove_Stop);
            }
        }
        else
        {
            foreach (KeyValuePair<Model, bool> model in dicStop_Move)
            {
                model.Key.Nav_Stop(model.Value);
            }
            dicStop_Move.Clear();
        }
    }
    public void Model_Next_Scene()
    {
        player.Set_FSM(eFsm_State.Idle);

        foreach (KeyValuePair<int, ObjcetPool<Monster>> mob in dicMonster)
        {
            mob.Value.Clear();
        }
        dicLive_Monster.Clear();
        lisDie_Monster.Clear();

        foreach (KeyValuePair<int, ObjcetPool<Skill_Player>> skill in dicSkill_Player)
        {
            skill.Value.Return_All();
        }
        lisLive_Skill_Player.Clear();
        lisDie_Skill_Player.Clear();

        dicStop_Move.Clear();

        for (int i = 0; i < lisLive_Gold_Obj.Count; ++i)
        {
            dicGold[lisLive_Gold_Obj[i].shop_Value].Return(lisLive_Gold_Obj[i]);
        }
        for (int i = 0; i < lisDie_Gold_Obj.Count; ++i)
        {
            dicGold[lisDie_Gold_Obj[i].shop_Value].Return(lisDie_Gold_Obj[i]);
        }

        lisLive_Gold_Obj.Clear();
        lisDie_Gold_Obj.Clear();
    }
    public void Mgr_Update()
    {
        if (player != null)
        {
            player.Update_Model();
            player.Set_Target(target_Obj);
        }

        if (lisMonster_Index.Count >= 1)
            fCreate_Time += Time.deltaTime;

        if (fCreate_Time >= fRespawn_Time)
        {
            if (lisMonster_Index.Count <= 0)
            {
                fCreate_Time = 0;
                return;
            }
            Create_Monster();
            fCreate_Time = 0;
        }

        if (dicLive_Monster.Count > 0)
        {
            foreach (KeyValuePair<GameObject, Monster> monster in dicLive_Monster)
            {
                if (monster.Value.nHp > 0)
                {
                    if (player.nHp <= 0)
                    {
                        monster.Value.Set_FSM(eFsm_State.Idle);
                        return;
                    }
                    monster.Value.Update_Model();
                    Player_SearchTarget(monster.Key);
                }
                else
                {
                    lisDie_Monster.Add(monster.Value);
                }
            }
        }

        if (lisDie_Monster.Count > 0)
        {
            for (int i = 0; i < lisDie_Monster.Count; ++i)
            {
                Die_Monster(lisDie_Monster[i].gameObject);
            }
            lisDie_Monster.Clear();
        }

        for (int i = 0; i < lisLive_Skill_Player.Count; ++i)
        {
            lisLive_Skill_Player[i].Update_Skil();
        }

        if (lisDie_Skill_Player.Count > 0)
        {
            for (int i = 0; i < lisDie_Skill_Player.Count; ++i)
            {
                lisLive_Skill_Player.Remove(lisDie_Skill_Player[i]);
                dicSkill_Player[lisDie_Skill_Player[i].nIndex].Return(lisDie_Skill_Player[i]);
            }
            lisDie_Skill_Player.Clear();
        }

        if (lisLive_Gold_Obj.Count > 0)
        {
            for (int i = 0; i < lisLive_Gold_Obj.Count; ++i)
            {
                lisLive_Gold_Obj[i].Update_Gold();
            }
        }

        if (lisDie_Gold_Obj.Count > 0)
        {
            for (int i = 0; i < lisDie_Gold_Obj.Count; ++i)
            {
                lisLive_Gold_Obj.Remove(lisDie_Gold_Obj[i]);
                dicGold[lisDie_Gold_Obj[i].shop_Value].Return(lisDie_Gold_Obj[i]);
            }
            lisDie_Gold_Obj.Clear();
        }
    }
    #region Player
    public void Player_Skill(SkillData skillData)
    {
        if (dicLive_Monster.Count == 0)
            return;

        if (skillData.nAni == 1)
        {
            player.skillData = skillData;
            player.Set_FSM(eFsm_State.Skill);
        }
        else
        {
            if (!dicSkill_Player.ContainsKey(skillData.nIndex))
            {
                dicSkill_Player.Add(skillData.nIndex, new ObjcetPool<Skill_Player>());
                dicSkill_Player[skillData.nIndex].Init(sSkill_Path + skillData.sPath, 5, transform);
            }

            for (int i = 0; i < skillData.nCount; ++i)
            {
                Skill_Player _skill_Player = dicSkill_Player[skillData.nIndex].Get();
                _skill_Player.Init(skillData.nIndex);
                lisLive_Skill_Player.Add(_skill_Player);
            }
        }
    }
    public void Die_Skill(Skill_Player skill_Player)
    {
        if (!lisDie_Skill_Player.Contains(skill_Player))
            lisDie_Skill_Player.Add(skill_Player);
    }

    public void Player_SearchTarget(GameObject obj)
    {
        if (isBoss)
            return;

        if (player.Get_TargetObj != null && (dicLive_Monster.ContainsKey(player.Get_TargetObj) && dicLive_Monster[player.Get_TargetObj].nHp > 0))
            return;

        float _fDistance = Vector3.Distance(player.transform.position, obj.transform.position);

        if (fTarget_Distance > _fDistance)
        {
            target_Obj = obj;
            fTarget_Distance = _fDistance;
        }
    }
    public Monster Play_Calculate_Damage(GameObject obj, int nDamage)
    {
        if (dicLive_Monster.ContainsKey(obj))
        {
            bool _bCri = Critical_Attack();
            if (_bCri)
                nDamage = nDamage + (int)(nDamage * (player.fCritical_Damage * 0.01f));

            if (nDamage == 0)
                nDamage = 1;

            dicLive_Monster[obj].nHp -= nDamage;
            Player_Healing((int)(nDamage * player.fAbsord_blood));

            GameObject _hitObj = FXManager.Instance.Get_Fx(sHit_Key, sHit_Path).gameObject;
            _hitObj.transform.position = dicLive_Monster[obj].offset_HitPos.position;
            if (dicLive_Monster[obj].nHp <= 0)
            {
                float _fExp = TableManager.Instance.monsterTable.Get_Exp(dicLive_Monster[obj].nIndex);
                Exp(_fExp);
                dicLive_Monster[obj].nHp = 0;
            }
            UIManager.Instance.Set_HpBar(dicLive_Monster[obj]);
            UIManager.Instance.Set_Damage(dicLive_Monster[obj].offset_HeadPos.gameObject, nDamage, _bCri);
            return dicLive_Monster[obj];
        }
        return null;
    }
    public void Exp(float _fExp)
    {
        player.fExp += _fExp + (_fExp * player.fExp_Increment);

        while (true)
        {
            int _nLevel_Max = TableManager.Instance.expTable.Get_ExpMax(player.nLevel);

            if (player.fExp >= _nLevel_Max)
            {
                player.fExp -= _nLevel_Max;
                player.Level_Up();
                UIManager.Instance.Set_Level();
                FXManager.Instance.Get_Fx(sLevel_Key, sLevel_Path);
                UIManager.Instance.Get_LevelUp();
                UIManager.Instance.Set_Hp();
            }
            else
                break;
        }

        UIManager.Instance.Set_Exp();
    }
    public void Player_Healing(int nHealing)
    {
        player.nHp += nHealing;
        UIManager.Instance.Set_Hp();
    }
    public bool Critical_Attack()
    {
        float _fRamdom = Random.Range(0f, 100f);
        if (_fRamdom <= player.fCritical_Percent)
        {
            return true;
        }
        return false;
    }
    public void Play_Die()
    {
        if (player.current_State == eFsm_State.Die)
            return;

        player.Set_FSM(eFsm_State.Die);
        Invoke("Die_Ani", 3.0f);
    }
    public void Die_Ani()
    {
        UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
        _uiOk_Popup.OnShow(TableManager.Instance.stringTable.Get_String("PlayerDie"), delegate
        {
            GameManager.Instance.Next_Scene(GameManager.Instance.localGame_DB.Get_Stage(),
                delegate
                {
                    player.nHp = player.nHp_Max;
                    UIManager.Instance.Set_Hp();
                    player.Set_FSM(eFsm_State.Idle);
                });
        });
    }
    #endregion

    #region Monster
    public Skill_Monster Monster_Skill(string sKey)
    {
        if (!dicSkill_Monster.ContainsKey(sKey))
        {
            GameObject _obj = Instantiate(Resources.Load(sSkill_Path + "Monster/" + sKey) as GameObject);
            dicSkill_Monster.Add(sKey, _obj.GetComponent<Skill_Monster>());

            _obj.transform.SetParent(transform);
            _obj.transform.localPosition = Vector3.zero;
            _obj.transform.localScale = Vector3.one;
            _obj.transform.localRotation = Quaternion.identity;
        }

        return dicSkill_Monster[sKey];
    }
    public void Create_Monster()
    {
        int _nIndex = Get_Percent_Monster(lisMonster_Index.Count);
        var _mob = dicMonster[_nIndex].Get();

        _mob.Active_Nav(false);
        _mob.transform.rotation = Quaternion.identity;
        _mob.transform.position = Get_Monster_Pos();
        _mob.Set_Target(player.gameObject);
        _mob.Init(_nIndex);

        dicLive_Monster.Add(_mob.gameObject, _mob);
    }
    public void Create_BossMonster()
    {
        InGameScene _inGameScene = GameManager.Instance.inGameScene;
        StageData _stageData = TableManager.Instance.stageTable.Get_StageData(GameManager.Instance.localGame_DB.Get_Stage());
        MonsterData _monsterData = TableManager.Instance.monsterTable.Get_MonsterDate(_stageData.nBossIndex);
        if (!dicMonster.ContainsKey(_stageData.nBossIndex))
        {
            MonsterData monsterTable = TableManager.Instance.monsterTable.Get_MonsterDate(_stageData.nBossIndex);
            ObjcetPool<Monster> _obj = new ObjcetPool<Monster>();
            _obj.Init(sMonster_Path + monsterTable.sPath, 3, transform);
            dicMonster.Add(_stageData.nBossIndex, _obj);
        }
        var _mob = dicMonster[_stageData.nBossIndex].Get(false);
        _mob.Active_Nav(false);

        _inGameScene.Set_BossCoro(_mob, true, delegate
         {
             _mob.Set_Target(player.gameObject);
             _mob.Init(_stageData.nBossIndex);
             dicLive_Monster.Add(_mob.gameObject, _mob);
             UIManager.Instance.Set_BossHp(_mob);
         });

        isBoss = true;
        target_Obj = _mob.gameObject;
    }

    public Vector3 Get_Monster_Pos()
    {
        Vector3 _vecPos;
        int _nRandom = Random.Range(0, arrSpawnPos.Length);

        Vector3 randomPoint = arrSpawnPos[_nRandom] + Random.insideUnitSphere * 7f;
        randomPoint.y = 0;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            _vecPos = hit.position;
            return _vecPos;
        }

        return arrSpawnPos[_nRandom];
    }
    public void Monster_Calculate_Damage(Monster monster)
    {
        int _nDamage = player.nDefend - monster.nAttack;

        if (_nDamage > 0)
            _nDamage = 0;


        if (dicLive_Monster.ContainsKey(monster.gameObject) && player.nAttack_Rank > dicLive_Monster[monster.gameObject].nWeapon_Rank)
        {
            if (!isBoss)
                target_Obj = monster.gameObject;
        }

        player.nHp += _nDamage;

        if (player.nHp <= 0)
        {
            Play_Die();
        }

        UIManager.Instance.Set_Hp();
    }

    public void Die_Monster(GameObject obj)
    {
        target_Obj = null;

        Monster _mob = dicLive_Monster[obj];
        _mob.Set_FSM(eFsm_State.Die);

        dicLive_Monster.Remove(obj);

        int _nRandom = Random.Range(1, 1000);
        eShop_Value shop_Value = eShop_Value.Gold;
        if (_nRandom <= 50)
            shop_Value = eShop_Value.Crystal;

        Gold_Obj gold_Obj = Get_Gold(shop_Value);
        lisLive_Gold_Obj.Add(gold_Obj);

        gold_Obj.Init();
        gold_Obj.transform.position = obj.transform.position;

        fTarget_Distance = 99999;
    }
    public void Remove_Monster(Monster mob)
    {
        if (!dicMonster.ContainsKey(mob.nIndex))
            return;
        dicMonster[mob.nIndex].Return(mob);
    }
    public Monster Random_Monster()
    {
        if (dicLive_Monster.Count == 0)
            return null;

        List<GameObject> _lisObj = new List<GameObject>(dicLive_Monster.Keys);
        int _nRandom = Random.Range(0, _lisObj.Count);
        return dicLive_Monster[_lisObj[_nRandom]];
    }

    public Monster Find_Monster(GameObject obj)
    {
        if (obj == null)
            return null;

        if (dicLive_Monster.ContainsKey(obj))
            return dicLive_Monster[obj];
        else
            return null;
    }
    public int Get_Percent_Monster(int nCount)
    {
        int _nRandom = Random.Range(1, nPercent_Max);
        switch (nCount)
        {
            case 2:
                if (_nRandom <= lisPercent[0])
                    return lisMonster_Index[0];
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    return lisMonster_Index[1];
                break;
            case 3:
                if (_nRandom <= lisPercent[0])
                    return lisMonster_Index[0];
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    return lisMonster_Index[1];
                else if (_nRandom > lisPercent[0] + lisPercent[1] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2])
                    return lisMonster_Index[2];
                break;
            case 4:
                if (_nRandom <= lisPercent[0])
                    return lisMonster_Index[0];
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    return lisMonster_Index[1];
                else if (_nRandom > lisPercent[0] + lisPercent[1] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2])
                    return lisMonster_Index[2];
                else if (_nRandom > lisPercent[0] + lisPercent[1] + lisPercent[2] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3])
                    return lisMonster_Index[3];
                break;
            case 5:
                if (_nRandom <= lisPercent[0])
                    return lisMonster_Index[0];
                else if (_nRandom > lisPercent[0] && _nRandom <= lisPercent[0] + lisPercent[1])
                    return lisMonster_Index[1];
                else if (_nRandom > lisPercent[0] + lisPercent[1] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2])
                    return lisMonster_Index[2];
                else if (_nRandom > lisPercent[0] + lisPercent[1] + lisPercent[2] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3])
                    return lisMonster_Index[3];
                else if (_nRandom > lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3] && _nRandom <= lisPercent[0] + lisPercent[1] + lisPercent[2] + lisPercent[3] + lisPercent[4])
                    return lisMonster_Index[4];
                break;
        }
        return lisMonster_Index[0];
    }
    #endregion

    #region Equipment
    public void Set_Player_Equipment(ePart_Type part_Type, SB_Item_Data item_Data)
    {
        GameObject obj = null;
        SB_Item_Data _equip = GameManager.Instance.localGame_DB.Get_Equip_ItemData(part_Type);
        if (part_Type == ePart_Type.Weapon)
        {
            if (dicWeapon_Player.ContainsKey(_equip))
                dicWeapon_Player[_equip].gameObject.SetActive(false);

            WeaponData _weaponData = TableManager.Instance.weaponTable.Get_WeaponDate(item_Data.nIndex);
            string _sWeaponPath = TableManager.Instance.weaponTable.Get_Path_Obj(item_Data.nIndex);
            if (_weaponData != null)
            {
                if (dicWeapon_Player.ContainsKey(item_Data))
                {
                    dicWeapon_Player[item_Data].gameObject.SetActive(true);
                }
                else
                {
                    obj = Instantiate(Resources.Load<GameObject>(_sWeaponPath));
                    obj.transform.SetParent(player.weapon_Pos);
                    obj.transform.localRotation = Quaternion.identity;
                    obj.transform.localPosition = Vector3.zero;

                    Weapon_Player _weapon_Player = obj.GetComponent<Weapon_Player>();
                    _weapon_Player.Init(_weaponData, item_Data);

                    dicWeapon_Player.Add(item_Data, _weapon_Player);
                    dicWeapon_Player[item_Data].Init(_weaponData, item_Data);
                }
                player.Set_Weapon(dicWeapon_Player[item_Data]);
            }
        }
        else
        {
            if (dicDefend_Player.ContainsKey(_equip))
                dicDefend_Player[_equip].gameObject.SetActive(false);

            DefendData defendData = TableManager.Instance.defendTable.Get_DefendData(item_Data.nIndex);
            string _sDefendPath = TableManager.Instance.defendTable.Get_Path_Obj(item_Data.nIndex);

            if (defendData != null)
            {
                if (dicDefend_Player.ContainsKey(item_Data))
                {
                    dicDefend_Player[item_Data].SetActive(true);
                }
                else
                {
                    if (item_Data.nIndex != 0)
                    {
                        GameObject _path = Resources.Load<GameObject>(_sDefendPath);
                        if (_path != null)
                        {
                            obj = Instantiate(Resources.Load<GameObject>(_sDefendPath));
                            switch (item_Data.part_Type)
                            {
                                case ePart_Type.Head_1:
                                case ePart_Type.Head_2:
                                case ePart_Type.Mask:
                                    obj.transform.SetParent(player.head_Pos);
                                    break;
                                case ePart_Type.Back:
                                case ePart_Type.Body:
                                    obj.transform.SetParent(player.body_Pos);
                                    break;
                            }
                            obj.transform.localRotation = Quaternion.identity;
                            obj.transform.localPosition = Vector3.zero;

                            dicDefend_Player.Add(item_Data, obj);
                        }
                    }
                }
            }
        }
        player.Apply_State(part_Type, item_Data);
    }
    #endregion


}
