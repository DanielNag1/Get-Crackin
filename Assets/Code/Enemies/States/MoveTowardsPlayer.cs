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
    private static readonly int speed = Animator.StringToHash("speed");

    private Vector3 _lastPosition = Vector3.zero;

    public float timeStuck;

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
        timeStuck = 0; //safety check to see if the enemy has not moved.
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_enemy.Player.transform.position);
        _animator.SetFloat(speed, 1);

        Debug.Log("Move Towards Player ENTER");
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetFloat(speed, 0);

        Debug.Log("Move Towards Player EXIT");
    }

    public void TimeTick()
    {
        if(Vector3.Distance(_enemy.transform.position, _lastPosition) <= 0)
        {
            timeStuck += Time.deltaTime;

            _lastPosition = _enemy.transform.position;
        }

        Debug.Log("Move Towards Player TIMETICK");
    }

    #endregion
}
