using Cinemachine;
using System;
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



    private bool IsMouseActive()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    private void Update()
    {

        if (IsMouseActive())
        {
            _freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
            _freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
            _freeLookCamera.m_XAxis.m_MaxSpeed = 300;
            _freeLookCamera.m_YAxis.m_MaxSpeed = 10;
        }
        else
        {
            _freeLookCamera.m_XAxis.m_InputAxisName = "Joystick X";
            _freeLookCamera.m_YAxis.m_InputAxisName = "Joystick Y"; 
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
    #endregion
}
