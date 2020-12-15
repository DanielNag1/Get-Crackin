using System.Collections.Generic;
using UnityEngine;

public class InputManager : ScriptableObject
{
    #region Variables
    private float _leftTrigger;
    private float _rightTrigger;
    private float _upButton;
    private float _downButton;
    private float _leftButton;
    private float _rightButton;
    private List<KeyCode> _pressedButtons = new List<KeyCode>(10);
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

    #region Methods
    /// <summary>
    /// Checks if a button has been pressed, if it's true it will execute the code for that button.
    /// </summary>
    public void Update()
    {
        _pressedButtons.Clear();
        if (ButtonAPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button0);
        }
        if (ButtonBPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button1);
        }
        if (buttonXPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button2);
        }
        if (ButtonYPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button3);
        }
        if (LeftBumperPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button4);
        }
        if (RightBumperPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button5);
        }
        if (StartButtonPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button6);
        }
        if (BackButtonPressed())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button7);
        }
        if (LeftStickClicked())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button8);
        }
        if (RightStickClicked())
        {
            _pressedButtons.Add(KeyCode.Joystick1Button9);
        }
        if (LeftTrigger() > 0)
        {
            _pressedButtons.Add(KeyCode.Joystick1Button10);
        }
        if (RightTrigger() > 0)
        {
            _pressedButtons.Add(KeyCode.Joystick1Button11);
        }
        if (UpButtonPressed() > 0)
        {
            _pressedButtons.Add(KeyCode.Joystick1Button12);
        }
        if (DownButtonPressed() > 0)
        {
            _pressedButtons.Add(KeyCode.Joystick1Button13);
        }
        if (LeftButtonPressed() > 0)
        {
            _pressedButtons.Add(KeyCode.Joystick1Button14);
        }
        if (RightButtonPressed() > 0)
        {
            _pressedButtons.Add(KeyCode.Joystick1Button15);
        }
        InputBuffer.Instance.pressedButtons = _pressedButtons;
        InputBuffer.Instance.StartBuffer();
        InputSave.Instance.pressedButtons = _pressedButtons;
        InputSave.Instance.SaveBuffer();
    }

    /// <summary>
    /// Public bools and floats methods that returns the correct button for the name to make it more readable
    /// </summary>
    private bool ButtonAPressed()
    {

        return Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Mouse0);
    }
    private bool ButtonBPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Mouse1);
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
        return Input.GetKeyDown(KeyCode.Joystick1Button5) || Input.GetKeyDown(KeyCode.LeftShift);
    }
    private bool StartButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button6);
    }
    private bool BackButtonPressed()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Escape);
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
        _leftTrigger = Input.GetAxis("LeftTrigger");
        return _leftTrigger;
    }
    private float RightTrigger()
    {
        _rightTrigger = Input.GetAxis("RightTrigger");
        return _rightTrigger;
    }
    private float UpButtonPressed()
    {
        _upButton = Input.GetAxis("UpKeyCode");
        return _upButton;
    }
    private float DownButtonPressed()
    {
        _downButton = Input.GetAxis("DownKeyCode");
        return _downButton;
    }
    private float LeftButtonPressed()
    {
        _leftButton = Input.GetAxis("LeftKeyCode");
        return _leftButton;
    }
    private float RightButtonPressed()
    {
        _rightButton = Input.GetAxis("RightKeyCode");
        return _rightButton;
    }
    #endregion
}
