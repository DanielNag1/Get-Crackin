using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXLoop : MonoBehaviour
{
    [SerializeField] private string SoundPath; //sound paths
    [SerializeField] private float volumeScale; //volume scale
    // Update is called once per frame
    void Update()
    {
        if (SoundPath == null)
        {
            return;
        }

        if (!transform.GetComponent<AudioSource>().isPlaying)
        {
            SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPath, 0, Time.fixedTime,volumeScale);
        }
    }
}