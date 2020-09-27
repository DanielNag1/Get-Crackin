using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Get input, Go over all buttons and save if the are pressed or not. (list of all buttons/triggers/etc.)
//analyse input, Go over the list.
//if bufferMode is off execute input, elsebufferedInput

//execute input that is imidiate actions, else
//buffer input
//wait for timer
//execute latest action in buffer if size>0



public class InputBuffer : ScriptableObject
{
  
    #region Variables
    public List<KeyCode> PressedButtons = new List<KeyCode>(10); //Get input
    private Stack<KeyCode> bufferStack = new Stack<KeyCode>();

    private bool bufferMode = true;
    private float bufferTimer = 0;

    public LockToTarget LockToTarget;
    #endregion

    #region singleton
    private static InputBuffer instance;
    public static InputBuffer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = CreateInstance<InputBuffer>();
            }
            return instance;
        }
    }
    #endregion

    #region Methods
    public void StartBuffer()
    {
        Debug.Log("BufferStart");
        CheckInput();
    }
    /// <summary>
    /// Analyse input
    /// </summary>
    private void CheckInput()
    {
        Debug.Log("BufferMode="+bufferMode);
        //if bufferMode is off execute input
        if (bufferMode == false)
        {
            Debug.Log("pressedButtons.Count=" + PressedButtons.Count);
            for (int i = 0; i < PressedButtons.Count; i++)
            {
                Debug.Log("PressedButtons[i] = "+ PressedButtons[i]);
                ExecuteInput(PressedButtons[i]);
            }
            return;
        }
     
        for (int i = 0; i < PressedButtons.Count; i++)
        {
            //execute input that is imidiate actions
            if (PressedButtons[i] == KeyCode.Joystick1Button6 || PressedButtons[i] == KeyCode.Joystick1Button7 || PressedButtons[i] == KeyCode.Joystick1Button5)
            {
                Debug.Log("ImidiateActionButton =" +PressedButtons[i]);
                ExecuteInput(PressedButtons[i]);
                continue;
            }
            BufferInput(PressedButtons[i]); //buffer input
        }
        ExecuteBuffer();
    }
    private void BufferInput(KeyCode pressedButton)
    {
        Debug.Log("BufferInput pushed ="+ pressedButton);
        bufferStack.Push(pressedButton); // populate the buffer in correct order, nice.
    }
    private void ExecuteBuffer()
    {
        if(bufferStack.Count <= 0)
        {
            return;
        }
        Debug.Log("BufferExecute Start");
        while (bufferTimer <= 0) // Wait for timer
        {
            bufferTimer -= Time.deltaTime;
            break;
        }
        ExecuteInput(bufferStack.Pop()); //execute the latest input on the stack
        bufferStack.Clear(); //clear the stack of inputs
    }
    private void ExecuteInput(KeyCode inputKeyCode)
    {
        Debug.Log("ExecuteInput Start, KeyCode ="+inputKeyCode);
        switch (inputKeyCode)
        {
            case KeyCode.Joystick1Button0:
                Debug.Log("ExecuteInput A");
                //MethodCall
                break;
            case KeyCode.Joystick1Button1:
                //MethodCall
                break;
            case KeyCode.Joystick1Button2:
                //MethodCall
                break;
            case KeyCode.Joystick1Button3:
                //MethodCall
                break;
            case KeyCode.Joystick1Button4:
                //MethodCall
                break;
            case KeyCode.Joystick1Button5:
                Debug.Log("ExecuteInput LockToTarget");
                LockToTarget.ManualTargeting();
                break;
            case KeyCode.Joystick1Button6:
                //MethodCall
                break;
            case KeyCode.Joystick1Button7:
                //MethodCall
                break;
            case KeyCode.Joystick1Button8:
                //MethodCall
                break;
            case KeyCode.Joystick1Button9:
                //MethodCall
                break;
            case KeyCode.Joystick1Button10:
                Debug.Log("ExecuteInput pressed trigger left");
                //MethodCall
                break;
            case KeyCode.Joystick1Button11:
                Debug.Log("ExecuteInput pressed trigger right");
                //MethodCall
                break;
        }
        Debug.Log("BufferDone");
    }
    #endregion
}
