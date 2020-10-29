using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public void PickupHealth()
    {
        // TO DO: Sound effect implementation 
        // TO DO: Pick up implementation

        Destroy(this.gameObject); // Destroys the mushroom object
    }

}