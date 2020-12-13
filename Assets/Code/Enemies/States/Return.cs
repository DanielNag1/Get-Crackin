using UnityEngine;
using UnityEngine.AI;

public class Return : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    public Vector3 spawnPosition;
    private GameObject _enemy;
    #endregion

    public Return(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        this._enemy = enemy;
    }

    #region Interface Functions

    public void OnEnter()
    {
        EnemyManager.Instance.AgentLeftCombat(_enemy);
        spawnPosition = new Vector3(_navMeshAgent.transform.root.position.x, _navMeshAgent.transform.root.position.y,
            _navMeshAgent.transform.root.position.z);
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
