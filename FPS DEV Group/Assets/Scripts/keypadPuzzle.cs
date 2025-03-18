using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class keypadPuzzle : MonoBehaviour
{
    [Header("----- Keypad Settings -----")]
    [Tooltip("The correct code to unlock the door")]
    [SerializeField] string correctCode = "1234";

    [Tooltip("Maximum number of digits in the code")]
    [SerializeField] int maxCodeLength = 4;

    [Tooltip("The display text for the keypad")]
    [SerializeField] TextMeshProUGUI displayText;

    [Tooltip("The door to unlock when the correct code is entered")]
    [SerializeField] door doorToUnlock;

    [Tooltip("Time in seconds to wait before resetting the code after a failed attempt")]
    [SerializeField] float resetTime = 1.5f;

    [Header("----- Interaction Settings -----")]
    [Tooltip("Distance player can interact with the keypad")]
    [Range(0.5f, 5f)]
    [SerializeField] float interactDistance = 2f;

    [Tooltip("Key player presses to interact with the keypad")]
    [SerializeField] KeyCode interactKey = KeyCode.E;

    [Header("----- Audio Settings -----")]
    [Tooltip("Sound played when a button is pressed")]
    [SerializeField] AudioClip buttonPressSound;

    [Tooltip("Sound played when the correct code is entered")]
    [SerializeField] AudioClip correctCodeSound;

    [Tooltip("Sound played when an incorrect code is entered")]
    [SerializeField] AudioClip incorrectCodeSound;

    [Tooltip("Volume level for sounds")]
    [Range(0f, 1f)]
    [SerializeField] float soundVolume = 0.7f;

    // Private variables
    private string currentCode = "";
    private bool isInteracting = false;
    private bool isSolved = false;
    private AudioSource audioSource;
    private Transform playerCamera;

    void Start()
    {
        // Initialize display
        if (displayText != null)
        {
            displayText.text = "_".PadRight(maxCodeLength, '_');
        }

        // Set up audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Get camera reference
        if (Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        // Check if player is looking at the keypad
        if (!isSolved)
        {
            CheckPlayerInteraction();
        }
    }

    void CheckPlayerInteraction()
    {
        if (playerCamera == null) return;

        // Check if player is within range and looking at the keypad
        if (Vector3.Distance(transform.position, playerCamera.position) <= interactDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactDistance))
            {
                // If player is looking at the keypad
                if (hit.collider.gameObject == gameObject)
                {
                    // Show interaction prompt if not already interacting
                    if (!isInteracting && gamemanager.instance != null)
                    {
                        gamemanager.instance.ShowInteractionPrompt($"Press {interactKey} to use keypad");
                    }

                    // Check for interaction input
                    if (Input.GetKeyDown(interactKey))
                    {
                        ToggleKeypadInteraction();
                    }

                    return;
                }
            }
        }

        // If we got here, player is not looking at keypad
        if (isInteracting)
        {
            // If player looks away while interacting, stop interacting
            ToggleKeypadInteraction();
        }
    }

    void ToggleKeypadInteraction()
    {
        isInteracting = !isInteracting;

        if (isInteracting)
        {
            // Start interacting
            if (gamemanager.instance != null)
            {
                gamemanager.instance.ShowInteractionPrompt("Enter code: 0-9 to input, Backspace to delete");
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Stop interacting
            if (gamemanager.instance != null)
            {
                gamemanager.instance.HideInteractionPrompt();
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnGUI()
    {
        // Only show buttons if player is interacting
        if (!isInteracting) return;

        // Keypad size and position parameters
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float keypadWidth = 200;
        float keypadHeight = 300;
        float keypadX = (screenWidth - keypadWidth) / 2;
        float keypadY = (screenHeight - keypadHeight) / 2;
        float buttonSize = 50;
        float spacing = 10;

        // Draw keypad background
        GUI.Box(new Rect(keypadX, keypadY, keypadWidth, keypadHeight), "");

        // Draw number buttons
        for (int i = 1; i <= 9; i++)
        {
            int row = (i - 1) / 3;
            int col = (i - 1) % 3;
            float x = keypadX + col * (buttonSize + spacing) + spacing;
            float y = keypadY + row * (buttonSize + spacing) + spacing + 70; // Extra space at top for display

            if (GUI.Button(new Rect(x, y, buttonSize, buttonSize), i.ToString()))
            {
                PressButton(i.ToString());
            }
        }

        // Draw 0 button (centered at bottom)
        if (GUI.Button(new Rect(keypadX + spacing + buttonSize + spacing, keypadY + 3 * (buttonSize + spacing) + 70, buttonSize, buttonSize), "0"))
        {
            PressButton("0");
        }

        // Draw clear button
        if (GUI.Button(new Rect(keypadX + spacing, keypadY + 3 * (buttonSize + spacing) + 70, buttonSize, buttonSize), "C"))
        {
            ClearCode();
        }

        // Draw enter button
        if (GUI.Button(new Rect(keypadX + 2 * (buttonSize + spacing) + spacing, keypadY + 3 * (buttonSize + spacing) + 70, buttonSize, buttonSize), "E"))
        {
            SubmitCode();
        }

        // Draw display at top
        GUI.Box(new Rect(keypadX + spacing, keypadY + spacing, keypadWidth - 2 * spacing, 50), currentCode.PadRight(maxCodeLength, '_'));
    }

    void PressButton(string digit)
    {
        // Play button press sound
        if (audioSource != null && buttonPressSound != null)
        {
            audioSource.PlayOneShot(buttonPressSound, soundVolume);
        }

        // Add digit to code if not at max length
        if (currentCode.Length < maxCodeLength)
        {
            currentCode += digit;

            // Update display
            if (displayText != null)
            {
                // Show asterisks for entered code and underscores for remaining digits
                string displayValue = new string('*', currentCode.Length).PadRight(maxCodeLength, '_');
                displayText.text = displayValue;
            }
        }
    }

    void ClearCode()
    {
        // Clear current code
        currentCode = "";

        // Update display
        if (displayText != null)
        {
            displayText.text = "_".PadRight(maxCodeLength, '_');
        }

        // Play button press sound
        if (audioSource != null && buttonPressSound != null)
        {
            audioSource.PlayOneShot(buttonPressSound, soundVolume);
        }
    }

    void SubmitCode()
    {
        // Check if code is correct
        if (currentCode == correctCode)
        {
            // Play correct code sound
            if (audioSource != null && correctCodeSound != null)
            {
                audioSource.PlayOneShot(correctCodeSound, soundVolume);
            }

            // Unlock the door
            if (doorToUnlock != null)
            {
                string keyID = "";  // The door script expects a key ID
                doorToUnlock.UseKey(keyID);  // This will unlock the door

                // Show success message
                if (gamemanager.instance != null)
                {
                    gamemanager.instance.ShowInteractionPrompt("Code accepted! Door unlocked.");
                }

                // Mark as solved and exit interaction mode
                isSolved = true;
                isInteracting = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            // Play incorrect code sound
            if (audioSource != null && incorrectCodeSound != null)
            {
                audioSource.PlayOneShot(incorrectCodeSound, soundVolume);
            }

            // Show error message
            if (gamemanager.instance != null)
            {
                gamemanager.instance.ShowInteractionPrompt("Incorrect code! Try again.");
            }

            // Reset code after delay
            StartCoroutine(ResetCodeAfterDelay());
        }
    }

    IEnumerator ResetCodeAfterDelay()
    {
        yield return new WaitForSeconds(resetTime);
        ClearCode();
    }
}