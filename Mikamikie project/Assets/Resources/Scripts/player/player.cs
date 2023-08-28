﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class player : MonoBehaviour
{
    private bool stoptrg = false;//優先度高めにプレイヤーを停止させるトリガー
    //ステータス、アイテムでいじれる系
    public int player_health = 20;
    public int player_stamina = 20;
    public int player_at = 1;
    public int player_knockbackresistance = 40;
    [SerializeField]private float player_speed = 2;//プレイヤーの速度
    [SerializeField] private float gravity = 32;//重力値
    [SerializeField] private Transform character;//プレイヤー本体、メインに対応するトランスフォーム
    [SerializeField] private Transform body;//プレイヤーのモデルに対応するトランスフォーム
    private bool movetrg = false;//移動時にアニメーションさせるかどうか
    [SerializeField] private string number_name;//アニメーターの変数名
    //移動で加算させるxyzそれぞれの値
    private float y_speed = 0;
    public float x_speed = 0;
    //プレイヤーのサウンド関係
    [SerializeField] private AudioClip groundse;
    [SerializeField] private AudioClip jumpse;
    [SerializeField] private AudioClip escapese;
    [SerializeField] private AudioClip attackse;
    [SerializeField] private AudioClip damagese;
    private AudioSource audioSource;

    [SerializeField] private Animator anim;//プレイヤーのアニメーションセット
    public Rigidbody rb;//プレイヤーの物理挙動をセット
    private CapsuleCollider capcol;
    //回転反転に使う値
    private float x_rotation = 0;
    private float y_rotation = 0;
    private Vector3 m_xaxiz;

    //各アニメーションに該当する値
    [SerializeField] private int move_num = 1;
    [SerializeField] private int stand_anim = 0;
    [SerializeField] private int walk_anim = 1;
    [SerializeField] private int jump_anim = 3;
    [SerializeField] private int dash_anim = 2;
    [SerializeField] private int at_anim = 4;
    [SerializeField] private int damage_anim = 4;

    //操作ボタンに関する
    private longbuttonclick rightbtn;
    private longbuttonclick leftbtn;
    private longbuttonclick jumpbtn;
    private longbuttonclick dashbtn;
    private longbuttonclick atbtn;
    [SerializeField] private string rightbtn_name="rightbtn";
    [SerializeField] private string leftbtn_name="leftbtn";
    [SerializeField] private string jumpbtn_name="jumpbtn";
    [SerializeField] private string dashbtn_name="dashbtn";
    [SerializeField] private string dashonlybtn_name = "dashonlybtn";
    private bool old_dasgcheck = false;

    //視点回転に関する
    //private float maxYAngle = 135f;
    //private float minYAngle = 45f;
    private Vector3 latest_pos;  //前回のPosition
    //反転感度
    public float kando = 90;
    //地面判定 ジャンプ関係
    private float jump_cooltime = 0f;
    private bool jump_uptrg = false;
    private bool jump_slowtrg = false;
    public float jump_maxuptime = 0.3f;//上昇時間
    private float jump_uptime = 0f;
    public float slow_max = 3f;
    public float upspeed_max = 16;
    public ColEvent colevent_ground;
    public ColEvent colevent_head;
    public ColEvent colevent_ass;
    //回避 攻撃
    public float flipspeed = -170;
    public float fliptime = 0f;
    private float flipcoomtime = 0f;
    private onPostSc onpost;
    public float attime = 0f;
    [SerializeField] private GameObject dasheffect;//ダッシュエフェクト
    [SerializeField] private GameObject damageeffect;//ダメージエフェクト
    [SerializeField] private GameObject jumpeffect;
    public GameObject popuiobj;
    //スタミナ回復
    private float stamina_time = 0f;
    private bool oneover = false;
    private bool nextframe_dash = false;
    //NPCかどうか
    [SerializeField] private bool npc_ai = false;
    // Start is called before the first frame update
    void Start()
    {
        //初手取得
        audioSource = this.GetComponent<AudioSource>();
        rb = this.GetComponent<Rigidbody>();
        capcol = this.GetComponent<CapsuleCollider>();
        rb.useGravity = false;
        m_xaxiz = body.transform.localEulerAngles;
        latest_pos = character.transform.position;  //前回のPositionの更新

        rightbtn = GameObject.Find(rightbtn_name).GetComponent<longbuttonclick>();
        leftbtn = GameObject.Find(leftbtn_name).GetComponent<longbuttonclick>();
        jumpbtn = GameObject.Find(jumpbtn_name).GetComponent<longbuttonclick>();
        atbtn = GameObject.Find(dashbtn_name).GetComponent<longbuttonclick>();
        if (GManager.instance.at_and_dash)dashbtn = GameObject.Find(dashbtn_name).GetComponent<longbuttonclick>();
        else dashbtn = GameObject.Find(dashonlybtn_name).GetComponent<longbuttonclick>();
        onpost = GameObject.Find("main_PostVolume").GetComponent<onPostSc>();
    }
    void StartSet()
    {
        
    }

    void FixedUpdate()
    {
        if (GManager.instance.walktrg && !stoptrg && (rightbtn && leftbtn))
        {
            if (GManager.instance.over == -1)
            {
                if (nextframe_dash)
                {
                    nextframe_dash = false;
                    anim.SetInteger(number_name, dash_anim);
                }
                //一部アニメーション
                if (anim.GetInteger(number_name) == dash_anim && fliptime <= 0) {anim.SetInteger(number_name, stand_anim); movetrg = false;}
            else if (anim.GetInteger(number_name) == at_anim && attime <= 0) { anim.SetInteger(number_name, stand_anim); movetrg = false; }
                //設定に切り替えがあったら
                if (old_dasgcheck != GManager.instance.at_and_dash)
                {
                    old_dasgcheck = GManager.instance.at_and_dash;
                    if (GManager.instance.at_and_dash) dashbtn = GameObject.Find(dashbtn_name).GetComponent<longbuttonclick>();
                    else dashbtn = GameObject.Find(dashonlybtn_name).GetComponent<longbuttonclick>();
                }
                //スタミナ回復
                if (player_stamina < 10)
                {
                    stamina_time += Time.deltaTime;
                    if (stamina_time >= 1.3f)
                    {
                        stamina_time = 0f;
                        player_stamina += 1;
                    }
                }

                x_speed = Mathf.Lerp(x_speed, 0, Time.deltaTime * player_knockbackresistance);
            }
            else if ((!GManager.instance.walktrg || GManager.instance.over!=-1) && anim.GetInteger(number_name) != stand_anim)
            {
                if (fliptime <= 0 && jump_cooltime <= 0 && attime <= 0) audioSource.Stop();
               anim.SetInteger(number_name, stand_anim);
                y_speed = 0;
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            //移動関係
            //重力部分
            if (rb.useGravity)
                rb.useGravity = false;
            x_rotation = 0;
            y_rotation = 0;

            if (GManager.instance.over==-1)
            {
                if (!jump_uptrg && !jump_slowtrg) y_speed = -gravity;
                else if (!jump_uptrg && jump_slowtrg) y_speed = -(gravity / slow_max);
                else if (jump_uptrg && !jump_slowtrg)
                {
                    y_speed = upspeed_max;
                    if (x_speed != 0) y_speed /= 2f;
                    jump_uptime += Time.deltaTime;
                    if (jump_uptime >= jump_maxuptime)
                    {
                        jump_uptime = 0f;
                        jump_uptrg = false;
                        jump_slowtrg = true;
                    }
                }
            }
            
            if (colevent_ground.coltrg && jump_slowtrg) { jump_slowtrg = false; onpost.motiontrg = false; }
            if (colevent_ground.coltrg && Math.Abs(x_speed) <= 0.5f) { x_speed = 0; }
            //if (!colevent_ground.coltrg && jump_uptrg && jump_uptime >= (jump_maxuptime / 3) && (!jumpbtn.push && !Input.GetKey(KeyCode.W) && x_speed <= 0))
            //{
            //    jump_uptrg = false;
            //    jump_slowtrg = true;
            //}
            if (!movetrg&& attime<=0&&fliptime<=0&& !jump_slowtrg && !jump_uptrg && ((rightbtn.push || (Input.GetKey(KeyCode.D) && !npc_ai)) || (leftbtn.push || (Input.GetKey(KeyCode.A) && !npc_ai))))
            {
                //この部分では歩きの効果音、アニメーションを操作
                movetrg = true;
                audioSource.clip = groundse;
                audioSource.loop = true;
                audioSource.Play();
                anim.SetInteger(number_name, walk_anim);
            }
            //移動メイン部分
            float inputX = 0;
            float inputZ = 0;
            //ジャンプ
            if ((jumpbtn.push || (Input.GetKey(KeyCode.W) && !npc_ai) || x_speed != 0) && colevent_ground.coltrg && player_stamina >= 1 && jump_cooltime <= 0 && fliptime <= 0 && anim.GetInteger(number_name) != dash_anim)
            {
                jump_cooltime = 0.3f;
                var tmpscale = character.transform.localScale;
                if (flipspeed < 0) tmpscale.x = -0.6f;
                else if (flipspeed > 0) tmpscale.x = 0.6f;
                character.transform.localScale = tmpscale;
                anim.SetInteger(number_name, jump_anim);
                Instantiate(jumpeffect, transform.position, transform.rotation);
                if (x_speed == 0)
                {
                    player_stamina -= 1;
                    audioSource.PlayOneShot(jumpse);
                }
                onpost.motiontrg = true;
                jump_uptrg = true;
            }
            //ダッシュ
            else if (((dashbtn.push) || (Input.GetKey(KeyCode.E) && !npc_ai)) && fliptime <= 0 && flipcoomtime <= 0 && player_stamina >= 2 && x_speed == 0 && attime <= 0 && !jump_uptrg && !jump_slowtrg)
            {
                fliptime = 0.3f;
                if (GManager.instance.at_and_dash) attime = 0.45f;
                flipcoomtime = 1f;
                player_stamina -= 2;
                anim.SetInteger(number_name, stand_anim);
                audioSource.PlayOneShot(escapese);
                nextframe_dash = true;
                var tmp = transform.position;
                tmp.x += flipspeed;
                Vector3 tmpdiff = tmp - transform.position;
                Quaternion tmpRotation = Quaternion.LookRotation(tmpdiff);
                Instantiate(dasheffect, transform.position, tmpRotation, transform);
                onpost.motiontrg = true;
                var tmpscale = character.transform.localScale;
                if (flipspeed < 0) tmpscale.x = 0.6f;
                else if (flipspeed > 0) tmpscale.x = -0.6f;
                character.transform.localScale = tmpscale;
            }
            //攻撃
            else if (attime <= 0 && ((Input.GetMouseButton(1) && !npc_ai) || (atbtn.push)) && !GManager.instance.at_and_dash && player_stamina >= 2 && x_speed == 0 && !jump_uptrg && !jump_slowtrg)
            {
                attime = 0.5f;
                player_stamina -= 2;
                var tmpscale = character.transform.localScale;
                if (flipspeed < 0) tmpscale.x = -0.6f;
                else if (flipspeed > 0) tmpscale.x = 0.6f;
                character.transform.localScale = tmpscale;
                anim.SetInteger(number_name, at_anim);
                onpost.motiontrg = true;
                audioSource.PlayOneShot(attackse);
            }


            if ((anim.GetInteger(number_name) == stand_anim || anim.GetInteger(number_name) == walk_anim) && !colevent_ground.coltrg)
            {
                anim.SetInteger(number_name, jump_anim); 
                var tmpscale = character.transform.localScale;
                if (flipspeed < 0) tmpscale.x = -0.6f;
                else if (flipspeed > 0) tmpscale.x = 0.6f;
                character.transform.localScale = tmpscale;
            }
            else if (anim.GetInteger(number_name) == jump_anim && colevent_ground.coltrg) anim.SetInteger(number_name, stand_anim);
            if (anim.GetInteger(number_name) == damage_anim && x_speed==0 && colevent_ground.coltrg) anim.SetInteger(number_name, stand_anim);
            if (jump_cooltime >= 0) jump_cooltime -= Time.deltaTime;
            if (flipcoomtime >= 0) flipcoomtime -= Time.deltaTime;
            
            if (x_speed != 0)
            {
                inputX = x_speed;
            }
            else if (fliptime >= 0 && x_speed == 0)
            {
                fliptime -= Time.deltaTime;
                inputX = flipspeed*2;
            }
            else if (fliptime <= 0 && x_speed == 0)
            {
                if (!jump_uptrg && onpost.motiontrg) onpost.motiontrg = false;
                if (fliptime<=0&&(rightbtn.push || (Input.GetKey(KeyCode.D) && !npc_ai)) && (jump_uptrg || jump_slowtrg)) { inputX = 0.7f; flipspeed = -(0.7f * 80); }
                else if ( fliptime <= 0 && (rightbtn.push || (Input.GetKey(KeyCode.D) && !npc_ai))) { inputX = 1; flipspeed = -80; }
                if ( fliptime <= 0 && (leftbtn.push || (Input.GetKey(KeyCode.A) && !npc_ai)) && (jump_uptrg || jump_slowtrg)) { inputX = -0.7f; flipspeed = 0.7f * 80; }
                else if ( fliptime <= 0 && (leftbtn.push || (Input.GetKey(KeyCode.A) && !npc_ai))) { inputX = -1; flipspeed = 80; }

                if (attime > 0 && !GManager.instance.at_and_dash) inputX /= 2;
            }
            if (attime >= 0)
            {
                attime -= Time.deltaTime;
            }
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
            if (diff != Vector3.zero && ((leftbtn.push || (Input.GetKey(KeyCode.A) && !npc_ai)) || (rightbtn.push || (Input.GetKey(KeyCode.D) && !npc_ai)) || (dashbtn.push || (Input.GetKey(KeyCode.E) && !npc_ai))))
            {
                Quaternion targetRotation = Quaternion.LookRotation(diff);
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, Time.deltaTime * kando);
            }
            latest_pos = character.transform.position;  //前回のPositionの更新

            if (movetrg && !(leftbtn.push || Input.GetKey(KeyCode.A)) && !(rightbtn.push || Input.GetKey(KeyCode.D))&&fliptime<=0&&colevent_ground.coltrg)
            {
                movetrg = false;
                var tmpscale = character.transform.localScale;
                if (flipspeed < 0) tmpscale.x = -0.6f;
                else if (flipspeed > 0) tmpscale.x = 0.6f;
                character.transform.localScale = tmpscale;
                if (attime<=0) anim.SetInteger(number_name, stand_anim);
                audioSource.loop = false;
                if (jump_cooltime <= 0 ) audioSource.Stop();
            }
            else if (movetrg&& (jump_uptrg || jump_slowtrg || !colevent_ground.coltrg||fliptime>0)||attime>0)
            {
                movetrg = false;

                if(attime<=0)anim.SetInteger(number_name, stand_anim);
                audioSource.loop = false;
                if (fliptime <= 0 && jump_cooltime <= 0 && attime <= 0) audioSource.Stop();
            }
        }
        if (GManager.instance.over!=-1)
        {
            if (((this.gameObject.name == "Player (1)" && GManager.instance.over == 2) || (this.gameObject.name == "Player" && GManager.instance.over == 1))&& anim.GetInteger("Anumber")!=12) anim.SetInteger("Anumber", 12);
            if (rb.velocity != Vector3.zero) rb.velocity = Vector3.zero;
            
        }
        else if(GManager.instance.over!=-1 && audioSource.isPlaying) audioSource.Stop();
        if (this.gameObject.name == "Player (1)" && GManager.instance.over == 2 && anim.GetInteger(number_name) != jump_anim) { anim.SetInteger(number_name, jump_anim); audioSource.Stop(); jump_uptrg = true; }
    }
    private void OnTriggerStay(Collider col)
    {
        if (GManager.instance.over==-1 &&!capcol.isTrigger && GManager.instance.walktrg && col.tag == "player" && col.gameObject!=colevent_ass.gameObject && attime<=0 && fliptime<=0 && col.gameObject.GetComponent<player>().attime>0&& x_speed==0&&colevent_ass.coltrg)
        {
            var tmp = this.transform.position - col.transform.position;
            player tmpplayer = col.gameObject.GetComponent<player>();
            player_health -= tmpplayer.player_at;

            var tmp2 = tmpplayer.gameObject.transform.position;
            tmp2.x += tmpplayer.flipspeed;
            Vector3 tmpdiff = tmpplayer.gameObject.transform.position- tmp2;
            Quaternion tmpRotation = Quaternion.LookRotation(tmpdiff);
            Instantiate(damageeffect, this.transform.position, tmpRotation, this.transform);

            if (player_health > 0) x_speed = tmp.x * 1600;
            else { x_speed = tmp.x * 8000;
                if (this.gameObject.name == "Player") GManager.instance.over = 1;
                else if (this.gameObject.name == "Player (1)") GManager.instance.over = 2;
               Instantiate(popuiobj, transform.position, transform.rotation); ;
            }
            audioSource.PlayOneShot(damagese);
            anim.SetInteger(number_name, damage_anim);
            Instantiate(damageeffect, transform.position, transform.rotation);
            var tmpscale = character.transform.localScale;
            if (flipspeed < 0) tmpscale.x = -0.6f;
            else if (flipspeed > 0) tmpscale.x = 0.6f;
            character.transform.localScale = tmpscale;
        }
        else if (GManager.instance.over==-1 && GManager.instance.walktrg && col.tag == "player" && col.gameObject != colevent_ass.gameObject && col.gameObject!=this.gameObject &&( col.gameObject.GetComponent<player>().attime > 0 || col.gameObject.GetComponent<player>().fliptime > 0||fliptime>0) &&x_speed==0 && !colevent_ass.coltrg)
        {
            if(rb.constraints!= (RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation)) rb.constraints = RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezePositionZ| RigidbodyConstraints.FreezeRotation;
            if (!capcol.isTrigger) capcol.isTrigger = true;
        }
       
    }
    private void OnTriggerExit(Collider col)
    {
        if (GManager.instance.over==-1 && GManager.instance.walktrg && col.tag == "player" && col.gameObject != colevent_ass.gameObject && col.gameObject != this.gameObject)
        {
            if (capcol.isTrigger) capcol.isTrigger = false;
            if (rb.constraints == (RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation)) rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            
        }
    }

}
