using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class fadein : MonoBehaviour
{
    public float r;
    public float g;
    public float b;
    Image imagecolor;
    public float MaxOutTime;

    private float FadeOutTime = 0f;
    private float alphacolor = 0.0f;

    void Start()
    {
        imagecolor = this.GetComponent<Image>();
    }

    void Update()
    {

        FadeOutTime += Time.deltaTime;
        if (FadeOutTime < MaxOutTime)
        {
            alphacolor = FadeOutTime ;
            imagecolor.color = new Color(r, g, b, alphacolor);
            imagecolor = this.GetComponent<Image>();
        }
    }
}
