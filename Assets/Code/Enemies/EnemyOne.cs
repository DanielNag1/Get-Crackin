using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    private FiniteStateMachine _finiteStateMachine;
    public GameObject _player;

    public int fieldOfView = 500;
    public int viewDistance = 10;

    private Vector3 rayDirection;

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


        Func<bool> HasATarget() => () => Detect();

        Func<bool> AttackTarget() => () => IsWithinAttackDistance();

        Detect();

    }

    private bool Detect()
    {
        RaycastHit hit;
        rayDirection = _player.transform.position - this.transform.position;
        if ((Vector3.Angle(rayDirection, this.transform.forward)) < fieldOfView)
        {
            if(Physics.Raycast(this.transform.position, rayDirection, out hit, viewDistance))
            {
                Debug.Log("Detech Hit");
                return true;
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmos()
    {
        if(_player.transform == null)
        {
            return;
        }

        Debug.DrawLine(transform.position, _player.transform.position, Color.red);

        Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

        Debug.DrawLine(transform.position, frontRayPoint, Color.blue);
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
        if (Vector3.Distance(this.transform.position, _player.transform.position) <= 1)
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
