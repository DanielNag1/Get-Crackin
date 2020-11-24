using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayerRanged : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private WeaponCollision weaponCollision;
    private GameObject _player;

    public float attackAnimationDurationTimeResetValue = 0.983f;
    public float attackAnimationDurationTimer = 0.983f;
    #endregion


    public AttackPlayerRanged(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        weaponCollision = enemy.GetComponent<WeaponCollision>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    #region Interface Methods

    public void OnEnter()
    {
        attackAnimationDurationTimer = attackAnimationDurationTimeResetValue;
        weaponCollision.collisionActive = true;
        _animator.SetBool("Fox_Ranged_Attack", true);
    }

    public void OnExit()
    {
        weaponCollision.collisionActive = false;
        _animator.SetBool("Fox_Ranged_Attack", false);
    }

    public void TimeTick()
    {
        attackAnimationDurationTimer -= Time.deltaTime;
        _navMeshAgent.transform.LookAt(new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z));
    }
    #endregion
}
