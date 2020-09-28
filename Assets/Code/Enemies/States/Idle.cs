using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    #region Variables

    private EnemyOne _enemy;

    #endregion

    public Idle(EnemyOne enemy)
    {
        this._enemy = enemy;
    }

    #region Interface functions

    public void OnEnter()
    {
        Debug.Log("EnterIdle");
        //Play Idle animation;
    }

    public void OnExit()
    {
        Debug.Log("Exit Idle");
    }

    public void TimeTick()
    {
        Debug.Log("TimeTick Idle");
    }

    #endregion
}
