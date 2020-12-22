using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class InputSave : ScriptableObject
{
    #region Variables
    public List<KeyCode> pressedButtons = new List<KeyCode>(16); //Get input
    public LockToTarget lockToTarget;
    private bool _bufferMode = true;
    private Stack<KeyCode> _bufferStack = new Stack<KeyCode>();

    private int ButtonX, ButtonY, ButtonA, ButtonB, ButtonRT, ButtonRB, ButtonLT, ButtonLB, ButtonStart, ButtonSelect, ButtonR3, ButtonL3, ButtonUp, ButtonDown, ButtonLeft, ButtonRight = 0;
    private Vector3 MoveVectorInput, LookVectorInput;

    private StreamWriter _streamWriter;
    #endregion

    #region singleton
    private static InputSave instance;
    public static InputSave Instance
    {
        get
        {
            if (instance == null)
            {
                instance = CreateInstance<InputSave>();
            }
            return instance;
        }
    }
    #endregion

    #region Methods

    private void Awake()
    {
        int number = 0;
        while (File.Exists(Application.dataPath + @"InputSave" + number + ".txt"))
        {
            number++;
            if (number == 11)
            {
                if (File.Exists(Application.dataPath + @"InputSave" + number + ".txt"))
                {
                    File.Delete(Application.dataPath + @"InputSave" + number + ".txt");
                }
            }
        }
        _streamWriter = new StreamWriter(Application.dataPath + @"InputSave" + number + ".txt", true);
    }
    public bool WantToQuit()
    {
        _streamWriter.Write("#|ButtonX|ButtonY|ButtonA|ButtonB|ButtonRT|ButtonRB|ButtonLT|ButtonLB|ButtonStart|ButtonSelect|ButtonR3|ButtonL3|ButtonUp|ButtonDown|ButtonLeft|ButtonRight" + "\r\n");
        _streamWriter.Write("$|"+ ButtonX + "|" + ButtonY + "|" + ButtonA + "|" + ButtonB + "|" + ButtonRT + "|" + ButtonRB + "|" + ButtonLT + "|" + ButtonLB + "|" + ButtonStart + "|" + ButtonSelect + "|" + ButtonR3 + "|" + ButtonL3 + "|" + ButtonUp + "|" + ButtonDown + "|" + ButtonLeft + "|" + ButtonRight);
        _streamWriter.Close();
        return true;
    }
    public void SaveSeed(int seed)
    {
        _streamWriter.Write("#|seed" + "\r\n");
        _streamWriter.Write("£|" + seed + "\r\n");
        _streamWriter.Write("#|MoveX|MoveY|CamX|CamY|ButtonKeyCodes" + "\r\n");
    }

    public void SaveBuffer()
    {
        SaveJoystickInput();
        CheckInput();
        _streamWriter.Write("\r\n");
    }

    private void SaveJoystickInput()
    {
        _streamWriter.Write(Input.GetAxis("Horizontal") + "|" + Input.GetAxis("Vertical") + "|" + Input.GetAxis("Mouse X") + "|" 
            + Input.GetAxis("Mouse Y"));
    }

    private void SaveKeyCode(KeyCode keyCode)
    {
        _streamWriter.Write("|" + keyCode);
    }

    private void CheckInput()
    {
        if (_bufferMode == false)
        {
            for (int i = 0; i < pressedButtons.Count; i++)
            {
                ExecuteInput(pressedButtons[i]);
            }
            return;
        }

        for (int i = 0; i < pressedButtons.Count; i++)
        {
            if (pressedButtons[i] == KeyCode.Joystick1Button6 || pressedButtons[i] == KeyCode.Joystick1Button7 ||
                pressedButtons[i] == KeyCode.Joystick1Button5)
            {
                ExecuteInput(pressedButtons[i]);
                continue;
            }
            BufferInput(pressedButtons[i]);
        }
        ExecuteBuffer();
    }
    private void BufferInput(KeyCode pressedButton)
    {
        _bufferStack.Push(pressedButton);
    }
    private void ExecuteBuffer()
    {
        if (_bufferStack.Count <= 0)
        {
            return;
        }
        ExecuteInput(_bufferStack.Pop());
        _bufferStack.Clear();
    }

    //Change keycodes to a bit representation might be faster
    private void ExecuteInput(KeyCode inputKeyCode)
    {
        switch (inputKeyCode)
        {
            case KeyCode.Joystick1Button0: //A Attack
                SaveKeyCode(inputKeyCode);
                ButtonA++;
                break;
            case KeyCode.Joystick1Button1: //B Dodge
                SaveKeyCode(inputKeyCode);
                ButtonB++;
                break;
            case KeyCode.Joystick1Button2: //X Jump
                SaveKeyCode(inputKeyCode);
                ButtonX++;
                break;
            case KeyCode.Joystick1Button3: //Y
                SaveKeyCode(inputKeyCode);
                ButtonY++;
                break;
            case KeyCode.Joystick1Button4: //LB
                SaveKeyCode(inputKeyCode);
                ButtonLB++;
                break;
            case KeyCode.Joystick1Button5: //RB
                if (lockToTarget.closestEnemy != null)
                {
                    SaveKeyCode(inputKeyCode);
                    ButtonRB++;
                }
                break;
            case KeyCode.Joystick1Button6: //Start Button
                SaveKeyCode(inputKeyCode);
                ButtonStart++;
                break;
            case KeyCode.Joystick1Button7: //Back
                SaveKeyCode(inputKeyCode);
                ButtonSelect++;
                break;
            case KeyCode.Joystick1Button8: //Left Stick Click
                SaveKeyCode(inputKeyCode);
                ButtonL3++;
                break;
            case KeyCode.Joystick1Button9: // Right Stick Click
                SaveKeyCode(inputKeyCode);
                ButtonR3++;
                break;
            case KeyCode.Joystick1Button10: //LT
                SaveKeyCode(inputKeyCode);
                ButtonLT++;
                break;
            case KeyCode.Joystick1Button11: //RT
                SaveKeyCode(inputKeyCode);
                ButtonRT++;
                break;
            case KeyCode.Joystick1Button12: //Up
                SaveKeyCode(inputKeyCode);
                ButtonUp++;
                break;
            case KeyCode.Joystick1Button13: //Down
                SaveKeyCode(inputKeyCode);
                ButtonDown++;
                break;
            case KeyCode.Joystick1Button14: //Left
                SaveKeyCode(inputKeyCode);
                ButtonLeft++;
                break;
            case KeyCode.Joystick1Button15: //Right
                SaveKeyCode(inputKeyCode);
                ButtonRight++;
                break;
        }
    }
    #endregion
}
