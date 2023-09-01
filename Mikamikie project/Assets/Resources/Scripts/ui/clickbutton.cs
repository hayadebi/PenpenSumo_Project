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
    [Header("最初のカウント")]
    private float cooltime = 0f;
    private int countnum=3;
    [SerializeField] private Text counttext;

    private bool clicktrg = false;
    [SerializeField] private bool resetwalk = false;
    private float localcooltime = 0f;
    [SerializeField] Toggle ontoggle=null;
    [SerializeField] GameObject targetobj;
    // Start is called before the first frame update
    void Start()
    {
        if (mode_text != null)
        {
            modes[0] = "EASY";
            modes[1] = "NORMAL";
            modes[2] = "HARD";
            mode_text.text = "難易度：" + modes[GManager.instance.difficulty_mode] + "でNPC戦";
        }
        if (resetwalk) GManager.instance.walktrg = false;
        if (ontoggle)
        {
            if (GManager.instance.isEnglish == 0) ontoggle.isOn = false;
            else if (GManager.instance.isEnglish == 1) ontoggle.isOn = true;
        }
    }
    private void Update()
    {
        if (cooltime >= 0) cooltime -= Time.deltaTime;
        else if (localcooltime >= 0) localcooltime -= Time.deltaTime;
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
        GManager.instance.over = -1;
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
            mode_text.text = "難易度：" + modes[GManager.instance.difficulty_mode] + "でNPC戦";
        }
    }
    public void quitClick()
    {
        Application.Quit();
    }
    public void OnWalk()
    {
        counttext.text = "尻相撲開始！";
        GManager.instance.walktrg = true;
        GManager.instance.setrg = 3;
    }
    public void CancelClick()
    {
        GManager.instance.setrg = 4;
    }
    public void OnCount()
    {
        GManager.instance.setrg = 2;
        counttext.text = countnum.ToString();
        countnum -= 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OnClickLoacl()
    {
        if (localcooltime <= 0 && ontoggle)
        {
            localcooltime = 0.3f;
            GManager.instance.setrg = 0;
            if (ontoggle.isOn) GManager.instance.isEnglish = 1;
            else if (!ontoggle.isOn) GManager.instance.isEnglish = 0;
        }
    }
    public void OnClickClose()
    {
        GManager.instance.setrg = 4;
        targetobj.SetActive(false);
    }
    public void OnClickUI()
    {
        GManager.instance.setrg = 0;
        targetobj.SetActive(true);
    }
}
