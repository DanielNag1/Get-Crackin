using UnityEngine;
using UnityEngine.UI;

public class SoundOptionsListener : MonoBehaviour
{
    public Slider mainSlider;
    public void MasterVolumeSliderChanged()
    {
        Debug.Log(mainSlider.value + "sliderVal");
        SoundEngine.Instance.SetMasterVolume = mainSlider.value;
    }
}
