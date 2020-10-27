using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : IState
{
    public void OnEnter()
    {
        //Play Dodge animation
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void TimeTick()
    {
        //Move in a direction away from player.
    }
}
