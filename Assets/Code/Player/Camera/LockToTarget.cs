using System.Collections.Generic;
using UnityEngine;

public class LockToTarget : MonoBehaviour
{
    #region Variables
    [SerializeField] public Cinemachine.CinemachineTargetGroup targetGroup;
    [SerializeField] private ChangeCamera _changeCamera;
    [SerializeField] private Transform _targetIcon;
    [SerializeField] private List<string> _soundPaths;
    [SerializeField] private List<float> _volumeScales;
    private List<Transform> _enemiesInRange;
    public Transform closestEnemy;
    private bool _isLockedToTarget;
    private Renderer _iconRenderer;
    private GameObject _lockedEnemy;
    #endregion
    #region Methods
    private void Start()
    {
        _enemiesInRange = new List<Transform>();
        _iconRenderer = _targetIcon.gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        AutomaticTargeting();
        TargetIconRotation();
    }

    /// <summary>
    /// Returns the transform of the closest enemy.
    /// </summary>
    private Transform FindClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        //Looks through the list of the nearby enemies and finds the nearest.
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                if (_isLockedToTarget)
                {
                    ExitLock();
                }
                continue;
            }
            if (enemies[i].gameObject.active == false)
            {
                enemies.RemoveAt(i);
                if (_isLockedToTarget)
                {
                    ExitLock();
                }
                continue;
            }
            Transform potentialTarget = enemies[i];
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float distanceToTarget = directionToTarget.sqrMagnitude;
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    private void ExitLock()
    {
        _changeCamera.EnterFreeCamera();
        targetGroup.RemoveMember(closestEnemy); //Remove the nearest enemy from the camera target group.
        _isLockedToTarget = false;
        _enemiesInRange.Clear();
        SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[0], 0, Time.fixedTime, _volumeScales[0]);
    }

    /// <summary>
    /// Lock the camera on the selected target with F.
    /// </summary>
    public void ManualTargeting()
    {
        if (_isLockedToTarget) // Exit manual Lock
        {
            ExitLock();
            return;
        }
        if (!_isLockedToTarget) //Enter manual Lock
        {
            if (targetGroup.FindMember(closestEnemy) != 1) //Checks if the closest enemy is not already locked on to.
            {
                if (closestEnemy != null)
                {
                    _lockedEnemy = closestEnemy.gameObject;
                }
                _changeCamera.EnterLockCamera();
                _iconRenderer.material.color = new Color(0, 150, 200);
                targetGroup.AddMember(closestEnemy, 1, 0); //The lockOn camera focuses on both the player and the target.
                _isLockedToTarget = true;
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[1], 0, Time.fixedTime, _volumeScales[1]);
            }
        }
    }

    /// <summary>
    /// Finds the nearest enemy and sets it as a currently selected target with an orange marker.
    /// The player can then force this lock on to the target, then the marker becomes blue.
    /// </summary>
    private void AutomaticTargeting()
    {
        if (FindClosestEnemy(_enemiesInRange) != null && !_isLockedToTarget) //If there is an enemy nearby
        {
            _iconRenderer.enabled = true;

            if (!_isLockedToTarget) //The player has not manually locked on to one enemy.
            {
                closestEnemy = FindClosestEnemy(_enemiesInRange); //Get the nearest enemy
                _iconRenderer.material.color = new Color(160, 100, 0);
                if (closestEnemy != null)
                {
                    _targetIcon.position = new Vector3(closestEnemy.position.x, closestEnemy.position.y + 1.5f, closestEnemy.position.z);
                }
            }
        }
        else if (_isLockedToTarget) //If the player has locked on to an enemy, the position of the target icon is being updated.
        {
            if (closestEnemy != null)
            {
                _targetIcon.position = new Vector3(_lockedEnemy.transform.position.x, _lockedEnemy.transform.position.y + 1.5f, _lockedEnemy.transform.position.z);
            }
        }
        else
        {
            closestEnemy = null; //Sets selected target to null.
            _lockedEnemy = null;
            _iconRenderer.enabled = false;
        }

    }

    ///<summary>
    ///The targeting icon is always rotated towards the camera
    ///</summary>
    private void TargetIconRotation()
    {
        _targetIcon.LookAt(Camera.main.transform.position, -Vector3.up);
        _targetIcon.localEulerAngles = new Vector3(0, _targetIcon.localEulerAngles.y, 0);
    }

    /// <summary>
    /// Returns the direction to the target.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEnemyDirection()
    {
        if (closestEnemy != null)
        {
            return closestEnemy.position - transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    public Transform GetEnemyTransform()
    {
        if (closestEnemy != null)
        {
            return closestEnemy.transform;
        }
        else
        {
            return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!_enemiesInRange.Contains(other.transform))
            {
                _enemiesInRange.Add(other.transform); //Adds an enemy to the potential target list.
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isLockedToTarget)
        {
            if (other.tag == "Enemy")
            {
                _enemiesInRange.Remove(other.transform); //Removes an enemy from the potential target list.
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (closestEnemy != null)
        {
            Gizmos.DrawLine(transform.position, closestEnemy.position);
        }
    }
    #endregion
}
