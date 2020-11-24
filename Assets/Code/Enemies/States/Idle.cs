using UnityEngine;
using UnityEngine.AI;
public class Idle : IState
{
    #region Variables
    private EnemyOne _enemy;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Quaternion _localRotation;
    public float boringTimer;
    #endregion

    public Idle(Animator animator, NavMeshAgent navMeshAgent)
    {
        this._animator = animator;
        _navMeshAgent = navMeshAgent;
    }

    #region Interface functions
    public void OnEnter()
    {
        _animator.SetBool("Fox_Idle", true);
        _localRotation = _navMeshAgent.transform.localRotation;
        boringTimer = Random.Range(2, 40) / 10;
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Idle", false);
    }

    public void TimeTick()
    {
        _navMeshAgent.transform.localRotation = _localRotation;
        boringTimer -= Time.deltaTime;
    }
    #endregion
}
