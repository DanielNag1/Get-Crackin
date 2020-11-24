using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MoveTowardsPlayer : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private GameObject _player;

    #endregion

    public MoveTowardsPlayer(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    #region Interface Functions
    public void OnEnter()
    {
        _animator.SetBool("Fox_Run", true);
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Run", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.LookAt(new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z));
        _navMeshAgent.SetDestination(_player.transform.position);
    }
    #endregion
}
