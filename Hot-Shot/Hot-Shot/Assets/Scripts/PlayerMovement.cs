using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // This references the CharacterController component
    [SerializeField] CharacterController characterController;
    [SerializeField] LayerMask ignoreMask;

    // Movement speed of player
    [SerializeField] float speed;

    // Jump speed of player
    [SerializeField] float jumpSpeed;

    // How fast the player can sprint
    [SerializeField] float sprintMod;

    // The amount of jumps the player can do
    [SerializeField] int jumpsMax;

    // Gravity that affects the player
    [SerializeField] float gravity;

    // How much damage the players gun can do
    [SerializeField] int shootDamage;
    [SerializeField] int maxAmmo;

    // How often the player can shoot
    [SerializeField] float shootRate;

    // How far the player can shoot
    [SerializeField] int shootDistance;

    // The amount of hp the player has

    // The ammo for the weapon
    [SerializeField] GameObject cube;

    [SerializeField] PickupFlash pickupFlash;


    // Direction vector for player mobility
    Vector3 moveDirection;
    Vector3 playerVelocity;

    int jumpCount;
    int ammoOrigin;

    // are we shooting?
    bool isShooting;

    // Wall jump stuff to make sure it feels better
    bool isTouchingWall;
    bool canWallJump = true;
    bool delayReturn = true;
    Vector3 wallNormal;

    GameObject lastCollidedObject;
    GameObject currentCollidedObject;


    // Start is called before the first frame update
    void Start()
    {
        ammoOrigin = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.yellow);

        HandleMovement();
        sprint();

        CheckCollisions();
    }

    // Method to handle ammo pickup
    public void PickupAmmo(int ammoAmount)
    {
        maxAmmo += ammoAmount;
        if (maxAmmo > ammoOrigin)
        {
            maxAmmo = ammoOrigin;
        }

        // Trigger the flash effect
        pickupFlash.Flash();
    }

    // Method that handles the player's movement
    void HandleMovement()
    {
        // Applies gravity to the movement direction
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the player using the CharacterController
        characterController.Move(moveDirection * Time.deltaTime);
        // Checks if the player is grounded
        if (characterController.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
            isTouchingWall = false;
        }

        // Transform the movement direction to be relative to the player's position
        Vector3 forwardMovement = Input.GetAxis("Vertical") * transform.forward;
        Vector3 sidewaysMovement = Vector3.zero;

        // Apply movement speed
        if (delayReturn)
            sidewaysMovement = Input.GetAxis("Horizontal") * transform.right;

        moveDirection = forwardMovement + sidewaysMovement;
        characterController.Move(moveDirection * speed * Time.deltaTime);

        //getButton is for variable height, getButtonDown is for pressed button, getButtonUp is for released button
        // Check if the player is trying to jump
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < jumpsMax)
            {
                jumpCount++;
                playerVelocity.y = jumpSpeed;
            }
            else if (isTouchingWall && !characterController.isGrounded && Input.GetButtonDown("Jump"))
            {
                StartCoroutine(DelayWallJump());
                jumpsMax += 1;
                jumpCount++;
                //force away from the wall
                Vector3 jumpDir = (Vector3.up + (wallNormal * 2) + moveDirection).normalized;
                playerVelocity = jumpDir * jumpSpeed;
                //resetting wall jumps
                isTouchingWall = false;
                jumpsMax -= 1;
            }
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        if (Input.GetButton("Shoot") && !isShooting)
            StartCoroutine(shoot());
    }

    void sprint()
    {
        // sprinting
        if (Input.GetButtonDown("Sprint"))
            speed *= sprintMod;

        else if (Input.GetButtonUp("Sprint"))
            speed /= sprintMod;

    }
    IEnumerator shoot()
    {
        //Ignoring the player
        int layerMask = ~ignoreMask & ~(1 << LayerMask.NameToLayer("Player"));
        RaycastHit hit;
        if (maxAmmo > 0)
        {
            isShooting = true;

            // used to return info on what the raycast hits
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, layerMask))
            {
                Debug.Log(hit.collider.name);
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                if (hit.transform != transform && dmg != null)
                    dmg.takeDamage(shootDamage);
                maxAmmo -= 1;
                //Instantiate(cube, hit.point, transform.rotation);
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;

        }
        else if (maxAmmo <= 0)
            Debug.Log("No Ammo");
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y < 0.1f)
        {
            isTouchingWall = true;
            wallNormal = hit.normal;
            //Debug.Log("Player is touching a wall: " + hit.collider.name);
        }
    }
    void CheckCollisions()
    {
        if (currentCollidedObject != null && currentCollidedObject != lastCollidedObject)
        {
            if (currentCollidedObject.GetComponent<Collider>() is BoxCollider && Vector3.Dot(characterController.velocity.normalized, Vector3.up) < -0.1f)
                Debug.Log(currentCollidedObject.name);
            else
                Debug.Log("Player is touching: " + currentCollidedObject.name);
            lastCollidedObject = currentCollidedObject;
        }
        else if (characterController.isGrounded && lastCollidedObject != null)
        {
            lastCollidedObject = null;
            //Debug.Log("Player is grounded.");
        }
        else if (!characterController.isGrounded && lastCollidedObject != null)
        {
            lastCollidedObject = null;
            //Debug.Log("Player is in the air.");
        }
    }
    private IEnumerator DelayWallJump()
    {
        canWallJump = false;
        delayReturn = false;
        yield return new WaitForSeconds(.2f);
        delayReturn = true;
        canWallJump = true;
    }

}