using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Enter : MonoBehaviour
{
    public Image t;

    public float fadeDuration = 3f; // How long the fade should take

    private AudioSource audioSource;

    public string outsideSceneName; // Name of the outside scene
    public string insideSceneName;  // Name of the inside scene
    public Transform outsideSpawnPoint; // Drag the spawn point Transform here


    // Start is called before the first frame update
    void Start()
    {
        t.color = new Color(0f, 0f, 0f, 1f); // Start with black screen
        StartCoroutine(FadeOut());
        audioSource = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeInAndLoadScene());

        }
    }

    IEnumerator FadeInAndLoadScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            audioSource.Play();

            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            t.color = new Color(0f, 0f, 0f, alpha); // Change alpha only
            yield return null;
        }

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == outsideSceneName)
        {
            // Entering the house
            SceneManager.LoadScene(insideSceneName);
        }
        else if (currentScene == insideSceneName)
        {
            // Exiting the house
            SceneManager.LoadScene(outsideSceneName);

            // Move the player to the spawn point AFTER the scene loads
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }


    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            t.color = new Color(0f, 0f, 0f, alpha); // Change alpha only
            yield return null;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == outsideSceneName)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = outsideSpawnPoint.position;
                player.transform.rotation = outsideSpawnPoint.rotation; // Optional: Set rotation
            }
            SceneManager.sceneLoaded -= OnSceneLoaded; // Remove the listener
        }
    }
}
