using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : IState
{
    #region Variables

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _animationTime = 0.317f; //OBS!!! Change to animationTime!!
    public float _animationTimer = 0;
    public Vector3 destination = Vector3.zero;

    #endregion

    public KnockBack(NavMeshAgent navMeshAgent, Animator animator)
    {
        this._animator = animator;
        _navMeshAgent = navMeshAgent;
        _animationTimer = _animationTime;
    }

    #region Interface methods

    public void OnEnter()
    {
        _animator.SetBool("Fox_Stagger", true);
        _navMeshAgent.SetDestination(destination);
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Stagger", false);
        _animationTimer = _animationTime;
    }

    public void TimeTick()
    {
        _animationTimer -= Time.deltaTime;
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity * -1, Vector3.up);
    }

    #endregion
}
