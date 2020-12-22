using System;
using System.Collections.Generic;
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
    protected FiniteStateMachine _finiteStateMachine = new FiniteStateMachine();
    public GameObject player;
    protected RaycastHit _hit;
    protected Vector3 _rayDirection;
    public int fieldOfView = 120;
    public float viewDistance = 20f;
    public float radiusOfHearing = 10f;
    public float talkingDistance = 35f;
    public float walkingSpeed = 3f;
    public float runningSpeed = 7f;
    public float interactionRange = 1.5f;
    //---------------------------
    protected bool _insideMaxViewDistance = false;
    public CombatRole combatRole = CombatRole.None;
    public bool attacking = false;
    public float meleeAttackRange = 2.5f;
    public float rangedAttackRange = 10f;
    public float maxSearchDistance = 100f;
    public float circleRadius = 4;
    public Vector3 destination;
    public int squareID;
    public Vector3 squareNormalisedPosition;
    public WeaponCollision meleeWeapon;
    public WeaponCollision rangedWeapon;
    #endregion

    #region States
    protected Idle idle;
    protected Lollygagging lollygagging;
    #region CombatBlock
    protected EnterCombat enterCombat;
    protected CombatIdle combatIdle;
    protected MoveToReloadPosition moveToReloadPosition;
    protected Reload reload;
    protected MoveToCircle moveToCircle;
    protected EncircleTarget encircleTarget;
    protected MoveToWithinAttackRange _moveToWithinAttackRange;
    protected Return returnState;
    protected AttackPlayerMelee attackPlayerMelee;
    protected AttackPlayerRanged attackPlayerRanged;
    public Knockback knockback;
    #endregion
    #endregion

    #region Methods
    protected void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponent<Animator>();
        var weapons = GetComponentsInChildren<WeaponCollision>();
        for (int element = 0; element < weapons.Length; element++)
        {
            switch (weapons[element].name)
            {
                case "Stone":
                    rangedWeapon = weapons[element];
                    break;
                case "SpikedBallV1.1":
                    meleeWeapon = weapons[element];
                    break;
            }
        }
        StatesInitialise(navMeshAgent, animator);
        TransitionsAndConditionsInitialise();
    }

    protected void StatesInitialise(NavMeshAgent navMeshAgent, Animator animator)
    {
        //The States
        idle = new Idle(animator, navMeshAgent);
        lollygagging = new Lollygagging(navMeshAgent, animator, walkingSpeed, runningSpeed);
        enterCombat = new EnterCombat(this.gameObject, talkingDistance);
        combatIdle = new CombatIdle(animator);
        moveToReloadPosition = new MoveToReloadPosition(this.gameObject, navMeshAgent, animator, maxSearchDistance);
        reload = new Reload(navMeshAgent, animator, rangedWeapon);
        moveToCircle = new MoveToCircle(this.gameObject, navMeshAgent, animator);
        encircleTarget = new EncircleTarget(this.gameObject, navMeshAgent, animator);
        _moveToWithinAttackRange = new MoveToWithinAttackRange(player, navMeshAgent, animator);
        returnState = new Return(this.gameObject, navMeshAgent, animator, walkingSpeed, runningSpeed);
        attackPlayerMelee = new AttackPlayerMelee(this.gameObject, navMeshAgent, animator, meleeWeapon);
        attackPlayerRanged = new AttackPlayerRanged(this.gameObject, navMeshAgent, animator, rangedWeapon);
        knockback = new Knockback(navMeshAgent, animator);
    }

    public virtual void TransitionsAndConditionsInitialise()
    {
        //The Transitions (From, To, Condition)
        #region Transitions
        _finiteStateMachine.AddTransition(idle, lollygagging, IdleTimer());
        _finiteStateMachine.AddTransition(idle, enterCombat, PlayerDetected());
        _finiteStateMachine.AddTransition(lollygagging, idle, LollygaggingPositionReached());
        _finiteStateMachine.AddTransition(lollygagging, enterCombat, PlayerDetected());
        _finiteStateMachine.AddTransition(enterCombat, combatIdle, FinishedEnteringCombat());
        _finiteStateMachine.AddTransition(enterCombat, idle, CannotEnterCombat());
        _finiteStateMachine.AddTransition(combatIdle, moveToCircle, AssignedMeleeCombatRole());
        _finiteStateMachine.AddTransition(combatIdle, moveToReloadPosition, AssignedRangedCombatRole());
        _finiteStateMachine.AddTransition(moveToReloadPosition, reload, WithinReloadInteractRange());
        _finiteStateMachine.AddTransition(moveToReloadPosition, combatIdle, NoAvailableReloadStation());
        _finiteStateMachine.AddTransition(reload, moveToCircle, FinishedReloading());
        _finiteStateMachine.AddTransition(moveToCircle, returnState, CombatHasEnded());
        _finiteStateMachine.AddTransition(moveToCircle, encircleTarget, CirclingTarget());
        _finiteStateMachine.AddTransition(encircleTarget, moveToCircle, ToFarFromCircle());
        _finiteStateMachine.AddTransition(encircleTarget, _moveToWithinAttackRange, Attacking());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, moveToCircle, TargetToFarAwayToAttack());
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
        Func<bool> CannotEnterCombat() => () => !enterCombat.ableToEnterCombat;
        Func<bool> AssignedMeleeCombatRole() => () => combatRole == CombatRole.Melee;
        Func<bool> AssignedRangedCombatRole() => () => combatRole == CombatRole.Ranged;
        Func<bool> WithinReloadInteractRange() => () => Vector3.Distance(moveToReloadPosition.destination, transform.position) < interactionRange;
        Func<bool> NoAvailableReloadStation() => () => !moveToReloadPosition.reloadStation.Item2;
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
    protected bool FieldOfViewCheck()
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

    protected bool CanThisAgentOrOtherAgentWithinTalkingDistanceSeeThePlayer()
    {
        //Close enough to hear the player, We allways detect the player if that is the case
        if (Vector3.Distance(player.transform.position, transform.position) < radiusOfHearing)
        {
            return true;
        }

        //Are we facing towards the player, then we can make the raycast to check that we have an unobscured view of the player.
        if (FieldOfViewCheck())
        {
            //What layers should block the AI's vision
            string[] maskStrings = new string[2] { "Player", "ground" };
            LayerMask mask = LayerMask.GetMask(maskStrings);

            if (Physics.Raycast(this.transform.position, _rayDirection, out _hit, Mathf.Infinity, mask))
            {
                if (_hit.collider.tag == player.tag)
                {
                    if (_hit.distance <= viewDistance)
                    {
                        _insideMaxViewDistance = true;
                    }
                    else
                    {
                        _insideMaxViewDistance = false;
                    }
                }
            }
            if (_insideMaxViewDistance)
            {
                return true;
            }
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

    protected void Update()
    {
        _finiteStateMachine.TimeTick(GetInstanceID());
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
