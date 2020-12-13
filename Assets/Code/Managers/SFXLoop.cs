using UnityEngine;

public class SFXLoop : MonoBehaviour
{
    [SerializeField] private string _soundPath; //sound paths
    [SerializeField] private float _volumeScale; //volume scale

    private void Update()
    {
        if (_soundPath == null)
        {
            return;
        }

        if (!transform.GetComponent<AudioSource>().isPlaying)
        {
            SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPath, 0, Time.fixedTime,_volumeScale);
        }
    }
}