using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Camera uiCamera;
    [SerializeField] private Canvas uiCanvas;
    private RectTransform uiCanvas_Pos;

    [Header("UIParent")]
    [SerializeField] private Transform uiWindow_Pos;
    [SerializeField] private Transform uiPopup_Pos;

    private Dictionary<eUIPopup_Type, UIPopup> dicUIPopup = new Dictionary<eUIPopup_Type, UIPopup>();
    private Dictionary<eUIWindow_Type, UIWindow> dicUIWindow = new Dictionary<eUIWindow_Type, UIWindow>();

    private Dictionary<eAtlas_Type, SpriteAtlas> dicSpriteAtlas = new Dictionary<eAtlas_Type, SpriteAtlas>();

    private UIScreen uiScreen;
    private UIHUD uiHUD;
    private UIFade uiFade;

    protected override void Init()
    {
        uiCanvas_Pos = uiCanvas.GetComponent<RectTransform>();
        eAtlas_Type _atlas_Type = 0;
        for (int i = 0; i < (int)eAtlas_Type.Max; ++i)
        {
            SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("Atlas/" + _atlas_Type.ToString());

            if (!dicSpriteAtlas.ContainsKey(_atlas_Type))
                dicSpriteAtlas.Add(_atlas_Type, spriteAtlas);
            ++_atlas_Type;
        }
    }
    public void Set_UICamera(Camera mainCamera)
    {
        mainCamera.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
    }

    public void Set_UIPos(Transform uiWindow, Transform uiPopup)
    {
        uiWindow_Pos = uiWindow;
        uiPopup_Pos = uiPopup;
    }
    public GameObject UICreate(string sPath, Transform parent)
    {
        GameObject _obj = Instantiate(Resources.Load(sPath) as GameObject);
        _obj.transform.SetParent(parent);
        _obj.transform.localScale = Vector3.one;
        _obj.transform.localRotation = Quaternion.identity;
        RectTransform _rect = _obj.GetComponent<RectTransform>();
        _rect.anchoredPosition3D = Vector3.zero;
        return _obj;
    }
    public void UIWindow_Open(eUIWindow_Type window_Type)
    {
        string _sPath = "UI/UIWindow/" + window_Type.ToString();
        switch (window_Type)
        {
            case eUIWindow_Type.UIInventory:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    UIInventory _uiInventory = _obj.GetComponent<UIInventory>();
                    _uiInventory.Init();
                    dicUIWindow.Add(window_Type, _uiInventory);
                }
                break;
            case eUIWindow_Type.UIScreen:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    uiScreen = _obj.GetComponent<UIScreen>();
                    uiScreen.Init();
                    dicUIWindow.Add(window_Type, uiScreen);
                }
                break;
            case eUIWindow_Type.UIShop:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    UIShop _uiShop = _obj.GetComponent<UIShop>();
                    _uiShop.Init();
                    dicUIWindow.Add(window_Type, _uiShop);
                }
                break;
            case eUIWindow_Type.UIReinforce:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    UIReinforce _uiReinforce = _obj.GetComponent<UIReinforce>();
                    _uiReinforce.Init();
                    dicUIWindow.Add(window_Type, _uiReinforce);
                }
                break;
            case eUIWindow_Type.UISkill:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    UISkill _uiSkill = _obj.GetComponent<UISkill>();
                    _uiSkill.Init();
                    dicUIWindow.Add(window_Type, _uiSkill);
                }
                break;
            case eUIWindow_Type.UIHUD:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    uiHUD = _obj.GetComponent<UIHUD>();
                    uiHUD.Init();
                    dicUIWindow.Add(window_Type, uiHUD);
                }
                break;
            case eUIWindow_Type.UIFade:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    uiFade = _obj.GetComponent<UIFade>();
                    uiFade.Init();
                    dicUIWindow.Add(window_Type, uiFade);
                }
                break;
            case eUIWindow_Type.UIClearEnd:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    UIClearEnd uiClearEnd = _obj.GetComponent<UIClearEnd>();
                    uiClearEnd.Init();
                    dicUIWindow.Add(window_Type, uiClearEnd);
                }
                break;
            case eUIWindow_Type.UIStart:
                if (!dicUIWindow.ContainsKey(window_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiWindow_Pos);
                    UIStart uiStart = _obj.GetComponent<UIStart>();
                    uiStart.Init();
                    dicUIWindow.Add(window_Type, uiStart);
                }
                break;
        }
        dicUIWindow[window_Type].Open();
    }
    public UIWindow Get_UIWindow(eUIWindow_Type window_Type)
    {
        if (!dicUIWindow.ContainsKey(window_Type))
            UIWindow_Open(window_Type);
        return dicUIWindow[window_Type];
    }

    public UIPopup Get_UIPopup(eUIPopup_Type popup_Type)
    {
        string _sPath = "UI/UIPopup/" + popup_Type.ToString();
        switch (popup_Type)
        {
            case eUIPopup_Type.UIItem_Popup:
                if (!dicUIPopup.ContainsKey(popup_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiPopup_Pos);
                    UIItem_Popup _uiItemPopup = _obj.GetComponent<UIItem_Popup>();
                    _uiItemPopup.Init();
                    dicUIPopup.Add(popup_Type, _uiItemPopup);
                }
                break;
            case eUIPopup_Type.UIBuy_Popup:
                if (!dicUIPopup.ContainsKey(popup_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiPopup_Pos);
                    UIBuy_Popup _uiBuyPopup = _obj.GetComponent<UIBuy_Popup>();
                    _uiBuyPopup.Init();
                    dicUIPopup.Add(popup_Type, _uiBuyPopup);
                }
                break;
            case eUIPopup_Type.UIOk_Popup:
                if (!dicUIPopup.ContainsKey(popup_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiPopup_Pos);
                    UIOk_Popup _uiOk_Popup = _obj.GetComponent<UIOk_Popup>();
                    _uiOk_Popup.Init();
                    dicUIPopup.Add(popup_Type, _uiOk_Popup);
                }
                break;
            case eUIPopup_Type.UICommon_Popup:
                if (!dicUIPopup.ContainsKey(popup_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiPopup_Pos);
                    UICommon_Popup _uiCommon_Popup = _obj.GetComponent<UICommon_Popup>();
                    _uiCommon_Popup.Init();
                    dicUIPopup.Add(popup_Type, _uiCommon_Popup);
                }
                break;
            case eUIPopup_Type.UIStage_Popup:
                if (!dicUIPopup.ContainsKey(popup_Type))
                {
                    GameObject _obj = UICreate(_sPath, uiPopup_Pos);
                    UIStage_Popup _uiStage_Popup = _obj.GetComponent<UIStage_Popup>();
                    _uiStage_Popup.Init();
                    dicUIPopup.Add(popup_Type, _uiStage_Popup);
                }
                break;
        }
        dicUIPopup[popup_Type].Open();
        return dicUIPopup[popup_Type];
    }
    public void UI_Next_Scene()
    {
        if (uiHUD != null)
            uiHUD.HUD_Next_Scene();
    }
    public void Mgr_Update()
    {
        if (uiHUD != null)
            uiHUD.HUD_Update();

        if (uiScreen != null)
            uiScreen.Screen_Update();
    }
    #region Screen
    public void Active_Screen(bool bActive) 
    {
        uiScreen.gameObject.SetActive(bActive);
    }
    public Vector2 Get_Joystick_Dir()
    {
        return uiScreen.uiJoystick.dir;
    }
    public bool Is_Auto_Joystick()
    {
        return uiScreen.bJoystick_Auto;
    }
    public void Set_Stage()
    {
        uiScreen.Set_Stage();
        uiScreen.Set_BossTime();
    }
    public void Set_Hp()
    {
        uiScreen.Set_Hp();
    }
    public void Set_Exp()
    {
        uiScreen.Set_Exp();
    }
    public void Set_Level()
    {
        uiScreen.Set_Level();
    }
    public void Set_Gold()
    {
        uiScreen.Set_Gold();
    }
    public void Set_BossHp(Monster monster)
    {
        uiScreen.Set_BossHp(monster.nHp_Max, monster.nHp);
    }
    #endregion

    #region HUD
    public void Active_Box(bool bActive)
    {
        uiHUD.Active_Box(bActive);
    }
    public void Set_Damage(GameObject obj, int nDamage, bool bCri)
    {
        RectTransform _pos = uiHUD.Set_DamageTmp(nDamage, bCri);
        Set_ObjPos(_pos, obj);
    }
    public void Get_LevelUp()
    {
        RectTransform _pos = uiHUD.Set_LevelTmp();
        Set_ObjPos(_pos, ModelManager.Instance.player.head_Pos.gameObject);
    }
    public void Set_HpBar(Monster mob)
    {
        if (mob.monster_Type != eMonster_Type.Boss)
            uiHUD.Set_HpBar(mob);
        else
            Set_BossHp(mob);
    }

    public void Show_BossTmp(string tmp)
    {
        uiHUD.Open_ShowBoss(tmp);
    }
    #endregion
    #region Fade
    public void Open_Fade(Action action)
    {
        if (uiFade == null)
            UIWindow_Open(eUIWindow_Type.UIFade);

        uiFade.Fade_On(action);
    }
    public void Close_Fade()
    {
        if (uiFade == null)
            return;

        if (uiFade.gameObject.activeSelf)
            uiFade.Fade_Off();
    }
    #endregion
    public void Set_ObjPos(RectTransform pos, GameObject obj)
    {
        Vector2 _uiPos;
        Vector2 _screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas_Pos, _screenPos, uiCamera, out _uiPos);

        pos.localPosition = _uiPos;
    }
    #region Atlas
    public Sprite Get_Sprite(eAtlas_Type atlas_Type, string sName)
    {
        if (!dicSpriteAtlas.ContainsKey(atlas_Type))
            return null;

        return dicSpriteAtlas[atlas_Type].GetSprite(sName);
    }
    #endregion

    #region Equipment
    public void Set_Equipment(ePart_Type part_Type, SB_Item_Data item_Data)
    {
        ModelManager.Instance.Set_Player_Equipment(part_Type, item_Data);
        Set_Equip_Index(part_Type, item_Data);
    }
    public void Set_Equip_Index(ePart_Type part_Type, SB_Item_Data item_Data)
    {
        LocalGameData localGame_DB = GameManager.Instance.localGame_DB;

        SB_Item_Data _item_Equip = localGame_DB.Get_Equip_ItemData(part_Type);
        if (_item_Equip.nIndex != 0)
            localGame_DB.Set_Change_Equip(item_Data, _item_Equip);
        else
            localGame_DB.Set_Equip_ItemData(part_Type, item_Data);
    }
    #endregion

    #region Skill
    public void Start_Skill(SkillData skillData)
    {
        ModelManager.Instance.Player_Skill(skillData);
    }

    public void Set_Skill()
    {
        uiScreen.Set_SkilSlot();
    }
    #endregion
}
