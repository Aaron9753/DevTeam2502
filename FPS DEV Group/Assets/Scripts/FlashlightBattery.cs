using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    public Image batteryFill;  
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float batteryDrainRate = 5f;  // How quickly the flashlight drains the battery

    private void Update()
    {
        // Simulate battery drain
        if (currentBattery > 0)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
        }
        else
        {
            currentBattery = 0;
        }

        // Update the UI fill based on battery percentage
        UpdateBatteryUI();
    }

    void UpdateBatteryUI()
    {
        // The battery fill's fill amount will be the current battery percentage.
        batteryFill.fillAmount = currentBattery / maxBattery;
    }
}