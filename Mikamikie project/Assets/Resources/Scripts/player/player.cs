using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class player : MonoBehaviour
{
    private bool stoptrg = false;//優先度高めにプレイヤーを停止させるトリガー
    public int player_health = 20;
    public int player_stamina = 20;
    [SerializeField]private float player_speed = 2;//プレイヤーの速度
    private float oldp_y = 0;//古いy軸の一時的情報。
    [SerializeField] private float gravity = 32;//重力値
    [SerializeField] private Transform character;//プレイヤー本体、メインに対応するトランスフォーム
    [SerializeField] private Transform body;//プレイヤーのモデルに対応するトランスフォーム
    private bool movetrg = false;//移動時にアニメーションさせるかどうか
    [SerializeField] private string number_name;//アニメーターの変数名
    //移動で加算させるxyzそれぞれの値
    private float x_speed = 0;
    private float y_speed = 0;
    //プレイヤーのサウンド関係
    [SerializeField] private AudioClip groundse;
    [SerializeField] private AudioClip jumpse;
    private AudioSource audioSource;

    [SerializeField] private Animator anim;//プレイヤーのアニメーションセット
    [SerializeField] private Rigidbody rb;//プレイヤーの物理挙動をセット
    //回転反転に使う値
    private float x_rotation = 0;
    private float y_rotation = 0;
    private Vector3 m_xaxiz;
    private Vector3 cm_axiz;

    //各アニメーションに該当する値
    [SerializeField] private int move_num = 1;
    [SerializeField] private int stand_anim = 0;
    [SerializeField] private int walk_anim = 1;
    [SerializeField] private int jump_anim = 2;
    //カメラ関係
    private GameObject cm;
    private Vector3 cm_vec;
    //操作ボタンに関する
    private longbuttonclick upbtn;
    private longbuttonclick downbtn;
    private longbuttonclick rightbtn;
    private longbuttonclick leftbtn;

    //視点回転に関する
    //private float maxYAngle = 135f;
    //private float minYAngle = 45f;
    private Vector3 latest_pos;  //前回のPosition
    //反転感度
    public float kando = 45;

    // Start is called before the first frame update

    void Start()
    {
        //初手取得
        audioSource = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody>();
        rb.useGravity = false;
        m_xaxiz = body.transform.localEulerAngles;
        latest_pos = character.transform.position;  //前回のPositionの更新

        cm = GameObject.Find("Main Camera");
        cm_vec = character.position - cm.transform.position;

        upbtn = GameObject.Find(nameof(upbtn)).GetComponent<longbuttonclick>();
        downbtn = GameObject.Find(nameof(downbtn)).GetComponent<longbuttonclick>();
        rightbtn = GameObject.Find(nameof(rightbtn)).GetComponent<longbuttonclick>();
        leftbtn = GameObject.Find(nameof(leftbtn)).GetComponent<longbuttonclick>();
    }
    void StartSet()
    {
        
    }

    void FixedUpdate()
    {
        if (GManager.instance.walktrg && !GManager.instance.over && !stoptrg && (upbtn && downbtn && rightbtn && leftbtn))
        {
            //重力部分
            if (rb.useGravity)
                rb.useGravity = false;
            x_rotation = 0;
            y_rotation = 0;
            x_speed = 0;
            y_speed = -gravity;
            //----ここからは移動----
            if (!movetrg && (rightbtn.push || leftbtn.push || upbtn.push || downbtn.push))
            {
                //この部分では歩きの効果音、アニメーションを操作
                movetrg = true;
                //audioSource.clip = groundse;
                audioSource.loop = true;
                audioSource.Play();
                anim.SetInteger(number_name, walk_anim);
            }
            //移動メイン部分
            var inputX = 0;
            var inputZ = 0;
            if (rightbtn.push) inputX = 1;
            if (leftbtn.push) inputX = -1;
            //if (upbtn.push) inputZ = 1;
            //if (downbtn.push) inputZ = -1;
            var tempVc = new Vector3(move_num * inputX, 0, inputZ);
            if (tempVc.magnitude > 1) tempVc = tempVc.normalized;
            var vec = tempVc;
            var movevec = vec * player_speed + Vector3.up * y_speed;
            rb.velocity = movevec;
            Vector3 targetPositon = latest_pos;
            // 高さがずれていると体ごと上下を向いてしまうので便宜的に高さを統一
            if (character.transform.position.y != latest_pos.y)
                targetPositon = new Vector3(latest_pos.x, character.transform.position.y, latest_pos.z);
            Vector3 diff = character.transform.position - targetPositon;
            if (diff != Vector3.zero && (upbtn.push || leftbtn.push || downbtn.push || rightbtn.push))
            {
                Quaternion targetRotation = Quaternion.LookRotation(diff);
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, Time.deltaTime * kando);
            }
            latest_pos = character.transform.position;  //前回のPositionの更新

            if (!upbtn.push && !leftbtn.push && !downbtn.push && !rightbtn.push)
            {
                //移動してない場合、またはジャンプ中の時はアニメーションや音を止める
                if (movetrg)
                    movetrg = false;
                anim.SetInteger(number_name, stand_anim);
                audioSource.loop = false;
                audioSource.Stop();
            }
        }
        else if ((!GManager.instance.walktrg || GManager.instance.over) && anim.GetInteger(number_name) != stand_anim)
        {
            audioSource.Stop();
            anim.SetInteger(number_name, stand_anim);
            y_speed = 0;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (!GManager.instance.over && GManager.instance.walktrg && player_health <= 0)
        {
            //GManager.instance.setrg = -1;　ゲームオーバー時の効果音 あとで指定
            //エフェクトもここで
            GManager.instance.over = true;
            GManager.instance.walktrg = false;
            //Instantiate(ゲームオーバー画面後で表示, transform.position, transform.rotation);
            body.gameObject.SetActive(false);
            character.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (!GManager.instance.over && GManager.instance.walktrg)
        {
            if (col.tag == "red")
            {
                //GManager.instance.setrg = -1;　ダメージの効果音 あとで指定
                //ダメージ処理は後ほど
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        
    }

}
