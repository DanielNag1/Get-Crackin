using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerInteraction : MonoBehaviour
{
    private TriggerComponent callableUnit;
    private LevelManager levelManager;
    //private GateOpen openGate;

    private void Awake()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        //openGate = GameObject.FindGameObjectWithTag("Gate").GetComponent<GateOpen>();
        
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.collider.gameObject.tag == "Trigger")
        {
            Debug.Log("Collided with Trigger");
            callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
            callableUnit.ActivateTrigger();
            //openGate.DoorClose();
            Debug.Log("Done");
        }
        if (hit.collider.gameObject.tag == "LevelTrigger")
        {
            Debug.Log("Collided with LevelTrigger");
            callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
            callableUnit.ActivateTrigger();
            levelManager.LoadNextLevel();
            Debug.Log("Done");
        }
    }
}
