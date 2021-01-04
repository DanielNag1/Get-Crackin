using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponCollision : MonoBehaviour
{
    //OBS!!! ADD COMMENTS!!!!!
    #region variables
    [SerializeField] private List<string> _soundPaths;
    [SerializeField] private List<float> _volumeScales;
    private Health _health;
    public int weaponDamage = 1;
    public bool destroyOnImpact = false;
    public string targetTag;
    private string crateTag = "crate";
    private GameObject crate;
    public int layerMaskValue;
    public List<Transform> weaponPoints;
    public float invulnerabilityTime;
    public bool collisionActive = false;
    [SerializeField] private int _rageAdjustmentValue = 10;
    [SerializeField] private float _shakeIntensity = 4f;
    [SerializeField] private float _shakeTime = 0.1f;
    private List<Vector3> _currentWeaponPointPositions = new List<Vector3>();
    private List<Vector3> _previousWeaponPointPositions = new List<Vector3>();
    private int _layerMask; //Make int LAYERMASK type
    private List<GameObject> _targetsHit = new List<GameObject>();
    private List<Tuple<float, GameObject>> _recentTargetsHit = new List<Tuple<float, GameObject>>();
    #endregion

    #region methods
    private void Start()
    {
        for (int i = 0; i < weaponPoints.Count; i++)
        {
            _currentWeaponPointPositions.Add(weaponPoints[i].position);
            _previousWeaponPointPositions.Add(_currentWeaponPointPositions[i]);
        }
        _layerMask = 1 << layerMaskValue;
        _layerMask = ~_layerMask;
        crate = GameObject.FindGameObjectWithTag("crate");
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
            _previousWeaponPointPositions[i] = _currentWeaponPointPositions[i];
            _currentWeaponPointPositions[i] = weaponPoints[i].position;
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
            if (Physics.Raycast(_previousWeaponPointPositions[i], transform.TransformDirection(
                (_currentWeaponPointPositions[i] - _previousWeaponPointPositions[i]).normalized), out hit,
                Vector3.Distance(_previousWeaponPointPositions[i], _currentWeaponPointPositions[i]), _layerMask))
            {
                if (hit.collider.tag == targetTag || hit.collider.tag == crateTag)
                {
                    if (!CompareTargetsHit(hit) && !CompareItem2RecentTargetsHit(hit))
                    {
                        _targetsHit.Add(hit.transform.gameObject);
                        _recentTargetsHit.Add(new Tuple<float, GameObject>(0, hit.transform.gameObject));

                    }
                }
            }
            Debug.DrawRay(_previousWeaponPointPositions[i], transform.TransformDirection(
                (_currentWeaponPointPositions[i] - _previousWeaponPointPositions[i]).normalized) *
                Vector3.Distance(_previousWeaponPointPositions[i], _currentWeaponPointPositions[i]), Color.white);
        }
    }
    public void DeliverDamageToTargetsHit()
    {
        for (int i = 0; i < _targetsHit.Count; i++)
        {
            Debug.Log(_targetsHit[i].name);
            //deal Damage Here
            if (targetTag != "Player")
            {
                VFXEvents.Instance.VFX1Play();
                FreeCameraShake.Instance.ShakeCamera(_shakeIntensity, _shakeTime);
                LockCameraShake.Instance.ShakeCamera(_shakeIntensity, _shakeTime);
                if (!gameObject.GetComponent<Animator>().GetBool("Rage Mode"))
                {
                    RageMode.Instance.ModifyRage(_rageAdjustmentValue); //Increase rage meter
                }
                else
                {
                    RageMode.Instance.ModifyRage(-_rageAdjustmentValue); //Decrease rage meter
                }
                //OBS!! weaponPoint location is hard coded, add info on what weaponPoint made the hit to the list: targetsHit
                //If we want knockback to depend on player position.
                if (_targetsHit[i] != crate)
                {
                    _targetsHit[i].GetComponent<enemyhealth>().TakeDamage(weaponDamage, weaponPoints[0].transform.root);
                    SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[Random.Range(5, _soundPaths.Count - 1)], 0,
                        Time.fixedTime, _volumeScales[2]);
                }
                else
                {
                    ShatterCrate.Instance.DestroyCrate();
                }
            }
            else
            {
                Vector3 knockbackDirection = (_targetsHit[i].transform.position - transform.position).normalized; //The direction from the enemy that hit the player
                if (destroyOnImpact)
                {
                    _targetsHit[i].GetComponent<Move>().Knockback(-knockbackDirection); //Projectiles get position behind player thus we reverse direction of knockback
                    GetComponent<MeshRenderer>().forceRenderingOff = true;
                }
                else
                {
                    _targetsHit[i].GetComponent<Move>().Knockback(knockbackDirection); //Set knockback on player
                }
                _targetsHit[i].GetComponent<Animator>().SetTrigger("GetHit");
                _health = _targetsHit[i].GetComponentInChildren<Health>();
                _health.ModifyHealth(-weaponDamage);
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[Random.Range(0, _soundPaths.Count - 1)], 0,
                    Time.fixedTime, _volumeScales[0]);
            }
        }
        _targetsHit.Clear();
    }

    bool CompareTargetsHit(RaycastHit hit)
    {
        for (int i = 0; i < _targetsHit.Count; i++)
        {
            if (_targetsHit[i].GetInstanceID() == hit.transform.gameObject.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    bool CompareItem2RecentTargetsHit(RaycastHit hit)
    {
        for (int i = 0; i < _recentTargetsHit.Count; i++)
        {
            if (_recentTargetsHit[i].Item2.GetInstanceID() == hit.transform.gameObject.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    void PurgeOldRecentTargetsHit()
    {
        for (int i = _recentTargetsHit.Count - 1; i >= 0; i--)
        {
            if (_recentTargetsHit[i].Item1 < invulnerabilityTime)
            {
                GameObject tempObject = _recentTargetsHit[i].Item2;
                float tempTime = _recentTargetsHit[i].Item1 + Time.deltaTime;
                _recentTargetsHit.RemoveAt(i);
                _recentTargetsHit.Add(new Tuple<float, GameObject>(tempTime, tempObject));
            }
            else
            {
                _recentTargetsHit.RemoveAt(i);
            }
        }
    }

    //Used as animatorEvents
    public void EnableCollision()
    {
        collisionActive = true;
    }
    //Used as animatorEvents
    public void DisableCollision()
    {
        collisionActive = false;
    }
    public void PlaySwingSFX(int selector)
    {
        switch (selector)
        {
            case 1:
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[Random.Range(0, 1)], 0,
                    Time.fixedTime, _volumeScales[0]);
                break;
            case 2:
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[Random.Range(2, 3)], 0,
                    Time.fixedTime, _volumeScales[1]);
                break;

            case 3:
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[Random.Range(4, 4)], 0,
                    Time.fixedTime, _volumeScales[1]);
                break;
            default:
                break;
        }

    }
    #endregion
}
