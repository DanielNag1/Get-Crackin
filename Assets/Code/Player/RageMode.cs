using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageMode : MonoBehaviour
{

    [SerializeField]
    private int maxRage = 100;
    private int startRage = 0;
    [SerializeField]
    private float TimerSec;
    private float elapsedTime;
    private Image rags;

    public float currentRage;


    public event Action<float> onRagePctChanged = delegate { };
    [SerializeField] private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        currentRage = startRage;
        rags = GetComponentInParent<RageBar>().RageBarss;
    }


    private void OnEnable()
    {
        currentRage = startRage;

    }

    public void ModifyRage(float amount)
    {

        currentRage += amount;
        float currentRagePct = (float)currentRage / (float)maxRage;
        onRagePctChanged(currentRagePct);

    }


    // Update is called once per frame
    void Update()
    {
        
        if (currentRage == maxRage)
        {
            animator.SetBool("Rage Mode", true);
        }

        else if (currentRage <= 0)
        {
            animator.SetBool("Rage Mode", false);
        }
        

        //elapsedTime += Time.deltaTime;



        //if (elapsedTime >= TimerSec)
        //{
        //    elapsedTime = 0;
        //    ResetRageMode();

        //}
    }



    private void ResetRageMode()
    {
        currentRage = startRage;
        rags.fillAmount = 0;


        //Debug.Log("RAGE MODE RESET");

    }
}
