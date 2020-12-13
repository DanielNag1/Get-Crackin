using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockCameraShake : MonoBehaviour
{
    #region Variables
    [SerializeField] private CinemachineVirtualCamera _lockCamera;
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    private float _shakeTimer;
    private float _intensity;
    public static LockCameraShake Instance { get; private set; }
    #endregion
    #region Methods
    private void Start()
    {
        _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        _lockCamera = GetComponent<CinemachineVirtualCamera>();
        Instance = this;
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            _intensity -= 0.1f;
        }
        if (_shakeTimer <= 0)
        {
            if (_cinemachineBrain.IsLive(_lockCamera)) //Resets the amplitude of the shake to 0
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    _lockCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float Inputintensity, float time)
    {
        _intensity = Inputintensity;
        _shakeTimer = time;
        if (_cinemachineBrain.IsLive(_lockCamera)) //Sets the amplitude of the shake
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _lockCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;
        }
    }
    #endregion
}
