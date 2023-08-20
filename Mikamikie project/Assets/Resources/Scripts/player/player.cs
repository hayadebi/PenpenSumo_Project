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
    [SerializeField] private AudioClip escapese;
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
    [SerializeField] private float cmlimit_width = 140;
    [SerializeField] private float cmlimit_up = 83;
    [SerializeField] private float cmlimit_down = -22;
    //操作ボタンに関する
    private longbuttonclick rightbtn;
    private longbuttonclick leftbtn;
    private longbuttonclick jumpbtn;
    private longbuttonclick dashbtn;

    //視点回転に関する
    //private float maxYAngle = 135f;
    //private float minYAngle = 45f;
    private Vector3 latest_pos;  //前回のPosition
    //反転感度
    public float kando = 90;
    //地面判定 ジャンプ関係
    private bool jump_uptrg = false;
    private bool jump_slowtrg = false;
    public float jump_maxuptime = 0.3f;//上昇時間
    private float jump_uptime = 0f;
    public float slow_max = 3f;
    public float upspeed_max = 16;
    public ColEvent colevent_ground;
    public ColEvent colevent_head;
    //回避
    private float flipspeed = -170;
    private float fliptime = 0f;
    private float flipcoomtime = 0f;
    private onPostSc onpost;
    [SerializeField] private GameObject dasheffect;
    //スタミナ回復
    private float stamina_time = 0f;
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
        cm_vec = cm.transform.position - character.position;

        rightbtn = GameObject.Find(nameof(rightbtn)).GetComponent<longbuttonclick>();
        leftbtn = GameObject.Find(nameof(leftbtn)).GetComponent<longbuttonclick>();
        jumpbtn = GameObject.Find(nameof(jumpbtn)).GetComponent<longbuttonclick>();
        dashbtn = GameObject.Find(nameof(dashbtn)).GetComponent<longbuttonclick>();
        onpost = GameObject.Find("main_PostVolume").GetComponent<onPostSc>();
    }
    void StartSet()
    {
        
    }

    void FixedUpdate()
    {
        if (GManager.instance.walktrg && !GManager.instance.over && !stoptrg && (rightbtn && leftbtn))
        {
            //スタミナ回復
            if (player_stamina < 10)
            {
                stamina_time += Time.deltaTime;
                if (stamina_time >= 2f)
                {
                    stamina_time = 0f;
                    player_stamina += 1;
                }
            }
            //カメラ部分
            if(cm.transform.position != character.position+cm_vec)
            {
                var tmp = character.position;
                if (tmp.x > cmlimit_width) tmp.x = cmlimit_width;
                else if (tmp.x < -cmlimit_width) tmp.x = -cmlimit_width;
                if (tmp.y > cmlimit_up) tmp.y = cmlimit_up;
                if (tmp.y < cmlimit_down) tmp.y = cmlimit_down;
                cm.transform.position =Vector3.Slerp(cm.transform.position, tmp + cm_vec, Time.deltaTime * 3);
            }
            //重力部分
            if (rb.useGravity)
                rb.useGravity = false;
            x_rotation = 0;
            y_rotation = 0;
            x_speed = 0;
            if (!jump_uptrg && !jump_slowtrg) y_speed = -gravity;
            else if (!jump_uptrg && jump_slowtrg) y_speed = -(gravity / slow_max);
            else if (jump_uptrg && !jump_slowtrg)
            {
                y_speed = upspeed_max;
                jump_uptime += Time.deltaTime;
                if (jump_uptime >= jump_maxuptime)
                {
                    jump_uptime = 0f;
                    jump_uptrg = false;
                    jump_slowtrg = true;
                }
            };
            if (colevent_ground.coltrg && jump_slowtrg) { jump_slowtrg = false; onpost.motiontrg = false; }
            if (!colevent_ground.coltrg && jump_uptrg && ((!jumpbtn.push && !Input.GetKey(KeyCode.W)) || colevent_head.coltrg))
            {
                jump_uptrg = false;
                jump_slowtrg = true;
            }
            //----ここからは移動----
            if (!movetrg && fliptime<=0&& !jump_slowtrg && !jump_uptrg && ((rightbtn.push || Input.GetKey(KeyCode.D)) || (leftbtn.push || Input.GetKey(KeyCode.A))))
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
            if ((jumpbtn.push || Input.GetKey(KeyCode.W)) && colevent_ground.coltrg)
            {
                if(!audioSource.isPlaying) player_stamina -= 1;
                if (!audioSource.isPlaying) audioSource.PlayOneShot(jumpse);
                onpost.motiontrg = true;
                jump_uptrg = true;
            }
            if (flipcoomtime >= 0) flipcoomtime -= Time.deltaTime;
            if ((dashbtn.push || Input.GetMouseButton(1)) && fliptime <= 0&&flipcoomtime<=0)
            {
                fliptime = 0.25f;
                flipcoomtime = 1f;
                player_stamina -= 2;
                audioSource.PlayOneShot(escapese);
                Instantiate(dasheffect, transform.position, transform.rotation,transform);
                onpost.motiontrg = true;
            }
            if(fliptime>=0)
            {
                fliptime -= Time.deltaTime;
                inputX = flipspeed;
            }
            else if (fliptime <= 0)
            {
                if(!jump_uptrg&&onpost.motiontrg) onpost.motiontrg = false;
                if ((rightbtn.push || Input.GetKey(KeyCode.D)) && (jump_uptrg || jump_slowtrg)) { inputX = 0.7f; flipspeed = -(0.7f * 20); }
                else if ((rightbtn.push || Input.GetKey(KeyCode.D))) { inputX = 1; flipspeed = -20; }
                if ((leftbtn.push || Input.GetKey(KeyCode.A)) && (jump_uptrg || jump_slowtrg)) { inputX = -0.7f; flipspeed = 0.7f * 20; }
                else if ((leftbtn.push || Input.GetKey(KeyCode.A))) { inputX = -1; flipspeed = 20; }
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
            if (diff != Vector3.zero && ((leftbtn.push || Input.GetKey(KeyCode.A)) || (rightbtn.push || Input.GetKey(KeyCode.D))||(dashbtn.push || Input.GetMouseButton(1))))
            {
                Quaternion targetRotation = Quaternion.LookRotation(diff);
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, targetRotation, Time.deltaTime * kando);
            }
            latest_pos = character.transform.position;  //前回のPositionの更新

            if (movetrg && !(leftbtn.push || Input.GetKey(KeyCode.A)) && !(rightbtn.push || Input.GetKey(KeyCode.D)))
            {
                movetrg = false;
                anim.SetInteger(number_name, stand_anim);
                audioSource.loop = false;
                if(fliptime<=0)audioSource.Stop();
            }
            else if (movetrg && (jump_uptrg || jump_slowtrg || !colevent_ground.coltrg))
            {
                movetrg = false;
                anim.SetInteger(number_name, stand_anim);
                audioSource.loop = false;
                if (fliptime <= 0) audioSource.Stop();
            }
        }
        else if ((!GManager.instance.walktrg || GManager.instance.over) && anim.GetInteger(number_name) != stand_anim)
        {
            if (fliptime <= 0) audioSource.Stop();
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
            if (col.tag == "red" && fliptime<=0)
            {
                //GManager.instance.setrg = -1;　ダメージの効果音 あとで指定
                //ダメージ処理は後ほど
            }
        }
    }
    private void OnTriggerExit(Collider col)
    {
        ;  
    }

}
