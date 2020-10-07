using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollision : MonoBehaviour
{
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

    void UpdateCollision()
    {
        for (int i = 0; i < weaponPoints.Count; i++)
        {
            previousWeaponPointPositions[i] = currentWeaponPointPositions[i];
            currentWeaponPointPositions[i] = weaponPoints[i].position;
        }
    }

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
    void DeliverDamageToTargetsHit()
    {
        for (int i = 0; i < targetsHit.Count; i++)
        {

            Debug.Log(targetsHit[i].name);
            //deal Damage Here
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
