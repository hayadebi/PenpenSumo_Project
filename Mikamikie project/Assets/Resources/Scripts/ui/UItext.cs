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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start_time > 0)start_time -= Time.deltaTime ;
        if (old_time - start_time >= 0.5)
        {
            old_time = (int)Mathf.Floor(start_time);
            _text.text = Mathf.Floor((old_time / 60)).ToString() + "：" + Mathf.Floor((old_time % 60)).ToString();
        }
    }
}
