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
    public GameObject rootGameObject;
    private List<GameObject> prefabList;

    private Vector3 smash;
    private CharacterController characterController;


    void Start()
    {
        //Transform rootTransform = rootGameObject.transform.root;
        //Debug.Log(rootTransform);
        //rootGameObject = rootTransform.GetComponent<GameObject>();//set to root gameObject
        Debug.Log(rootGameObject);
        rb = rootGameObject.GetComponent<Rigidbody>();
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        prefabList = new List<GameObject>();
        currentHealth = startHealth;
        //deathSound = Random.Range(0, SoundPaths.Count - 1);
        deathSound = 0;
    }

    //Update is called once per frame
    void Update()
    {

        //OBS, Not used??
        if (smash.magnitude >= 0.1)
        {
            characterController.Move(smash * Time.deltaTime);
        }
        smash = Vector3.Lerp(smash, Vector3.zero, 2 * Time.deltaTime);


        if (currentHealth <= 0)
        {
            //StartCoroutine(DeathCoroutine());
            gameObject.active = false;
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(2);
        //}

    }

    public void TakeDamage(int amount, Transform damageDealer)
    {
        Vector3 knockbackDirection = (rootGameObject.transform.position - damageDealer.position).normalized;
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * amount, ForceMode.Impulse);
            //rb.AddExplosionForce(10, transform.position,5,5);
            currentHealth -= amount;

        }


        //UseSmash(target.transform.position, 10);


        //foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        //{
        //    prefabList.Add(enemy);
        //}
        //for (int i = 0; i < prefabList.Count; i++)
        //{

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


