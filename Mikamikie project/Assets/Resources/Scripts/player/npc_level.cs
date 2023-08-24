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
    private float return_flip = -1f;
    private GameObject width_maxx=null;
    private GameObject npc=null;
    // Start is called before the first frame update
    void Start()
    {
        width_maxx = GameObject.Find(nameof(width_maxx));
        npc = GameObject.Find("Player (1)");
    }

    // Update is called once per frame
    void Update()
    {
        if (GManager.instance.difficulty_mode == 0)
        {
            if (width_maxx && npc && width_maxx.transform.position.x <= npc.transform.position.x && return_flip > 0) return_flip *= -1;
            else if (width_maxx && npc && -width_maxx.transform.position.x >= npc.transform.position.x && return_flip < 0) return_flip *= -1;

            if (return_flip > 0 && !rightbtn.push) rightbtn.push = true;
            else if (return_flip < 0 && rightbtn.push) rightbtn.push = false;

            if (return_flip > 0 && leftbtn.push) leftbtn.push = false;
            else if (return_flip < 0 && !leftbtn.push) leftbtn.push = true;
        }
    }
}
