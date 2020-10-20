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

    public bool isGrounded;
    public float groundedHeight = 0.5f;
    public LayerMask groundLayer;
    public float highOffset = 0.25f;

    Idle idle;
    MoveTowardsPlayer moveTowardsPlayer;
    AttackPlayer attack;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();

        //This is where we initalize the different states that the enemy can have:
        //var idle = new Idle(this, animator);
        //var moveTowardsPlayer = new MoveTowardsPlayer(this, navMeshAgent, animator);
        //var runAway = new RunAway(this, navMeshAgent, animator);
        //var attack = new AttackPlayer(this, navMeshAgent, animator);

        idle = new Idle(this, animator);
        moveTowardsPlayer = new MoveTowardsPlayer(this, navMeshAgent, animator);
        attack = new AttackPlayer(this, navMeshAgent, animator);

        _finiteStateMachine = new FiniteStateMachine();

        Detect();

        //The states with conditions
        _finiteStateMachine.AddAnyTransition(moveTowardsPlayer, HasATarget());

        _finiteStateMachine.AddAnyTransition(attack, AttackTarget());

       

        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).

        Func<bool> AttackTarget() => () => isWithinAttachRange == true;
        Func<bool> HasATarget() => () => isWithinChaseRange == true && isWithinAttachRange == false;
    }


    public void GroundCheck()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + highOffset, transform.position.z), Vector3.down, groundedHeight + highOffset, groundLayer))
        {
            isGrounded = true;
            //Debug.Log("Grounded TRUE");
        }
        else
        {
            isGrounded = false;
            //Debug.Log("Grounded FALSE");
        }
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
        else
        {
            isWithinAttachRange = false;
        }

        if (Physics.Raycast(this.transform.position, rayDirection, out hit, viewDistance))
        {
            isWithinChaseRange = true;
            Debug.Log("CHASE RANGE");
        }
        else
        {
            isWithinChaseRange = false;
        }
    }

    private void FixedUpdate()
    {
        RangeBools();
        GroundCheck();
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }

    #region Gizmos

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

    #endregion
}
