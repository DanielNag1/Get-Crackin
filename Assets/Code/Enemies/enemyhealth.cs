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
    [SerializeField] private GameObject SoundObjectPrefab;
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
            Debug.Log("TriggerComponent:SoundPath = " + SoundPaths[deathSound]);
            GameObject temp = Instantiate(SoundObjectPrefab, this.transform.position, Quaternion.identity); //Creates the temporary SoundObject
            SoundComponent tempComponent = temp.GetComponent<SoundComponent>(); //Gets the temporary SoundObjects SoundComponent.
            tempComponent.soundPath = SoundPaths[deathSound]; //Assignes the correct sound to the SoundComponent.
            tempComponent.volumeScale = VolumeScales[0];//Assignes the correct soundVolume to the SoundComponent.
            StartCoroutine(DeathCoroutine());
            gameObject.active = false;
        }
    }

    public void MakeDamage(int amount)
    {
        currentHealth -= amount;
    }
    IEnumerator DeathCoroutine()
    {
        //yield on a new YieldInstruction that wait 
        yield return new WaitForSeconds(Time.deltaTime * 3);

        Destroy(this.gameObject);
    }
}


