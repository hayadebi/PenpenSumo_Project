using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestItem : MonoBehaviour
{
    //着地時エフェクト
    [SerializeField] private GameObject groundeffect;
    //エフェクト出現場所
    [SerializeField] private Transform effect_pos;
    [SerializeField] private AudioClip ground_se;
    private AudioSource audioSource;
    private bool first_ground = false;
    private bool destroy_chest = false;
    private Rigidbody rb;
    [SerializeField] private GameObject destroyeffect;
    [SerializeField] private GameObject itemeffect;
    //アイテム画像一覧  0=回復
    [System.Serializable]
    public struct ItemList
    {
        public Sprite itemsprite;
        public int eventnum;
    }
    public ItemList[] itemList;
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

            Instantiate(groundeffect, effect_pos.position, groundeffect.transform.rotation);
            audioSource.PlayOneShot(ground_se);
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "player" && col.GetComponent<player>() && col.GetComponent<player>().attime > 0 && !destroy_chest)
        {
            destroy_chest = true;
            Instantiate(destroyeffect, effect_pos.position, groundeffect.transform.rotation);
            int tmprandom = Random.Range(0, itemList.Length);
            GameObject tmpobj = Instantiate(itemeffect, transform.position, itemeffect.transform.rotation);
            if (tmpobj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>())
            {
                tmpobj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = itemList[tmprandom].itemsprite;
                if (itemList[tmprandom].eventnum == 0) col.GetComponent<player>().player_health += 2;
                if (col.GetComponent<player>().player_health > 5) col.GetComponent<player>().player_health = 5;
                Destroy(gameObject, 0.1f);
            }

            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
