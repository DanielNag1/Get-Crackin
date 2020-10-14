using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : IState
{
    #region Variables

    private EnemyOne _enemy;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    public BoxCollider boxC;

    #endregion


    public AttackPlayer(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
    }

    #region Interface Methods

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        //play attack animation
    }

    public void OnExit()
    {
        //Stop attack Animation.
    }

    public void TimeTick()
    {

    }

    #endregion
}
