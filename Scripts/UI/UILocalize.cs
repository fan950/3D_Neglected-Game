using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UILocalize : MonoBehaviour
{
    public string sKey;
    private TextMeshProUGUI tmp;
    public void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = string.Format(TableManager.Instance.stringTable.Get_String(sKey));
    }
}
