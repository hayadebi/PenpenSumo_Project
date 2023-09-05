using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headground : MonoBehaviour
{
    private BoxCollider boxcol;
    private float noheadtime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        boxcol = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boxcol.isTrigger)
        {
            noheadtime += Time.deltaTime;
            if (noheadtime >= 1f)
            {
                noheadtime = 0;
                boxcol.isTrigger = false;
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "head" && !boxcol.isTrigger) { boxcol.isTrigger = true; noheadtime = 0; }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "pground" && boxcol.isTrigger) boxcol.isTrigger = false;
    }
}
