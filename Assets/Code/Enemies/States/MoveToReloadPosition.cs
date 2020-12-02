using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToReloadPosition : IState
{
    #region Variables
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    public Vector3 destination;
    public float interactionRange = 1.0f;
    private GameObject gameObject;
    #endregion

    /// <summary>
    /// Move to a destination other then Vector3.zero!
    /// </summary>
    public MoveToReloadPosition(NavMeshAgent navMeshAgent, Animator animator)
    {
        this._animator = animator;
        this._navMeshAgent = navMeshAgent;
        gameObject = GameObject.FindGameObjectWithTag("ReloadStation");
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        //Go thrue list of all reload positions and pick the closest as destination
        // if this returns false, become melee and go to Idle. 
        destination = FindClosestReloadStation();
        _animator.SetBool("Fox_Run", true);
        _navMeshAgent.SetDestination(destination);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _animator.SetBool("Fox_Run", false);
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
    }

    private Vector3 FindClosestReloadStation()
    {
        Vector3 temp = new Vector3(gameObject.transform.position.x, _navMeshAgent.transform.position.y, gameObject.transform.position.z);
        return temp;
    }
    #endregion
}

