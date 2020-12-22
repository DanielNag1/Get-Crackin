using UnityEngine;

/// <summary>
/// OBS!!! Add simple explination!
/// </summary>
public class ChangeCamera : MonoBehaviour
{
    #region variables
    private Cinemachine.CinemachineVirtualCamera _lockOn;
    private Cinemachine.CinemachineFreeLook _thirdPerson;
    private Cinemachine.CinemachineTargetGroup _targetGroup;
    #endregion

    #region Methods
    private void Start()
    {
        _lockOn = GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>();
        _thirdPerson = GetComponentInChildren<Cinemachine.CinemachineFreeLook>();
        _targetGroup = GetComponentInChildren<Cinemachine.CinemachineTargetGroup>();
    }

    /// <summary>
    /// Sets LockOn Camera to active.
    /// </summary>
    public void EnterLockCamera()
    {
        _lockOn.Priority = 2;
        _thirdPerson.Priority = 1;
    }

    /// <summary>
    /// Sets FreeCamera to active.
    /// </summary>
    public void EnterFreeCamera()
    {
        _thirdPerson.Priority = 2;
        _lockOn.Priority = 1;
    }
    #endregion
}
