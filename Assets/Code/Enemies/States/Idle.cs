using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    #region Variables

    private EnemyOne _enemy;
    private Animator _animator;
    private Rigidbody rb;

    #endregion

    public Idle(EnemyOne enemy, Animator animator)
    {
        this._enemy = enemy;
        this._animator = animator;
        rb = _enemy.GetComponent<Rigidbody>();
    }

    #region Interface functions

    public void OnEnter()
    {
      
        //Debug.Log("EnterIdle");
        //Play Idle animation;
    }

    public void OnExit()
    {
        //Debug.Log("Exit Idle");
    }

    public void TimeTick()
    {
        rb.isKinematic = true;
        //Debug.Log("TimeTick Idle");
    }

    #endregion
}
