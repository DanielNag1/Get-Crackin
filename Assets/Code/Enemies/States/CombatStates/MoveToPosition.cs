using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPosition : IState
{
    #region Variables
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    public Vector3 destination = Vector3.zero;
    #endregion

    /// <summary>
    /// Move to a destination other then Vector3.zero!
    /// </summary>
    public MoveToPosition(NavMeshAgent navMeshAgent, Animator animator)
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
        _animator.SetBool("Fox_Run", true);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _animator.SetBool("Fox_Run", false);
        destination = Vector3.zero;
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        if (destination != Vector3.zero)
        {
            _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
            _navMeshAgent.SetDestination(destination);
        }
    }
    #endregion
}
