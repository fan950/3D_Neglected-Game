using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIShowBossTmp : MonoBehaviour
{
    private RectTransform rectTransform;
    public TextMeshProUGUI boos_tmp;
    public void Init()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        rectTransform.anchoredPosition3D = Vector3.zero;
        rectTransform.localScale = Vector3.one;
    }
    public void Open(string tmp)
    {
        boos_tmp.text = TableManager.Instance.stringTable.Get_String(tmp);
        gameObject.SetActive(true);
    }
    public void End_BossTmp()
    {
        gameObject.SetActive(false);
    }
}
