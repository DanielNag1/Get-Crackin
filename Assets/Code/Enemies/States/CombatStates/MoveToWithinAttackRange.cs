using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToWithinAttackRange : IState
{
    #region Variables

    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private GameObject _player;
    #endregion

    public MoveToWithinAttackRange(GameObject player, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        _player = player;
    }

    #region Interface Functions
    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _animator.SetBool("Fox_Run", true);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Fox_Run", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.LookAt(new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z));
       // _navMeshAgent.SetDestination(_player.transform.position - Vector3.
    }
    #endregion
}
