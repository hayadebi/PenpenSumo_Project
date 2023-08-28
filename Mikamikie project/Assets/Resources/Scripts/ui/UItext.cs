using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UItext : MonoBehaviour
{
    public string mode = "timer";
    public float start_time = 300;
    private int old_time = 300;
    public Text _text;
    public player playeruser;
    public player playernpc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == "timer")
        {
            if (start_time > 0 && GManager.instance.walktrg && GManager.instance.over == -1) start_time -= Time.deltaTime;
            if (old_time - start_time >= 0.5)
            {
                old_time = (int)Mathf.Floor(start_time);
                _text.text = Mathf.Floor((old_time / 60)).ToString() + "：" + Mathf.Floor((old_time % 60)).ToString();
            }
            if (start_time <= 0)
            {
                if (playeruser.player_health<playernpc.player_health) GManager.instance.over = 1;
                else if (playeruser.player_health >= playernpc.player_health) GManager.instance.over = 2;
                Instantiate(playeruser.popuiobj, transform.position, transform.rotation);
            }
        }
    }
}
