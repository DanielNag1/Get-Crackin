﻿using System.Collections;
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
    public float attackCoolDownTimeMain = 1;
    public float attackCoolDownTime = 1;

    private WeaponCollision weaponCollision;
    private GameObject _player;

    #endregion


    public AttackPlayer(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        weaponCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponCollision>();
        _player = GameObject.FindGameObjectWithTag("Player");
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
        
        if (weaponCollision.collisionActive == true)
        {
            addImpact(_enemy.transform.position, 10);
            Debug.Log("Attack TRUE!!");
        }
        //Play Attack Animation;
        //Debug.Log("ATTACK");
    }

    private void addImpact(Vector3 direction, float force)
    {
        var mass = 3;
        var impact = Vector3.zero;

        direction.Normalize();

        if(direction.y < 0)
        {
            direction.y = -direction.y;
        }

        impact += direction.normalized * force / mass;
    }

    #endregion
}
