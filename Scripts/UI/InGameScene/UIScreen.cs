using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScreen : UIWindow
{
    public UIBtn uiInventory_Btn;
    public UIBtn uiShop_Btn;
    public UIBtn uiReinforce_Btn;
    public UIBtn uiSkill_Btn;
    public UIBtn uiAutoSkill_Btn;
    public UIBtn uiAutoJoystick_Btn;
    public UIBtn uiStage_Btn;

    [Header("Tmp")]
    public TextMeshProUGUI level_Tmp;
    public TextMeshProUGUI stage_Tmp;
    public TextMeshProUGUI autoSkill_Tmp;
    public TextMeshProUGUI autoJoystick_Tmp;
    public TextMeshProUGUI bossTime_Tmp;

    public UIJoystick uiJoystick;
    private Image uiAutoSkill_Img;
    private Image uiAutoJoystick_Img;

    [Header("Hp")]
    public Image hp_Value;
    public TextMeshProUGUI hp_Tmp;
    public TextMeshProUGUI bssHp_Tmp;
    [Header("Exp")]
    public Image exp_Value;
    public TextMeshProUGUI exp_Tmp;
    [Header("Gold")]
    public TextMeshProUGUI gold_Tmp;
    public TextMeshProUGUI crystal_Tmp;
    [Header("Skill")]
    public UISkill_Slot[] arrSkill_Slot;
    [Header("Boss")]
    public GameObject bossTime_Obj;
    public GameObject bossHp_Obj;
    public Image bossHp_Img;

    private bool bSkill_Auto = false;
    private float fBossTime;
    private bool bBoss = false;
    [HideInInspector] public bool bJoystick_Auto = false;
    public override void Init()
    {
        base.Init();
        uiJoystick.Init();

        uiInventory_Btn.Init(UIInventory_Open);
        uiShop_Btn.Init(UIShop_Open);
        uiReinforce_Btn.Init(UIReinforce_Open);
        uiSkill_Btn.Init(UISkill_Open);
        uiStage_Btn.Init(UIStage_Open);
        uiAutoSkill_Btn.Init(AutoSkill);
        uiAutoJoystick_Btn.Init(AutoJoyStick);

        Set_Gold();
        Set_SkilSlot();

        uiAutoSkill_Img = uiAutoSkill_Btn.GetComponent<Image>();
        uiAutoJoystick_Img = uiAutoJoystick_Btn.GetComponent<Image>();

        bSkill_Auto = false;
        bJoystick_Auto = false;

        uiAutoSkill_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UICommon_Atlas, "Button_little2");
        autoSkill_Tmp.text = TableManager.Instance.stringTable.Get_String("Auto off");

        uiAutoJoystick_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UICommon_Atlas, "Button_little2");
        autoJoystick_Tmp.text = TableManager.Instance.stringTable.Get_String("Auto off");

        Set_BossTime();
    }
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public void Set_Stage()
    {
        stage_Tmp.text = string.Format(TableManager.Instance.stringTable.Get_String("Stage"), GameManager.Instance.localGame_DB.Get_Stage().ToString());
    }
    public void Set_BossTime()
    {
        bBoss = false;
        fBossTime = TableManager.Instance.stageTable.Get_BossTime(GameManager.Instance.localGame_DB.Get_Stage());
        bossTime_Tmp.text = string.Format("{0:00} : {1:00}", (int)fBossTime / 60, (int)fBossTime % 60);
        bossTime_Obj.gameObject.SetActive(true);
        bossHp_Obj.gameObject.SetActive(false);
    }
    public void Set_Hp()
    {
        Player _player = ModelManager.Instance.player;
        hp_Value.fillAmount = _player.nHp / (_player.nHp_Max * 1.0f);
        hp_Tmp.text = string.Format("{0} / {1}", _player.nHp, _player.nHp_Max);
    }
    public void Set_Exp()
    {
        Player _player = ModelManager.Instance.player;
        float _fPercent = _player.fExp / (TableManager.Instance.expTable.Get_ExpMax(_player.nLevel) * 1.0f);
        float _float2 = Mathf.Floor(_fPercent * 100f) / 100f;
        exp_Value.fillAmount = _fPercent;
        exp_Tmp.text = string.Format("{0}%", _float2 * 100);
    }
    public void Set_Level()
    {
        level_Tmp.text = string.Format("{0}", ModelManager.Instance.player.nLevel);
    }
    public void Set_Gold()
    {
        gold_Tmp.text = GameManager.Instance.localGame_DB.Get_Gold().ToString();
        crystal_Tmp.text = GameManager.Instance.localGame_DB.Get_Crystal().ToString();
    }

    public void Set_BossHp(float fMax, float fHp)
    {
        bossHp_Img.fillAmount = fHp / fMax;
        bssHp_Tmp.text = string.Format("{0} / {1}", fHp, fMax);
    }
    #region Popup
    public void UIInventory_Open()
    {
        UIManager.Instance.UIWindow_Open(eUIWindow_Type.UIInventory);
    }
    public void UIShop_Open()
    {
        UIManager.Instance.UIWindow_Open(eUIWindow_Type.UIShop);
    }
    public void UIReinforce_Open()
    {
        UIManager.Instance.UIWindow_Open(eUIWindow_Type.UIReinforce);
    }
    public void UISkill_Open()
    {
        UIManager.Instance.UIWindow_Open(eUIWindow_Type.UISkill);
    }
    public void UIStage_Open()
    {
        UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIStage_Popup);
    }
    #endregion

    #region Skill
    public void Set_SkilSlot()
    {
        List<SB_Skill_Data> _lisSkill_Data = GameManager.Instance.localGame_DB.Get_Equip_ListSkillData();
        for (int i = 0; i < _lisSkill_Data.Count; ++i)
        {
            arrSkill_Slot[i].Init(i, _lisSkill_Data[i].skillData.nIndex, Play_Skill);
        }
    }

    public void Play_Skill(UISkill_Slot uiSkill_Slot, SB_Skill_Data skill_Data)
    {
        if (uiSkill_Slot.bCoolTime)
            return;

        if (skill_Data.skillData.nIndex == 0)
            return;

        uiSkill_Slot.Start_CoolTime();
        UIManager.Instance.Start_Skill(skill_Data.skillData);
    }
    #endregion
    public void AutoSkill()
    {
        if (bSkill_Auto)
        {
            uiAutoSkill_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UICommon_Atlas, "Button_little2");
            autoSkill_Tmp.text = TableManager.Instance.stringTable.Get_String("Auto off");

            bSkill_Auto = false;
        }
        else
        {
            uiAutoSkill_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UICommon_Atlas, "Button_little");
            autoSkill_Tmp.text = TableManager.Instance.stringTable.Get_String("Auto on");

            bSkill_Auto = true;
        }
    }
    public void AutoJoyStick()
    {
        if (bJoystick_Auto)
        {
            uiAutoJoystick_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UICommon_Atlas, "Button_little2");
            autoJoystick_Tmp.text = TableManager.Instance.stringTable.Get_String("Auto off");

            bJoystick_Auto = false;
            uiJoystick.gameObject.SetActive(true);
        }
        else
        {
            uiAutoJoystick_Img.sprite = UIManager.Instance.Get_Sprite(eAtlas_Type.UICommon_Atlas, "Button_little");
            autoJoystick_Tmp.text = TableManager.Instance.stringTable.Get_String("Auto on");

            bJoystick_Auto = true;
            uiJoystick.gameObject.SetActive(false);
        }
    }
    public void Screen_Update()
    {
        if (!bBoss)
        {
            fBossTime -= Time.deltaTime;

            if (fBossTime >= 60f)
            {
                bossTime_Tmp.text = string.Format("{0:00} : {1:00}", (int)fBossTime / 60, (int)fBossTime % 60);
            }
            else if (fBossTime > 0 && fBossTime < 60f)
            {
                bossTime_Tmp.text = string.Format("00 : {0:00}", (int)fBossTime % 60);
            }
            else
            {
                UIManager.Instance.Show_BossTmp("CreateBoss");
                bossTime_Obj.gameObject.SetActive(false);
                bossHp_Obj.gameObject.SetActive(true);

                StageData _stageData = TableManager.Instance.stageTable.Get_StageData(GameManager.Instance.localGame_DB.Get_Stage());
                MonsterData _monsterData = TableManager.Instance.monsterTable.Get_MonsterDate(_stageData.nBossIndex);
                Set_BossHp(_monsterData.nHp, _monsterData.nHp);
                bBoss = true;

                ModelManager.Instance.Create_BossMonster();
                GameManager.Instance.Set_Stop(true);
            }
        }

        if (bSkill_Auto)
        {
            for (int i = 0; i < arrSkill_Slot.Length; ++i)
            {
                arrSkill_Slot[i].Call_Skill();
            }
        }

        for (int i = 0; i < arrSkill_Slot.Length; ++i)
        {
            arrSkill_Slot[i].Update_SkillSlot();
        }
    }
}
