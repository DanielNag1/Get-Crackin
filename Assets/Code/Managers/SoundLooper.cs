using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLooper : MonoBehaviour
{
    [SerializeField] private List<string> SoundPaths; //sound paths
    [SerializeField] private float volumeScale = 1; //volume scale
    int currentSong = 0;

    // Update is called once per frame
    void Update()
    {
        if (SoundPaths == null || SoundPaths.Count <= 0)
        {
            return;
        }
        

        if (!transform.GetComponent<AudioSource>().isPlaying)
        {
            SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPaths[currentSong], 0, Time.fixedTime,volumeScale);
            ++currentSong;
            if (currentSong >= SoundPaths.Count)
            {
                currentSong = 0;
            }
            
        }
    }
}
