using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHUD : UIWindow
{

    [Header("Parent")]
    public Transform hpBox_Pos;
    public Transform tmpBox_Pos;
  
    private ObjcetPool<UIDamage_Tmp> damageTmp_Pool;
    private ObjcetPool<UIHp_Bar> hp_Pool;
    private ObjcetPool<UILevel_Tmp> levelTmp_Pool;
    private Dictionary<Monster, UIHp_Bar> dicHp_Bar = new Dictionary<Monster, UIHp_Bar>();

    private const string sDamageTmp_Path = "UI/InGameScene/UIDamageTmp";
    private const string sLevel_Path = "UI/InGameScene/UILevelUpTmp";
    private const string sShowTmp_Path = "UI/InGameScene/UIShowBossTmp";
    private const string sHp_Path = "UI/InGameScene/UIHpBar";

    private const int nDamageTmp_Max = 30;
    private const int nHp_Max = 50;
    private const int nLevel_Max = 5;

    private UIShowBossTmp uiShowBossTmp;
    private List<UIHp_Bar> lis_DieHp = new List<UIHp_Bar>();
    public override void Init()
    {
        base.Init();

        damageTmp_Pool = new ObjcetPool<UIDamage_Tmp>();
        damageTmp_Pool.Init(sDamageTmp_Path, nDamageTmp_Max, tmpBox_Pos);

        levelTmp_Pool = new ObjcetPool<UILevel_Tmp>();
        levelTmp_Pool.Init(sLevel_Path, nLevel_Max, tmpBox_Pos);

        hp_Pool = new ObjcetPool<UIHp_Bar>();
        hp_Pool.Init(sHp_Path, nHp_Max, hpBox_Pos);
    }

    public void HUD_Next_Scene()
    {
        damageTmp_Pool.Return_All();
        hp_Pool.Return_All();

        lis_DieHp.Clear();
        dicHp_Bar.Clear();
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }
    public void Open_ShowBoss(string tmp)
    {
        if (uiShowBossTmp == null)
        {
            GameObject obj = Resources.Load(sShowTmp_Path) as GameObject;
            var Instan = Instantiate(obj, transform);
            Instan.transform.localPosition = Vector3.zero;
            Instan.transform.localScale = Vector3.one;

            uiShowBossTmp = Instan.GetComponent<UIShowBossTmp>();
            uiShowBossTmp.Init();
        }
        uiShowBossTmp.Open(tmp);
    }
    public void Active_Box(bool bActive)
    {
        hpBox_Pos.gameObject.SetActive(bActive);
        tmpBox_Pos.gameObject.SetActive(bActive);
    }
    public RectTransform Set_DamageTmp(int nDamage, bool bCri)
    {
        UIDamage_Tmp _uiDamage_Tmp = damageTmp_Pool.Get();
        _uiDamage_Tmp.Init(nDamage, bCri, (damage) =>
        {
            damageTmp_Pool.Return(damage);
        });

        return _uiDamage_Tmp.rectTransform;
    }
    public RectTransform Set_LevelTmp()
    {
        UILevel_Tmp _uiLevel_Tmp = levelTmp_Pool.Get();
        _uiLevel_Tmp.Init((level) =>
        {
            levelTmp_Pool.Return(level);
        });

        return _uiLevel_Tmp.rectTransform;
    }

    public void Set_HpBar(Monster mob)
    {
        if (dicHp_Bar.ContainsKey(mob))
        {
            dicHp_Bar[mob].fLive_Time = 0;
            return;
        }

        UIHp_Bar _uiHp_Bar = hp_Pool.Get(false);
        _uiHp_Bar.Init(mob);

        dicHp_Bar.Add(mob, _uiHp_Bar);
    }

    public void HUD_Update()
    {
        List<UIHp_Bar> _lis_HpBar = hp_Pool.Get_ListActive();
        for (int i = 0; i < _lis_HpBar.Count; ++i)
        {
            _lis_HpBar[i].Set_Hp();
            UIManager.Instance.Set_ObjPos(_lis_HpBar[i].rectTransform, _lis_HpBar[i].target_Pos.gameObject);

            _lis_HpBar[i].fLive_Time += Time.deltaTime;
            if (_lis_HpBar[i].fHp <= 0 || _lis_HpBar[i].bCheck_Time)
            {
                if (dicHp_Bar.ContainsKey(_lis_HpBar[i].monster))
                    dicHp_Bar.Remove(_lis_HpBar[i].monster);
                lis_DieHp.Add(_lis_HpBar[i]);
            }
        }

        if (lis_DieHp.Count > 0)
        {
            for (int i = 0; i < lis_DieHp.Count; ++i)
            {
                hp_Pool.Return(lis_DieHp[i]);
            }

            lis_DieHp.Clear();
        }
    }
}
