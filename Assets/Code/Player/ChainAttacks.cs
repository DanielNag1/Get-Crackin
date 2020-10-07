using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAttacks : MonoBehaviour
{
    //gets command to start an attack from input buffer.
    //check chainCounter for what attack to execute, and increment it by one, create a coroutine with a timer, once timer is zero reset chainCounter.
    //Execute attack with correpsponding chainCounter Value.
    private int chainCounter = 0;
    private Animator animator;

    public void ChainAttackDriver()
    {
        CheckChainCounter();
    }
     private void CheckChainCounter()
    {
        
    }

    private void ExecuteChainAttack()
    {
        animator.GetCurrentAnimatorStateInfo(0).IsName("");
    }


}
