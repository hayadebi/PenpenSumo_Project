using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headground : MonoBehaviour
{
    private BoxCollider boxcol;
    // Start is called before the first frame update
    void Start()
    {
        boxcol = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "head" && !boxcol.isTrigger) boxcol.isTrigger = true;
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "pground" && boxcol.isTrigger) boxcol.isTrigger = false;
    }
}
