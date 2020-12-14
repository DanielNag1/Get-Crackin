using System;
using UnityEngine;
using UnityEngine.AI;

public class FoxAgentNoRetreat : FoxAgentFSM
{
    #region Methods
    public override void TransitionsAndConditionsInitialise()
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
        _finiteStateMachine.AddTransition(moveToCircle, encircleTarget, CirclingTarget());
        _finiteStateMachine.AddTransition(encircleTarget, moveToCircle, ToFarFromCircle());
        _finiteStateMachine.AddTransition(encircleTarget, _moveToWithinAttackRange, Attacking());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, moveToCircle, TargetToFarAwayToAttack());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, attackPlayerMelee, TargetWithinMeleeAttackRange());
        _finiteStateMachine.AddTransition(_moveToWithinAttackRange, attackPlayerRanged, TargetWithinRangedAttackRange());
        _finiteStateMachine.AddTransition(attackPlayerMelee, combatIdle, FinishedMeleeAttack());
        _finiteStateMachine.AddTransition(attackPlayerRanged, combatIdle, FinishedRangedAttack());
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
        #endregion
    }
    #endregion
}
