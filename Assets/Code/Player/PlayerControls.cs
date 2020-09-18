using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    PlayerController controller;

    Vector2 movement;
    Vector2 rotate;

    /// <summary>
    ///  Using Lamdba expression to tell Unity that we are aware that
    ///  there is some context for this action but we don't wanna use is
    ///  we only wanna call the function
    /// </summary>
    void Awake()
    {
        controller = new PlayerController();
       
        controller.Gameplay.Move.performed += context => movement = context.ReadValue<Vector2>();
        controller.Gameplay.Move.canceled += context => movement = Vector2.zero;
        controller.Gameplay.Rotate.performed += contex => rotate = contex.ReadValue<Vector2>();
        controller.Gameplay.Rotate.canceled += context => rotate = Vector2.zero;
    
    }

    private void Update()
    {
        Vector2 move = new Vector2(movement.x, movement.y) * Time.deltaTime;
        transform.Translate(move, Space.World);

        Vector2 rot = new Vector2(rotate.y, rotate.x) * 100f * Time.deltaTime;
        transform.Rotate(rot, Space.World);

    }
    /// <summary>
    /// Being called whenever the object gets enabled.
    /// </summary>
    private void OnEnable()
    {
        controller.Gameplay.Enable();
    }
    /// <summary>
    /// Being called whenever the object gets disabled.
    /// </summary>
    private void OnDisable()
    {
        controller.Gameplay.Disable();
    }




}
