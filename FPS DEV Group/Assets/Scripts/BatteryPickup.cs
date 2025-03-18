using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [Header("Battery Settings")]
    [SerializeField] private float batteryAmount = 20f; // Amount of battery this pickup gives

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the battery is the player
        if (other.CompareTag("Player"))
        {
            // Find the player's flashlight script
            Flashlight flashlight = other.GetComponentInChildren<Flashlight>();
            if (flashlight != null)
            {
                // Recharge the player's flashlight
                flashlight.RechargeBattery(batteryAmount);

                // Destroy the battery object after pickup
                Destroy(gameObject);
            }
        }
    }
}