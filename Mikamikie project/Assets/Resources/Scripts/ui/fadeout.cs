using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fadeout : MonoBehaviour
{
    [HideInInspector] public bool compFadeIn = false;   //フェードイン完了
    [HideInInspector] public bool compFadeOut = false;  //フェードイン完了

    
    private Image img = null;
    private float timer = 0;
    private float wait = 0.5f;
    private bool fadeIn = false;
    private bool fadeOut = false;
    public float r = 1;
    public float g = 1;
    public float b = 1;

    /// <summary>
    /// フェードインを開始する
    /// </summary>
    public void StartFadeIn()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeIn = true;
        compFadeIn = false;
        timer = 0.0f;
        img.color = new Color(r, g, b, 1);
        img.fillAmount = 1;
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードアウトを開始する
    /// </summary>
    public void StartFadeOut()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeOut = true;
        compFadeOut = false;
        timer = 0.0f;
        img.color = new Color(r, g, b, 0);
        img.fillAmount = 0;
        img.raycastTarget = true;
    }

    void Start()
    {
        img = GetComponent<Image>();
        StartFadeIn();
    }

    void Update()
    {
        if (fadeIn)
        {
            //フェードイン中
            if (timer < 1 + wait && timer > wait)
            {
                img.color = new Color(r, g, b, 1 - timer + wait);
                img.fillAmount = 1 - timer + wait;
            }
            //フェードイン完了
            else if (timer >= 1 + wait)
            {
                img.color = new Color(r, g, b, 0);
                img.fillAmount = 0;
                img.raycastTarget = false;
                timer = 0.0f;
                fadeIn = false;
                compFadeIn = true;
            }
            timer += Time.deltaTime;
        }
        else if (fadeOut)
        {
            //フェードアウト中
            if (timer < 1)
            {
                img.color = new Color(r, g, b, timer);
                img.fillAmount = timer;
            }
            //フェードアウト完了
            else if (timer >= 1)
            {
                img.color = new Color(r, g, b, 1);
                img.fillAmount = 1;
                img.raycastTarget = false;
                timer = 0.0f;
                fadeOut = false;
                compFadeOut = true;
            }
            timer += Time.deltaTime;
        }
    }
}