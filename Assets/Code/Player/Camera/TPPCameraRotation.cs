using Cinemachine;
using UnityEngine;

public class TPPCameraRotation : MonoBehaviour
{
    #region Variables
    private CinemachineFreeLook _freeLookCamera;
    private float _movementDirectionX;
    private bool _mouseActive;

    #endregion
    #region Methods
    private void Start()
    {
        Cursor.visible = false;
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
    }
    //private bool IsMouseActive()
    //{
    //    return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    //}

    private void Update()
    {
        _movementDirectionX = Input.GetAxis("Horizontal");
        if (_movementDirectionX != 0)
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = 1800;
            //_mouseActive = true;
        }
       else
        {
            _freeLookCamera.m_XAxis.m_MaxSpeed = 1500;
            //if (_movementDirectionX != 0)
            //{
            //    _freeLookCamera.m_XAxis.m_MaxSpeed = 1800;
            //}
            //else
            //{
            //    _freeLookCamera.m_XAxis.m_MaxSpeed = 1500;
            //}
        }
    }
    #endregion
}
