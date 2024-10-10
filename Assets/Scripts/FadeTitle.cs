using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTitle : MonoBehaviour
{
    public Image t;

    public float fadeDuration = 3f; // How long the fade should take

    // Start is called before the first frame update
    void Start()
    {
        t.color = new Color(0f, 0f, 0f, 1f); // Start with black screen
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(FadeInAndLoadScene());
        }
    }


    IEnumerator FadeInAndLoadScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            t.color = new Color(0f, 0f, 0f, alpha); // Change alpha only
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Replace with your scene name
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
}
