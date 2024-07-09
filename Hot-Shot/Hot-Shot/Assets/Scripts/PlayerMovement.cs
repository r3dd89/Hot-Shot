using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speed of player
    [SerializeField] float speed;

    // Jump speed of player
    [SerializeField] float jumpSpeed;

    // Gravity that affects the player
    [SerializeField] float gravity;

    // Direction vector for player mobility
    Vector3 movementDirection = Vector3.zero;

    // This references the CharacterController component
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    // Method that handles the player's movement
    void HandleMovement()
    {
        // Checks if the player is grounded
        if(characterController.isGrounded)
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Transform the movement direction to be relative to the player's position
            movementDirection = transform.TransformDirection(movementDirection);

            // Apply movement speed
            movementDirection *= speed;

            // Check if the player is trying to jump
            if(Input.GetButton("Jump"))
            {
                movementDirection.y = jumpSpeed;
            }
        }

        // Applies gravity to the movement direction
        movementDirection.y -= gravity * Time.deltaTime;

        // Move the player using the CharacterController
        characterController.Move(movementDirection * Time.deltaTime);
    }
}
