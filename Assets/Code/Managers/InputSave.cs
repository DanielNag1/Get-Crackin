using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class InputSave : ScriptableObject
{
    #region Variables
    public List<KeyCode> PressedButtons = new List<KeyCode>(16); //Get input
    private Stack<KeyCode> bufferStack = new Stack<KeyCode>();
    private bool bufferMode = true;
    private float bufferTimer = 0;
    public LockToTarget LockToTarget;

    private int ButtonX, ButtonY, ButtonA, ButtonB, ButtonRT, ButtonRB, ButtonLT, ButtonLB, ButtonStart, ButtonSelect, ButtonR3, ButtonL3, ButtonUp, ButtonDown, ButtonLeft, ButtonRight = 0;
    private Vector3 MoveVectorInput, LookVectorInput;

    StreamWriter streamWriter;
    Stream stream;
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

    void Awake()
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
        streamWriter = new StreamWriter(Application.dataPath + @"InputSave" + number + ".txt", true);
    }
    public bool WantToQuit()
    {
        streamWriter.Write("#|ButtonX|ButtonY|ButtonA|ButtonB|ButtonRT|ButtonRB|ButtonLT|ButtonLB|ButtonStart|ButtonSelect|ButtonR3|ButtonL3|ButtonUp|ButtonDown|ButtonLeft|ButtonRight" + "\r\n");
        streamWriter.Write("$|"+ ButtonX + "|" + ButtonY + "|" + ButtonA + "|" + ButtonB + "|" + ButtonRT + "|" + ButtonRB + "|" + ButtonLT + "|" + ButtonLB + "|" + ButtonStart + "|" + ButtonSelect + "|" + ButtonR3 + "|" + ButtonL3 + "|" + ButtonUp + "|" + ButtonDown + "|" + ButtonLeft + "|" + ButtonRight);
        streamWriter.Close();
        return true;
    }
    public void SaveSeed(int seed)
    {
        streamWriter.Write("#|seed" + "\r\n");
        streamWriter.Write("£|" + seed + "\r\n");
        streamWriter.Write("#|MoveX|MoveY|CamX|CamY|ButtonKeyCodes" + "\r\n");
    }


    public void SaveBuffer()
    {
        SaveJoystickInput();
        CheckInput();
        streamWriter.Write("\r\n");
    }
    private void SaveJoystickInput()
    {
        streamWriter.Write(Input.GetAxis("Horizontal") + "|" + Input.GetAxis("Vertical") + "|" + Input.GetAxis("Mouse X") + "|" + Input.GetAxis("Mouse Y"));
    }
    private void SaveKeyCode(KeyCode keyCode)
    {
        streamWriter.Write("|" + keyCode);
    }

    private void CheckInput()
    {
        if (bufferMode == false)
        {
            for (int i = 0; i < PressedButtons.Count; i++)
            {
                ExecuteInput(PressedButtons[i]);
            }
            return;
        }

        for (int i = 0; i < PressedButtons.Count; i++)
        {
            if (PressedButtons[i] == KeyCode.Joystick1Button6 || PressedButtons[i] == KeyCode.Joystick1Button7 || PressedButtons[i] == KeyCode.Joystick1Button5)
            {
                ExecuteInput(PressedButtons[i]);
                continue;
            }
            BufferInput(PressedButtons[i]);
        }
        ExecuteBuffer();
    }
    private void BufferInput(KeyCode pressedButton)
    {
        bufferStack.Push(pressedButton);
    }
    private void ExecuteBuffer()
    {
        if (bufferStack.Count <= 0)
        {
            return;
        }
        while (bufferTimer <= 0)
        {
            bufferTimer -= Time.deltaTime;
            break;
        }
        ExecuteInput(bufferStack.Pop());
        bufferStack.Clear();
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
                if (LockToTarget.closestEnemy != null)
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
