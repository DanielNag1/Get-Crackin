using UnityEngine;
using UnityEngine.AI;

public class CombatIdle : IState
{
    #region Variables
   
    private readonly Animator _animator;
    
    #endregion

    public CombatIdle(Animator animator)
    {
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
