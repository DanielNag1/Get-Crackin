using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 movementDirection = new Vector2();
    private Transform cameraTransform;
    [SerializeField] float MovementSpeed;
    [SerializeField] float DodgeTimeSeconds;
    [SerializeField] float DodgeDistance;
    [SerializeField] float attackTowardsDistance;
    [SerializeField] float attackTowardSeconds;
    private Vector3 desiredDirection;
    private Animator animator;
    public int layerMaskValue;
    private int layerMask;
    private bool jumping;
    [SerializeField] float fallSpeed = 0;
    [SerializeField] float highOffset = 1.58f;
    [SerializeField] float knockbackAmount;
    [SerializeField] float knockbackTimer;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        layerMask = 1 << layerMaskValue;
        layerMask = ~layerMask;
    }

    void Update()
    {
        ModifyAttackSpeed();
        NormalMovement();
        animator.SetFloat("movementMagnitude", movementDirection.magnitude);
    }

    /// <summary>
    /// The character is moving in a direction which is calculated from the cameras perspective.
    /// </summary>
    void RelativeToCameraMovement()
    {
        cameraTransform = Camera.main.transform;

        movementDirection.x = Input.GetAxis("Horizontal");
        movementDirection.y = Input.GetAxis("Vertical");

        Vector3 vertical = cameraTransform.forward;
        Vector3 horizontal = cameraTransform.right;

        vertical.y = 0;
        horizontal.y = 0;

        vertical.Normalize();
        horizontal.Normalize();

        desiredDirection = (vertical * movementDirection.y + horizontal * movementDirection.x).normalized; //Gets the direction from the cameras position.

        if (desiredDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(desiredDirection), 0.3f);
        }
    }

    void NormalMovement()
    {
        Gravity();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Get Hit") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Recover") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack2") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack3") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack4") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack2") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack3"))
        //What animation states does NOT allow the character to move.
        {
            RelativeToCameraMovement();
            characterController.Move(desiredDirection * MovementSpeed * movementDirection.magnitude * Time.deltaTime);  //The object moves.
        }


    }

    private void Gravity()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {

            if (hit.distance > highOffset + 0.5f)
            {
                characterController.Move(-Vector3.up * Mathf.Min(hit.distance, fallSpeed));
                fallSpeed =9.82f;
            }
            else if (hit.distance > highOffset)
            {
                characterController.Move(-Vector3.up * Mathf.Min(hit.distance, 0.2f));
            }
            else
            {
                characterController.Move(Vector3.up * (float)(highOffset - hit.distance));
            }
        }

    }
    public void Knockback(Vector3 direction)
    {
        animator.ResetTrigger("Get Hit");
        StartCoroutine(KnockbackMovement(direction));
    }

    public void ModifyAttackSpeed()
    {
        if (animator.GetBool("Rage Mode"))
        {
            attackTowardsDistance = 7;
            //attackTowardSeconds = 0.34f;
        }
        else
        {
            attackTowardsDistance = 2;
            //attackTowardSeconds = 0.34f;
        }
    }


    public void AttackTowardsMovementStart(Transform trans)
    {
        StartCoroutine(AttackTowardsMovement(trans));
    }
    public IEnumerator AttackTowardsMovement(Transform trans)
    {
        float timer = attackTowardSeconds;
        while (timer > 0)
        {
            var target = GetComponentInChildren<LockToTarget>();
            Transform targetTransform = target.GetEnemyTransform();

            if (targetTransform != null)
            {
                Vector3 targetPos = new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z);
                transform.LookAt(targetPos);

                //OBS!!! We should not dash closer to the enemy then needed for an attack(may solve collision).
                if (Vector3.Distance(target.GetEnemyTransform().position, transform.position) < attackTowardsDistance)
                {
                    characterController.Move(target.GetEnemyDirection().normalized * ((Vector3.Distance(targetTransform.position, transform.position) - 1) / attackTowardSeconds) * Time.deltaTime);  //The object moves.
                }
                else
                {
                    characterController.Move(target.GetEnemyDirection().normalized * (attackTowardsDistance / attackTowardSeconds) * Time.deltaTime);  //The object moves.
                }
            }
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    public void DodgeMovementStart(Transform trans)
    {
        StartCoroutine(DodgeMovement(trans));
    }
    public IEnumerator DodgeMovement(Transform trans)
    {
        float timer = DodgeTimeSeconds;
        var lockedDesiredDirection = desiredDirection;
        var lockedMovementDirection = movementDirection.magnitude;
        while (timer > 0)
        {
            RelativeToCameraMovement();
            characterController.Move(lockedDesiredDirection * (DodgeDistance / DodgeTimeSeconds) * Time.deltaTime);  //The object moves.
            timer -= Time.deltaTime;
            yield return null;
        }

    }

    public IEnumerator KnockbackMovement(Vector3 direction)
    {
        float timer = knockbackTimer;
        while (timer > 0)
        {
            characterController.Move(direction * knockbackAmount * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
    }

    public Vector3 GetInputDirection()
    {
        if (desiredDirection != null)
        {
            return desiredDirection;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
