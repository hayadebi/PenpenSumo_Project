using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class clickbutton : MonoBehaviour
{
    [Header("シーンチェンジ用")]
    public string next_scene = "loadstage";
    public GameObject fadeui;

    private bool clicktrg = false;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }
    public void NextScene()
    {
        if (!clicktrg)
        {
            clicktrg = true;
            Instantiate(fadeui, transform.position, transform.rotation);
            GManager.instance.setrg = 0;
            Invoke(nameof(SceneChange), 1f);
        }
    }
    void SceneChange()
    {
        GManager.instance.over = false;
        GManager.instance.walktrg = true;
        GManager.instance.setmenu = 0;
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(next_scene);
    }
    public void quitClick()
    {
        Application.Quit();
    }
}
