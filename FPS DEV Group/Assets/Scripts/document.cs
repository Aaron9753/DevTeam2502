using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class document : MonoBehaviour
{

    [Header("----- Document Settings -----")]
    [Tooltip("Title of the document")]
    [SerializeField] string documentTitle;

    [Tooltip("Content of the document")]
    [SerializeField] [TextArea(5, 20)] string documentContent;

    [Tooltip("Sound played when player picks up the document")]
    [SerializeField] AudioClip paperSound;

    [Tooltip("Volume level for paper sound")]
    [SerializeField] [Range(0f, 1f)] float paperSoundVolume = 0.5f;

    [Header("----- Visual Effects -----")]
    [Tooltip("Does the document float and rotate?")]
    [SerializeField] bool useVisualEffects = true;

    [Tooltip("How fast the document rotates")]
    [SerializeField] float rotationSpeed = 20f;

    [Tooltip("How high the document bobs up and down")]
    [SerializeField] [Range(0f, 0.5f)] float bobHeight = 0.1f;

    [Tooltip("How fast the document bobs up and down")]
    [SerializeField] [Range(0.1f, 5f)] float bobSpeed = 0.8f;

    [Header("----- Interaction Settings -----")]
    [Tooltip("Distance player can interact with the document")]
    [SerializeField] [Range(0.5f, 5f)] float interactDistance = 2f;

    [Tooltip("Key player presses to interact with/pickup document ")]
    [SerializeField] KeyCode interactKey = KeyCode.E;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
