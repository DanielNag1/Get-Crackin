using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EncircleTarget : IState
{
    #region Variables
    private GameObject _gameObject;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent; // We need this. 
    //private
    #endregion

    public EncircleTarget(GameObject gameObject, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._animator = animator;
        this._navMeshAgent = navMeshAgent;
        this._gameObject = gameObject;
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        EnemyManager.Instance.SetReadyToAttack(_gameObject, true);
        _animator.SetBool("Fox_Idle", true);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        EnemyManager.Instance.SetReadyToAttack(_gameObject, false);
        _animator.SetBool("Fox_Idle", false);
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        /*
         destination = CM.SteeringBehaviorDestinationUpdate();
         */
    }
    #endregion
}
