using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    public event Action<float> onHealthPctChanged = delegate{ };

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }


    private void OnEnable()
    {
        currentHealth = maxHealth;

    }

    public void ModifyHealth(int amount)
    {

        currentHealth += amount;
        float currentHealthPct = (float)currentHealth / (float)maxHealth;
        onHealthPctChanged(currentHealthPct);
    
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("minus health");
            ModifyHealth(-10);
        }
    }
}
