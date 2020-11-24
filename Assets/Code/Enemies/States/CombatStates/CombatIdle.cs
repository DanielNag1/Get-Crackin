using UnityEngine;
using UnityEngine.AI;

public class CombatIdle : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    #endregion

    public CombatIdle(NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
    }

    #region Interface Functions

    public void OnEnter()
    {
        _animator.SetBool("Fox_Idle", true);
    }

    public void OnExit()
    {
        _animator.SetBool("Fox_Idle", false);
    }

    public void TimeTick()
    {
    }
    #endregion
}
