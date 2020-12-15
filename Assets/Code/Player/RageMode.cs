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

    public float currentRage;//Set this value when loading!

    public event Action<float> onRagePctChanged = delegate { };

    [SerializeField] Animator animator;

    public static RageMode Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentRage = startRage;
        rags = GetComponentInParent<RageBar>().RageBarss;
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
                //animator.ResetTrigger("Attack");
                animator.SetBool("Rage Mode", false);
                VFXEvents.Instance.VFX4Stop();
                VFXEvents.Instance.VFX5Stop();
            }
        }
        float currentRagePct = (float)currentRage / (float)maxRage;
        onRagePctChanged(currentRagePct);
    }

    // Update is called once per frame
    void Update()
    {
        ModifyRage(Time.deltaTime);
    }

    
    private void ResetRageMode()
    {
        currentRage = startRage;
        rags.fillAmount = 0;
        //Debug.Log("RAGE MODE RESET");
    }
}
