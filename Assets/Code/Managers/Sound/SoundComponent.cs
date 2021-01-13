using System.Collections;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    public string soundPath; //sound paths
    public float volumeScale = 1; //volume scale
    public bool looper = false;

    #region singleton
    public static SoundComponent Instance { get; private set; }
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //If the soundpath is null we should not continue
        if (soundPath == null || soundPath == "")
        {
            Debug.Log("SoundPath is null or empty, Soundpath: " + soundPath);
            return;
        }
        //Starts a coroutine as there is a wait in the execution.
        if (looper)
        {
            StartCoroutine(LooperSoundCoroutine());
        }
        else
        {
            StartCoroutine(SoundCoroutine());
        }

    }

    

  

    private IEnumerator LooperSoundCoroutine()
    {
        while (true)
        {
            if (!transform.GetComponent<AudioSource>().isPlaying)
            {
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), soundPath, 0, Time.fixedTime, volumeScale);
            }
            yield return new WaitForSeconds(Resources.Load<AudioClip>(soundPath).length);
        }
    }

   

    private IEnumerator SoundCoroutine()
    {
        //Sends a request to play a specific sound.
        SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), soundPath, 0, Time.fixedTime, volumeScale);

        //yield on a new YieldInstruction that waits the duration of the AudioClip.
        yield return new WaitForSeconds(Resources.Load<AudioClip>(soundPath).length);

        //Once the AudioClip has finished playing we destory the object to free up resources
        Destroy(this.gameObject);
    }
}
