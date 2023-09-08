using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_level : MonoBehaviour
{
    [SerializeField] private longbuttonclick rightbtn;
    [SerializeField] private longbuttonclick leftbtn;
   public float return_flip = 1f;
    private GameObject npc=null;
    private GameObject player = null;
    public float cooltime = 2f;
    private float dashcooltime;
    private float jumpcooltime;
    public ColEvent colev;
    public player pl;
    private player enemy;
    private float tmpcooltime = 2;
    // Start is called before the first frame update
    void Start()
    {
        npc = GameObject.Find("Player (1)");
        player = GameObject.Find("Player");
        enemy = player.GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.instance.walktrg && GManager.instance.over == -1 && cooltime<=0)
        {
            if (GManager.instance.difficulty_mode == 0)
            {
                if (npc && player && !rightbtn.push && ForwardCheckRight(player) && !((rightbtn.push || leftbtn.push) && Mathf.Abs(pl.character.position.x - player.transform.position.x) <= 10f))
                {
                    rightbtn.push = true;
                    leftbtn.push = false;
                    tmpcooltime = 2;
                    Invoke(nameof(CoolTimeSet), 2f);
                }
                else if (npc && player && !leftbtn.push && ForwardCheckLeft(player) && !((rightbtn.push || leftbtn.push) && Mathf.Abs(pl.character.position.x - player.transform.position.x) <= 10f))
                {
                    rightbtn.push = false;
                    leftbtn.push = true;
                    tmpcooltime = 2;
                    Invoke(nameof(CoolTimeSet), 2f);
                }
                else if ((rightbtn.push || leftbtn.push) && ((Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 10f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 10f) || (Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 40f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) > 10f)))
                {
                    cooltime = 1f;
                    if (rightbtn.push) rightbtn.push = false;
                    else if (leftbtn.push) leftbtn.push = false;
                }
                if (cooltime <= 0 && Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 30f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 10f)
                {
                    cooltime = 5f;
                    StopMove();
                    pl.PlayerAttack();
                }
            }
            else if (GManager.instance.difficulty_mode == 1)
            {
                if (npc && player && !rightbtn.push && ForwardCheckRight(player) && !((rightbtn.push || leftbtn.push) && Mathf.Abs(pl.character.position.x - player.transform.position.x) <= 10f))
                {
                    rightbtn.push = true;
                    leftbtn.push = false;
                    tmpcooltime = 1.3f;
                    Invoke(nameof(CoolTimeSet), 7f);
                }
                else if (npc && player && !leftbtn.push && ForwardCheckLeft(player) && !((rightbtn.push || leftbtn.push) && Mathf.Abs(pl.character.position.x - player.transform.position.x) <= 10f))
                {
                    rightbtn.push = false;
                    leftbtn.push = true;
                    tmpcooltime = 1.3f;
                    Invoke(nameof(CoolTimeSet), 7f);
                }
               
                if ( pl.attime<=0&&dashcooltime <= 0 && Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 120f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 40f && enemy.attime>0)
                {
                    dashcooltime = 3;
                    pl.x_speed = 0;
                    StopMove();
                    pl.PlayerDash();
                }
                else if (pl.fliptime <= 0&&cooltime <= 0 && Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 25f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 15f)
                {
                    cooltime = 4f;
                    StopMove();
                    pl.PlayerAttack();
                }
                else if ((rightbtn.push || leftbtn.push) && ((Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 10f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 20f) || (Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 30f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) > 20f)))
                {
                    cooltime = 0.4f;
                    if (rightbtn.push) rightbtn.push = false;
                    else if (leftbtn.push) leftbtn.push = false;
                }
            }
            else if (GManager.instance.difficulty_mode == 2)
            {
                if (npc && player && !rightbtn.push && ForwardCheckRight(player) && !((rightbtn.push || leftbtn.push) && Mathf.Abs(pl.character.position.x - player.transform.position.x) <= 10f))
                {
                    rightbtn.push = true;
                    leftbtn.push = false;
                    tmpcooltime = 1;
                    Invoke(nameof(CoolTimeSet), 14f);
                }
                else if (npc && player && !leftbtn.push && ForwardCheckLeft(player) && !((rightbtn.push || leftbtn.push) && Mathf.Abs(pl.character.position.x - player.transform.position.x) <= 10f))
                {
                    rightbtn.push = false;
                    leftbtn.push = true;
                    tmpcooltime = 1;
                    Invoke(nameof(CoolTimeSet), 14f);
                }
                
                if (pl.attime<=0&& dashcooltime <= 0 && Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 120f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 40f && enemy.attime > 0)
                {
                    dashcooltime = 2;
                    pl.x_speed = 0;
                    StopMove();
                    pl.PlayerDash();
                }
                else if (pl.attime<=0&&pl.fliptime<=0&&jumpcooltime <= 0 && Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 65f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) >= 15f)
                {
                    jumpcooltime = 0.3f;
                    StopMove();
                    pl.PlayerJump();
                }
                else if (pl.fliptime<=0&&cooltime <= 0 && Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 25f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 15f)
                {
                    cooltime = 3f;
                    StopMove();
                    pl.PlayerAttack();
                }
                else if (dashcooltime <= 0 && jumpcooltime <= 0 && (rightbtn.push || leftbtn.push) && ((Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 10f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) <= 20f) || (Mathf.Abs(pl.gameObject.transform.position.x - player.transform.position.x) <= 30f && Mathf.Abs(pl.gameObject.transform.position.y - player.transform.position.y) > 20f)))
                {
                    cooltime = 0.3f;
                    if (rightbtn.push) rightbtn.push = false;
                    else if (leftbtn.push) leftbtn.push = false;
                }

            }
        }

        if (cooltime >= 0) cooltime -= Time.deltaTime;
        if (dashcooltime >= 0) dashcooltime -= Time.deltaTime;
        if (jumpcooltime >= 0) jumpcooltime -= Time.deltaTime;
    }
    void CoolTimeSet()
    {
        cooltime = tmpcooltime;
        StopMove();
    }
    private void StopMove()
    {
        if (rightbtn.push) rightbtn.push = false;
        else if (leftbtn.push) leftbtn.push = false;
        if (!pl.ForwardCheck(enemy.gameObject.GetComponent<Collider>())) pl.PlayerDash();
    }

    private bool ForwardCheckRight(GameObject col)
    {
        if (pl.character.position.x < col.transform.position.x) return true;
        return false;
    }
    private bool ForwardCheckLeft(GameObject col)
    {
        if (pl.character.position.x > col.transform.position.x)return true;
        return false;
    }

}
