using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Variables
    private float leftTrigger;
    private float rightTrigger;
    [SerializeField] private LockToTarget LockToTarget;
    [SerializeField] private string SoundPath;

    #endregion
    /// <summary>
    /// Checks if a button has been pressed, if it's true it will execute the code for that button.
    /// </summary>
    /// 
#if DEBUG
    private void Update()
    {
        if (ButtonAPressed())
        {
            Debug.Log("pressed button A");
        }
        if (ButtonBPressed())
        {
            Debug.Log("Pressed Button B");
        }
        if (buttonXPressed())
        {
            Debug.Log("Pressed Button X");
        }
        if (ButtonYPressed())
        {
            Debug.Log("Pressed Button Y");
        }
        if (LeftBumperPressed())
        {
            Debug.Log("Pressed LeftBumberButton");
        }
        if (RightBumperPressed())
        {
            LockToTarget.ManualTargeting();
            Debug.Log("Pressed RightBumberButton");
        }
        if (StartButtonPressed())
        {
            Debug.Log("Pressed startButton");
        }
        if (BackButtonPressed())
        {
            Debug.Log("Pressed BackButton");
        }
        if(LeftStickClicked())
        {
            Debug.Log(" Pressed LeftJoystick Down");
        }
        if (RightStickClicked())
        {
            Debug.Log(" Pressed RightJoystick Down");
        }
        if (LeftTrigger() > 0)
        {
            Debug.Log("Pressed Left Trigger");
        }
        if (RightTrigger() > 0)
        {
            Debug.Log("Pressed Right Trigger");
        }

    }
#endif

    #region Methods
    /// <summary>
    /// Public bools and floats methods that returns the correct button for the name to make it more readable
    /// </summary>
    /// <returns></returns>
    public bool ButtonAPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button0);
    }
    public bool ButtonBPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button1);
    }
    public bool buttonXPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button2);
    }
    public bool ButtonYPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button3);
    }
    public bool LeftBumperPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button4);
    }
    public bool RightBumperPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button5);
    }
    public bool StartButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button6);
    }
    public bool BackButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button7);
    }
    public bool LeftStickClicked()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button8);
    }
    public bool RightStickClicked()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button9);
    }
    public float LeftTrigger()
    {
        leftTrigger = Input.GetAxis("LeftTrigger");
        return leftTrigger;
    }
    public float RightTrigger()
    {
        rightTrigger = Input.GetAxis("RightTrigger");
        return rightTrigger;
    }
    #endregion
}
