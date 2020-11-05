using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponCollision : MonoBehaviour
{
    [SerializeField] RageMode rage;
    [SerializeField] private List<string> HitSoundPaths;
    [SerializeField] private List<float> VolumeScales;
    private Health health;
    [SerializeField] private Animator animator;
    Random random;

    public int weaponDamage = 1;
    public string targetTag;
    public int layerMaskValue;
    public List<Transform> weaponPoints;
    public float InvulnerabilityTime;
    public bool collisionActive = false;
    private List<Vector3> currentWeaponPointPositions = new List<Vector3>();
    private List<Vector3> previousWeaponPointPositions = new List<Vector3>();
    private int layerMask;
    private List<GameObject> targetsHit = new List<GameObject>();
    private List<Tuple<float, GameObject>> recentTargetsHit = new List<Tuple<float, GameObject>>();
   
   

    private void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
       
        for (int i = 0; i < weaponPoints.Count; i++)
        {
            currentWeaponPointPositions.Add(weaponPoints[i].position);
            previousWeaponPointPositions.Add(currentWeaponPointPositions[i]);
        }
        layerMask = 1 << layerMaskValue;
        layerMask = ~layerMask;
    }

    private void Update()
    {
      
        PurgeOldRecentTargetsHit();
        if (collisionActive)
        {
            UpdateCollision();
            WeaponCollisionCheck();
            DeliverDamageToTargetsHit();
        }
       
    }

    /// <summary>
    /// Moves the collision lines to be between current frame and last frame
    /// </summary>
    void UpdateCollision()
    {
        for (int i = 0; i < weaponPoints.Count; i++)
        {
            previousWeaponPointPositions[i] = currentWeaponPointPositions[i];
            currentWeaponPointPositions[i] = weaponPoints[i].position;
        }
    }
    /// <summary>
    /// Raycasts along the collision line.
    /// </summary>
    void WeaponCollisionCheck()
    {
        for (int i = 0; i < weaponPoints.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(previousWeaponPointPositions[i], transform.TransformDirection((currentWeaponPointPositions[i] - previousWeaponPointPositions[i]).normalized), out hit, Vector3.Distance(previousWeaponPointPositions[i], currentWeaponPointPositions[i]), layerMask))
            {
                if (hit.collider.tag == targetTag)
                {

                    if (!CompereTargetsHit(hit))
                    {
                        if (!CompereItem2RecentTargetsHit(hit))
                        {
                            targetsHit.Add(hit.transform.gameObject);
                            recentTargetsHit.Add(new Tuple<float, GameObject>(0, hit.transform.gameObject));
                        }
                    }
                }

            }
            Debug.DrawRay(previousWeaponPointPositions[i], transform.TransformDirection((currentWeaponPointPositions[i] - previousWeaponPointPositions[i]).normalized) * Vector3.Distance(previousWeaponPointPositions[i], currentWeaponPointPositions[i]), Color.white);
        }
    }
    public void DeliverDamageToTargetsHit()
    {
        for (int i = 0; i < targetsHit.Count; i++)
        {
            Debug.Log(targetsHit[i].name);
            //deal Damage Here
            if (targetTag != "Player")
            {
                if (animator.GetBool("Rage Mode") == false)
                {
                    rage.ModifyRage(10); //Increase rage meter
                }
                else
                {
                    rage.ModifyRage(-5); //Decrease rage meter OBS!!! We need to add a drain to rage as well!
                }

                //OBS!! weaponPoint location is hard coded, add info on what weaponPoint made the hit to the list: targetsHit
                //If we want knockback to depend on weapon hit location.
                //targetsHit[i].GetComponent<enemyhealth>().TakeDamage(weaponDamage, weaponPoints[0].transform); // if this breaks check weaponpoints null
                //If we want knockback to depend on player position.
                targetsHit[i].GetComponent<enemyhealth>().TakeDamage(weaponDamage, weaponPoints[0].transform.root);
            }
            else
            {
                animator.SetTrigger("GetHit");
                health = targetsHit[i].GetComponentInChildren<Health>();
                health.ModifyHealth(weaponDamage);
            }
            SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), HitSoundPaths[Random.Range(0, HitSoundPaths.Count - 1)], 0, Time.fixedTime, VolumeScales[0]);
        }
        targetsHit.Clear();
    }

    bool CompereTargetsHit(RaycastHit hit)
    {
        for (int i = 0; i < targetsHit.Count; i++)
        {
            if (targetsHit[i].GetInstanceID() == hit.transform.gameObject.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }
    bool CompereItem2RecentTargetsHit(RaycastHit hit)
    {
        for (int i = 0; i < recentTargetsHit.Count; i++)
        {
            if (recentTargetsHit[i].Item2.GetInstanceID() == hit.transform.gameObject.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    void PurgeOldRecentTargetsHit()
    {
        for (int i = recentTargetsHit.Count - 1; i >= 0; i--)
        {
            if (recentTargetsHit[i].Item1 < InvulnerabilityTime)
            {
                GameObject tempObject = recentTargetsHit[i].Item2;
                float tempTime = recentTargetsHit[i].Item1 + Time.deltaTime;
                recentTargetsHit.RemoveAt(i);
                recentTargetsHit.Add(new Tuple<float, GameObject>(tempTime, tempObject));
            }
            else
            {
                recentTargetsHit.RemoveAt(i);
            }
        }
    }
}
