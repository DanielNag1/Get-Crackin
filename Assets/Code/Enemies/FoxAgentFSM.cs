using System;
using UnityEngine;
using UnityEngine.AI;



public class FoxAgentFSM : MonoBehaviour
{
    #region variables
    [Flags]
    private enum CombatRole
    {
        None = 0b_0000,  // 0
        Melee = 0b_0001,  // 1
        Ranged = 0b_0010,  // 2
    }
    private FiniteStateMachine _finiteStateMachine = new FiniteStateMachine();
    private GameObject _player;
    private RaycastHit hit;
    private Vector3 rayDirection;
    private int layerMaskValue = 10;
    public int fieldOfView = 120;
    public float viewDistance = 20f;
    public float talkingDistance = 15f;
    public LayerMask playerLayerMask;
    public float highOffset = 0.25f;
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 3.5f;
    //---------------------------
    private bool isWithinAttackRange = false;
    private CombatRole combatRole = CombatRole.Melee; //OBS!!! Change to None!
    public bool attacking = false;
    public float meleeAttackRange = 1.61f;
    public float rangedAttackRange = 10;
    public float circleRadius = 4;
    #endregion

    #region States
    private Idle idle;
    private Lollygagging lollygagging;
    #region CombatBlock
    private EnterCombat enterCombat;
    private CombatIdle combatIdle;
    private MoveToReloadPosition moveToReloadPosition;
    private Reload reload;
    private MoveToCircle moveToCircle;
    private EncircleTarget encircleTarget;
    private MoveTowardsPlayer moveTowardsPlayer;
    private Return returnState;
    private AttackPlayerMelee attackPlayerMelee;
    private AttackPlayerRanged attackPlayerRanged;
    public Knockback knockback;
    #endregion
    #endregion

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();

        //The States
        #region States
        idle = new Idle(animator, navMeshAgent);
        lollygagging = new Lollygagging(this.gameObject, navMeshAgent, animator);
        enterCombat = new EnterCombat(this.gameObject);
        combatIdle = new CombatIdle(navMeshAgent, animator);
        moveToReloadPosition = new MoveToReloadPosition(navMeshAgent, animator);
        reload = new Reload(navMeshAgent, animator);
        moveToCircle = new MoveToCircle(this.gameObject, navMeshAgent, animator);
        encircleTarget = new EncircleTarget(navMeshAgent, animator);
        moveTowardsPlayer = new MoveTowardsPlayer(this.gameObject, navMeshAgent, animator);
        returnState = new Return(this.gameObject, navMeshAgent, animator);
        attackPlayerMelee = new AttackPlayerMelee(this.gameObject, navMeshAgent, animator);
        attackPlayerRanged = new AttackPlayerRanged(this.gameObject, navMeshAgent, animator);
        knockback = new Knockback(navMeshAgent, animator);
        #endregion

        //The Transitions (From, To, Condition)
        #region Transitions
        _finiteStateMachine.AddTransition(idle, lollygagging, IdleTimer());
        _finiteStateMachine.AddTransition(idle, enterCombat, PlayerDetected());
        _finiteStateMachine.AddTransition(lollygagging, idle, LollygaggingPositionReached());
        _finiteStateMachine.AddTransition(lollygagging, enterCombat, PlayerDetected());
        _finiteStateMachine.AddTransition(enterCombat, combatIdle, FinishedEnteringCombat());
        _finiteStateMachine.AddTransition(combatIdle, moveToCircle, AssignedMeleeCombatRole());
        _finiteStateMachine.AddTransition(combatIdle, moveToReloadPosition, AssignedRangedCombatRole());
        _finiteStateMachine.AddTransition(moveToReloadPosition, reload, WithinReloadInteractRange());
        _finiteStateMachine.AddTransition(reload, moveToCircle, FinishedReloading());
        _finiteStateMachine.AddTransition(moveToCircle, returnState, CombatHasEnded());
        _finiteStateMachine.AddTransition(moveToCircle, encircleTarget, CirclingTarget());
        _finiteStateMachine.AddTransition(encircleTarget, moveToCircle, ToFarFromCircle());
        _finiteStateMachine.AddTransition(encircleTarget, moveTowardsPlayer, Attacking());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, combatIdle, TargetToFarAwayToAttack());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, returnState, CombatHasEnded());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, attackPlayerMelee, TargetWithinMeleeAttackRange());
        _finiteStateMachine.AddTransition(moveTowardsPlayer, attackPlayerRanged, TargetWithinRangedAttackRange());
        _finiteStateMachine.AddTransition(attackPlayerMelee, combatIdle, FinishedMeleeAttack());
        _finiteStateMachine.AddTransition(attackPlayerRanged, combatIdle, FinishedRangedAttack());
        _finiteStateMachine.AddTransition(returnState, idle, AgentReturnedToSpawnPosition());
        _finiteStateMachine.AddAnyTransition(knockback, AgentHitByPlayerAttack());
        _finiteStateMachine.AddTransition(knockback, combatIdle, FinishedKnockbackAnimation());
        _finiteStateMachine.SetState(idle);  //setting the default state (the initial state).
        #endregion

        //The Conditions
        #region conditions
        Func<bool> IdleTimer() => () => idle.boringTimer < 0;
        Func<bool> LollygaggingPositionReached() => () => Vector3.Distance(lollygagging.targetPos, transform.position) < 0.1f;
        Func<bool> PlayerDetected() => () => true;
        Func<bool> FinishedEnteringCombat() => () => enterCombat.finishedEnteringCombat;
        Func<bool> AssignedMeleeCombatRole() => () => combatRole == CombatRole.Melee;
        Func<bool> AssignedRangedCombatRole() => () => combatRole == CombatRole.Ranged;
        Func<bool> WithinReloadInteractRange() => () => Vector3.Distance(moveToReloadPosition.destination, transform.position) < moveToReloadPosition.interactionRange;
        Func<bool> FinishedReloading() => () => reload.animationTimer < 0;
        Func<bool> CombatHasEnded() => () => (Vector3.Distance(_player.transform.position, transform.position) > viewDistance) /*&& combatManager.distanceClosestOtherAgent() > talkingDistance*/;
        Func<bool> CirclingTarget() => () => Vector3.Distance(_player.transform.position, transform.position) < circleRadius;
        Func<bool> ToFarFromCircle() => () => Vector3.Distance(_player.transform.position, transform.position) > (circleRadius + circleRadius / 10);
        Func<bool> Attacking() => () => attacking;
        Func<bool> TargetToFarAwayToAttack() => () => Vector3.Distance(_player.transform.position, transform.position) > circleRadius * 1.4f;
        Func<bool> TargetWithinMeleeAttackRange() => () => Vector3.Distance(_player.transform.position, transform.position) < meleeAttackRange && combatRole == CombatRole.Melee;
        Func<bool> TargetWithinRangedAttackRange() => () => Vector3.Distance(_player.transform.position, transform.position) < meleeAttackRange && combatRole == CombatRole.Ranged;
        Func<bool> FinishedMeleeAttack() => () => attackPlayerMelee.attackAnimationDurationTimer < 0;
        Func<bool> FinishedRangedAttack() => () => attackPlayerRanged.attackAnimationDurationTimer < 0;
        Func<bool> AgentHitByPlayerAttack() => () => knockback.GotHit;
        Func<bool> FinishedKnockbackAnimation() => () => knockback.animationTimer < 0;
        Func<bool> AgentReturnedToSpawnPosition() => () => Vector3.Distance(returnState.spawnPosition, transform.position) < 1.0f;
        #endregion

        Detect();
    }

    /// <summary>
    /// Checks if player is within agents view cone (FOV). Returns true if player is within FOV.
    /// </summary>
    /// <returns></returns>
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
            if (hit.distance <= meleeAttackRange)
            {
                isWithinAttackRange = true;
            }
            else
            {
                isWithinAttackRange = false;
            }
            if (hit.distance <= viewDistance)
            {
                //isWithinChaseRange = true;
            }
            else
            {
                //isWithinChaseRange = false;
            }
        }
        else
        {
            isWithinAttackRange = false;
            //isWithinChaseRange = false;
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
