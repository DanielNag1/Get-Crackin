using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : ScriptableObject
{
    #region Variables
    private float leftTrigger;
    private float rightTrigger;
    private List<KeyCode> PressedButtons = new List<KeyCode>(10);
    #endregion

    #region singleton
    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = CreateInstance<InputManager>();
            }
            return instance;
        }
    }
    #endregion
    /// <summary>
    /// Checks if a button has been pressed, if it's true it will execute the code for that button.
    /// </summary>
    ///
#if DEBUG
    public void Update()
    {
        PressedButtons.Clear();
        if (ButtonAPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button0); //Attack
            Debug.Log("pressed button A");
        }
        if (ButtonBPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button1); //Dodge
            Debug.Log("Pressed Button B");
        }
        if (buttonXPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button2); //Jump
            Debug.Log("Pressed Button X");
        }
        if (ButtonYPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button3);
            Debug.Log("Pressed Button Y");
        }
        if (LeftBumperPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button4);
            Debug.Log("Pressed LeftBumberButton");
        }
        if (RightBumperPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button5);
            Debug.Log("Pressed RightBumberButton");
        }
        if (StartButtonPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button6);
            Debug.Log("Pressed startButton");
        }
        if (BackButtonPressed())
        {
            PressedButtons.Add(KeyCode.Joystick1Button7);
            Debug.Log("Pressed BackButton");
        }
        if(LeftStickClicked())
        {
            PressedButtons.Add(KeyCode.Joystick1Button8);
            Debug.Log(" Pressed LeftJoystick Down");
        }
        if (RightStickClicked())
        {
            PressedButtons.Add(KeyCode.Joystick1Button9);
            Debug.Log(" Pressed RightJoystick Down");
        }
        if (LeftTrigger() > 0)
        {
            PressedButtons.Add(KeyCode.Joystick1Button10);//This might not work, we added new buttons in projectSettings, If something breakes we know why! :D ^^
            Debug.Log("Pressed Left Trigger");
        }
        if (RightTrigger() > 0)
        {
            PressedButtons.Add(KeyCode.Joystick1Button11);//This might not work, we added new buttons in projectSettings, If something breakes we know why! :D ^^
            Debug.Log("Pressed Right Trigger");
        }
        InputBuffer.Instance.PressedButtons = PressedButtons;
        InputBuffer.Instance.StartBuffer();
    }
#endif

    #region Methods
    /// <summary>
    /// Public bools and floats methods that returns the correct button for the name to make it more readable
    /// </summary>
    /// <returns></returns>
    private bool ButtonAPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button0);
    }
    private bool ButtonBPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button1);
    }
    private bool buttonXPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button2);
    }
    private bool ButtonYPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button3);
    }
    private bool LeftBumperPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button4);
    }
    private bool RightBumperPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button5);
    }
    private bool StartButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button6);
    }
    private bool BackButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button7);
    }
    private bool LeftStickClicked()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button8);
    }
    private bool RightStickClicked()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button9);
    }
    private float LeftTrigger()
    {
        leftTrigger = Input.GetAxis("LeftTrigger");
        return leftTrigger;
    }
    private float RightTrigger()
    {
        rightTrigger = Input.GetAxis("RightTrigger");
        return rightTrigger;
    }
    #endregion
}
