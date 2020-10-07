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

    private int currentRage;


    public event Action<float> onRagePctChanged = delegate { };



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

    public void ModifyRage(int amount)
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

            elapsedTime += Time.deltaTime;
            EnableRageMode();


            if (elapsedTime >= TimerSec)
            {
                elapsedTime = 0;
                ResetRageMode();

            }
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Rage increase");
            ModifyRage(25);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            currentRage = startRage;

            rags.fillAmount = 0;


            Debug.Log("RAGE MODE RESET");
        }



    }

    private void EnableRageMode()
    {
        Debug.Log("RAGE MODE ENABLE");
    }

    private void ResetRageMode()
    {
        currentRage = startRage;
        rags.fillAmount = 0;


        Debug.Log("RAGE MODE RESET");

    }
}
