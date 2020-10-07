using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField]
    private int maxHealth = 30;

    public int currentHealth;

    public event Action<float> onHealthPctChanged = delegate { };
    [SerializeField] Animator animator;

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
        if (currentHealth <= 0)
        {
            if(animator.GetBool("isDead")==false)
            {
                animator.SetBool("isDead", true);
            }
        }
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Debug.Log("minus health");
        //    ModifyHealth(-10);
        //}
    }
}
