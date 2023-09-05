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
        public string itemname;
        public Sprite itemsprite;
        public int eventnum;
    }
    public ItemList[] itemList;
    public ItemManager itemmanager=null;
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
                switch (itemList[tmprandom].eventnum)
                {
                    case 0:
                        col.GetComponent<player>().player_health = 5;
                        col.GetComponent<player>().player_stamina = 10;
                        break;
                    case 1:
                        col.GetComponent<player>().effect_jumpspeedup = 1.5f;
                        if (col.name == "Player") { itemmanager.player0_effectui[0].SetBool("Abool", false); itemmanager.player0_effectui[0].gameObject.SetActive(true); }
                        else if (col.name == "Player (1)") { itemmanager.player1_effectui[0].SetBool("Abool", false); itemmanager.player1_effectui[0].gameObject.SetActive(true); }
                        break;
                    case 2:
                        col.GetComponent<player>().effect_dummytrg = true; ;
                        if (col.name == "Player") { itemmanager.player0_effectui[1].SetBool("Abool", false); itemmanager.player0_effectui[1].gameObject.SetActive(true); }
                        else if (col.name == "Player (1)") { itemmanager.player1_effectui[1].SetBool("Abool", false); itemmanager.player1_effectui[1].gameObject.SetActive(true); }
                        break;
                    case 3:
                        col.GetComponent<player>().effect_powerup = 2f;
                        if (col.name == "Player") { itemmanager.player0_effectui[2].SetBool("Abool", false); itemmanager.player0_effectui[2].gameObject.SetActive(true); }
                        else if (col.name == "Player (1)") { itemmanager.player1_effectui[2].SetBool("Abool", false); itemmanager.player1_effectui[2].gameObject.SetActive(true); }
                        break;
                }

                Destroy(gameObject, 0.1f);
            }

            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
