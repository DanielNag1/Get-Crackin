using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public void PickupHealth()
    {
        // TO DO: Music implementation 

        Destroy(this.gameObject); // Destroys the mushroom object
    }
}
