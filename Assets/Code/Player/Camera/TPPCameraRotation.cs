using Cinemachine;
using UnityEngine;

public class TPPCameraRotation : MonoBehaviour
{
    #region Variables
    private CinemachineFreeLook _freeLookCamera;
    private float _movementDirectionX;
    #endregion
    #region Methods
    private void Start()
    {
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        _movementDirectionX = Input.GetAxis("Horizontal");
        if(_movementDirectionX!=0)
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = 1800;
        }
        else
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = 1500;
        }
    }
    #endregion
}
