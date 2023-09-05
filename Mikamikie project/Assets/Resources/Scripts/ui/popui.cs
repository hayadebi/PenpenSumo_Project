using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popui : MonoBehaviour
{
    private bool checktrg = false;
    [SerializeField] private GameObject uiobj;
    [SerializeField] private int setrg = 1;
    private player player0sc;
    private player player1sc;
    // Start is called before the first frame update
    void Start()
    {
        player0sc = GameObject.Find("Player").GetComponent<player>();
        player1sc = GameObject.Find("Player (1)").GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider col)
    {
        if (!checktrg && col.tag == "player")
        {
            if ((col.gameObject == player0sc.gameObject && player0sc.effect_dummytrg) || (col.gameObject == player1sc.gameObject && player1sc.effect_dummytrg))
            {
                if (col.gameObject == player0sc.gameObject && player0sc.effect_dummytrg) player0sc.DummyItem(player0sc.gameObject, player1sc.gameObject);
                else if (col.gameObject == player1sc.gameObject && player1sc.effect_dummytrg) player1sc.DummyItem(player1sc.gameObject, player0sc.gameObject);
            }
            else
            {
                checktrg = true;
                GManager.instance.walktrg = false;
                if (col.gameObject.name == "Player") GManager.instance.over = 1;
                else if (col.gameObject.name == "Player (1)") GManager.instance.over = 2;
                GManager.instance.setrg = 1;
                Instantiate(uiobj, transform.position, transform.rotation);
            }
        }
    }
}
