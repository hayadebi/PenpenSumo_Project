using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private GameObject chestobj;
    [SerializeField] private float max_time = 15f;
    private GameObject onchest = null;
    private float summon_chesttime = 0f;
    public Transform[] area1;
    public Transform[] area2;
    private int areatype = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GManager.instance.walktrg && GManager.instance.over == -1 && !onchest)
        {
            summon_chesttime += Time.deltaTime;
            if (summon_chesttime >= max_time)
            {
                summon_chesttime = 0;
                areatype = Random.Range(1, 3);
                if (areatype == 1)
                {
                    var tmp = area1[0].position;
                    tmp.x = Random.Range(area1[0].position.x, area1[1].position.x);
                    onchest = Instantiate(chestobj, tmp, chestobj.transform.rotation);
                    GManager.instance.setrg = 5;
                }
                else if (areatype == 2)
                {
                    var tmp = area2[0].position;
                    tmp.x = Random.Range(area2[0].position.x, area2[1].position.x);
                    onchest = Instantiate(chestobj, tmp, chestobj.transform.rotation);
                    GManager.instance.setrg = 5;
                }
            }
        }
    }
}
