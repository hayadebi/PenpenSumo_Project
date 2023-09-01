using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    //着地時エフェクト
    [SerializeField] private GameObject effect;
    //エフェクト出現場所
    [SerializeField] private Transform effect_pos;
    [SerializeField] private AudioClip ground_se;
    private AudioSource audioSource;
    private bool first_ground = false;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag=="ground" && !first_ground)
        {
            first_ground = true;

            Instantiate(effect, effect_pos.position, effect.transform.rotation);
            audioSource.PlayOneShot(ground_se);
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
