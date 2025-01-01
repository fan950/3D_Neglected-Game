using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScene : MonoBehaviour
{
    [SerializeField] private Follow_Camera follow_Camera;
    public ParticleSystem magicSquare;

    [Header("Pos")]
    public Transform[] arrSpawn_Pos;
    public Transform boss_Pos;
    public Transform bossCamera_Pos;
    [Header("Obj")]
    public GameObject[] arrBossWall_Obj;
    private Coroutine bossCoro;

    [HideInInspector] public bool isBoss = false;

    public void Start()
    {
        GameManager.Instance.inGameScene = this;

        UIManager.Instance.UIWindow_Open(eUIWindow_Type.UIHUD);
        UIManager.Instance.UIWindow_Open(eUIWindow_Type.UIScreen);

        UIManager.Instance.Set_UICamera(Camera.main);

        ModelManager.Instance.isBoss = false;
        ModelManager.Instance.Init_Monster_Pool(arrSpawn_Pos);

        magicSquare.gameObject.SetActive(false);
        follow_Camera.Init(ModelManager.Instance.player.gameObject);

        UIManager.Instance.Set_Stage();
        UIManager.Instance.Close_Fade();

        for (int i = 0; i < arrBossWall_Obj.Length; ++i)
        {
            arrBossWall_Obj[i].SetActive(true);
        }
        UIManager.Instance.Get_UIWindow(eUIWindow_Type.UIStart).Open();
        ModelManager.Instance.player.Active_Box(true);
    }

    public void Set_ShakeCamera()
    {
        follow_Camera.Set_Shake();
    }

    public void LateUpdate()
    {
        if (isBoss)
            return;

        follow_Camera.Follow_Player();
    }
    public void Set_BossCoro(Monster mob, bool bStart, Action action)
    {
        if (bStart)
        {
            if (bossCoro != null)
                StopCoroutine(bossCoro);

            bossCoro = StartCoroutine(CoroBossCamera(mob, action));
        }
        else
        {
            if (bossCoro != null)
                StopCoroutine(bossCoro);
            bossCoro = null;
        }
    }
    private IEnumerator CoroBossCamera(Monster mob, Action action)
    {
        isBoss = true;
        float _fMoveSpeed = 200f;
        float _fLookAt = 2f;
        float _fPosY = -10;
        Vector3 _vecLootAt = new Vector3(magicSquare.gameObject.transform.position.x, magicSquare.gameObject.transform.position.y + _fLookAt, magicSquare.gameObject.transform.position.z);

        UIManager.Instance.Active_Screen(false);
        UIManager.Instance.Active_Box(false);

        for (int i = 0; i < arrBossWall_Obj.Length; ++i)
        {
            arrBossWall_Obj[i].SetActive(false);
        }

        while (true)
        {
            yield return null;
            bool _bPos = follow_Camera.Follow_Boss(bossCamera_Pos.transform.position, _vecLootAt, _fMoveSpeed);
            if (_bPos)
            {
                magicSquare.gameObject.SetActive(true);

                yield return new WaitForSeconds(2f);

                break;
            }
        }

        mob.transform.rotation = Quaternion.Euler(0, 180, 0);
        mob.transform.position = new Vector3(boss_Pos.transform.position.x, boss_Pos.transform.position.y + _fPosY, boss_Pos.transform.position.z);

        mob.gameObject.SetActive(true);

        while (true)
        {
            yield return null;
            mob.transform.position = Vector3.MoveTowards(mob.transform.position, boss_Pos.transform.position, Time.deltaTime * 5f);
            if (Vector3.Distance(mob.transform.position, boss_Pos.transform.position) <= 0.01f)
            {
                break;
            }
        }
        yield return new WaitForSeconds(3.0f);

        follow_Camera.LookAt_Player();
        magicSquare.gameObject.SetActive(false);
        isBoss = false;

        GameManager.Instance.Set_Stop(false);
        UIManager.Instance.Active_Box(true);
        UIManager.Instance.Active_Screen(true);
        action();
    }
    public void Clear_Stage()
    {
        GameManager.Instance.Set_Stop(true);
        GameManager.Instance.localGame_DB.Set_Clear_Stage(GameManager.Instance.localGame_DB.Get_Stage());
        UIManager.Instance.Get_UIWindow(eUIWindow_Type.UIClearEnd).Open();
        GameManager.Instance.Save_DB();
    }
}
