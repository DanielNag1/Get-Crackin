using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXEvents : MonoBehaviour
{
    [SerializeField] VisualEffect VFX1;

    void VFX1Play()
    {
        VFX1.transform.position = new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z);
        VFX1.Play();
    }
    void VFX1Stop()
    {
        VFX1.Stop();
    }

}
