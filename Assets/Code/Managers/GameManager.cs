using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private LockToTarget LockToTarget;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        InputBuffer.Instance.LockToTarget = LockToTarget;
        InputBuffer.Instance.animator = animator;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager.Instance.Update();
        SoundEngine.Instance.Update();
    }
}
