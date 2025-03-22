using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlight Settings")]
    [SerializeField] private Light flashlight; // The light source for the flashlight
    [SerializeField] private KeyCode toggleKey = KeyCode.F; // The key to toggle flashlight on/off
    [SerializeField] private float batteryLife = 100f; // Starting battery life (100%)
    [SerializeField] private float batteryDrainRate = 1f; // Battery drain rate per second when flashlight is on
    [SerializeField] private float batteryRechargeRate = 10f; // Recharge rate when flashlight is off

    private bool isFlashlightOn = false; // Is the flashlight on?

    void Update()
    {
        // Toggle flashlight when the toggle key (e.g., F) is pressed
        if (Input.GetKeyDown(toggleKey) && batteryLife > 0)
        {
            ToggleFlashlight();
        }

        // If the flashlight is on, drain battery over time
        if (isFlashlightOn)
        {
            batteryLife -= batteryDrainRate * Time.deltaTime;
            if (batteryLife <= 0)
            {
                batteryLife = 0;
                ToggleFlashlight(); // Turn off flashlight when battery is empty
            }
        }
        else
        {
            // Recharge battery when flashlight is off
            batteryLife += batteryRechargeRate * Time.deltaTime;
            batteryLife = Mathf.Min(batteryLife, 100f); // Cap battery life at 100%
        }

        // Adjust flashlight intensity based on remaining battery life (optional)
        // flashlight.intensity = Mathf.Lerp(0.1f, 1f, batteryLife / 100f);
    }

    // Toggle the flashlight on or off
    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashlight.enabled = isFlashlightOn; // Enable or disable the flashlight light
    }

    // Public method to allow other scripts to recharge the flashlight's battery
    public void RechargeBattery(float amount)
    {
        batteryLife += amount;
        batteryLife = Mathf.Min(batteryLife, 100f); // Cap the battery at 100%
    }

    // Optional: Get the current battery life for UI updates
    public float GetBatteryLife()
    {
        return batteryLife;
    }
}