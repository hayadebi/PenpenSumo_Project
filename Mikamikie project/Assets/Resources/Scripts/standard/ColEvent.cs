using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColEvent : MonoBehaviour
{
    [SerializeField]private string[] target_tag;
    public bool coltrg = false;
    [SerializeField] private int event_num = -1;//イベント
    [SerializeField] private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        ;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (event_num==0&&col.tag == "head")
        {
            rb.isKinematic = true;
            rb.gameObject.tag = "no";
        }
        if (event_num == 0 && col.tag == "pground")
        {
            rb.isKinematic = false;
            rb.gameObject.tag = "ground";
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (!coltrg && TargetCheck(col)) coltrg = true;
        
    }
    private void OnTriggerExit(Collider col)
    {
        if (coltrg && TargetCheck(col)) coltrg = false;
    }
    private bool TargetCheck(Collider tmp)
    {
        for(int i = 0; i < target_tag.Length;)
        {
            if (target_tag[i] == tmp.gameObject.tag) return true;
            i++;
        }
        return false;
    }
}
