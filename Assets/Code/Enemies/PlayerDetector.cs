using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool PlayerInRange => player != null;

    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            StartCoroutine(ClearDetectedPlayerAfterADelay());
        }
    }

    private IEnumerator ClearDetectedPlayerAfterADelay()
    {
        yield return new WaitForSeconds(2);  //run away for minimun 2 seconds.
        player = null;
    }

    public Vector3 GetPosition()
    {
        //returns value of player.transform.position if it isnt null, otherwise it evaluates Vector3.zero and returns its result.
        return player?.transform.position ?? Vector3.zero;
    }
}
