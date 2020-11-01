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
    private List<GameObject> prefabList;
    private GameObject enemy;
    private Vector3 smash;
    private CharacterController characterController;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Transform>();
        rb = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody>();
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        prefabList = new List<GameObject>();
        currentHealth = startHealth;
        //deathSound = Random.Range(0, SoundPaths.Count - 1);
        deathSound = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(smash.magnitude >= 0.1)
        {
            characterController.Move(smash * Time.deltaTime);
        }
        smash = Vector3.Lerp(smash, Vector3.zero, 2 * Time.deltaTime);
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
        //foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        //{
        //    prefabList.Add(enemy);
        //}
        //for (int i = 0; i < prefabList.Count; i++)
        //{
            currentHealth -= amount;
            UseSmash(target.position, 2);
           // rb.AddForce(transform.position * 2, ForceMode.Impulse);
     //  }
      
    }
    private void UseSmash(Vector3 direction, float force)
    {
        direction.Normalize();
        smash += direction.normalized * force;
    }
    //IEnumerator DeathCoroutine()
    //{
    //    SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPaths[deathSound], 0, Time.fixedTime, VolumeScales[0]);

    //    //yield on a new YieldInstruction that waits the duration of the AudioClip.
    //    yield return new WaitForSeconds(Resources.Load<AudioClip>(SoundPaths[deathSound]).length);

    //    Destroy(this.gameObject);
    //}
}


