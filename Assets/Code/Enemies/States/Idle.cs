using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{
    #region Variables



    #endregion

    public Idle()
    {

    }

    #region Interface functions

    public void OnEnter()
    {
        Debug.Log("EnterIdle");
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
