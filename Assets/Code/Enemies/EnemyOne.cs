using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    private FiniteStateMachine _finiteStateMachine;

    [SerializeField]
    public GameObject PlayerPrefab;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        var playerDetector = gameObject.AddComponent<PlayerDetector>();

        //This is where we initalize the different states that the enemy can have:
        var idle = new Idle(this, animator);
        var moveTowardsPlayer = new MoveTowardsPlayer(this, navMeshAgent, animator);
        var runAway = new RunAway(this, navMeshAgent, playerDetector, animator);

        _finiteStateMachine = new FiniteStateMachine();

        //creating a function inside of the method that we can re-use.
        void AddTransition(IState newState, IState previousState, Func<bool> condition) => _finiteStateMachine.AddTransition(newState, previousState, condition);

        AddTransition(moveTowardsPlayer, idle, HasATarget());
        AddTransition(runAway, idle, () => playerDetector.PlayerInRange == false);

        _finiteStateMachine.AddAnyTransition(runAway, () => playerDetector.PlayerInRange);


        Func<bool> HasATarget() => () => (Vector3.Distance(this.transform.position, PlayerPrefab.transform.position) < 10);

        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }
}
