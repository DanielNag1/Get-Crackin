using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class enemyhealth : MonoBehaviour
{

    public int currentHealth;
    public int startHealth;

    [SerializeField] private List<string> SoundPaths;
    [SerializeField] private List<float> VolumeScales;
    private int deathSound;
    void Start()
    {
        currentHealth = startHealth;
        //deathSound = Random.Range(0, SoundPaths.Count - 1);
        deathSound = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            //StartCoroutine(DeathCoroutine());
            gameObject.active = false;
        }
    }

    public void MakeDamage(int amount)
    {
        currentHealth -= amount;
    }
    //IEnumerator DeathCoroutine()
    //{
    //    SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPaths[deathSound], 0, Time.fixedTime, VolumeScales[0]);

    //    //yield on a new YieldInstruction that waits the duration of the AudioClip.
    //    yield return new WaitForSeconds(Resources.Load<AudioClip>(SoundPaths[deathSound]).length);

    //    Destroy(this.gameObject);
    //}
}


