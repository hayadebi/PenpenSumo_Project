﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [Header("言語管理の値。0=jp,1=en…etc")] public int isEnglish = 0;
    [Header("値に該当する言語を格納")] public string[] LanguageList;
    [Header("動ける状態かどうか")] public bool walktrg = true;
    [Header("ゲームオーバーかどうか")] public bool over = false;
    [Header("SoundManagerと連携したグローバルな効果音トリガー。0以上で効果音")] public int setrg = -1;
    [Header("メニューといったUIを開いているかどうか。0以下で開いてない状態")] public int setmenu = 0;
    //設定できる値
    public float audioMax = 0.16f;
    public float seMax = 0.08f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}