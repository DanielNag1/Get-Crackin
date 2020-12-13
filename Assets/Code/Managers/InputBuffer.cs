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
    private bool _bufferMode = true;
    private bool _menuMode = false;
    private Stack<KeyCode> _bufferStack = new Stack<KeyCode>();
    public List<KeyCode> pressedButtons = new List<KeyCode>(10); //Get input
    public GameObject player;
    public LockToTarget lockToTarget;
    public Animator animator;
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
        CheckInput();
    }

    /// <summary>
    /// Analyse input
    /// </summary>
    private void CheckInput()
    {
        //if bufferMode is off execute input
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
            //execute input that is imidiate actions
            if (pressedButtons[i] == KeyCode.Joystick1Button6 || pressedButtons[i] == KeyCode.Joystick1Button7 ||
                    pressedButtons[i] == KeyCode.Joystick1Button5)
            {
                ExecuteInput(pressedButtons[i]);
                continue;
            }
            BufferInput(pressedButtons[i]); //buffer input
        }
        ExecuteBuffer();
    }

    private void BufferInput(KeyCode pressedButton)
    {
        _bufferStack.Push(pressedButton); //populate the buffer in correct order, nice.
    }

    private void ExecuteBuffer()
    {
        if (_bufferStack.Count <= 0)
        {
            return;
        }
        ExecuteInput(_bufferStack.Pop()); //execute the latest input on the stack
        _bufferStack.Clear(); //clear the stack of inputs
    }

    private void ExecuteInput(KeyCode inputKeyCode)
    {
        if (_menuMode)
        {
            ExecuteMenuInput(inputKeyCode);
            return;
        }
        switch (inputKeyCode)
        {
            case KeyCode.Joystick1Button0: //A Attack
                if (animator.GetBool("Rage Mode")) //If the player is in rage mode -> Do the rage chain
                {
                    RageGroundCombos();
                }
                else
                {
                    GroundCombos();
                }
                break;
            case KeyCode.Mouse0:
                if (animator.GetBool("Rage Mode")) //If the player is in rage mode -> Do the rage chain
                {
                    RageGroundCombos();
                }
                else
                {
                    GroundCombos();
                }
                break;
            case KeyCode.Joystick1Button1: //B Dodge
                GroundDodge();
                break;
            case KeyCode.Mouse1:
                GroundDodge();
                break;
            case KeyCode.Joystick1Button4: //LB
                ActivateRageMode();
                break;
            case KeyCode.Joystick1Button5: //RB
                if (lockToTarget.closestEnemy != null)
                {
                    Debug.Log("ExecuteInput LockToTarget");
                    lockToTarget.ManualTargeting();
                }
                break;
            case KeyCode.Joystick1Button7: //Start Button
                _menuMode = true;
                PauseMenu.Instance.Pause();
                break;
            #region Unused Buttons
            case KeyCode.Joystick1Button2: //X Jump

                break;
            case KeyCode.Joystick1Button3: //Y

                break;
            case KeyCode.Joystick1Button6: //Back

                break;
            case KeyCode.Joystick1Button8: //Left Stick Click

                break;
            case KeyCode.Joystick1Button9: // Right Stick Click

                break;
            case KeyCode.Joystick1Button10: //LT

                break;
            case KeyCode.Joystick1Button11: //RT

                break;
                #endregion
        }
    }
    private void ExecuteMenuInput(KeyCode inputKeyCode)
    {
        switch (inputKeyCode)
        {
            case KeyCode.Joystick1Button7:
                _menuMode = false;
                PauseMenu.Instance.Resume();
                break;

            default:
                break;
        }
    }

    private void GroundCombos()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||  //If the player is in Idle/Walk/Run
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("GroundChain", 1);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack1")) //If the player continues the chain from 1 to 2
        {
            animator.SetInteger("GroundChain", 2);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack2")) //If the player continues the chain from 2 to 3
        {
            animator.SetInteger("GroundChain", 3);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack3")) //If the player continues the chain from 3 to 4
        {
            animator.SetInteger("GroundChain", 4);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
    }

    private void RageGroundCombos()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||  //If the player is in Idle/Walk/Run
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            animator.SetInteger("Rage GroundChain", 1);
            animator.SetTrigger("Rage Attack");
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack1")) //If the player continues the chain from 1 to 2
        {
            animator.SetInteger("Rage GroundChain", 2);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack2")) //If the player continues the chain from 2 to 3
        {
            animator.SetInteger("Rage GroundChain", 3);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
    }

    private void GroundDodge()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
                            animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") ||
                            animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (animator.GetFloat("movementMagnitude") > 0)
            {
                animator.SetTrigger("Dodge");
                //Move the charater
                Move move = player.GetComponent<Move>();
                move.DodgeMovementStart(player.GetComponent<Transform>());
            }
        }
    }

    private void ActivateRageMode()
    {
        if (RageMode.Instance.currentRage > 0)
        {
            if (animator.GetBool("Rage Mode"))
            {
                animator.SetBool("Rage Mode", false);
                VFXEvents.Instance.VFX5Stop();
                VFXEvents.Instance.VFX4Play();
            }
            else
            {
                animator.SetBool("Rage Mode", true);
                VFXEvents.Instance.VFX4Stop();
                VFXEvents.Instance.VFX5Play();
            }
        }
    }
    #endregion
}
