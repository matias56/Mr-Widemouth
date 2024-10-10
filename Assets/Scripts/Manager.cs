using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject creaturePrefab; // Drag your creature prefab here

    public int tombstonesToRead = 6;
    public int tombstonesRead = 0;

    public bool creatureSpawned = false;
    public GameObject totem;
    public bool totemSp = false;

    public GameObject done;


    public Image screen;


    public float fadeDuration = 6f; // How long the fade should take
    // Start is called before the first frame update
    void Start()
    {
        totem = GameObject.FindGameObjectWithTag("Totem");

        screen.color = new Color(0f, 0f, 0f, 1f); // Start with black screen

        if (totem == null )
        {
            return;
        }

        done.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        totem = GameObject.FindGameObjectWithTag("Totem");


        if (creaturePrefab == null )
        {
            creatureSpawned = false;
        }

        if (totem != null)
        {
            totemSp = true;
        }

        if (tombstonesRead >= tombstonesToRead && !creatureSpawned)
        {
            SpawnCreature();
            creatureSpawned = true;
        }

        if (totemSp == true)
        {
            done.SetActive(true);
            StartCoroutine(FadeInAndLoadScene());

        }
    }

    void SpawnCreature()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Get player's position and rotation
            Vector3 playerPos = player.transform.position;
            Quaternion playerRot = player.transform.rotation;

            // Calculate spawn position behind the player
            Vector3 spawnOffset = playerRot * Vector3.back * 5f; // 5 units behind
            Vector3 spawnPos = playerPos + spawnOffset;

            // Instantiate the creature
            Instantiate(creaturePrefab, spawnPos, creaturePrefab.transform.rotation);
        }
    }

    IEnumerator FadeInAndLoadScene()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

            screen.color = new Color(0f, 0f, 0f, alpha); // Change alpha only
            yield return null;
        }

        SceneManager.LoadScene("Epilogue"); // Replace with your scene name
    }
}