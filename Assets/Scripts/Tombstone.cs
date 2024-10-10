using UnityEngine;
using UnityEngine.UI; // For UI elements

public class Tombstone : MonoBehaviour
{
    public GameObject interactionPrompt; // Drag the UI element for the spacebar prompt here

    public int totalTombs = 6;
    public static int collectedTombs = 0;
    public Text tombCountText; //  Drag your UI Text element here
    public Text tombText; // Drag the Text component for the book page content
    public string[] tombTexts; // Array to hold text for each book

    public GameObject tombPanel;

    private bool isReading = false;

    public bool inRange = false;

    public Collider coll;

    public Manager m;

    private bool isRead = false;

    private void Start()
    {
        m = FindObjectOfType<Manager>();
    }
    public void CollectTomb()
    {
        collectedTombs++;
        UpdateTombCountText();

        ShowBookPage(collectedTombs - 1); // Pass the index of the collected book (0-indexed)

        m.tombstonesRead++;

    }

    void UpdateTombCountText()
    {
        if (tombCountText != null)
        {
            tombCountText.text = "Tombs: " + collectedTombs + "/" + totalTombs;
        }
    }

    void ShowBookPage(int tombIndex)
    {
        if (tombIndex < tombTexts.Length)
        {
            tombText.text = tombTexts[tombIndex];
            tombPanel.SetActive(true);
            isReading = true;
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor to press E
            Time.timeScale = 0f; // Stop time (optional, to completely freeze the game)
        }
    }

    void Update()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isRead)
            {
                interactionPrompt.SetActive(false); // Hide the prompt
                CollectTomb();
                isRead = true;
            }
        }


        if (isReading && Input.GetKeyDown(KeyCode.E))
        {
            tombPanel.SetActive(false);
            isReading = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
            Time.timeScale = 1f; // Resume time

            Destroy(this);
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(true); // Show the spacebar prompt
            inRange = true;
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionPrompt.SetActive(false); // Hide the prompt
        }
    }
}