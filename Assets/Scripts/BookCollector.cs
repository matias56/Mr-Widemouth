using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class BookCollector : MonoBehaviour
{
    public int totalBooks = 9;
    public static int collectedBooks = 0;
    public Text bookCountText; //  Drag your UI Text element here
    public Text bookPageText; // Drag the Text component for the book page content
    public Text bookPageText2; // Drag the Text component for the book page content
    public string[] bookPageTexts; // Array to hold text for each book


    public GameObject bookPagePanel; // Drag the panel containing your book page text here

    private bool isReading = false;



    private void Start()
    {
    }

    public void CollectBook()
    {
        collectedBooks++;
        UpdateBookCountText();

        ShowBookPage(collectedBooks - 1); // Pass the index of the collected book (0-indexed)


    }

    void UpdateBookCountText()
    {
        if (bookCountText != null)
        {
            bookCountText.text = "Books: " + collectedBooks + "/" + totalBooks;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Book"))
        {
            CollectBook();
        }
    }

    void ShowBookPage(int bookIndex)
    {
        if (bookIndex < bookPageTexts.Length)
        {
            bookPageText.text = bookPageTexts[bookIndex];
            bookPageText2.text = bookPageTexts[bookIndex + 1];
            bookPagePanel.SetActive(true);
            isReading = true;
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor to press E
            Time.timeScale = 0f; // Stop time (optional, to completely freeze the game)
        }
    }

    void Update()
    {
        if (isReading && Input.GetKeyDown(KeyCode.E))
        {
            bookPagePanel.SetActive(false);
            isReading = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again
            Time.timeScale = 1f; // Resume time
        }
    }
}