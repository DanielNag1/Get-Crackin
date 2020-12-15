using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float _health;
    private float _lerpTimer;

    public float maxHealth = 10;
    public float chipSpeed = 2f;

    public Image frontHealthBar;
    public Image backHealthBar;
    

    void Start()
    {
        _health = maxHealth;
        
    }

    /// Don't forget to change to Input Manager! This is just for testing!
    
    void Update()
    {
        _health = Mathf.Clamp(_health, 0, maxHealth);
        UpdateHealthUI();

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TakeDamage(Random.Range(5, 10));
        //}

        //if (Input.GetKeyDown(KeyCode.LeftAlt))
        //{
        //    RestoreHealth(Random.Range(5, 10));
        //}





    }

    private void UpdateHealthUI()
    {

        float _fillF = frontHealthBar.fillAmount;
        float _fillB = backHealthBar.fillAmount;
        float _fractionOfHealth = _health / maxHealth;

        if(_fillB > _fractionOfHealth)
        {
            frontHealthBar.fillAmount = _fractionOfHealth;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float _percentComplete = _lerpTimer / chipSpeed;
            _percentComplete = _percentComplete * _percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(_fillB, _fractionOfHealth, _percentComplete);
        }

        if(_fillF < _fractionOfHealth)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = _fractionOfHealth;
            _lerpTimer += Time.deltaTime;
            float _percentComplete = _lerpTimer / chipSpeed;
            _percentComplete = _percentComplete * _percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(_fillF, backHealthBar.fillAmount, _percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        _health += healAmount;
        _lerpTimer = 0f;
    }

}
