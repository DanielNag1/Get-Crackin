using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    //private Image foregroundImage;
    private float updateSpeedSec = 0.5f;

    public Image frontHealthBar;
    public Image backHealthBar;

    //private float _health;
    public float _lerpTimer;

    //public float maxHealth = 10;
    public float chipSpeed = 2f;

    private void Awake()
    {
        GetComponentInParent<Health>().onHealthPctChanged += HandleHealthChanged;
        
    }

    private void HandleHealthChanged(float pct)
    {
        //foregroundImage.fillAmount = pct;

        StartCoroutine(UpdateHealthUI(pct));
        //_lerpTimer = 0f;

        //UpdateHealthUI(pct);
    
    }

    private IEnumerator UpdateHealthUI(float pct)
    {
        float _fillF = frontHealthBar.fillAmount;
        float _fillB = backHealthBar.fillAmount;
        //pct = _health / maxHealth;
        

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

        yield return null;
        //_lerpTimer = 0f;
    }

    //private IEnumerator ChangeToPct(float pct)
    //{
    //    float preChangedPct = foregroundImage.fillAmount;
    //    float elapsed = 0f;

    //    while (elapsed < updateSpeedSec)
    //    {
    //        elapsed += Time.deltaTime;
    //        foregroundImage.fillAmount = Mathf.Lerp(preChangedPct, pct, elapsed / updateSpeedSec);
    //        yield return null;
    //    }
    //    foregroundImage.fillAmount = pct;
    //}

    // Start is called before the first frame update
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
