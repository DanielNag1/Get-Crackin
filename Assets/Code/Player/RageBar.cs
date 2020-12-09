using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{
    #region Variables
    [SerializeField] public Image rageBar;
    private float _updateSpeedSec = 0.5f;
    #endregion
    #region Methods
    private void Awake()
    {
        GetComponentInParent<RageMode>().onRagePctChanged += HandleRageChanged;
    }

    private void HandleRageChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangedPct = rageBar.fillAmount;
        float elapsed = 0f;

        while (elapsed < _updateSpeedSec)
        {
            elapsed += Time.deltaTime;
            rageBar.fillAmount = Mathf.Lerp(preChangedPct, pct, elapsed / _updateSpeedSec);
            yield return null;
        }
        rageBar.fillAmount = pct;
    }
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
    #endregion
}
