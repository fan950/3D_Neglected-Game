using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : UIWindow
{
    public Image backgroud_Img;

    private Coroutine fade_Coro;

    public void Fade_On(Action action)
    {
        Open();

        backgroud_Img.color = new Color(0, 0, 0, 0);
        backgroud_Img.gameObject.SetActive(true);
        transform.SetAsLastSibling();

        if (fade_Coro != null)
            StopCoroutine(fade_Coro);
        fade_Coro = StartCoroutine(FadeOn(action));
    }

    public void Fade_Off()
    {
        if (fade_Coro != null)
            StopCoroutine(fade_Coro);
        fade_Coro = StartCoroutine(FadeOff());
    }

    private IEnumerator FadeOn(Action action)
    {
        float fTime = 0;
        while (true)
        {
            yield return null;

            fTime += Time.deltaTime;
            backgroud_Img.color = new Color(0, 0, 0, fTime);

            if (fTime >= 1)
            {
                if (action != null)
                    action();
                break;
            }
        }
    }

    private IEnumerator FadeOff()
    {
        float fTime = 1;
        backgroud_Img.color = new Color(0, 0, 0, 1);

        while (true)
        {
            yield return null;

            fTime -= Time.deltaTime;
            backgroud_Img.color = new Color(0, 0, 0, fTime);

            if (fTime <= 0)
            {
                backgroud_Img.gameObject.SetActive(false);

                fade_Coro = null;
                break;
            }
        }
        Close();
    }
}
