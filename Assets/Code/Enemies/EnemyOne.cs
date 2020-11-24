using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOne : MonoBehaviour
{
    #region variables
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
    #endregion

    #region States
    Idle idle;
    MoveTowardsPlayer moveTowardsPlayer;
    AttackPlayer attack;
    Return returnState;
    Lollygagging lollygagging;
    Reload reload;
    EncircleTarget encircleTarget;
    MoveToReloadPosition moveToReloadPosition;
    public Knockback knockback;
    #endregion

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();

        //The States
        #region States
        idle = new Idle(/*this,*/ animator, navMeshAgent);
        moveTowardsPlayer = new MoveTowardsPlayer(this.gameObject, navMeshAgent, animator);
        attack = new AttackPlayer(this.gameObject, navMeshAgent, animator);
        returnState = new Return(this.gameObject, navMeshAgent, animator);
        lollygagging = new Lollygagging(this.gameObject, navMeshAgent, animator);
        reload = new Reload(navMeshAgent, animator);
        encircleTarget = new EncircleTarget(navMeshAgent, animator);
        moveToReloadPosition = new MoveToReloadPosition(navMeshAgent, animator);
        knockback = new Knockback(navMeshAgent, animator);
        #endregion

        //The Transitions (From, To, Condition)
        #region Transitions
        _finiteStateMachine.AddTransition(idle, moveTowardsPlayer, HasATarget());
        _finiteStateMachine.AddTransition(idle, lollygagging, BoredTimer());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, attack, WithinAttackRange());
        _finiteStateMachine.AddTransition(attack, moveTowardsPlayer, OutOfAttackRange());
        _finiteStateMachine.AddTransition(attack, returnState, HasNoTarget());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, returnState, HasNoTarget());
        _finiteStateMachine.AddTransition(returnState, idle, AtSpawn());
        _finiteStateMachine.AddTransition(lollygagging, idle, AtLollygaggingPosition());
        _finiteStateMachine.AddTransition(lollygagging, moveTowardsPlayer, HasATarget());
        _finiteStateMachine.AddTransition(knockback, moveTowardsPlayer, KnockBackFinished());//THIS
        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).
        #endregion

        //The Conditions
        #region conditions
        Func<bool> WithinAttackRange() => () => isWithinAttackRange == true;
        Func<bool> OutOfAttackRange() => () => isWithinAttackRange == false;
        Func<bool> HasATarget() => () => isWithinChaseRange == true && isWithinAttackRange == false;
        Func<bool> HasNoTarget() => () => isWithinChaseRange == false;
        Func<bool> AtSpawn() => () => Vector3.Distance(returnState.spawnPosition, transform.position) < 1.0f;
        Func<bool> AtLollygaggingPosition() => () => Vector3.Distance(lollygagging.targetPos, transform.position) < 1.0f;
        Func<bool> BoredTimer() => () => idle.boringTimer < 0;
        Func<bool> AtReloadPosition() => () => Vector3.Distance(moveToReloadPosition.destination, transform.position) < moveToReloadPosition.interactionRange;
        Func<bool> FinishedReloading() => () => reload.animationTimer < 0;
        Func<bool> KnockBackFinished() => () => knockback.animationTimer < 0;//THIS
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

    public void SetFSMState(string stateName) //THIS
    {
        if (stateName == "idle")
        {
            _finiteStateMachine.SetState(idle);
        }
        else if (stateName == "moveTowardsPlayer")
        {
            _finiteStateMachine.SetState(moveTowardsPlayer);
        }
        else if (stateName == "knockback")
        {
            _finiteStateMachine.SetState(knockback);
        }
    }
}
