using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class enemyhealth : MonoBehaviour
{

    public int currentHealth;
    public int startHealth;


    [SerializeField] private List<string> hurtSoundPaths;
    [SerializeField] private List<string> deathSoundPaths;
    [SerializeField] private List<float> volumeScales;
    [SerializeField] private GameObject SoundObjectPrefab;
    private int deathSound;
    private Rigidbody rb;
    public GameObject rootGameObject;

    private Vector3 smash;
    private CharacterController characterController;
    public float knockbackAmount;

    void Start()
    {
        rb = rootGameObject.GetComponent<Rigidbody>();
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        currentHealth = startHealth;
        deathSound = Random.Range(0, deathSoundPaths.Count - 1);
    }

    public void TakeDamage(int amount, Transform damageDealer)
    {
        if (rb != null)
        {
            currentHealth -= amount;
            if (currentHealth > 0)
            {
                Vector3 knockbackDirection = (rootGameObject.transform.position - damageDealer.position).normalized;
                float knockbackDistance = 50;

                if (characterController.gameObject.GetComponent<Animator>().GetBool("Rage Mode"))
                {
                    knockbackAmount = 6000000 * 1.5f;
                }
                else
                {
                    knockbackAmount = 6000000;
                }
                //rb.AddForce(knockbackDirection * knockbackAmount/*direction * 6.000.000 gave nice result(Save this)*/, ForceMode.Impulse);
                rb.AddForce(knockbackDirection * 6000000/*direction * 6.000.000 gave nice result(Save this)*/, ForceMode.Impulse);

                rb.GetComponent<FoxAgentFSM>().knockback.destination = (knockbackDirection * knockbackDistance) + rb.GetComponent<FoxAgentFSM>().transform.position;//experimental new knockback
                rb.GetComponent<FoxAgentFSM>().SetFSMState("knockback");
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), hurtSoundPaths[Random.Range(0, hurtSoundPaths.Count - 1)], 0, Time.fixedTime, volumeScales[0]);
            }
            else
            {
                EnemyManager.Instance.AgentLeftCombat(transform.root.gameObject);
                VFXEvents.Instance.VFX6Play(transform);
                EnemyManager.Instance.enemyPool.Find(x => x.enemy.transform.root.GetInstanceID() == rootGameObject.transform.root.GetInstanceID()).elementAvailable = true;//If this crashes someone else fucked up! All enemies should exist in the EnemyManagers enemyPool!
                StartCoroutine(DeathCoroutine());
            }
        }
    }

    public void Reset()
    {
        currentHealth = startHealth;
        rootGameObject.transform.localPosition = Vector3.zero;
    }

    IEnumerator DeathCoroutine()
    {
        Debug.Log(this.name + "Dead");
        Reset();

        GameObject temp = Instantiate(SoundObjectPrefab, this.transform.position, Quaternion.identity); //Creates the temporary SoundObject
        SoundComponent tempComponent = temp.GetComponent<SoundComponent>(); //Gets the temporary SoundObjects SoundComponent.
        tempComponent.soundPath = deathSoundPaths[deathSound]; //Assignes the correct sound to the SoundComponent.
        tempComponent.volumeScale = volumeScales[0];//Assignes the correct soundVolume to the SoundComponent.
        rootGameObject.SetActive(false);

        //yield on a new YieldInstruction that waits the duration of the AudioClip.
        yield return new WaitForSeconds(Resources.Load<AudioClip>(deathSoundPaths[deathSound]).length);

    }
}
