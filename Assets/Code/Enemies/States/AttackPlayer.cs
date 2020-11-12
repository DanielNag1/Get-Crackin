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
    private Rigidbody rb;

    public int attackDamageMinimun;
    public int attackDamageMaximun;
    public float attackCoolDownTimeMain = 0.983f;
    public float attackCoolDownTime = 1;

    private WeaponCollision weaponCollision;
    private GameObject _player;

    #endregion


    public AttackPlayer(EnemyOne enemy, NavMeshAgent navMeshAgent, Animator animator)
    {
        this._enemy = enemy;
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        weaponCollision = enemy.GetComponent<WeaponCollision>();
        _player = GameObject.FindGameObjectWithTag("Player");
        rb = _enemy.GetComponent<Rigidbody>();
    }

    #region Interface Methods

    public void OnEnter()
    {
        rb.isKinematic = false;
        weaponCollision.collisionActive = true;
        _animator.SetBool("Fox_Attack", true);
    }

    public void OnExit()
    {
        weaponCollision.collisionActive = false;
        _animator.SetBool("Fox_Idle", false);
        _animator.SetBool("Fox_Attack", false);
    }

    public void TimeTick()
    {
        Vector3 targetPos = new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z);
        _navMeshAgent.transform.LookAt(targetPos);// OBS!!! Check if this is correct!
        if (attackCoolDownTime > 0)
        {
            _animator.SetBool("Fox_Idle", false);
            _animator.SetBool("Fox_Attack", true);
            attackCoolDownTime -= Time.deltaTime;
        }
        else
        {
            attackCoolDownTime = attackCoolDownTimeMain;
            _animator.SetBool("Fox_Attack", false);
            _animator.SetBool("Fox_Idle", true);
        }
    }

    private void Attack()
    {
        //addImpact(_player.transform.position, 10); //OBS!!!THIS NEEDS TO BE MOVED OUTSIDE OF THE AI!! Has nothing to do with AI, but with taking damage!
    }

    /// <summary>
    /// MOVE THIS!!!
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    private void addImpact(Vector3 direction, float force)
    {
        var mass = 3;
        var impact = Vector3.zero;

        direction.Normalize();

        if (direction.y < 0)
        {
            direction.y = -direction.y;
        }

        impact += direction.normalized * force / mass;
    }

    #endregion
}
