using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popui : MonoBehaviour
{
    private bool checktrg = false;
    [SerializeField] private GameObject uiobj;
    [SerializeField] private int setrg = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider col)
    {
        if (!checktrg && col.tag == "player")
        {
            checktrg = true;
            GManager.instance.walktrg = false;
            if(col.gameObject.name=="Player")GManager.instance.over = 1;
            else if (col.gameObject.name == "Player (1)") GManager.instance.over = 2;
            GManager.instance.setrg = 1;
            Instantiate(uiobj, transform.position, transform.rotation);
        }
    }
}
