using UnityEngine;
using UnityEngine.AI;

public class MoveToCircle : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    public Vector3 targetPos;
    private GameObject _player;
    #endregion

    public MoveToCircle( NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    #region Interface Functions

    public void OnEnter()
    {
        _animator.SetBool("Fox_Run", true);
        //Debug.Log("Destination OnEnter: " + _navMeshAgent.destination);
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Run", false);
        _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
    }

    public void TimeTick()
    {
        //Debug.Log("Destination: " + _navMeshAgent.destination);
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
        _navMeshAgent.SetDestination(_player.transform.position);
    }
    #endregion
}
