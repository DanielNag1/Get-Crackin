using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    private FiniteStateMachine _finiteStateMachine;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        var playerDetector = gameObject.AddComponent<PlayerDetector>();

        _finiteStateMachine = new FiniteStateMachine();

        //This is where we initalize the different states that the enemies can have
        var idle = new Idle();
        var moveTowardsPlayer = new MoveTowardsPlayer(this, navMeshAgent, animator);
        var runAway = new RunAway(this, navMeshAgent, playerDetector, animator);

        //creating a function inside of the method that we can re-use.
        void AddTransition(IState newState, IState previousState, Func<bool> condition) => _finiteStateMachine.AddTransition(newState, previousState, condition);

        AddTransition(moveTowardsPlayer, idle, HasATarget());

        _finiteStateMachine.AddAnyTransition(runAway, () => playerDetector.PlayerInRange);
        AddTransition(runAway, idle, () => playerDetector.PlayerInRange == false);

        Func<bool> HasATarget() => () => Target != null;  //target is the player which is not implemented yet.


        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }
}
