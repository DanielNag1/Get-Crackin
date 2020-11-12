using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MoveTowardsPlayer : IState
{
    #region Variables

    private readonly EnemyOne _enemy;
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private GameObject _player;
    private Rigidbody rb;

    #endregion

    public MoveTowardsPlayer(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        _player = GameObject.FindGameObjectWithTag("Player");
        rb = _enemy.GetComponent<Rigidbody>();
    }

    private void EnableNavMeshAgent()
    {
        _navMeshAgent.enabled = true;
    }

    #region Interface Functions
    public void OnEnter()
    {
        rb.isKinematic = false;
        _navMeshAgent.enabled = true;
        _animator.SetBool("Fox_Run", true);
        //OBS!!! Inform other agents of targets
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Fox_Run", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.LookAt(new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z));
        _navMeshAgent.SetDestination(_player.transform.position);
    }
    #endregion
}
