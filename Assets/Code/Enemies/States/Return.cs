using UnityEngine;
using UnityEngine.AI;

public class Return : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    public Vector3 spawnPosition;
    #endregion

    public Return(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        spawnPosition = new Vector3(_navMeshAgent.transform.root.position.x, _navMeshAgent.transform.root.position.y, _navMeshAgent.transform.root.position.z);
    }

    #region Interface Functions

    public void OnEnter()
    {
        _animator.SetBool("Fox_Run", true);
        _navMeshAgent.SetDestination(spawnPosition);
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Run", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
    }
    #endregion
}
