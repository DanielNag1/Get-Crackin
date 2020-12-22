using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image _foregroundImage;
    private float _updateSpeedSec = 0.5f;
    #endregion
    #region Methdos
    private void Awake()
    {
        GetComponentInParent<Health>().onHealthPctChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangedPct = _foregroundImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < _updateSpeedSec)
        {
            elapsed += Time.deltaTime;
            _foregroundImage.fillAmount = Mathf.Lerp(preChangedPct, pct, elapsed / _updateSpeedSec);
            yield return null;
        }
        _foregroundImage.fillAmount = pct;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
    #endregion
}
