using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FreeCameraShake : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLookCamera;
    [SerializeField] CinemachineBrain cinemachineBrain;
    private float shakeTimer;
    private float intensity;
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
            intensity -= 0.1f;
        }
        if (shakeTimer <= 0)
        {
            if (cinemachineBrain.IsLive(freeLookCamera)) //Resets the amplitude of the shake to 0
            {
                //Time.timeScale = 1; //Reset gamespeed
                freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float Inputintensity, float time)
    {
        intensity = Inputintensity;
        shakeTimer = time;
        if (cinemachineBrain.IsLive(freeLookCamera)) //Sets the amplitude of the shake
        {
            //Time.timeScale = 0.2f; //Slow down gamespeed
            freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        }
    }
}
