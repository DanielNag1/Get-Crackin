using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    public GameObject gate;

    void OnTriggerEnter(Collider other)
    {
        gate.GetComponent<Animator>().Play("Gate_Open");
        this.GetComponent<BoxCollider>().enabled = false;  //turning off the boxcollider
        StartCoroutine(DoorClose());
    }
   
    public IEnumerator DoorClose()
    {
        yield return new WaitForSeconds(5);
        gate.GetComponent<Animator>().Play("Gate_Close");
        this.GetComponent<BoxCollider>().enabled = true;
    }
}
