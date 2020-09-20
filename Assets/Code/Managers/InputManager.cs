﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
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
}
