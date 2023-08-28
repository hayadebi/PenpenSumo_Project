using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class over_cmmove : MonoBehaviour
{
    [SerializeField] private Transform endcmpos;
    [SerializeField] private float movespeed=2;
    [SerializeField] Camera cmmain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GManager.instance.over != -1)
        {
            if (endcmpos.position != transform.position) transform.position = Vector3.Slerp(transform.position, endcmpos.position , Time.deltaTime * movespeed);
            if (cmmain.orthographicSize>60)cmmain.orthographicSize = Mathf.Lerp(cmmain.orthographicSize, 60, Time.deltaTime * movespeed);
        }
    }
}
