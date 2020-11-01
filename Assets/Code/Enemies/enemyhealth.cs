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
    private Rigidbody rb;
    private Transform target;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody>();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(2);
        }

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        rb.AddForce(transform.position * 2, ForceMode.Impulse);
    }
    //IEnumerator DeathCoroutine()
    //{
    //    SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPaths[deathSound], 0, Time.fixedTime, VolumeScales[0]);

    //    //yield on a new YieldInstruction that waits the duration of the AudioClip.
    //    yield return new WaitForSeconds(Resources.Load<AudioClip>(SoundPaths[deathSound]).length);

    //    Destroy(this.gameObject);
    //}
}


