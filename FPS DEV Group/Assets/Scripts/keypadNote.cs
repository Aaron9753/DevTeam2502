using UnityEngine;

public class keypadNote : MonoBehaviour
{
    [Header("----- Note Settings -----")]
    [Tooltip("The code hint to display to the player")]
    [SerializeField] string codeHint = "The lab security code is in Dr. Miller's birthday: 0517";

    [Tooltip("Sound played when note is picked up")]
    [SerializeField] AudioClip pickupSound;

    [Tooltip("Volume level for pickup sound")]
    [Range(0f, 1f)]
    [SerializeField] float soundVolume = 0.5f;

    [Header("----- Visual Effects -----")]
    [Tooltip("Should the note float and rotate for visibility?")]
    [SerializeField] bool useVisualEffects = true;

    [Tooltip("How fast the note rotates")]
    [Range(0f, 180f)]
    [SerializeField] float rotationSpeed = 20f;

    [Tooltip("How high the note bobs up and down")]
    [Range(0f, 0.5f)]
    [SerializeField] float bobHeight = 0.1f;

    [Tooltip("How fast the note bobs up and down")]
    [Range(0.1f, 5f)]
    [SerializeField] float bobSpeed = 0.8f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (useVisualEffects)
        {
            // Rotate the note
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Make the note bob up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReadNote();
        }
    }

    void ReadNote()
    {
        // Play pickup sound
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
        }

        // Show message to player
        if (gamemanager.instance != null)
        {
            gamemanager.instance.ShowInteractionPrompt(codeHint);
        }

        // Optional: Remove the note from the game
        Destroy(gameObject);
    }
}

