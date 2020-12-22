using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healingValue = 10;
    public void PickupHealth()
    {
        Destroy(this.gameObject); // Destroys the mushroom object
    }

}
