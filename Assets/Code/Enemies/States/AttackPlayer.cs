using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : IState
{
    #region Variables

    private EnemyOne _enemy;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    public BoxCollider boxC;

    public int attackDamageMinimun;
    public int attackDamageMaximun;
    public float attackCoolDownTimeMain = 3;
    public float attackCoolDownTime = 3;
    WeaponCollision weaponCollision;


    #endregion


    public AttackPlayer(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        weaponCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponCollision>();
    }

    #region Interface Methods

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
    }

    public void OnExit()
    {
        //Stop attack Animation.
    }

    public void TimeTick()
    {
        if (attackCoolDownTime > 0)
        {
            attackCoolDownTime -= Time.deltaTime;
        }
        else
        {
            attackCoolDownTime = attackCoolDownTimeMain;
            Attack();
        }
    }

    private void Attack()
    {
        weaponCollision.DeliverDamageToTargetsHit();
        //Play Attack Animation;
        //Debug.Log("ATTACK");
    }

    #endregion
}
