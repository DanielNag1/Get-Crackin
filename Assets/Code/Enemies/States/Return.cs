using UnityEngine;
using UnityEngine.AI;

public class Return : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    public Vector3 spawnPosition;
    private GameObject _enemy;
    private float _walkSpeed;
    private float _runSpeed;
    #endregion

    public Return(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator, float walkSpeed, float runSpeed)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        this._enemy = enemy;
        this._walkSpeed = walkSpeed;
        this._runSpeed = runSpeed;
    }

    #region Interface Functions

    public void OnEnter()
    {
        _navMeshAgent.speed = _walkSpeed;
        EnemyManager.Instance.AgentLeftCombat(_enemy);
        spawnPosition = new Vector3(_navMeshAgent.transform.root.position.x, _navMeshAgent.transform.root.position.y,
            _navMeshAgent.transform.root.position.z);
        _animator.SetBool("Fox_Run", true);
        _navMeshAgent.SetDestination(spawnPosition);
    }

    public void OnExit()
    {
        _navMeshAgent.speed = _runSpeed;
        _animator.SetBool("Fox_Run", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.rotation = Quaternion.LookRotation(_navMeshAgent.velocity, Vector3.up);
        _animator.SetFloat("maxSpeed / currentSpeed", (_navMeshAgent.velocity / _navMeshAgent.speed).magnitude);
    }
    #endregion
}
