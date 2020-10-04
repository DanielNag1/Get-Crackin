using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerInteraction : MonoBehaviour
{
    private TriggerComponent callableUnit;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag != "Trigger")
            return;

        Debug.Log("Collided with Trigger");
        callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
        callableUnit.ActivateTrigger();
        Debug.Log("Done");
    }
}
