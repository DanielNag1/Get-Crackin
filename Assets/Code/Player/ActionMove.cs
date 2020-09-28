using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMove : MonoBehaviour
{

    private Move move;
    private LockToTarget target;
    private Animator animator;
    private CharacterController cc;
    private InputManager inputManager;

    private Vector3 inputDirection, targetDirection;
    public float jumpSpeed, dodgeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<Move>();
        target = GetComponent<LockToTarget>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = move.GetInputDirection();

        Dodge();
        Jump();
        Attack();
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            cc.Move(new Vector3(inputDirection.x, animator.GetFloat("posY"), inputDirection.y) * jumpSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack1") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack3"))
        {
            cc.Move(target.GetEnemyDirection() * dodgeSpeed/2 * Time.deltaTime);
        }
    }
}
