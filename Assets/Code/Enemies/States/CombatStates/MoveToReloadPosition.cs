using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveToReloadPosition : IState
{
    #region Variables
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    public Vector3 destination;
    private float maxSearchDistance;
    public Tuple<Vector3, bool> reloadStation;
    private GameObject _gameObject;
    #endregion

    /// <summary>
    /// Move to a destination other then Vector3.zero!
    /// </summary>
    public MoveToReloadPosition(GameObject gameObject, NavMeshAgent navMeshAgent, Animator animator, float maxSearchDistance)
    {
        this._animator = animator;
        this._navMeshAgent = navMeshAgent;
        this._gameObject = gameObject;
        this.maxSearchDistance = maxSearchDistance;
    }
    private Tuple<Vector3, bool> FindClosestReloadStation()
    {
        return EnemyManager.Instance.ClosestReloadStation(_gameObject.transform, maxSearchDistance);
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        //Go thrue list of all reload positions and pick the closest as destination
        // if this returns false, become melee and go to Idle. 
        reloadStation = FindClosestReloadStation();
        if (reloadStation.Item2)
        {
            destination = reloadStation.Item1;
            Debug.Log("Ranged Moving to reload at:" + destination);
            _animator.SetBool("Fox_Run", true);
        }
        else
        {
            _animator.SetBool("Fox_Idle", true);
            destination = _navMeshAgent.transform.position;
        }
        _navMeshAgent.SetDestination(destination);

    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
        _animator.SetBool("Fox_Run", false);
        _animator.SetBool("Fox_Idle", false);
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
        _animator.SetFloat("maxSpeed / currentSpeed", (_navMeshAgent.velocity / _navMeshAgent.speed).magnitude);
    }
    #endregion
}

