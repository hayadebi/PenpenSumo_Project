using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] se;
    AudioSource audioS;
    public string targetremove_scene="null";
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        GManager.instance.setmenu = 0;
        GManager.instance.over = -1;
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown (KeyCode.Delete))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        else if( GManager.instance.setrg != -1 && GManager.instance.setrg != 99)
        {
            if(SceneManager.GetActiveScene().name== targetremove_scene) audioS.Stop();
            audioS.PlayOneShot(se[GManager.instance.setrg]);
            GManager.instance.setrg = -1;
        }
    }

}
