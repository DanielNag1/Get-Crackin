using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterCrate : MonoBehaviour
{
    #region Singleton
    public static ShatterCrate Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public GameObject crate_shattered;


    public void DestroyCrate()
    {
        Instantiate(crate_shattered, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
