using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyhealth : MonoBehaviour
{

    public int currentHealth;
    public int startHealth;
    // Start is called before the first frame update

    void Start()
    {
        currentHealth = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
           gameObject.active = false;
        }
    }

    public void MakeDamage(int amount)
    {
        currentHealth -= amount;
    }
}
