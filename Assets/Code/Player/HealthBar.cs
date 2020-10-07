using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    private Image foregroundImage;
    private float updateSpeedSec = 0.5f;

    private void Awake()
    {
        GetComponentInParent<Health>().onHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        //foregroundImage.fillAmount = pct;

        StartCoroutine(ChangeToPct(pct));
    
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangedPct = foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSec)
        {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(preChangedPct, pct, elapsed / updateSpeedSec);
            yield return null;
        }
        foregroundImage.fillAmount = pct;
    }

    // Start is called before the first frame update
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
