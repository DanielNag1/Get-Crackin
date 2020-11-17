using UnityEngine;
using UnityEngine.AI;

public class Knockback : IState
{
    #region Variables
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _animationTime = 0.317f; //OBS!!! Change to animationTime!!
    public float _animationTimer = 0;
    public Vector3 destination = Vector3.zero;
    #endregion

    /// <summary>
    /// Move to a destination other then Vector3.zero!
    /// </summary>
    public Knockback(NavMeshAgent navMeshAgent, Animator animator)
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
        _animator.SetBool("Fox_Stagger", true);
        _navMeshAgent.SetDestination(destination);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _animator.SetBool("Fox_Stagger", false);
        _animationTimer = _animationTime;
    }

    /// <summary>
    /// Update for the state.
    /// </summary>
    public void TimeTick()
    {
        _animationTimer -= Time.deltaTime;
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity * -1, Vector3.up);
    }
    #endregion
}
