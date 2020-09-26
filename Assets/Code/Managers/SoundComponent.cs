using System.Collections;
using UnityEngine;

public class SoundComponent : MonoBehaviour
{
    public string SoundPath; //sound paths

    
    private void Awake()
    {
        //If the soundpath is null we should not continue
        if (SoundPath == null || SoundPath == "")
        {
            Debug.Log("SoundPath is null or empty, Soundpath: " + SoundPath);
            return;
        }
        //Starts a coroutine as there is a wait in the execution.
        StartCoroutine(SoundCoroutine());

        IEnumerator SoundCoroutine()
        {
            //Sends a request to play a specific sound.
            SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPath, 0, Time.fixedTime);

            //float time = Resources.Load<AudioClip>(SoundPath).length;

            //yield on a new YieldInstruction that waits the duration of the AudioClip.
            yield return new WaitForSeconds(Resources.Load<AudioClip>(SoundPath).length);

            //Once the AudioClip has finished playing we destory the object to free up resources
            Destroy(this.gameObject);
        }
        
    }
}
