using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXEvents : MonoBehaviour
{
    #region Variables
    [Header("Visual Effects")]
    [SerializeField] private VisualEffect VFX1;
    [SerializeField] private VisualEffect VFX2;
    [SerializeField] private VisualEffect VFX3;
    [SerializeField] private VisualEffect VFX4;
    [SerializeField] private VisualEffect VFX5;
    [SerializeField] private VisualEffect VFX6;
    public static VFXEvents Instance { get; private set; }

    #endregion
    #region Methods
    private void Start()
    {
        Instance = this;
        VFX4.gameObject.SetActive(false);
        VFX5.gameObject.SetActive(false);
    }

    private void Update()
    {
        VFX2.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        VFX4.transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
        VFX5.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public void VFX1Play()
    {
        Vector3 forward = (transform.rotation * Vector3.forward).normalized;
        VFX1.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        VFX1.transform.position += forward * 1;
        var VisualEffect = Instantiate(VFX1);
        StartCoroutine(VFX1Timer(VisualEffect));
    }

    IEnumerator VFX1Timer(VisualEffect vfx)
    {
        vfx.Play();
        yield return new WaitForSeconds(0.3f);
        vfx.Stop();
        yield return new WaitForSeconds(3f);
        Destroy(vfx.gameObject);
    }
    IEnumerator VFX6Timer(VisualEffect vfx)
    {
        vfx.Play();
        yield return new WaitForSeconds(1f);
        vfx.Stop();
        yield return new WaitForSeconds(1f);
        Destroy(vfx.gameObject);
    }

    public void VFX2Play()
    {
        VFX2.Play();
    }
    public void VFX2Stop()
    {
        VFX2.Stop();
    }

    void VFX3Play()
    {
        Vector3 forward = (transform.rotation * Vector3.forward).normalized;
        VFX3.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        VFX3.transform.position += forward * 2;
        VFX3.transform.rotation = transform.rotation;
        VFX3.Play();

    }
    void VFX3Stop()
    {
        VFX3.Stop();
    }

    public void VFX4Play()
    {
        VFX4.gameObject.SetActive(true);
    }
    public void VFX4Stop()
    {
        VFX4.gameObject.SetActive(false);
    }

    public void VFX5Play()
    {
        VFX5.gameObject.SetActive(true);
    }
    public void VFX5Stop()
    {
        VFX5.gameObject.SetActive(false);
    }

    public void VFX6Play(Transform enemy)
    {
        VFX6.transform.position = enemy.position;
        var VisualEffect = Instantiate(VFX6);
        StartCoroutine(VFX6Timer(VisualEffect));
    }
    #endregion
}
