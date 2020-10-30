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
    private Vector3 desiredDirection;
    private Animator animator;
    public int layerMaskValue;
    private int layerMask;
    private bool jumping;


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
        RelativeToCameraMovement();
        characterController.Move(desiredDirection * MovementSpeed * movementDirection.magnitude * Time.deltaTime);  //The object moves.
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            jumping = true;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Land"))
        {
            jumping = false;
            return;
        }
        if (!jumping)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag == "Ground")
                    if (hit.distance > 0.5f)
                    {
                        characterController.Move(-Vector3.up * Mathf.Min(hit.distance, 0.1f));
                    }
            }
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
            //trans.position += lockedDesiredDirection * (DodgeDistance / DodgeTimeSeconds) * Time.deltaTime;
            characterController.Move(lockedDesiredDirection * (DodgeDistance / DodgeTimeSeconds) * Time.deltaTime);  //The object moves.
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
