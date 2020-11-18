﻿using System.Collections;
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
    public GameObject player;
    private Rigidbody enemyRB;
    public LockToTarget LockToTarget;
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
        //Debug.Log("BufferStart");
        CheckInput();

    }
    /// <summary>
    /// Analyse input
    /// </summary>
    private void CheckInput()
    {
        //Debug.Log("BufferMode="+bufferMode);
        //if bufferMode is off execute input
        if (bufferMode == false)
        {
            Debug.Log("pressedButtons.Count=" + PressedButtons.Count);
            for (int i = 0; i < PressedButtons.Count; i++)
            {
                Debug.Log("PressedButtons[i] = " + PressedButtons[i]);
                ExecuteInput(PressedButtons[i]);
            }
            return;
        }

        for (int i = 0; i < PressedButtons.Count; i++)
        {
            //execute input that is imidiate actions
            if (PressedButtons[i] == KeyCode.Joystick1Button6 || PressedButtons[i] == KeyCode.Joystick1Button7 || PressedButtons[i] == KeyCode.Joystick1Button5)
            {
                //Debug.Log("ImidiateActionButton =" +PressedButtons[i]);
                ExecuteInput(PressedButtons[i]);
                continue;
            }
            BufferInput(PressedButtons[i]); //buffer input
        }
        ExecuteBuffer();
    }
    private void BufferInput(KeyCode pressedButton)
    {
        //Debug.Log("BufferInput pushed ="+ pressedButton);
        bufferStack.Push(pressedButton); // populate the buffer in correct order, nice.
    }
    private void ExecuteBuffer()
    {
        if (bufferStack.Count <= 0)
        {
            return;
        }
        //Debug.Log("BufferExecute Start");
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
        //Debug.Log("ExecuteInput Start, KeyCode ="+inputKeyCode);
        switch (inputKeyCode)
        {
            case KeyCode.Joystick1Button0: //A Attack
                GroundCombos();
                RageGroundCombos();

                break;
            case KeyCode.Joystick1Button1: //B Dodge
                GroundDodge();
                break;
            case KeyCode.Joystick1Button2: //X Jump
                break;
            case KeyCode.Joystick1Button3: //Y
                break;
            case KeyCode.Joystick1Button4: //LB
                ActivateRageMode();
                break;
            case KeyCode.Joystick1Button5: //RB
                if (LockToTarget.closestEnemy != null)
                {
                    Debug.Log("ExecuteInput LockToTarget");
                    LockToTarget.ManualTargeting();
                }
                break;
            case KeyCode.Joystick1Button6: //Start Button

                break;
            case KeyCode.Joystick1Button7: //Back

                break;
            case KeyCode.Joystick1Button8: //Left Stick Click

                break;
            case KeyCode.Joystick1Button9: // Right Stick Click

                break;
            case KeyCode.Joystick1Button10: //LT

                break;
            case KeyCode.Joystick1Button11: //RT

                break;
        }
        //Debug.Log("BufferDone");
    }

    void GroundCombos()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||  //If the player is in Idle/Walk/Run
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("GroundChain", 1);
            player.GetComponentInChildren<WeaponCollision>().collisionActive = true;
            FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
            LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack1")) //If the player continues the chain from 1 to 2
        {

            animator.SetInteger("GroundChain", 2);
            player.GetComponentInChildren<WeaponCollision>().collisionActive = true;
            FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
            LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());

        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack2")) //If the player continues the chain from 2 to 3
        {
            animator.SetInteger("GroundChain", 3);
            player.GetComponentInChildren<WeaponCollision>().collisionActive = true;
            FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
            LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Chain1_Attack3")) //If the player continues the chain from 3 to 4
        {
            animator.SetInteger("GroundChain", 4);
            player.GetComponentInChildren<WeaponCollision>().collisionActive = true;
            FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
            LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
    }
    void RageGroundCombos()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||  //If the player is in Idle/Walk/Run
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (animator.GetBool("Rage Mode") == true) //If the player is in rage mode -> Do the rage chain
            {

                animator.SetInteger("Rage GroundChain", 1);
                animator.SetTrigger("Rage Attack");
                FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
                LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
                //player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
            }
        }

        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack1")) //If the player continues the chain from 1 to 2
        {
            animator.SetInteger("Rage GroundChain", 2);
            FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
            LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack2")) //If the player continues the chain from 2 to 3
        {
            animator.SetInteger("Rage GroundChain", 3);
            FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
            LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Rage Mode_Attack3")) //If the player is in chain 3
        {
            FreeCameraShake.Instance.ShakeCamera(1f, 0.1f);
            LockCameraShake.Instance.ShakeCamera(1f, 0.1f);
            player.GetComponent<Move>().AttackTowardsMovementStart(player.GetComponent<Transform>());
        }


    }
    void GroundDodge()
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
    

    void ActivateRageMode()
    {
        animator.ResetTrigger("Attack");
        if (RageMode.Instance.currentRage > 0)
        {
            if (!animator.GetBool("Rage Mode"))
            {
                animator.SetBool("Rage Mode", true);
                VFXEvents.Instance.VFX4Stop();
                VFXEvents.Instance.VFX5Play();
            }
            else if (animator.GetBool("Rage Mode"))
            {
                animator.SetBool("Rage Mode", false);
                VFXEvents.Instance.VFX5Stop();
            }

        }

    }

    #endregion
}
