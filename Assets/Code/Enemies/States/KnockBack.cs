using UnityEngine;
using UnityEngine.AI;

public class Knockback : IState
{
    #region Variables
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _animationTimeResetValue = 0.317f;
    public float animationTimer = 0.317f;
    public bool GotHit;
    public Vector3 destination = Vector3.zero;
    #endregion

    /// <summary>
    /// Move to a destination other then Vector3.zero!
    /// </summary>
    public Knockback(NavMeshAgent navMeshAgent, Animator animator)
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
        _animator.SetBool("Fox_Stagger", true);
        animationTimer = _animationTimeResetValue;
        NavMeshHit hit;
        int samplePosition = 1;
        while (!NavMesh.SamplePosition(destination, out hit, samplePosition, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            samplePosition++;
        }
        destination = hit.position; //set as target position.
        _navMeshAgent.SetDestination(destination);
    }

    /// <summary>
    /// Peforms this action when it exits this state.
    /// </summary>
    public void OnExit()
    {
        _animator.SetBool("Fox_Stagger", false);
        GotHit = false;
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
