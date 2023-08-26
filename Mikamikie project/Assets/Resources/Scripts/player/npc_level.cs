using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_level : MonoBehaviour
{
    [SerializeField] private longbuttonclick rightbtn;
    [SerializeField] private longbuttonclick leftbtn;
    [SerializeField] private longbuttonclick jumpbtn;
    [SerializeField] private longbuttonclick atbtn;
    [SerializeField] private longbuttonclick dashbtn;
    private float return_flip = 1f;
    private GameObject width_maxx=null;
    private GameObject npc=null;
    private float cooltime = 0f;
    private float movetime = 0f;
    private float attime = 0f;
    public ColEvent colev;
    public player pl;
    // Start is called before the first frame update
    void Start()
    {
        width_maxx = GameObject.Find(nameof(width_maxx));
        npc = GameObject.Find("Player (1)");
    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.instance.walktrg && GManager.instance.over == -1)
        {
            if (GManager.instance.difficulty_mode == 0)
            {
                if (cooltime >= 0) cooltime -= Time.deltaTime;
                if (attime > 0) { attime -= Time.deltaTime; if (!atbtn.push) atbtn.push = true; }
                else if (attime<=0 && atbtn.push) atbtn.push = false;
                if (movetime >= 3f)
                {
                    movetime = 0f;
                    cooltime = 1.3f;
                    StopMove();
                }
                if (width_maxx && npc && width_maxx.transform.position.x <= npc.transform.position.x && return_flip > 0) 
                { return_flip *= -1; pl.rb.velocity = Vector3.zero; }
                else if (width_maxx && npc && -width_maxx.transform.position.x >= npc.transform.position.x && return_flip < 0) 
                { return_flip *= -1; pl.rb.velocity = Vector3.zero; }
                if (width_maxx && npc && width_maxx.transform.position.x <= npc.transform.position.x && pl.rb.velocity != Vector3.zero && (attime>0||cooltime>0)) 
                { pl.x_speed = 0;pl.flipspeed = 0; pl.rb.velocity = Vector3.zero; }
                else if (width_maxx && npc && -width_maxx.transform.position.x >= npc.transform.position.x && pl.rb.velocity != Vector3.zero&&(attime>0||cooltime>0)) 
                { pl.x_speed = 0; pl.flipspeed = 0; pl.rb.velocity = Vector3.zero; }
                if (cooltime <= 0)
                {
                    if (attime <= 0)
                    {
                        

                        if (return_flip > 0 && !rightbtn.push) rightbtn.push = true;
                        else if (return_flip < 0 && rightbtn.push) rightbtn.push = false;

                        if (return_flip > 0 && leftbtn.push) leftbtn.push = false;
                        else if (return_flip < 0 && !leftbtn.push) leftbtn.push = true;
                    }
                    else
                    {
                        StopMove();
                    }
                }
                //if (cooltime <= 0 && colev.coltrg)
                //{
                //    cooltime = 1.3f;
                //    attime = 0.3f;
                //}
            }
            movetime += Time.deltaTime;
        }
        else StopMove();
    }
    private void StopMove()
    {
        if (rightbtn.push) rightbtn.push = false;
        else if (leftbtn.push) leftbtn.push = false;
        else if (jumpbtn.push) jumpbtn.push = false;
        else if (dashbtn.push) dashbtn.push = false;
    }
    
}
