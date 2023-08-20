using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;//Volumeを使うのに必要
public class onPostSc : MonoBehaviour
{
    [SerializeField]private PostProcessVolume ppv;
    private ChromaticAberration _effect;
    public bool motiontrg = false;
    private float animspeed = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        foreach (PostProcessEffectSettings item in ppv.profile.settings)
        {
            if (item as ChromaticAberration)
            {
                _effect = item as ChromaticAberration;
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!motiontrg && _effect.intensity.value>0)
        {
            _effect.intensity.value -= (Time.deltaTime * animspeed);
        }
        else if(motiontrg && _effect.intensity.value != 1)
        {
            _effect.intensity.value =1;
        }
    }
}
