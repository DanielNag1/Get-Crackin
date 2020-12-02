using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerTriggerInteraction : MonoBehaviour
{
    private TriggerComponent callableUnit;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Trigger")
        {
            callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
            callableUnit.ActivateTrigger();
        }
    }
}
