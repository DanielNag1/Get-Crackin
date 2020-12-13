using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Reload : IState
{
    #region Variables
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _animationTime = 1.0f;/*OBS!!! Change to animationTime!!*/
    public float animationTimer = 0;
    #endregion

    public Reload(NavMeshAgent navMeshAgent, Animator animator)
    {
        this._animator = animator;
        this._navMeshAgent = navMeshAgent;
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        animationTimer = _animationTime;
        _animator.SetBool("Fox_Reload", true);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _animator.SetBool("Fox_Reload", false);
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        animationTimer -= Time.deltaTime;
    }
    #endregion
}
