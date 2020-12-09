using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class enemyhealth : MonoBehaviour
{
    #region Variables
    public int currentHealth;
    public int startHealth;
    [SerializeField] private List<string> _hurtSoundPaths;
    [SerializeField] private List<string> _deathSoundPaths;
    [SerializeField] private List<float> _volumeScales;
    [SerializeField] private GameObject _soundObjectPrefab;
    private int _deathSound;
    private Rigidbody _rb;
    private CharacterController _characterController;
    private float _knockbackAmount;
    public float knockbackAmountResetVal = 6000000;
    public GameObject rootGameObject;
    #endregion
    #region Methods
    private void Start()
    {
        _rb = rootGameObject.GetComponent<Rigidbody>();
        _characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        currentHealth = startHealth;
        _deathSound = Random.Range(0, _deathSoundPaths.Count - 1);
    }

    public void TakeDamage(int amount, Transform damageDealer)
    {
        if (_rb != null)
        {
            currentHealth -= amount;
            if (currentHealth > 0)
            {
                Vector3 knockbackDirection = (rootGameObject.transform.position - damageDealer.position).normalized;
                float knockbackDistance = 50;

                if (_characterController.gameObject.GetComponent<Animator>().GetBool("Rage Mode"))
                {
                    _knockbackAmount = knockbackAmountResetVal * 1.5f;
                }
                else
                {
                    _knockbackAmount = knockbackAmountResetVal;
                }
                //Direction * 6.000.000 gave nice result(Save this)
                _rb.AddForce(knockbackDirection * _knockbackAmount, ForceMode.Impulse);
                _rb.GetComponent<FoxAgentFSM>().knockback.destination = (knockbackDirection * knockbackDistance) +
                    _rb.GetComponent<FoxAgentFSM>().transform.position;//experimental new knockback
                _rb.GetComponent<FoxAgentFSM>().SetFSMState("knockback");
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _hurtSoundPaths[
                    Random.Range(0, _hurtSoundPaths.Count - 1)], 0, Time.fixedTime, _volumeScales[0]);
            }
            else
            {
                EnemyManager.Instance.AgentLeftCombat(gameObject);
                VFXEvents.Instance.VFX6Play(transform);
                EnemyManager.Instance.enemyPool.Find(x => x.enemy.transform.root.GetInstanceID() ==
                rootGameObject.transform.root.GetInstanceID()).elementAvailable = true;//If this crashes someone else fucked up! All enemies should exist in the EnemyManagers enemyPool!
                StartCoroutine(DeathCoroutine());
            }
        }
    }

    public void Reset()
    {
        currentHealth = startHealth;
        rootGameObject.transform.localPosition = Vector3.zero;
    }

    private IEnumerator DeathCoroutine()
    {
        Debug.Log(this.name + "Dead");
        Reset();

        GameObject temp = Instantiate(_soundObjectPrefab, this.transform.position, Quaternion.identity); //Creates the temporary SoundObject
        SoundComponent tempComponent = temp.GetComponent<SoundComponent>(); //Gets the temporary SoundObjects SoundComponent.
        tempComponent.soundPath = _deathSoundPaths[_deathSound]; //Assignes the correct sound to the SoundComponent.
        tempComponent.volumeScale = _volumeScales[0];//Assignes the correct soundVolume to the SoundComponent.

        rootGameObject.SetActive(false);

        //yield on a new YieldInstruction that waits the duration of the AudioClip.
        yield return new WaitForSeconds(Resources.Load<AudioClip>(_deathSoundPaths[_deathSound]).length);
    }
    #endregion
}
