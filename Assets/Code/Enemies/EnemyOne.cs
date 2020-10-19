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

    RaycastHit hit;
    private Vector3 rayDirection;

    private bool isWithinAttachRange, isWithinChaseRange = false;

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

        Detect();

        Func<bool> HasATarget() => () => isWithinChaseRange;

        Func<bool> AttackTarget() => () => isWithinAttachRange;

        Detect();

    }

    private bool Detect()
    {
        rayDirection = _player.transform.position - this.transform.position;
        if ((Vector3.Angle(rayDirection, this.transform.forward)) < fieldOfView)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RangeBools()
    {
        if (Physics.Raycast(this.transform.position, rayDirection, out hit, 5f))
        {
            isWithinAttachRange = true;
            Debug.Log("ATTACK RANGE");
        }

        if (Physics.Raycast(this.transform.position, rayDirection, out hit, viewDistance))
        {
            isWithinChaseRange = true;
            Debug.Log("CHASE RANGE");
        }
    }


    private void OnDrawGizmos()
    {
        if (_player.transform == null)
        {
            return;
        }

        Debug.DrawLine(transform.position, _player.transform.position, Color.red);

        Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

        Debug.DrawLine(transform.position, frontRayPoint, Color.blue);
    }

    #region Conditions

    //private bool IsWithinChaseDistance()
    //{
    //    if (Vector3.Distance(this.transform.position, _player.transform.position) <= 20 && Vector3.Distance(this.transform.position, _player.transform.position) > 1)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //private bool IsWithinAttackDistance()
    //{
    //    if (Vector3.Distance(this.transform.position, _player.transform.position) <= 1)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    #endregion

    private void FixedUpdate()
    {
        RangeBools();
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }
}
