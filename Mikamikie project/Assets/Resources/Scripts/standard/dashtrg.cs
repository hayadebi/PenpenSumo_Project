using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashtrg : MonoBehaviour
{
    //対象とするオブジェクト
    [SerializeField]
    private GameObject target_obj;
    //at_and_dashと同じ状態かをチェックするのに使用。
    [SerializeField]private bool set_target = false;
    private bool old_check = false;

    // Start is called before the first frame update
    void Start()
    {
    //    if (GManager.instance.at_and_dash==set_target) target_obj.SetActive(false);
    //    else if (GManager.instance.at_and_dash != set_target) target_obj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //if (GManager.instance.at_and_dash != old_check)
        //{
        //    old_check = GManager.instance.at_and_dash;
        //    if (GManager.instance.at_and_dash == set_target) target_obj.SetActive(false);
        //    else if (GManager.instance.at_and_dash != set_target) target_obj.SetActive(true);
        //} 
    }
}
