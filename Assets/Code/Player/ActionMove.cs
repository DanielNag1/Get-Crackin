using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : MonoBehaviour
{

    private Move move;
    private LockToTarget target;
    private Animator animator;
    private CharacterController cc;

    private Vector3 inputDirection, targetDirection;
    public float jumpSpeed, dodgeSpeed, posY, gravity, attackSpeed;


    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        target = GetComponent<LockToTarget>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        attackSpeed = dodgeSpeed / 2;
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = move.GetInputDirection();
        posY = animator.GetFloat("posY");

        Dodge();
        Jump();
        Attack();
        Fall();
        Land();
        GetHit();
        AttackJump();
    }

    void Dodge()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            cc.Move(inputDirection * dodgeSpeed * Time.deltaTime);
        }
    }
    void Jump()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Double Jump")
            )
        {
            cc.Move(new Vector3(inputDirection.x, posY, inputDirection.y) * jumpSpeed * Time.deltaTime);
        }
    }
    void AttackJump()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Jump"))
        {
            cc.Move(new Vector3(0, posY, 0) * jumpSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack3") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack4") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("In Air_Chain1_Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("In Air_Chain1_Attack2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("In Air_Chain1_Attack3") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("In Air_Chain1_Attack4") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack3"))
        {
            cc.Move(target.GetEnemyDirection() * attackSpeed * Time.deltaTime);
            transform.LookAt(target.GetEnemyTransform());
        }
    }

    void Fall()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
        {
            if (cc.isGrounded)
            {
                animator.SetTrigger("onGround");
            }

            else
            {
                cc.Move(new Vector3(inputDirection.x, gravity, inputDirection.y) * Time.deltaTime);
            }
        }
    }

    void Land()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            transform.SetPositionAndRotation(new Vector3(transform.position.x, 1, transform.position.z), transform.rotation);
        }
    }

    void GetHit()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Get Hit"))
        {
            cc.Move(Vector3.back * Time.deltaTime); //Vector is placeholder, should be calculated from the enemy that has landed a hit.
        }
    }
}
