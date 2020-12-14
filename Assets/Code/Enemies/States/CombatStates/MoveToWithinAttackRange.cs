using UnityEngine;
using UnityEngine.AI;

public class MoveToWithinAttackRange : IState
{
    #region Variables

    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private GameObject _player;
    #endregion

    public MoveToWithinAttackRange(GameObject player, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        this._player = player;
    }

    #region Interface Functions
    public void OnEnter()
    {
        _animator.SetBool("Fox_Run", true);
    }

    public void OnExit()
    {
        _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
        _animator.SetBool("Fox_Run", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.LookAt(new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z));
        _navMeshAgent.SetDestination(_player.transform.position);
        _animator.SetFloat("maxSpeed / currentSpeed", (_navMeshAgent.velocity / _navMeshAgent.speed).magnitude);
    }
    #endregion
}
