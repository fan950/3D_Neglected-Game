using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopup : MonoBehaviour
{
    [Header("UIBtn")]
    public UIBtn close_Btn;
    public virtual void Init()
    {
        close_Btn.Init(Close);
    }
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
