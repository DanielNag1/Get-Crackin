using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToCircle : IState
{
    #region Variables
    private EnemyOne _AgentBehavior;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    #endregion

    public MoveToCircle(NavMeshAgent navMeshAgent, Animator animator)
    {
        this._animator = animator;
        _navMeshAgent = navMeshAgent;
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        _animator.SetBool("Animation_Name", true);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _animator.SetBool("Animation_Name", false);
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
    }
    #endregion
}

