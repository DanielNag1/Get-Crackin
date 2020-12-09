using UnityEngine;
using Cinemachine;

/// <summary>
/// OBS!!! Add simple explination!
/// </summary>
public class FreeCameraShake : MonoBehaviour
{
    #region Variables
    [SerializeField] CinemachineFreeLook _freeLookCamera;
    [SerializeField] CinemachineBrain _cinemachineBrain;
    private float _shakeTimer;
    private float _intensity;
    public static FreeCameraShake Instance { get; private set; }
    #endregion

    #region Methods
    private void Start()
    {
        _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        _freeLookCamera = GetComponent<CinemachineFreeLook>();

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
            if (_cinemachineBrain.IsLive(_freeLookCamera)) //Resets the amplitude of the shake to 0
            {
                //timeScale is used for slowmo effect
                //Time.timeScale = 1; //Reset gamespeed
                _freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                _freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                _freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            }
        }
    }

    public void ShakeCamera(float Inputintensity, float time)
    {
        _intensity = Inputintensity;
        _shakeTimer = time;
        if (_cinemachineBrain.IsLive(_freeLookCamera)) //Sets the amplitude of the shake
        {
            //timeScale is used for slowmo effect
            //Time.timeScale = 0.2f; //Slow down gamespeed
            _freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _intensity;
            _freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _intensity;
            _freeLookCamera.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _intensity;
        }
    }
    #endregion
}
