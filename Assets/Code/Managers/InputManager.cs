using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    int buttonInput;
    public enum ButtonInput
    {
        ButtonA, // 0
        ButtonB, // 1
        ButtonX, // 2
        ButtonY, // 3
        LeftBumberButton, // 4
        RightBumberButton, // 5
        StartButton, // 6
        BackButton // 7
    }

    private void Update()
    {
        if (Input.GetButtonDown("ButtonA"))
        {
            Debug.Log("pressed button A");
        }
        if (Input.GetButtonDown("ButtonB"))
        {
            Debug.Log("Pressed Button B");
        }
        if (Input.GetButtonDown("ButtonX"))
        {
            Debug.Log("Pressed Button X");
        }
        if (Input.GetButtonDown("ButtonY"))
        {
            Debug.Log("Pressed Button Y");
        }
        if (Input.GetButtonDown("LeftBumperButton"))
        {
            Debug.Log("Pressed LeftBumberButton");
        }
        if (Input.GetButtonDown("RightBumperButton"))
        {
            Debug.Log("Pressed RightBumberButton");
        }
        if (Input.GetButtonDown("StartButton"))
        {
            Debug.Log("Pressed startButton");
        }
        if (Input.GetButtonDown("BackButton"))
        {
            Debug.Log("Pressed BackButton");
        }
    }


}
