﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [Header("Slashes")]
    [SerializeField] GameObject slash1;
    [SerializeField] GameObject slash2;
    [SerializeField] GameObject slash3;
    [SerializeField] GameObject slash4;

    void Slash1()
    {
        Quaternion rotation = Quaternion.Euler(30, 90, 0);
        Instantiate(slash1, transform.position, transform.rotation * rotation);
    }
    void Slash2()
    {

        Quaternion rotation = Quaternion.Euler(45, 100, 0);
        Instantiate(slash2, transform.position, transform.rotation * rotation);
    }
    void Slash3()
    {
        Quaternion rotation = Quaternion.Euler(85, 30, 0);
        Instantiate(slash3, transform.position, transform.rotation * rotation);
    }

    void Slash4()
    {
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        Instantiate(slash4, transform.position, transform.rotation * rotation);
    }
}
