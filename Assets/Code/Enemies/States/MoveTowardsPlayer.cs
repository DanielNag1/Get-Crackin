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
        _navMeshAgent.enabled = true;
        _animator.SetBool("Fox_Run", true);
        //Play chase animation

        //Debug.Log("Move Towards Player ENTER");
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Fox_Run", false);
        //Stop chase animation.
        //Debug.Log("Move Towards Player EXIT");
    }

    public void TimeTick()
    {
        Vector3 targetPos = new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z);

        rb.isKinematic = false;
        //_navMeshAgent.transform.LookAt(_player.transform.position);
        _navMeshAgent.transform.LookAt(targetPos);// OBS!!! Check if this is correct!
        _navMeshAgent.SetDestination(_player.transform.position);
        //Debug.Log("Move Towards Player TIMETICK");
    }

    #endregion
}
