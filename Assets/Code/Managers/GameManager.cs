using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LockToTarget LockToTarget;
    // Start is called before the first frame update
    void Start()
    {
        InputBuffer.Instance.LockToTarget = LockToTarget;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager.Instance.Update();
    }
}
