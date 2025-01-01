using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : Singleton<FXManager>
{
    private Dictionary<string, ObjcetPool<Base_FX>> dicFX = new Dictionary<string, ObjcetPool<Base_FX>>();
    private List<Base_FX> lisLive_FX = new List<Base_FX>();
    private List<Base_FX> lisDie_FX = new List<Base_FX>();

    private const int nCount = 5;
    public Base_FX Get_Fx(string sKey, string sPath)
    {
        if (!dicFX.ContainsKey(sKey))
        {
            ObjcetPool<Base_FX> objcetPool = new ObjcetPool<Base_FX>();
            objcetPool.Init(sPath, nCount, transform);
            dicFX.Add(sKey, objcetPool);
        }
        Base_FX base_FX = dicFX[sKey].Get();
        lisLive_FX.Add(base_FX);
        base_FX.Init(sKey);

        return base_FX;
    }

    public void Mgr_Update()
    {
        if (lisLive_FX.Count > 0)
        {
            for (int i = 0; i < lisLive_FX.Count; ++i)
            {
                lisLive_FX[i].Update_FX();
            }
        }

        if (lisDie_FX.Count > 0)
        {
            for (int i = 0; i < lisDie_FX.Count; ++i)
            {
                dicFX[lisDie_FX[i].sKey].Return(lisDie_FX[i]);
                lisLive_FX.Remove(lisDie_FX[i]);
            }

            lisDie_FX.Clear();
        }
    }
    public void Die_FX(Base_FX base_FX)
    {
        lisDie_FX.Add(base_FX);
    }
}
