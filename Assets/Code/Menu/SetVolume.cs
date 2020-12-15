using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetVolume : MonoBehaviour
{
    public Slider slider;
    public void SetLevel(float slidervalue)
    {
        SoundEngine.Instance.VolumeSet = slidervalue;
        Debug.Log(slider.value.ToString());
    }
}
