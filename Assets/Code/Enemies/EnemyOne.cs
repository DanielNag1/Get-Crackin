using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    private FiniteStateMachine _finiteStateMachine;
    public GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();

        //This is where we initalize the different states that the enemy can have:
        var idle = new Idle(this, animator);
        var moveTowardsPlayer = new MoveTowardsPlayer(this, navMeshAgent, animator);
        var runAway = new RunAway(this, navMeshAgent, animator);
        var attack = new AttackPlayer(this, navMeshAgent, animator);

        _finiteStateMachine = new FiniteStateMachine();

        //The states with conditions
        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).

        _finiteStateMachine.AddAnyTransition(moveTowardsPlayer, HasATarget());

        _finiteStateMachine.AddAnyTransition(attack, AttackTarget());


        Func<bool> HasATarget() => () => IsWithinChaseDistance();

        Func<bool> AttackTarget() => () => IsWithinAttackDistance();

    }

    #region Conditions

    private bool IsWithinChaseDistance()
    {
        if (Vector3.Distance(this.transform.position, _player.transform.position) <= 20 && Vector3.Distance(this.transform.position, _player.transform.position) > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsWithinAttackDistance()
    {
        if(Vector3.Distance(this.transform.position, _player.transform.position) <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    private void FixedUpdate()
    {
        IsWithinChaseDistance();
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }
}
