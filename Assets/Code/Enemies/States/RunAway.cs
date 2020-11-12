using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunAway : IState
{
    #region Variables

    private readonly EnemyOne _enemy;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private GameObject _player;

    private float _speed;
    private float Flee_speed = 5f;
    private float runAwayDistance = 4f;

    private static int RunAwayHash = Animator.StringToHash("RunAway");

    #endregion

    public RunAway(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    #region Interface functions

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _animator.SetBool(RunAwayHash, true);
        _speed = _navMeshAgent.speed;
        _navMeshAgent.speed = Flee_speed;

        Debug.Log("RunAway ENTER");
    }

    public void OnExit()
    {
        _navMeshAgent.speed = _speed;
        _navMeshAgent.enabled = false;
        _animator.SetBool(RunAwayHash, false);

        Debug.Log("RunAway EXIT");
    }

    /// <summary>
    /// Are we within 1(meter? float? distance?) of the randomly chosen destination and if we are, we choose another random point one.
    /// </summary>
    public void TimeTick()
    {
        if(_navMeshAgent.remainingDistance < 1f)
        {
            var away = GetNewPoint();
            _navMeshAgent.SetDestination(away); //Set as destination.
            Debug.Log("RunAway TIMETICK");
        }
    }

    private Vector3 GetNewPoint()
    {
        var directionFromPlayer = _enemy.transform.position - _player.transform.position;
        directionFromPlayer.Normalize();

        var lastPoint = _enemy.transform.position + (directionFromPlayer * runAwayDistance);
        if (NavMesh.SamplePosition(lastPoint, out var hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return _enemy.transform.position;
    }

    #endregion
}
