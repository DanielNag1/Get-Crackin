using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    private FiniteStateMachine _finiteStateMachine = new FiniteStateMachine();
    private GameObject _player;

    public int fieldOfView = 90;
    public float viewDistance = 10f;

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
    Return returnState;
    Lollygagging lollygagging;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();

        //playerLayerMask = 1 << layerMaskValue;
        //playerLayerMask = ~playerLayerMask;

        //The States
        #region States
        idle = new Idle(/*this,*/ animator, navMeshAgent);
        moveTowardsPlayer = new MoveTowardsPlayer(this, navMeshAgent, animator);
        attack = new AttackPlayer(this, navMeshAgent, animator);
        returnState = new Return(this, navMeshAgent, animator);
        lollygagging = new Lollygagging(this, navMeshAgent, animator);
        #endregion

        //The Transitions (From, To, Condition)
        #region Transitions
        _finiteStateMachine.AddTransition(idle, moveTowardsPlayer, HasATarget());
        _finiteStateMachine.AddTransition(idle, lollygagging, BoredTimer());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, attack, AttackTarget());
        _finiteStateMachine.AddTransition(attack, moveTowardsPlayer, OutOfAttackRange());
        _finiteStateMachine.AddTransition(attack, returnState, HasNoTarget());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, returnState, HasNoTarget());
        _finiteStateMachine.AddTransition(returnState, idle, AtSpawn());
        _finiteStateMachine.AddTransition(lollygagging, idle, AtTargetPosition());
        _finiteStateMachine.AddTransition(lollygagging, moveTowardsPlayer, HasATarget());
        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).
        #endregion

        //The Conditions
        #region conditions
        Func<bool> AttackTarget() => () => isWithinAttackRange == true;
        Func<bool> OutOfAttackRange() => () => isWithinAttackRange == false;
        Func<bool> HasATarget() => () => isWithinChaseRange == true && isWithinAttackRange == false;
        Func<bool> HasNoTarget() => () => isWithinChaseRange == false;
        Func<bool> AtSpawn() => () => Vector3.Distance(returnState.targetPos, transform.position) < 1.0f;
        Func<bool> AtTargetPosition() => () => Vector3.Distance(lollygagging.targetPos, transform.position) < 1.0f;
        Func<bool> BoredTimer() => () => idle.boringTimer < 0;
        #endregion

        Detect();
    }

    private bool Detect()
    {
        #region Don't touch
        rayDirection = _player.transform.position - this.transform.position;
        float AngleBetweenFacingAndPlayerpos = 0f;
        if (_player.transform.position.x < this.transform.position.x)
        {
            AngleBetweenFacingAndPlayerpos = 360.0f - Vector3.Angle(Vector3.forward, rayDirection);//player in third or fourth quadrant
        }
        else
        {
            AngleBetweenFacingAndPlayerpos = Vector3.Angle(Vector3.forward, rayDirection);//player in first or second quadrant
        }
        float AgentFacing = Quaternion.LookRotation(transform.forward).eulerAngles.y;//Heading


        float A = (Math.Abs(AgentFacing - fieldOfView) % 360);
        float B = AgentFacing + fieldOfView;
        float C = ((AgentFacing + fieldOfView) % 360);
        float D = AgentFacing - fieldOfView;
        float E = 360 + D;
        float alpha = AngleBetweenFacingAndPlayerpos;

        if (A < alpha && alpha < B || 360 < B && alpha < C || D < 0 && E < alpha || D < 0 && alpha < C)
        {
            return true;
        }
        else
        {
            return false;
        }
        #endregion
    }

    private void RangeBools()
    {
        Physics.Raycast(this.transform.position, rayDirection, out hit, Mathf.Infinity, playerLayerMask);
        if (Detect())
        {
            if (hit.distance <= attackRange)
            {
                isWithinAttackRange = true;
            }
            else
            {
                isWithinAttackRange = false;
            }
            if (hit.distance <= viewDistance)
            {
                isWithinChaseRange = true;
            }
            else
            {
                isWithinChaseRange = false;
            }
        }
        else
        {
            isWithinAttackRange = false;
            isWithinChaseRange = false;
        }
    }

    private void FixedUpdate()
    {
        RangeBools();
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }

    public void Reset()
    {
        Detect();
    }
}
