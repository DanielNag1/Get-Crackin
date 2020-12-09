using System;
using UnityEngine;
using UnityEngine.AI;

public class FoxAgentFSM : MonoBehaviour
{
    #region variables
    [Flags]
    public enum CombatRole
    {
        None = 0b_0000,  // 0
        Melee = 0b_0001,  // 1
        Ranged = 0b_0010,  // 2
    }
    private FiniteStateMachine _finiteStateMachine = new FiniteStateMachine();
    public GameObject player;
    private RaycastHit _hit;
    private Vector3 _rayDirection;
    public int fieldOfView = 120;
    public float viewDistance = 20f;
    public float radiusOfHearing = 5f;
    public float talkingDistance = 15f;
    public LayerMask playerLayerMask;
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 3.5f;
    //---------------------------
    private bool _insideMaxViewDistance = false;
    public CombatRole combatRole = CombatRole.None;
    public bool attacking = false;
    public float meleeAttackRange = 1.61f;
    public float rangedAttackRange = 10;
    public float circleRadius = 4;
    public Vector3 destination;
    public int squareID;
    public Vector3 squareNormalisedPosition;
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
    private MoveToWithinAttackRange _moveToWithinAttackRange;
    private Return returnState;
    private AttackPlayerMelee attackPlayerMelee;
    private AttackPlayerRanged attackPlayerRanged;
    public Knockback knockback;
    #endregion
    #endregion

    #region Methods
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();
        StatesInitialise(navMeshAgent, animator);
        TransitionsAndConditionsInitialise();
    }

    private void StatesInitialise(NavMeshAgent navMeshAgent, Animator animator)
    {
        //The States
        idle = new Idle(animator, navMeshAgent);
        lollygagging = new Lollygagging(this.gameObject, navMeshAgent, animator);
        enterCombat = new EnterCombat(this.gameObject, talkingDistance);
        combatIdle = new CombatIdle(animator);
        moveToReloadPosition = new MoveToReloadPosition(navMeshAgent, animator);
        reload = new Reload(navMeshAgent, animator);
        moveToCircle = new MoveToCircle(this.gameObject, navMeshAgent, animator);
        encircleTarget = new EncircleTarget(this.gameObject, navMeshAgent, animator);
        _moveToWithinAttackRange = new MoveToWithinAttackRange(player, navMeshAgent, animator);
        returnState = new Return(this.gameObject, navMeshAgent, animator);
        attackPlayerMelee = new AttackPlayerMelee(this.gameObject, navMeshAgent, animator);
        attackPlayerRanged = new AttackPlayerRanged(this.gameObject, navMeshAgent, animator);
        knockback = new Knockback(navMeshAgent, animator);
    }

    private void TransitionsAndConditionsInitialise()
    {
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
        _finiteStateMachine.AddTransition(encircleTarget, _moveToWithinAttackRange, Attacking());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, combatIdle, TargetToFarAwayToAttack());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, returnState, CombatHasEnded());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, attackPlayerMelee, TargetWithinMeleeAttackRange());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, attackPlayerRanged, TargetWithinRangedAttackRange());
        _finiteStateMachine.AddTransition(attackPlayerMelee, combatIdle, FinishedMeleeAttack());
        _finiteStateMachine.AddTransition(attackPlayerRanged, combatIdle, FinishedRangedAttack());
        _finiteStateMachine.AddTransition(returnState, idle, AgentReturnedToSpawnPosition());
        _finiteStateMachine.AddAnyTransition(knockback, AgentHitByPlayerAttack());
        _finiteStateMachine.AddTransition(knockback, combatIdle, FinishedKnockbackAnimation());
        SetFSMState("idle");  //setting the default state (the initial state).
        #endregion

        //The Conditions
        #region conditions
        Func<bool> IdleTimer() => () => idle.boringTimer < 0;
        Func<bool> LollygaggingPositionReached() => () => Vector3.Distance(lollygagging.targetPos, transform.position) < 0.1f;
        Func<bool> PlayerDetected() => () => CanThisAgentOrOtherAgentWithinTalkingDistanceSeeThePlayer();
        Func<bool> FinishedEnteringCombat() => () => enterCombat.finishedEnteringCombat;
        Func<bool> AssignedMeleeCombatRole() => () => combatRole == CombatRole.Melee;
        Func<bool> AssignedRangedCombatRole() => () => combatRole == CombatRole.Ranged;
        Func<bool> WithinReloadInteractRange() => () => Vector3.Distance(moveToReloadPosition.destination, transform.position) < moveToReloadPosition.interactionRange;
        Func<bool> FinishedReloading() => () => reload.animationTimer < 0;
        Func<bool> CombatHasEnded() => () => !CanThisAgentOrOtherAgentWithinTalkingDistanceSeeThePlayer();
        Func<bool> CirclingTarget() => () => Vector2.Distance(new Vector2(destination.x, destination.z), new Vector2(transform.position.x, transform.position.z)) < 0.5f;
        Func<bool> ToFarFromCircle() => () => Vector2.Distance(new Vector2(destination.x, destination.z), new Vector2(transform.position.x, transform.position.z)) > 1f;
        Func<bool> Attacking() => () => attacking;
        Func<bool> TargetToFarAwayToAttack() => () => Vector3.Distance(player.transform.position, transform.position) > circleRadius * 1.4f;
        Func<bool> TargetWithinMeleeAttackRange() => () => Vector3.Distance(player.transform.position, transform.position) < meleeAttackRange && combatRole == CombatRole.Melee;
        Func<bool> TargetWithinRangedAttackRange() => () => Vector3.Distance(player.transform.position, transform.position) < rangedAttackRange && combatRole == CombatRole.Ranged;
        Func<bool> FinishedMeleeAttack() => () => attackPlayerMelee.attackAnimationDurationTimer < 0;
        Func<bool> FinishedRangedAttack() => () => attackPlayerRanged.attackAnimationDurationTimer < 0;
        Func<bool> AgentHitByPlayerAttack() => () => knockback.GotHit;
        Func<bool> FinishedKnockbackAnimation() => () => knockback.animationTimer < 0;
        Func<bool> AgentReturnedToSpawnPosition() => () => Vector3.Distance(returnState.spawnPosition, transform.position) < 1.0f;
        #endregion
    }

    /// <summary>
    /// Checks if player is within agents view cone (FOV). Returns true if player is within FOV.
    /// </summary>
    /// <returns></returns>
    private bool FieldOfViewCheck()
    {
        #region Don't touch
        _rayDirection = player.transform.position - this.transform.position;
        float AngleBetweenFacingAndPlayerpos = 0f;
        if (player.transform.position.x < this.transform.position.x)
        {
            AngleBetweenFacingAndPlayerpos = 360.0f - Vector3.Angle(Vector3.forward, _rayDirection);//player in third or fourth quadrant
        }
        else
        {
            AngleBetweenFacingAndPlayerpos = Vector3.Angle(Vector3.forward, _rayDirection);//player in first or second quadrant
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

    private bool CanThisAgentOrOtherAgentWithinTalkingDistanceSeeThePlayer()
    {
        Physics.Raycast(this.transform.position, _rayDirection, out _hit, Mathf.Infinity, playerLayerMask);
        if (_hit.distance <= viewDistance)
        {
            _insideMaxViewDistance = true;
        }
        else
        {
            _insideMaxViewDistance = false;
        }


        if (Vector3.Distance(player.transform.position, transform.position) < radiusOfHearing)
        {
            return true;
        }

        if (FieldOfViewCheck() && _insideMaxViewDistance)
        {
            return true;
        }

        foreach (var agent in EnemyManager.Instance.agentsInCombat)
        {
            if (agent.agentGameObject.Equals(this))
            {
                continue;
            }
            if (talkingDistance > Vector3.Distance(agent.agentGameObject.transform.position, transform.position))
            {
                if (agent.agentGameObject.GetComponentInChildren<FoxAgentFSM>().FieldOfViewCheck() && agent.agentGameObject.GetComponentInChildren<FoxAgentFSM>()._insideMaxViewDistance)
                {
                    return true;
                }
                continue;
            }
        }
        return false;
    }

    private void Update()
    {
        _finiteStateMachine.TimeTick();
    }

    public void SetFSMState(string stateName)
    {
        if (stateName == "idle")
        {
            _finiteStateMachine.SetState(idle);
        }
        else if (stateName == "knockback")
        {
            _finiteStateMachine.SetState(knockback);
        }
    }
    #endregion
}
