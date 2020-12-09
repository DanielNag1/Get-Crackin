using UnityEngine;

public class PlayerTriggerInteraction : MonoBehaviour
{
    private TriggerComponent _callableUnit;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.tag == "Trigger")
        {
            _callableUnit = hit.collider.gameObject.GetComponent<TriggerComponent>();
            _callableUnit.ActivateTrigger();
        }
    }
}
