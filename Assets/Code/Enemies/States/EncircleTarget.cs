using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EncircleTarget : IState
{
    #region Variables
    private GameObject _agent;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent; // We need this. 
    private GameObject _player;
    private Vector3 _playerPreviousPos;
    //private
    #endregion

    public EncircleTarget(GameObject gameObject, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._animator = animator;
        this._navMeshAgent = navMeshAgent;
        this._agent = gameObject;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        EnemyManager.Instance.SetReadyToAttack(_agent, true);
        _animator.SetBool("Fox_Idle", true);
        _playerPreviousPos = _player.transform.position;
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        EnemyManager.Instance.SetReadyToAttack(_agent, false);
        _animator.SetBool("Fox_Idle", false);
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        NavMeshHit hit;
        int i = 1;
        while (!NavMesh.SamplePosition(_agent.GetComponent<FoxAgentFSM>().destination + (_player.transform.position - _playerPreviousPos), out hit, i, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            i++;
        }
        _agent.GetComponent<FoxAgentFSM>().destination = hit.position; //set as target position.
        Debug.Log("SamplePosition On Circle Adjusted for Player movement=" + hit.position);
        //_navMeshAgent.SetDestination(_agent.GetComponent<FoxAgentFSM>().destination);
        _playerPreviousPos = _player.transform.position;
        _navMeshAgent.transform.LookAt(new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z));
    }
    #endregion
}
