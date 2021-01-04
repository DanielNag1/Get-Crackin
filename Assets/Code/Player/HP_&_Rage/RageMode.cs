using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageMode : MonoBehaviour
{
    [SerializeField]
    public int maxRage = 100;
    public int startRage;
    [SerializeField]
    private float TimerSec;
    private float elapsedTime;
    private Image rags;

    [SerializeField] private List<string> _soundPaths;
    [SerializeField] private List<float> _volumeScales;

    public float currentRage;//Set this value when loading!
    public event Action<float> onRagePctChanged = delegate { };

    [SerializeField] Animator animator;

    public static RageMode Instance { get; private set; }
    void Start()
    {
        Instance = this;
        currentRage = startRage;
        rags = GetComponentInChildren<RageBar>().rageBar;
    }

    private void OnEnable()
    {
        currentRage = startRage;
    }

    public void ActivateRageMode()
    {
        animator.SetBool("Rage Mode", true);
        VFXEvents.Instance.VFX4Stop();
        VFXEvents.Instance.VFX5Play();
        SoundEngine.Instance.RequestSFX(transform.GetComponent<AudioSource>(), _soundPaths[UnityEngine.Random.Range(0, _soundPaths.Count - 1)],
            0, Time.fixedTime, _volumeScales[0]);
    }

    public void DisableRageMode()
    {
        //animator.ResetTrigger("Attack");
        animator.SetBool("Rage Mode", false);
        VFXEvents.Instance.VFX5Stop();
        if (currentRage > 0)
        {
            VFXEvents.Instance.VFX4Play();
        }
        else
        {
            VFXEvents.Instance.VFX4Stop();
        }
    }

    public void ModifyRage(float amount)
    {
        if (amount > 0)
        {
            currentRage = Math.Min(maxRage, currentRage + amount);
            if (!animator.GetBool("Rage Mode"))
            {
                VFXEvents.Instance.VFX4Play();
            }
        }
        else
        {
            currentRage = Math.Max(0, currentRage + amount);
            if (currentRage == 0)
            {
                DisableRageMode();
            }
        }
        float currentRagePct = (float)currentRage / (float)maxRage;
        onRagePctChanged(currentRagePct);
    }

    void Update()
    {
        if (animator.GetBool("Rage Mode"))
        {
            ModifyRage(-Time.deltaTime);
        }
    }

    private void ResetRageMode()
    {
        currentRage = startRage;
        rags.fillAmount = 0;
    }
}
