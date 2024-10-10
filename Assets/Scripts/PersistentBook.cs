using UnityEngine;

public class PersistentBook : MonoBehaviour
{
    public static bool[] bookCollected = new bool[9]; // Array to store book collection status

    private int bookIndex; // Index of this book in the array

    void Start()
    {
        bookIndex = transform.GetSiblingIndex(); // Get the book's index in the hierarchy

        // Check if this book was already collected
        if (bookCollected[bookIndex])
        {
            Destroy(gameObject);
        }
    }

    public void Collect()
    {
        bookCollected[bookIndex] = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }
}