using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FreeCameraShake : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLookCamera;
    [SerializeField] CinemachineBrain cinemachineBrain;
    private float shakeTimer;
    public static FreeCameraShake Instance { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        freeLookCamera = GetComponent<CinemachineFreeLook>();


        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            
        }
        if (shakeTimer <= 0)
        {
            if (cinemachineBrain.IsLive(freeLookCamera))
            {
                freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        shakeTimer = time;
        if (cinemachineBrain.IsLive(freeLookCamera))
        {
            freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        }
    }
}
