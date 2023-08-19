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
    [Header("難易度変化")]
    public int mode_num = 1;
    public Text mode_text;
    private string[] modes = new string[3];
    private float cooltime = 0f;

    private bool clicktrg = false;
    // Start is called before the first frame update
    void Start()
    {
        if (mode_text != null)
        {
            modes[0] = "EASY";
            modes[1] = "NORMAL";
            modes[2] = "HARD";
            mode_text.text = "難易度：" + modes[GManager.instance.difficulty_mode] + "で開始(仮)";
        }
    }
    private void Update()
    {
        if (cooltime >= 0) cooltime -= Time.deltaTime;
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
    public void DifficultyChange()
    {
        if (mode_text != null && cooltime<=0f)
        {
            cooltime = 0.3f;
            GManager.instance.setrg = 0;
            GManager.instance.difficulty_mode += mode_num;
            if (GManager.instance.difficulty_mode > 2) GManager.instance.difficulty_mode = 0;
            else if (GManager.instance.difficulty_mode < 0) GManager.instance.difficulty_mode = 2;
            mode_text.text = "難易度：" + modes[GManager.instance.difficulty_mode] + "で開始(仮)";
        }
    }
    public void quitClick()
    {
        Application.Quit();
    }
}
