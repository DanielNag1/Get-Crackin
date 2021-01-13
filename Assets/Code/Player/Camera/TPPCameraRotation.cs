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
        Cursor.visible = true;
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
    }
    private bool IsMouseActive()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    private void Update()
    {
        _movementDirectionX = Input.GetAxis("Horizontal");
        if (IsMouseActive())
        {
            _freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
            _freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
            _freeLookCamera.m_XAxis.m_MaxSpeed = 150;
            _freeLookCamera.m_YAxis.m_MaxSpeed = 5;
            _freeLookCamera.m_XAxis.m_AccelTime = .2f;
            _freeLookCamera.m_XAxis.m_DecelTime = .2f;
            _freeLookCamera.m_YAxis.m_AccelTime = .2f;
            _freeLookCamera.m_YAxis.m_DecelTime = .2f;
            _freeLookCamera.m_YAxis.m_InvertInput = true;
        }
        else
        {
            _freeLookCamera.m_XAxis.m_InputAxisName = "Joystick X";
            _freeLookCamera.m_YAxis.m_InputAxisName = "Joystick Y";
            _freeLookCamera.m_YAxis.m_MaxSpeed = 25;
            _freeLookCamera.m_YAxis.m_InvertInput = false;
            _freeLookCamera.m_XAxis.m_AccelTime = 0;
            _freeLookCamera.m_XAxis.m_DecelTime = 0;
            _freeLookCamera.m_YAxis.m_AccelTime = 0;
            _freeLookCamera.m_YAxis.m_DecelTime = 0;
            if (_movementDirectionX != 0)
            {
                _freeLookCamera.m_XAxis.m_MaxSpeed = 1800;

            }
            else
            {
                _freeLookCamera.m_XAxis.m_MaxSpeed = 1500;

            }
        }



    }
    private void OnMouseActive()
    {

    }
    private void OnJoystickActive()
    {

    }
    #endregion
}
