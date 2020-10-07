using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    [SerializeField] GameObject slash1;

    void Slash1()
    {
        Quaternion rotation = Quaternion.Euler(30, 90, 0);
        Instantiate(slash1, transform.position, transform.rotation * rotation);
    }
}
