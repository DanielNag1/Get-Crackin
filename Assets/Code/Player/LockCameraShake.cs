using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockCameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera lockCamera;
    [SerializeField] CinemachineBrain cinemachineBrain;
    private float shakeTimer;
    private float intensity;
    public static LockCameraShake Instance { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        lockCamera = GetComponent<CinemachineVirtualCamera>();


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
            if (cinemachineBrain.IsLive(lockCamera)) //Resets the amplitude of the shake to 0
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    lockCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float Inputintensity, float time)
    {
        intensity = Inputintensity;
        shakeTimer = time;
        if (cinemachineBrain.IsLive(lockCamera)) //Sets the amplitude of the shake
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = lockCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        }
    }
}
