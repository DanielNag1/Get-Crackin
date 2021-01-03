﻿using System.Collections;
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

    #region Variables
    public GameObject crate_shattered;
    private GameObject clone;
    private float waitToDestroy = 5f;
    #endregion
    /// <summary>
    /// Creates a clone of the shattered object, destroys the whole one and destroys the shattered object after 5 seconds.
    /// </summary>
    public void DestroyCrate()
    {
        clone = Instantiate(crate_shattered, transform.position, transform.rotation);
        Destroy(gameObject);
        Destroy(clone, waitToDestroy);
    }

}
