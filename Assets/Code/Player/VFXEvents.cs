using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXEvents : MonoBehaviour
{
    [Header("Visual Effects")]
    [SerializeField] VisualEffect VFX1;
    [SerializeField] VisualEffect VFX2;
    [SerializeField] VisualEffect VFX3;


    public static VFXEvents Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }
    private void Update()
    {
        VFX2.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }

    void VFX1Play()
    {
        Vector3 forward = (transform.rotation * Vector3.forward).normalized;

        VFX1.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        VFX1.transform.position += forward *1;
        VFX1.Play();

    }
    void VFX1Stop()
    {
        VFX1.Stop();
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
        VFX3.transform.position += forward *2;
        VFX3.transform.rotation = transform.rotation;
        VFX3.Play();

    }
    void VFX3Stop()
    {
        VFX3.Stop();
    }



}
