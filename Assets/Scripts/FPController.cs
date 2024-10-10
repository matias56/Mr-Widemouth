using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FPController : MonoBehaviour
{
    public float speed = 10.0f;
    public float gravity = 10.0f;
    public Camera playerCamera;
    public float mouseSensitivity = 2.0f;
    public float verticalRange = 60.0f;

    private float verticalRotation = 0;
    private CharacterController controller;
    private Vector3 moveDirection;

    public AudioClip[] footstepSounds;
    public float footstepInterval = 0.5f; // Time between footsteps

    private float footstepTimer = 0f;
    private AudioSource audioSource;

    public GameObject done;



    public Image screen;


    public float fadeDuration = 3f; // How long the fade should take

    public Manager sp;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide the cursor
        controller = GetComponent<CharacterController>();

        audioSource = GetComponent<AudioSource>();

        screen.color = new Color(0f, 0f, 0f, 1f); // Start with black screen
        
        done.SetActive(false);

        sp = FindObjectOfType<Manager>();


       

        
    }

    void Update()
    {

       

        // Mouse look
        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRange, verticalRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Movement
    
        if(controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }



       

        if (sp.totemSp == true && Input.GetKeyDown(KeyCode.Return))
        {

            StartCoroutine(FadeInAndLoadScene());

        }

        moveDirection.y += -gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (controller.isGrounded && (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f))
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0)
            {
                audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Length)]);
                footstepTimer = footstepInterval;
            }
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