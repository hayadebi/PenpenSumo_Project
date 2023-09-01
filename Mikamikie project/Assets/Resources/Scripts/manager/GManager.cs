using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    [Header("言語管理の値。0=jp,1=en…etc")] public int isEnglish = 0;
    [Header("値に該当する言語を格納")] public string[] LanguageList;
    [Header("動ける状態かどうか")] public bool walktrg = true;
    [Header("ゲーム終了かどうか")] public int over = -1;
    [Header("SoundManagerと連携したグローバルな効果音トリガー。0以上で効果音")] public int setrg = -1;
    [Header("メニューといったUIを開いているかどうか。0以下で開いてない状態")] public int setmenu = 0;
    [Header("難易度。0=easy,1=normal,2=hard")] public int difficulty_mode = 1;
    //設定できる値
    [Header("オーディオ関係の設定")]
    public float audioMax = 0.16f;
    public float seMax = 0.08f;
    //[Header("モーションブラーをかけるかどうか")]
    //public bool motion_trg = false;
   // [Header("回避と攻撃を兼用するかどうか")]
    //public bool at_and_dash = false;
    [Header("タイトルのアニメーションが有効かどうか")]
    public bool title_anim = true;
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