using System;
using UnityEngine;
using UnityEngine.UI;

public class RageMode : MonoBehaviour
{
    #region Variables
    [SerializeField] public int maxRage = 100;
    public int startRage;
    [SerializeField] private float _timerSec;
    private float _elapsedTime;
    private Image _rags;
    public float currentRage;//Set this value when loading!
    public event Action<float> onRagePctChanged = delegate { };
    [SerializeField] private Animator _animator;
    public static RageMode Instance { get; private set; }
    #endregion

    #region Methods
    private void Start()
    {
        Instance = this;
        currentRage = startRage;
        _rags = GetComponentInParent<RageBar>().rageBar;
    }

    private void OnEnable()
    {
        currentRage = startRage;
    }

    public void ModifyRage(float amount)
    {
        if (amount > 0)
        {
            currentRage = Math.Min(maxRage, currentRage + amount);
            if (!_animator.GetBool("Rage Mode"))
            {
                VFXEvents.Instance.VFX4Play();
            }
        }
        else
        {
            currentRage = Math.Max(0, currentRage + amount);
            if (currentRage == 0)
            {
                _animator.SetBool("Rage Mode", false);
                VFXEvents.Instance.VFX4Stop();
                VFXEvents.Instance.VFX5Stop();
            }
        }
        float currentRagePct = (float)currentRage / (float)maxRage;
        onRagePctChanged(currentRagePct);
    }
#endregion
}
