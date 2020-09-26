using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LockToTarget LockToTarget;

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
    }

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
}
