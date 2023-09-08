using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KanbanItem : MonoBehaviour
{
    private AudioSource audioSource;
    //エフェクト出現場所
    private float getcooltime = 0f;
    [SerializeField] private GameObject itemeffect;
    //アイテム画像一覧  0=回復
    [System.Serializable]
    public struct ItemList
    {
        public string itemname;
        public Sprite itemsprite;
        public GameObject effectobj;
        public int eventnum;
    }
    public ItemList[] itemList;

    public Sprite nosprite;
    public SpriteRenderer _spriteren;
    private int selectitem = 0;
    private bool counttrg = true;
    public AudioClip selectse;
    public AudioClip getse;
    public player[] pls;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _spriteren.sprite = nosprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (getcooltime >= 0)
        {
            if (_spriteren && _spriteren.sprite!=nosprite) _spriteren.sprite = nosprite;
            getcooltime -= Time.deltaTime;
        }
        else if (_spriteren && _spriteren.sprite == nosprite) _spriteren.sprite = nosprite;
        if(GManager.instance.walktrg && GManager.instance.over == -1&&counttrg&& getcooltime<=0)
        {
            StartCoroutine(TimeSprite());
        }
    }
    private IEnumerator TimeSprite()
    {
            counttrg = false;
        yield return new WaitForSeconds(0.3f);
        if (getcooltime <= 0)
        {
            selectitem += 1;
            if (selectitem >= itemList.Length) selectitem = 0;
            _spriteren.sprite = itemList[selectitem].itemsprite;
            if (audioSource) audioSource.PlayOneShot(selectse);
        }
        counttrg = true;
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.tag == "player" && col.GetComponent<player>() && col.GetComponent<player>().attime > 0 && getcooltime <= 0)
        {
            getcooltime = 5f;
           audioSource.PlayOneShot(getse);
            int tmprandom = UnityEngine.Random.Range(0, itemList.Length);
            GameObject tmpobj = Instantiate(itemeffect, transform.position, itemeffect.transform.rotation);
            if (tmpobj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>())
            {
                tmpobj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = itemList[tmprandom].itemsprite;
                switch (itemList[tmprandom].eventnum)
                {
                    case 0:
                        if(col.GetComponent<player>().player_speed<280) col.GetComponent<player>().player_speed += 25;
                        Instantiate(itemList[tmprandom].effectobj, col.transform.position, col.transform.rotation, col.transform);
                        break;
                    case 1:
                        if (pls[0].gameObject == col.gameObject && pls[1].player_knockbackresistance < 120)
                            pls[1].player_knockbackresistance *= 2;
                        else if(pls[1].gameObject == col.gameObject && pls[0].player_knockbackresistance < 120)
                                pls[0].player_knockbackresistance *= 2;
                        Instantiate(itemList[tmprandom].effectobj, col.transform.position, col.transform.rotation, col.transform);
                        break;
                    case 2:
                        if(col.GetComponent<player>().player_at<5) col.GetComponent<player>().player_at +=1;
                        Instantiate(itemList[tmprandom].effectobj, col.transform.position, col.transform.rotation, col.transform);
                        break;
                }

            }
        }
    }
}
