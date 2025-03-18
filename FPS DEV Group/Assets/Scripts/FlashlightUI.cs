using UnityEngine;
using UnityEngine.UI; 

public class FlashlightUI : MonoBehaviour
{
    [Header("Battery UI Settings")]
    [SerializeField] private Image batteryFillImage; // The yellow bar representing battery life

    private Flashlight flashlightScript; // Reference to the Flashlight script

    void Start()
    {
        // Get the reference to the Flashlight script attached to the player (or flashlight)
        flashlightScript = GetComponentInParent<Flashlight>();

        // Ensure the batteryFillImage is set in the inspector
        if (batteryFillImage == null)
        {
            Debug.LogError("Battery Fill Image not assigned in the inspector.");
        }
    }

    void Update()
    {
        if (flashlightScript != null)
        {
            // Update the battery bar fill based on the flashlight's battery life
            float batteryLife = flashlightScript.GetBatteryLife(); // Get the current battery life from the Flashlight script
            batteryFillImage.fillAmount = batteryLife / 100f; // Normalize the battery life (from 0 to 1) for the UI
        }
    }
}