using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private CharacterController characterController;
    private Vector2 movementDirection = new Vector2();
    private Transform cameraTransform;
    [SerializeField] float MovementSpeed;
    private Vector3 desiredDirection;
    private Animator animator;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RelativeToCameraMovement();
        animator.SetFloat("movementMagnitude",movementDirection.magnitude);
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

        characterController.Move(desiredDirection * MovementSpeed * movementDirection.magnitude * Time.deltaTime);  //The object moves.
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
