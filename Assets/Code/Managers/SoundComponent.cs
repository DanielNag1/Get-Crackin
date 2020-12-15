using System.Collections;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    public string soundPath; //sound paths
    public float volumeScale = 1; //volume scale

  


    private void Start()
    {
        //If the soundpath is null we should not continue
        if (soundPath == null || soundPath == "")
        {
            Debug.Log("SoundPath is null or empty, Soundpath: " + soundPath);
            return;
        }
        //Starts a coroutine as there is a wait in the execution.
        StartCoroutine(SoundCoroutine());
    }
    IEnumerator SoundCoroutine()
    {
        //Sends a request to play a specific sound.
        SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), soundPath, 0, Time.fixedTime, volumeScale);


        //yield on a new YieldInstruction that waits the duration of the AudioClip.
        yield return new WaitForSeconds(Resources.Load<AudioClip>(soundPath).length);

        //Once the AudioClip has finished playing we destory the object to free up resources
        Destroy(this.gameObject);
    }
}
