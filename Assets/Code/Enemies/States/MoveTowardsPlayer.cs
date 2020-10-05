using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayer : IState
{
    #region Variables

    private readonly EnemyOne _enemy;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private float speed = 5;
    private int minimunDistancce = 10;

    #endregion


    public MoveTowardsPlayer(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
    }

    #region Interface Functions

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;

        Debug.Log("Move Towards Player ENTER");
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;

        Debug.Log("Move Towards Player EXIT");
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.position += _navMeshAgent.transform.forward * speed * Time.deltaTime;
        _navMeshAgent.SetDestination(_enemy.PlayerPrefab.transform.position);
        _navMeshAgent.transform.LookAt(_enemy.PlayerPrefab.transform.position);

        Debug.Log("Move Towards Player TIMETICK");
    }

    #endregion
}
