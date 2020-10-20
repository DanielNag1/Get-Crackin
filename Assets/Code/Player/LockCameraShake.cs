using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockCameraShake : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera lockCamera;
    [SerializeField] CinemachineBrain cinemachineBrain;
    private float shakeTimer;
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
        Debug.Log(shakeTimer);
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

        }
        if (shakeTimer <= 0)
        {
            if (cinemachineBrain.IsLive(lockCamera))
            {
                Debug.Log("lockCamera shake ends!"); CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = lockCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        shakeTimer = time;
        if (cinemachineBrain.IsLive(lockCamera))
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = lockCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        }
    }
}
