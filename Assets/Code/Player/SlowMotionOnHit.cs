using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionOnHit : MonoBehaviour
{
    private float _slowMotionTimer;
    private float _slowMotionAmount;
    public static SlowMotionOnHit Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_slowMotionTimer > 0)
        {
            Time.timeScale = _slowMotionAmount;
            _slowMotionTimer -= 0.001f;
        }

        if (_slowMotionTimer <= 0)
        {
            Time.timeScale = 1;
        }
    }

    public void StartSlowMotion(float amount, float timer)
    {
        _slowMotionAmount = amount;
        _slowMotionTimer = timer;
    }


}
