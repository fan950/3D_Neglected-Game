using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{
    [Header("UIBtn")]
    public UIBtn close_Btn;

    private RectTransform rectTransform;
    public virtual void Init()
    {
        if (close_Btn != null)
            close_Btn.Init(Close);

        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.rotation = Quaternion.identity;
            rectTransform.localScale = Vector3.one;
        }
    }
    public virtual void Open()
    {
        GameManager.Instance.Set_Stop(true);
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        GameManager.Instance.Set_Stop(false);
        gameObject.SetActive(false);
    }
}
