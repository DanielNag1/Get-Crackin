using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    [SerializeField]
    private int _maxHealth = 30;
    public int currentHealth;
    public event Action<float> onHealthPctChanged = delegate { };
    [SerializeField] Animator animator;
    public bool healthPickedUp;
   
    private float _lerpTimer;
    public Image frontHealthBar;
    public Image backHealthBar;
    public float chipSpeed = 2f;



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
        _lerpTimer = 0f;

    }


    void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
        DrawHealthPickUpVFX();
        float currentHealthPct = (float)currentHealth / (float)_maxHealth;

        UpdateHealthUI(currentHealthPct);
        

        if (currentHealth <= 0)
        {
            if (animator.GetBool("isDead") == false)
            {
                animator.SetBool("isDead", true);
            }
        }
        
    }

    

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Health" && currentHealth != _maxHealth)
        {
            HealthPickup HP;
            HP = hit.collider.gameObject.GetComponent<HealthPickup>();
            healthPickedUp = true;
            HP.PickupHealth();
            ModifyHealth(HP.healingValue);
        }
    }

    private void UpdateHealthUI(float pct)
    {
        float _fillF = frontHealthBar.fillAmount;
        float _fillB = backHealthBar.fillAmount;
        

        if (_fillB > pct)
        {
            frontHealthBar.fillAmount = pct;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float _percentComplete = _lerpTimer / chipSpeed;
            _percentComplete = _percentComplete * _percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(_fillB, pct, _percentComplete);
        }

        if (_fillF < pct)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = pct;
            _lerpTimer += Time.deltaTime;
            float _percentComplete = _lerpTimer / chipSpeed;
            _percentComplete = _percentComplete * _percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(_fillF, backHealthBar.fillAmount, _percentComplete);
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
