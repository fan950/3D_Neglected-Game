using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStage_Popup : UIPopup
{
    public UIBtn Cancel_Btn;
    [Header("Scroll")]
    public ScrollRect scrollRect;

    private const string sStageInfo_Path = "UI/InGameScene/UIStageInfo";
    public override void Init()
    {
        base.Init();
        Cancel_Btn.Init(Close);

        List<StageData> _lisStageData = TableManager.Instance.stageTable.Get_ListStageData();
        for (int i = 0; i < _lisStageData.Count; ++i)
        {
            GameObject _obj = Instantiate(Resources.Load(sStageInfo_Path) as GameObject);
            _obj.transform.SetParent(scrollRect.content);
            _obj.transform.localRotation = Quaternion.identity;
            _obj.transform.localScale = Vector3.one;
            _obj.transform.localPosition = Vector3.zero;

            UIStageInfo _uiStageInfo = _obj.GetComponent<UIStageInfo>();

            bool bClear = false;

            if (GameManager.Instance.localGame_DB.Get_Clear_Stage() >= _lisStageData[i].nIndex)
            {
                bClear = true;
                _uiStageInfo.Change_interactable(true);
            }

            _uiStageInfo.Init();
            _uiStageInfo.Set_State(_lisStageData[i], Open_StagePopup, bClear);
        }
    }

    public void Open_StagePopup(StageData stageData)
    {
        //if (GameManager.Instance.localGame_DB.Get_Clear_Stage() >= stageData.nIndex)
        //{
        UICommon_Popup _uiCommon_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UICommon_Popup) as UICommon_Popup;
        _uiCommon_Popup.OnShow(TableManager.Instance.stringTable.Get_String("Like to enter"), delegate
         {
             GameManager.Instance.Next_Scene(stageData.nIndex);
             Close();
         });
        //}
        //else
        //{
        //    UIOk_Popup _uiOk_Popup = UIManager.Instance.Get_UIPopup(eUIPopup_Type.UIOk_Popup) as UIOk_Popup;
        //    _uiOk_Popup.OnShow(TableManager.Instance.stringTable.Get_String("Level Low Enter"));
        //}
    }
}
