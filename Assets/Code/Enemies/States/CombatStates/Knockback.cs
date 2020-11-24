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
        _navMeshAgent = navMeshAgent;
        animationTimer = _animationTimeResetValue;
    }

    #region Interface functions
    /// <summary>
    /// Peforms this action when it enters this state.
    /// </summary>
    public void OnEnter()
    {
        _animator.SetBool("Fox_Stagger", true);
        animationTimer = _animationTimeResetValue;
        #region Testing if this works
        NavMeshHit hit;
        int i = 1;
        while (!NavMesh.SamplePosition(destination, out hit, i, NavMesh.AllAreas)) //get where we should go on the navMesh
        {
            i++; 
        }
        destination = hit.position; //set as target position.
        //destination.y = destination.y + 1; //Unsure if we need this!
        #endregion

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
        if (_navMeshAgent.velocity != Vector3.zero) // maybe a solution to our problem  #Twerk
        {
            _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity * -1, Vector3.up); // vektor * -1 ger motsatt vektor
        }
    }
    #endregion
}
