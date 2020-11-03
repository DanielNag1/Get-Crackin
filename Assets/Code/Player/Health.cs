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
    public bool healthPickedUp;

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
        DrawHealthPickUpVFX();

        if (currentHealth <= 0)
        {
            if (animator.GetBool("isDead") == false)
            {
                animator.SetBool("isDead", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("minus health");
            ModifyHealth(-10);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Health" && currentHealth != maxHealth)
        {
            HealthPickup HP;
            HP = hit.collider.gameObject.GetComponent<HealthPickup>();
            healthPickedUp = true;
            HP.PickupHealth();
            ModifyHealth(HP.healingValue);
            Debug.Log("Gained 10 Health");
        }
    }
    /// <summary>
    /// Triggers the healing particles on the player.
    /// </summary>
    void DrawHealthPickUpVFX()
    {
        if (healthPickedUp)
        {
            VFXEvents.Instance.VFX2Play();
            healthPickedUp = false;
            StartCoroutine(Healing());
        }
    }

    IEnumerator Healing()
    {
        yield return new WaitForSeconds(1.3f);
        VFXEvents.Instance.VFX2Stop();
    }



}
