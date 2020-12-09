using UnityEngine;

/// <summary>
/// OBS!!! Add simple explination!
/// </summary>
public class AnimationEvents : MonoBehaviour
{
    #region Variables
    [Header("Slashes")]
    [SerializeField] GameObject slash1;
    [SerializeField] GameObject slash2;
    [SerializeField] GameObject slash3;
    [SerializeField] GameObject slash4;

    [Header("Rage Slashes")]
    [SerializeField] GameObject rageslash1;
    [SerializeField] GameObject rageslash2;
    #endregion
    #region Methodes
    private void Slash1()
    {
        Quaternion rotation = Quaternion.Euler(30, 90, 0);
        Instantiate(slash1, transform.position, transform.rotation * rotation);
    }
    private void Slash2()
    {

        Quaternion rotation = Quaternion.Euler(45, 100, 0);
        Instantiate(slash2, transform.position, transform.rotation * rotation);
    }
    private void Slash3()
    {
        Quaternion rotation = Quaternion.Euler(85, 30, 0);
        Instantiate(slash3, transform.position, transform.rotation * rotation);
    }

    private void Slash4()
    {
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Instantiate(slash4, transform.position, transform.rotation * rotation);
    }

    private void RageSlash1()
    {
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Instantiate(rageslash1, transform.position, transform.rotation * rotation);
    }
    private void RageSlash2()
    {
        Quaternion rotation = Quaternion.Euler(-90, 90, 0);
        Instantiate(rageslash2, transform.position, transform.rotation * rotation);
    }
    #endregion
}
