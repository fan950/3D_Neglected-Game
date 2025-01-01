using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private float fSaveTime = 0;
    private const float fSave = 180f;
    private bool bStop;

    public eLocalize_Type localize_Type = eLocalize_Type.Kr;
    private LocalGameData localGameData;
    public LocalGameData localGame_DB { get { return localGameData; } }

    public InGameScene inGameScene;
    [HideInInspector] public List<SB_Item_Data> lisItemData = new List<SB_Item_Data>();
    public override void Awake()
    {
        base.Awake();
        TableManager.Instance.All_TableLoading();

        for (int i = 0; i < (int)ePart_Type.Max; ++i)
        {
            SB_Item_Data _item_Data = new SB_Item_Data();
            _item_Data.nIndex = 0;
            _item_Data.nStar = 0;
            _item_Data.part_Type = (ePart_Type)i;
            _item_Data.lisState_Data = new List<SB_Equip_Item_State_Data>();
            lisItemData.Add(_item_Data);
        }

        Load_DB();
        bStop = false;
    }
    public void Set_Stop(bool bStop)
    {
        ModelManager.Instance.AllNav_Stop(bStop);
        this.bStop = bStop;
    }
    public bool Get_Stop()
    {
        return bStop;
    }
    #region DB
    public void Save_DB()
    {
        if (localGameData.player_Data.nHp == 0)
        {
            localGameData.player_Data.nHp = ModelManager.Instance.player.nHp_Max;
        }
        File_DB.Save(localGameData);
    }
    public void Load_DB()
    {
        localGameData = File_DB.Load();
        if (localGameData == null)
        {
            localGameData = new LocalGameData();
            localGameData.Init();
            Save_DB();
        }
    }
    public void OnDisable()
    {
        Save_DB();
    }
    #endregion

    #region Scene
    public void Next_Scene(int _nIndex, Action action = null)
    {
        localGame_DB.Set_Stage(_nIndex);
        UIManager.Instance.Open_Fade(delegate
        {
            if (action != null)
                action();

            StageData _stageData = TableManager.Instance.stageTable.Get_StageData(localGame_DB.Get_Stage());
            StartCoroutine(Scene_Coro(_stageData.sScene));
        });
    }

    IEnumerator Scene_Coro(string sceneName)
    {
        bStop = true;

        ModelManager.Instance.Model_Next_Scene();
        BulletManager.Instance.Bullet_Next_Scene();
        UIManager.Instance.UI_Next_Scene();

        yield return Resources.UnloadUnusedAssets();

        SceneManager.LoadScene(sceneName);
        bStop = false;
    }
    #endregion
    public void Update()
    {
        fSaveTime += Time.deltaTime;
        if (fSaveTime >= fSave)
        {
            Save_DB();
            fSaveTime = 0;
        }

        if (bStop)
            return;

        ModelManager.Instance.Mgr_Update();
        BulletManager.Instance.Mgr_Update();
        FXManager.Instance.Mgr_Update();
        UIManager.Instance.Mgr_Update();

    }
}
