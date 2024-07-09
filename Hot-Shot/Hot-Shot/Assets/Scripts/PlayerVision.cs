using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    // Sensitivity of the mouse movement
    [SerializeField] float mouseSensitivity;

    // Reference to the player's body
    [SerializeField] Transform playerBod;

    // Vertical rotation 
    float xRotation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        // Gets mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculates vertical rotation and clamping it to prevent the user from over rotating
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        // Apply rotation to the camera and player body
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBod.Rotate(Vector3.up * mouseX);
    }
}
