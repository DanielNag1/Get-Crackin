using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetVolume : MonoBehaviour
{

    SoundComponent sound = new SoundComponent();
    
    public Slider slider;
    // Start is called before the first frame update
    public void SetLevel(float slidervalue)

    {
        Debug.Log(slider.value.ToString());
        sound.VolumeSet = slidervalue;

    }
}
