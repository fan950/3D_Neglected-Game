using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIExp : MonoBehaviour
{
    public Image exp_Value;

    public void Set_Exp()
    {
        float _fExp = ModelManager.Instance.player.fExp / TableManager.Instance.expTable.Get_ExpMax(ModelManager.Instance.player.nLevel);
        exp_Value.fillAmount = _fExp;
    }
}
