using UnityEngine;
using System.Collections;
using UnityEngine.UI; // ←※これを忘れずに入れる

public class slider : MonoBehaviour
{
    public string sliderType = "";
    Slider _slider;
    void Start()
    {
        // スライダーを取得する
        _slider = this.GetComponent<Slider>();
        GManager.instance.audioMax = PlayerPrefs.GetFloat("audioMax", GManager.instance.audioMax);
        GManager.instance.seMax = PlayerPrefs.GetFloat("seMax", GManager.instance.seMax);
        if (sliderType == "audio")
        {
            _slider.value = GManager.instance.audioMax;
        }
        else if (sliderType == "se")
        {
            _slider.value = GManager.instance.seMax;
        }
    }

    void Update()
    {
        ;

    }
    public void ChangedVolume()
    {
        if (sliderType == "audio")
        {
            GManager.instance.audioMax = _slider.value;
        }
        else if (sliderType == "se")
        {
            GManager.instance.seMax = _slider.value;
        }
    }
}