using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    private Dictionary<string, ObjcetPool<Bullet>> dicBullet = new Dictionary<string, ObjcetPool<Bullet>>();
    private Dictionary<GameObject, Bullet> dicLive_Bullet = new Dictionary<GameObject, Bullet>();
    private List<Bullet> lisDie_Bullet = new List<Bullet>();

    private const string sBulletPath = "Bullet/";
    private const int nBullet_Count = 10;

    public void Create_Bullet(Model model, GameObject weaponObj, int nIndex, string sPath)
    {
        if (!dicBullet.ContainsKey(sPath))
        {
            ObjcetPool<Bullet> _obj = new ObjcetPool<Bullet>();
            _obj.Init(sBulletPath + sPath, nBullet_Count, transform);

            dicBullet.Add(sPath, _obj);
        }
        Bullet _bullet = dicBullet[sPath].Get();
        _bullet.transform.rotation = Quaternion.identity;
        _bullet.transform.position = weaponObj.transform.position;
        _bullet.Init(sPath, model, nIndex);
        dicLive_Bullet.Add(_bullet.gameObject, _bullet);
    }
    public void Bullet_Next_Scene()
    {
        foreach (KeyValuePair<string, ObjcetPool<Bullet>> bullet in dicBullet)
        {
            bullet.Value.Clear();
        }

        dicBullet.Clear();
        dicLive_Bullet.Clear();
        lisDie_Bullet.Clear();
    }
    public void Mgr_Update()
    {
        foreach (KeyValuePair<GameObject, Bullet> bullet in dicLive_Bullet)
        {
            if (!bullet.Value.bRemove)
                bullet.Value.Update_Bullet();
            else
                lisDie_Bullet.Add(bullet.Value);
        }

        if (lisDie_Bullet.Count > 0)
        {
            for (int i = 0; i < lisDie_Bullet.Count; ++i)
            {
                Die_Bullet(lisDie_Bullet[i]);
            }
            lisDie_Bullet.Clear();
        }
    }

    public void Die_Bullet(Bullet bullet)
    {
        Bullet _mob = dicLive_Bullet[bullet.gameObject];
        dicLive_Bullet.Remove(bullet.gameObject);

        dicBullet[_mob.sKey_Name].Return(bullet);
    }
}
