using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Reload : IState
{
    #region Variables
    private EnemyOne _AgentBehavior;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _animationTime = 1.0f/*OBS!!! Change to animationTime!!*/;
    public float _animationTimer = 0;
    #endregion

    public Reload(NavMeshAgent navMeshAgent, Animator animator)
    {
        this._animator = animator;
        _navMeshAgent = navMeshAgent;
        _animationTimer = _animationTime;
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        _animator.SetBool("Fox_Reload", true);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _animator.SetBool("Fox_Reload", false);
        _animationTimer = _animationTime;
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        _animationTimer -= Time.deltaTime;
    }
    #endregion
}
