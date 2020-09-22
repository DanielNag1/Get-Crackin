using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera LockOn;
    private Cinemachine.CinemachineFreeLook ThirdPerson;
    private Cinemachine.CinemachineTargetGroup targetGroup;

    void Start()
    {
        LockOn = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        ThirdPerson = GetComponentInChildren<Cinemachine.CinemachineFreeLook>();
        targetGroup = GetComponentInChildren<Cinemachine.CinemachineTargetGroup>();
    }

    /// <summary>
    /// Sets LockOn Camera to active.
    /// </summary>
    public void EnterLockCamera()
    {
        LockOn.Priority = 2;
        ThirdPerson.Priority = 1;
    }

    /// <summary>
    /// Sets FreeCamera to active.
    /// </summary>
    public void EnterFreeCamera()
    {
        ThirdPerson.Priority = 2;
        LockOn.Priority = 1;
    }
}
