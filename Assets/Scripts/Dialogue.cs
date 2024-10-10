using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public GameObject dialoguePanel; // The panel containing your dialogue UI
    public Text dialogueText;        // The Text component to display the dialogue
    public string[] dialogueLines;   // Array of dialogue lines
    public float dialogueSpeed = 0.05f; // Time between characters

    public float triggerDistance = 3f; // Distance to trigger the dialogue
    public float viewAngle = 60f;    // Angle within which the player must be looking

    private bool isDialogueStarted = false;
    private int currentLine = 0;
    private FPController playerController; // Reference to the player's FPController script

    public Color playerTextColor = Color.white;  // Color for the player's dialogue
    public Color creatureTextColor = Color.red; // Color for the creature's dialogue

    public GameObject interactionPrompt; // Drag the UI element for the spacebar prompt here
    public GameObject totemPrefab;      // Drag your totem prefab here

    public GameObject purpleParticlesPrefab; // Drag your purple particles prefab here


    void Start()
    {
        dialoguePanel.SetActive(false); // Hide the dialogue panel initially
        playerController = FindObjectOfType<FPController>(); // Get the FPController
    }

    void Update()
    {
        if (interactionPrompt.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            PlaceTotem();
            interactionPrompt.SetActive(false); // Hide the prompt
        }
        if (!isDialogueStarted)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                transform.LookAt(player.transform);

                // Check distance
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= triggerDistance)
                {
                    // Check view angle
                    Vector3 directionToPlayer = player.transform.position - transform.position;
                    float angle = Vector3.Angle(transform.forward, directionToPlayer);

                    Camera playerCamera = player.GetComponentInChildren<Camera>();
                    if (playerCamera != null)
                    {
                        Vector3 screenPoint = playerCamera.WorldToScreenPoint(transform.position);
                        bool isInCenter = screenPoint.x == Screen.width / 2f && screenPoint.y == Screen.height / 2f;

                        if (isInCenter)
                        {
                            // Start the dialogue
                            StartDialogue();
                            isDialogueStarted = true;

                            // Stop player movement
                            playerController.enabled = false;
                        }
                    }
                        

                    if (angle <= viewAngle / 2f)
                    {
                        // Start the dialogue
                        StartDialogue();
                        isDialogueStarted = true;

                        // Stop player movement
                        playerController.enabled = false;
                    }
                }


            }
        }
        else
        {
            // Continue the dialogue (e.g., display the next line when spacebar is pressed)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentLine++;
                if (currentLine < dialogueLines.Length)
                {
                    // Display the next line
                    StopAllCoroutines(); // Stop previous coroutine if running
                    StartCoroutine(TypeSentence(dialogueLines[currentLine], currentLine % 2 == 0));
                }
               
            }
        }

        if (isDialogueStarted && currentLine >= dialogueLines.Length && Input.GetKeyDown(KeyCode.Space))
        {
            EndDialogue(); // Call EndDialogue() to hide the panel
        }
    }

    void PlaceTotem()
    {
        // Calculate the position in front of the creature
        Vector3 spawnOffset = transform.forward * 2f; // 2 units in front
        Vector3 spawnPos = transform.position + spawnOffset;

        // Instantiate the totem
        Instantiate(totemPrefab, spawnPos, totemPrefab.transform.rotation);

        purpleParticlesPrefab.SetActive(true);

        Destroy(gameObject, 3f);
    }

    void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeSentence(dialogueLines[currentLine], currentLine % 2 == 0));
    }

    // Coroutine to type out the dialogue text character by character
    System.Collections.IEnumerator TypeSentence(string sentence, bool isPlayerSpeaking)
    {
        dialogueText.text = "";
        dialogueText.color = isPlayerSpeaking ? playerTextColor : creatureTextColor; // Set color

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueSpeed);

        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueStarted = false;

        interactionPrompt.SetActive(true); // Show the spacebar prompt
    }
}
