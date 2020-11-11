using UnityEngine;
using UnityEngine.AI;

public class Return : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private Rigidbody rb;
    public Vector3 targetPos;
    #endregion

    public Return(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        rb = enemy.GetComponent<Rigidbody>();

    }

    #region Interface Functions

    public void OnEnter()
    {
        rb.isKinematic = false;
        targetPos = new Vector3(_navMeshAgent.transform.root.position.x, _navMeshAgent.transform.root.position.y, _navMeshAgent.transform.root.position.z);
        _navMeshAgent.enabled = true;
        _animator.SetBool("Fox_Run", true);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetBool("Fox_Run", false);
    }

    public void TimeTick()
    {

        _navMeshAgent.transform.LookAt(targetPos);
        _navMeshAgent.SetDestination(targetPos);
    }
    #endregion
}
