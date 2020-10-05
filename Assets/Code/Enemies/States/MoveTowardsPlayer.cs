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
    private GameObject _player;

    #endregion


    public MoveTowardsPlayer(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    #region Interface Functions

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _navMeshAgent.transform.LookAt(_player.transform.position);

        Debug.Log("Move Towards Player ENTER");
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;

        Debug.Log("Move Towards Player EXIT");
    }

    public void TimeTick()
    {
        _navMeshAgent.SetDestination(_player.transform.transform.position);
        _navMeshAgent.transform.position += _navMeshAgent.transform.forward * speed * Time.deltaTime;

        Debug.Log("Move Towards Player TIMETICK");
    }

    #endregion
}
