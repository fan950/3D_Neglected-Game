using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Camera : MonoBehaviour
{
    private const float fPosY = 7.5f;
    private const float fPosz = -8.5f;
    private const float fRotatX = 37f;
    private GameObject player_Obj;
    private bool bShake;
    public void Init(GameObject player)
    {
        player_Obj = player;
        LookAt_Player();
    }
    public void LookAt_Player()
    {
        transform.localRotation = Quaternion.Euler(fRotatX, 0, 0);
    }
    public void Follow_Player()
    {
        if (ModelManager.Instance.player == null)
            return;

        if (bShake)
            return;
     
        transform.position = new Vector3(player_Obj.transform.position.x, fPosY, player_Obj.transform.position.z + fPosz);
    }

    public bool Follow_Boss(Vector3 pos, Vector3 lootAt, float fSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * fSpeed);
        transform.LookAt(lootAt);
        if (Vector3.Distance(transform.position, pos) <= 0.1f)
        {
            return true;
        }
        return false;
    }
    public void Set_Shake()
    {
        StartCoroutine(ShakeCamera(0.1f, 0.3f));
    }
    public IEnumerator ShakeCamera(float duration, float magnitudePos)
    {
        bShake = true;
        float passTime = 0.0f;

        while (passTime < duration)
        {
            Vector3 shakePos = Random.insideUnitSphere;

            transform.position += shakePos * magnitudePos;

            passTime += Time.deltaTime;
            yield return null;
        }

        bShake = false;
    }
}
