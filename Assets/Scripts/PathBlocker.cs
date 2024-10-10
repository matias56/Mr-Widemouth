using UnityEngine;
using UnityEngine.UI;

public class PathBlocker : MonoBehaviour
{
    public BookCollector bookCollector; // Drag the BookCollector object here
    public Text warningText; // Drag the UI Text element for the warning message
    public float triggerDistance = 5f; // Distance at which the warning appears

    private bool isShowingWarning = false;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, bookCollector.transform.position);

        if (distanceToPlayer <= triggerDistance)
        {
            if (BookCollector.collectedBooks != bookCollector.totalBooks && !isShowingWarning)
            {
                warningText.gameObject.SetActive(true);
                isShowingWarning = true;
            }
            else if (BookCollector.collectedBooks == bookCollector.totalBooks)
            {
                warningText.gameObject.SetActive(false);
                isShowingWarning = false;
                Destroy(this.gameObject);
            }
        }
        else if (isShowingWarning)
        {
            warningText.gameObject.SetActive(false);
            isShowingWarning = false;
        }
    }
}