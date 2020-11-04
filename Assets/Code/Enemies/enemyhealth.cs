using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class enemyhealth : MonoBehaviour
{

    public int currentHealth;
    public int startHealth;


    [SerializeField] private List<string> SoundPaths;
    [SerializeField] private List<float> VolumeScales;

    private int deathSound;
    private Rigidbody rb;
    public GameObject rootGameObject;
    private List<GameObject> prefabList;

    private Vector3 smash;
    private CharacterController characterController;


    void Start()
    {
        rb = rootGameObject.GetComponent<Rigidbody>();
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        prefabList = new List<GameObject>();
        currentHealth = startHealth;
        deathSound = Random.Range(0, SoundPaths.Count - 1);
        deathSound = 0;
    }

    //Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            EnemyManager enemyManager = EnemyManager.Instance;
            enemyManager.enemyPool.Find(x => x.enemy.transform.root.GetInstanceID() == rootGameObject.transform.root.GetInstanceID()).elementAvailable = true;//If this crashes someone else fucked up! All enemies should exist in the EnemyManagers enemyPool!
            StartCoroutine(DeathCoroutine());
        }
    }

    public void TakeDamage(int amount, Transform damageDealer)
    {
        Vector3 knockbackDirection = (rootGameObject.transform.position - damageDealer.position).normalized;
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * 60000000/*direction * 60.000.000 gave nice result(Save this)*/, ForceMode.Impulse);
            currentHealth -= amount;
        }
    }

    public void Reset()
    {
        currentHealth = startHealth;
    }

    IEnumerator DeathCoroutine()
    {
        Debug.Log(this.name + "Dead");
        rootGameObject.SetActive(false);

        SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPaths[deathSound], 0, Time.fixedTime, VolumeScales[0]);

        //yield on a new YieldInstruction that waits the duration of the AudioClip.
        yield return new WaitForSeconds(Resources.Load<AudioClip>(SoundPaths[deathSound]).length);

        //Destroy(this.gameObject);
    }
}


