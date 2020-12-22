using System.Collections.Generic;
using UnityEngine;

public class SoundLooper : MonoBehaviour
{
    [SerializeField] private List<string> _soundPaths; //sound paths
    [SerializeField] private float _volumeScale = 1; //volume scale
    private int _currentSong = 0;

    private void Update()
    {
        if (_soundPaths == null || _soundPaths.Count <= 0)
        {
            return;
        }
        
        if (!transform.GetComponent<AudioSource>().isPlaying)
        {
            SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[_currentSong], 0, Time.fixedTime,_volumeScale);
            ++_currentSong;
            if (_currentSong >= _soundPaths.Count)
            {
                _currentSong = 0;
            }
        }
    }
}
