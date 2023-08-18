using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despown : MonoBehaviour
{
    public float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Tds", time);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Tds()
    {
        Destroy(this.gameObject, 0.1f);

    }
}
