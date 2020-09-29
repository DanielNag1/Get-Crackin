using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToTarget : MonoBehaviour
{
    [SerializeField] public Cinemachine.CinemachineTargetGroup targetGroup;
    [SerializeField] private ChangeCamera changeCamera;
    [SerializeField] private Transform targetIcon;
    [SerializeField] private List<string> SoundPaths;
    [SerializeField] private List<float> VolumeScales;

    List<Transform> enemiesInRange;
    Transform closestEnemy;
    bool isLockedToTarget;

    private Renderer iconRenderer;

    private void Start()
    {
        enemiesInRange = new List<Transform>();
        iconRenderer = targetIcon.gameObject.GetComponent<Renderer>();
    }

    private void Update()
    {
        AutomaticTargeting();
        TargetIconRotation();
    }

    /// <summary>
    /// Returns the transform of the closest enemy.
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    Transform FindClosestEnemy(List<Transform> enemies)
    {
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        //Looks through the list of the nearby enemies and finds the nearest.
        foreach (Transform potentialTarget in enemies)
        {
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

    /// <summary>
    /// Lock the camera on the selected target with F.
    /// </summary>
    public void ManualTargeting()
    {
        if (closestEnemy == null)
        {
            AutomaticTargeting();
        }

        if (isLockedToTarget) // Exit manual Lock
        {
            changeCamera.EnterFreeCamera();
            targetGroup.RemoveMember(closestEnemy); //Remove the nearest enemy from the camera target group.
            isLockedToTarget = false;
            enemiesInRange.Clear();
            SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPaths[0], 0, Time.fixedTime,VolumeScales[0]);
            return;
        }

        if (!isLockedToTarget) //Enter manual Lock
        {
            if (targetGroup.FindMember(closestEnemy) != 1) //Checks if the closest enemy is not already locked on to.
            {
                changeCamera.EnterLockCamera();
                iconRenderer.material.color = new Color(0, 150, 200);
                targetGroup.AddMember(closestEnemy, 1, 0); //The lockOn camera focuses on both the player and the target.
                isLockedToTarget = true;
                SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), SoundPaths[1], 0, Time.fixedTime, VolumeScales[1]);
            }
        }

    }

    /// <summary>
    /// Finds the nearest enemy and sets it as a currently selected target with an orange marker.
    /// The player can then force this lock on to the target, then the marker becomes blue.
    /// </summary>
    void AutomaticTargeting()
    {
        if (FindClosestEnemy(enemiesInRange) != null) //If there is an enemy nearby
        {
            iconRenderer.enabled = true;

            if (!isLockedToTarget) //The player has not manually locked on to one enemy.
            {
                closestEnemy = FindClosestEnemy(enemiesInRange); //Get the nearest enemy
                iconRenderer.material.color = new Color(160, 100, 0);
                targetIcon.position = new Vector3(closestEnemy.position.x, closestEnemy.position.y + 1.5f, closestEnemy.position.z);
            }
        }
        else
        {
            closestEnemy = null; //Sets selected target to null.
            iconRenderer.enabled = false;
        }
    }

    ///<summary>
    ///The targeting icon is always rotated towards the camera
    ///</summary>
    void TargetIconRotation()
    {
        targetIcon.LookAt(Camera.main.transform.position, -Vector3.up);
        targetIcon.localEulerAngles = new Vector3(0, targetIcon.localEulerAngles.y, 0);
    }

    /// <summary>
    /// Returns the direction to the target.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEnemyDirection()
    {
        return closestEnemy.position - transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!enemiesInRange.Contains(other.transform))
            {
                enemiesInRange.Add(other.transform); //Adds an enemy to the potential target list.
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isLockedToTarget)
        {
            if (other.tag == "Enemy")
            {
                enemiesInRange.Remove(other.transform); //Removes an enemy from the potential target list.
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

}



