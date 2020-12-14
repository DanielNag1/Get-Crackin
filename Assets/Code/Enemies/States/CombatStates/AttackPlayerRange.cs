using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayerRanged : IState
{
    #region Variables
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private WeaponCollision _weaponCollision;
    private GameObject _player;
    private GameObject _enemy;
    public float attackAnimationDurationTimeResetValue = 2f;//OBS!!! Set RangedAttackAnimationDuration!
    public float attackAnimationDurationTimer = 2f;
    #endregion


    public AttackPlayerRanged(GameObject enemy, NavMeshAgent navMeshAgent, Animator animator, WeaponCollision weaponCollision)
    {
        this._navMeshAgent = navMeshAgent;
        this._animator = animator;
        this._weaponCollision = weaponCollision;
        this._player = GameObject.FindGameObjectWithTag("Player");
        this._enemy = enemy;
    }

    #region Interface Methods
    public void OnEnter()
    {
        _enemy.GetComponent<FoxAgentFSM>().attacking = false;
        attackAnimationDurationTimer = attackAnimationDurationTimeResetValue;
        _weaponCollision.collisionActive = true;
        _animator.SetBool("Fox_Ranged_Attack", true);
    }

    public void OnExit()
    {
        _weaponCollision.collisionActive = false;
        _animator.SetBool("Fox_Idle", false);
    }

    public void TimeTick()
    {
        attackAnimationDurationTimer -= Time.deltaTime;
        if (attackAnimationDurationTimer < 1.3f)
        {

            if (!_weaponCollision.transform.GetComponent<MeshRenderer>().forceRenderingOff)
            {
                FireProjectile();
            }
            _weaponCollision.transform.GetComponent<MeshRenderer>().forceRenderingOff = true;

        }
        if (attackAnimationDurationTimer < 1f)
        {
            _animator.SetBool("Fox_Ranged_Attack", false);
            _animator.SetBool("Fox_Idle", true);
        }
        _navMeshAgent.transform.LookAt(new Vector3(_player.transform.position.x, _navMeshAgent.transform.position.y, _player.transform.position.z));
    }

    private void FireProjectile()
    {
        _weaponCollision.transform.GetComponent<ProjectileMotion>().CreateProjectile(_weaponCollision.transform.position, _weaponCollision.transform.rotation, _player.transform);
    }
    #endregion
}
