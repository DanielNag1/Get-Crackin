using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TPPCameraRotation : MonoBehaviour
{

    CinemachineFreeLook freeLookCamera;
    float movementDirectionX;
    // Start is called before the first frame update
    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirectionX = Input.GetAxis("Horizontal");

        if(movementDirectionX!=0)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 1800;

        }
        else
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = 1500;
        }

    }


}
