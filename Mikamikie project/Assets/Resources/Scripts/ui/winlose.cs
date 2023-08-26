using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class winlose : MonoBehaviour
{
    [SerializeField] private Color wincolor;
    [SerializeField] private Color losecolor;
    [SerializeField] private int losenum = 1;
    private Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        if (GManager.instance.over == losenum) { _text.color = losecolor;_text.text = "Lose"; }
        else {_text.color = wincolor; _text.text = "Win"; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
