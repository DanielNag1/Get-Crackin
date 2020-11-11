using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Idle : IState
{
    #region Variables
    private EnemyOne _enemy;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    #endregion

    public Idle(/*EnemyOne enemy,*/ Animator animator, NavMeshAgent navMeshAgent)
    {
        //this._enemy = enemy;
        this._animator = animator;
        _navMeshAgent = navMeshAgent;
    }

    #region Interface functions

    public void OnEnter()
    {
      _animator.SetBool("Fox_Idle", true);
        _navMeshAgent.enabled = false;
    }

    public void OnExit()
    {
       _animator.SetBool("Fox_Idle", false);
        _navMeshAgent.enabled = true;
    }

    public void TimeTick()
    {
    }

    #endregion
}
