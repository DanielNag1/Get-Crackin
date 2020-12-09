using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region Variables
    [SerializeField] private int _maxHealth = 30;
    public int currentHealth;
    public event Action<float> onHealthPctChanged = delegate { };
    [SerializeField] private Animator animator;
    public bool healthPickedUp;
    #endregion
    #region Methods
    void Start()
    {
        currentHealth = _maxHealth;
    }

    private void OnEnable()
    {
        currentHealth = _maxHealth;
    }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        float currentHealthPct = (float)currentHealth / (float)_maxHealth;
        onHealthPctChanged(currentHealthPct);
    }

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
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("minus health");
            ModifyHealth(-10);
        }
#endif
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Health" && currentHealth != _maxHealth)
        {
            HealthPickup HP;
            HP = hit.collider.gameObject.GetComponent<HealthPickup>();
            healthPickedUp = true;
            //OBS! If we want mushroms to make sound when picked up add here.
            HP.PickupHealth();
            ModifyHealth(HP.healingValue);
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
    #endregion
}
