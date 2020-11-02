using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    private FiniteStateMachine _finiteStateMachine = new FiniteStateMachine();
    private GameObject _player;

    public int fieldOfView = 500;
    public int viewDistance = 10;

    RaycastHit hit;
    private Vector3 rayDirection;
    private int layerMaskValue = 10;
    private bool isWithinAttackRange, isWithinChaseRange = false;

    public bool isGrounded;
    public float groundedHeight = 0.5f;
    public float attackRange = 1;
    public LayerMask groundLayer;
    public LayerMask playerLayerMask;
    public float highOffset = 0.25f;
    public NavMeshAgent navMeshAgent;
    Idle idle;
    MoveTowardsPlayer moveTowardsPlayer;
    AttackPlayer attack;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();

        playerLayerMask = 1 << layerMaskValue;
        playerLayerMask = ~playerLayerMask;

        //The States
        #region States
        idle = new Idle(/*this,*/ animator, navMeshAgent);
        moveTowardsPlayer = new MoveTowardsPlayer(this, navMeshAgent, animator);
        attack = new AttackPlayer(this, navMeshAgent, animator);
        #endregion

        //The Transitions (From, To, Condition)
        #region Transitions
        _finiteStateMachine.AddTransition(idle, moveTowardsPlayer, HasATarget());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, attack, AttackTarget());
        _finiteStateMachine.AddTransition(attack, moveTowardsPlayer, OutOfAttackRange());
        _finiteStateMachine.AddTransition(attack, idle, HasNoTarget());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, idle, HasNoTarget());
        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).
        #endregion

        //The Conditions
        #region conditions
        Func<bool> AttackTarget() => () => isWithinAttackRange == true;
        Func<bool> OutOfAttackRange() => () => isWithinAttackRange == false;
        Func<bool> HasATarget() => () => isWithinChaseRange == true && isWithinAttackRange == false;
        Func<bool> HasNoTarget() => () => isWithinChaseRange == false;
        #endregion

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

        if (Physics.Raycast(this.transform.position, rayDirection, attackRange, playerLayerMask))
        {
            Debug.DrawRay(this.transform.position, rayDirection, Color.green);
            isWithinAttackRange = true;
        }
        else
        {
            isWithinAttackRange = false;
        }

        if (Physics.Raycast(this.transform.position, rayDirection, out hit, viewDistance))
        {
            isWithinChaseRange = true;
        }
        else
        {
            isWithinChaseRange = false;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    if (_player.transform == null)
    //    {
    //        return;
    //    }
    //    Debug.DrawRay(this.transform.position, rayDirection, Color.cyan);
    //    Debug.DrawLine(transform.position, _player.transform.position, Color.red);

    //    Vector3 frontRayPoint = transform.position + (transform.forward * viewDistance);

    //    Debug.DrawLine(transform.position, frontRayPoint, Color.blue);
    //}

    private void FixedUpdate()
    {
        RangeBools();
        Detect();
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }

}
