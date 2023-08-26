using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderManager : MonoBehaviour
{
    [SerializeField] private int _event = 0;
    private Slider _slider;
    [SerializeField] private string player_name = "Player";
    private player _player;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        if(_event==0 || _event == 1) Invoke(nameof(GetPlayer),1f);
    }
    private void GetPlayer()
    {
        GameObject main_player = GameObject.Find(player_name);
        if(main_player!=null) _player = main_player.GetComponent<player>();
        if (_player&&_slider)
        {
            if(_event == 0) _slider.value = _player.player_health;
            if (_event == 1) _slider.value = _player.player_stamina;
        }
    }
    private void Update()
    {
            if (_event == 0 && _player && _slider && (int)_slider.value != _player.player_health)
            {
                _slider.value = _player.player_health;
                iTween.ShakePosition(_slider.gameObject, iTween.Hash("x", 3f, "y", 3f, "time", 0.5f));
            }
            else if (_event == 1 && _player && _slider && (int)_slider.value != _player.player_stamina)
            {
                _slider.value = _player.player_stamina;
                iTween.ShakePosition(_slider.gameObject, iTween.Hash("x", 2.5f, "y", 2.5f, "time", 0.4f));
            }
    }
}
