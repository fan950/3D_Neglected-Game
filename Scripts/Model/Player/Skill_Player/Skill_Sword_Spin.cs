using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Spin
{
    [HideInInspector] public int nIndex;

    [HideInInspector] public string sAni_Name = "Skill_Sword_Spin";
    private const float fSpine_Max = 5;
    private float fSpine_Time;
    private int nDamage;

    public SB_Skill_Data skill_Data;

    public void Init(int nIndex)
    {
        this.nIndex = nIndex;
        skill_Data = GameManager.Instance.localGame_DB.Get_SkillData(nIndex);

        nDamage = (int)(ModelManager.Instance.player.nAttack + ModelManager.Instance.player.nAttack * ((skill_Data.skillData.fValue * (skill_Data.nLevel * skill_Data.skillData.fUpgrade_Value)) * 0.0001f));
        fSpine_Time = 0;
    }

    public void Start_Skill()
    {
        ModelManager.Instance.player.weapon_Player.Switch_Collider(true);
    }

    public void Update_Skil()
    {
        fSpine_Time += Time.deltaTime;
        if (fSpine_Time >= fSpine_Max)
        {
            Die_Skil();
            fSpine_Time = 0;
            return;
        }
    }

    public void Die_Skil()
    {
        Player _Player = ModelManager.Instance.player;
        _Player.fAttack_Time = 0;
        _Player.bAttack = false;

        if (_Player.nMonster_Hp <= 0)
        {
            _Player.Play_AniTrigger("Fast_Idle");
            _Player.Set_Target(null);
        }
        _Player.weapon_Player.Switch_Collider(false);
        _Player.Set_FSM(eFsm_State.Idle);
    }
}
