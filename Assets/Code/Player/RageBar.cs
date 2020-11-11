using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RageBar : MonoBehaviour
{

    [SerializeField]
    public Image RageBarss;
    private float updateSpeedSec = 0.5f;

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
        float preChangedPct = RageBarss.fillAmount;
        float elapsed = 0f;

        while (elapsed < updateSpeedSec)
        {
            elapsed += Time.deltaTime;
            RageBarss.fillAmount = Mathf.Lerp(preChangedPct, pct, elapsed / updateSpeedSec);
            yield return null;
        }
        RageBarss.fillAmount = pct;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }


}
