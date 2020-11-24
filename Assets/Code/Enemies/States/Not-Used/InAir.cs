using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAir : IState
{
    public void OnEnter()
    {
        //Disable NavMeshAgent
    }

    public void OnExit()
    {
        //Enable NavMeshAgent
        //check to see if grounded again, if not, force enemy to be grounded.
    }

    public void TimeTick()
    {
        //Change the enemiesgravity to be the same as the players.
        
    }
}
