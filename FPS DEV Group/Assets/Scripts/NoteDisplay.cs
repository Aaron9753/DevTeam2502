using UnityEngine;
using UnityEngine.UI;

public class NoteDisplay : MonoBehaviour
{
    public GameObject noteUI;  // Reference to the panel that displays the note
    public Text noteText;      // Reference to the Text UI element

    private bool isPlayerNear = false; // Check if player is near the note

    public string[] noteContents;  // The contents of the note 

    private int currentPage = 0;   // Track the current page of the note

    void Update()
    {
        // If the player is near the note and presses the interact button (E)
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            ToggleNote();  // Toggle the note UI visibility
        }

        // If the note UI is visible and the player presses the "Next" button (Space)
        if (noteUI.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            NextPage();
        }
    }

    // Toggle the note's visibility on or off
    void ToggleNote()
    {
        noteUI.SetActive(!noteUI.activeSelf);  // If the note UI is visible, hide it; if hidden, show it

        if (noteUI.activeSelf)
        {
            DisplayPage(currentPage);  // Display the current page of the note
        }
    }

    // Display the content of a specific note page
    void DisplayPage(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < noteContents.Length)
        {
            noteText.text = noteContents[pageIndex];  // Set the text of the note to the current page
        }
    }

    // Go to the next page of the note
    void NextPage()
    {
        currentPage++;
        if (currentPage >= noteContents.Length)
        {
            currentPage = 0;  // Loop back to the first page if on the last page
            ToggleNote();  // Close the note when reaching the last page
        }

        DisplayPage(currentPage);  // Update the page display
    }

    // Detect if the player is near the note
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            isPlayerNear = true;  // Player is near the note
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;  // Player left the trigger area
        }
    }
}