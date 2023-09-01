using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useG : MonoBehaviour
{
    public player ps = null;
    Rigidbody rb;
    public float gravity = 0.08f;
    private float addY = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GManager.instance.walktrg == true )
        {
            if (ps == null)
            {
                addY = 0;
                addY -= gravity;
                rb.AddForce(0, addY, 0);
            }
            else if (ps != null )
            {
                addY = 0;
                addY -= gravity;
                rb.AddForce(0, addY, 0);
            }
        }
        
    }
}
