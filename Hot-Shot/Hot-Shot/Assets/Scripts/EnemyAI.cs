using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
<<<<<<< HEAD
<<<<<<< HEAD
    // Reference to the NavMeshAgent component
    [SerializeField] NavMeshAgent enemyAgent;

    // Refernce to Renderer component
    [SerializeField] Renderer model;

    // Color to flash when damaged
    [SerializeField] Color colorDamage;

    //Position to instantiate bullets from
    [SerializeField] Transform ShootPos;

    // Enemy health 
    [SerializeField] int HP;

    // Speed the enemy turns to face the player
    [SerializeField] int faceTargetSpeed;

    // Field of view angle for detecting player
    [SerializeField] int viewAngle;

    // Bullet prefab
    [SerializeField] GameObject bullet;

    // How the fast the enemy shoots
    [SerializeField] float shootRate;

    // Enemies original color
    Color colorigin;

    // Is the enemy shooting
    bool isShooting;

    // Is the player within in range of the enemy
    bool playerInRange;

    // Angle to player
    float angToPlayer;

    // Direction vector to the player
    Vector3 playerDir;
=======
    [SerializeField] NavMeshAgent enemyAgent; // Reference to the NavMeshAgent component
    [SerializeField] Renderer model; // Reference to the Renderer component
    [SerializeField] Color colorDamage; // Color to use when the enemy takes damage
    [SerializeField] Transform ShootPos; // Position from which the enemy shoots

    [SerializeField] int HP; // Health points of the enemy
    [SerializeField] int faceTargetSpeed; // Speed at which the enemy faces the target
    [SerializeField] int viewAngle; // Viewing angle of the enemy

    [SerializeField] GameObject bullet; // Bullet prefab
    [SerializeField] float shootRate; // Rate of shooting

    Color colorigin; // Original color of the enemy

    bool isShooting; // Whether the enemy is currently shooting
    bool playerInRange; // Whether the player is within range

    float angToPlayer; // Angle to the player

    Vector3 playerDir; // Direction to the player
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
=======
    [SerializeField] NavMeshAgent enemyAgent; // Reference to the NavMeshAgent component
    [SerializeField] Renderer model; // Reference to the Renderer component
    [SerializeField] Color colorDamage; // Color to use when the enemy takes damage
    [SerializeField] Transform ShootPos; // Position from which the enemy shoots

    [SerializeField] int HP; // Health points of the enemy
    [SerializeField] int faceTargetSpeed; // Speed at which the enemy faces the target
    [SerializeField] int viewAngle; // Viewing angle of the enemy

    [SerializeField] GameObject bullet; // Bullet prefab
    [SerializeField] float shootRate; // Rate of shooting

    Color colorigin; // Original color of the enemy

    bool isShooting; // Whether the enemy is currently shooting
    bool playerInRange; // Whether the player is within range

    float angToPlayer; // Angle to the player

    Vector3 playerDir; // Direction to the player
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        // Stores the original color of the enemy
        colorigin = model.material.color;

        // Update the game goal when the enemy spawns
        gameManager.instance.updateGameGoal(1);
=======
        colorigin = model.material.color; // Save the original color of the enemy
        gameManager.instance.updateGameGoal(1); // Notify the game manager that a new enemy has spawned
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
=======
        colorigin = model.material.color; // Save the original color of the enemy
        gameManager.instance.updateGameGoal(1); // Notify the game manager that a new enemy has spawned
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is in range and the enemy can see the player, engage the player
        if (playerInRange && canSeePlayer())
        {
<<<<<<< HEAD
<<<<<<< HEAD
            Debug.Log("Player in range and visible");
            // Sets the enemy's destination to the player's position
            enemyAgent.SetDestination(gameManager.instance.player.transform.position);

            faceTarget();

            // If the enemy is not shooting, start shooting
            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }

    // Check if the enemy can see the player
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;
        angToPlayer = Vector3.Angle(playerDir, transform.forward);
=======
            // Engage the player (e.g., shoot at the player)
        }
    }

    // Checks if the enemy can see the player
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position; // Direction to the player
        angToPlayer = Vector3.Angle(playerDir, transform.forward); // Angle to the player
        Debug.DrawRay(transform.position, playerDir); // Draw a debug ray to the player
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
=======
            // Engage the player (e.g., shoot at the player)
        }
    }

    // Checks if the enemy can see the player
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position; // Direction to the player
        angToPlayer = Vector3.Angle(playerDir, transform.forward); // Angle to the player
        Debug.DrawRay(transform.position, playerDir); // Draw a debug ray to the player
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
<<<<<<< HEAD
<<<<<<< HEAD
            // If the raycast hits the player and the player is within view angle
            if (hit.collider.CompareTag("Player") && angToPlayer <= viewAngle)
            {
=======
=======
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
                // If the raycast hits the player and the angle to the player is within the view angle
                if (hit.collider.CompareTag("Player") && angToPlayer <= viewAngle)
                {
                    enemyAgent.SetDestination(gameManager.instance.player.transform.position); // Set the destination to the player's position

                    if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
                    {
                        faceTarget(); // Face the target if within stopping distance
                    }

                    if (!isShooting)
                    {
                        StartCoroutine(shoot()); // Start shooting if not already shooting
                    }

<<<<<<< HEAD
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
    =======
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
                return true;
                }
            }

            return false;
        }

<<<<<<< HEAD
<<<<<<< HEAD
        // Take damage method
        public void takeDamage(int amount)
        {
            // Reduces HP by the damage amount
            HP -= amount;

            // Flash Damage effect
            StartCoroutine(flashDamage());

            if (HP <= 0)
            {
                // Update game goal on enemy
                gameManager.instance.updateGameGoal(-1);

                // Destroy enemy
                Destroy(gameObject);
=======
    // Takes damage and updates the enemy's state
    public void takeDamage(int amount)
    {
        HP -= amount; // Reduce health points
        enemyAgent.SetDestination(gameManager.instance.player.transform.position); // Set the destination to the player's position
        StartCoroutine(flashDamage()); // Flash the damage color
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1); // Notify the game manager that an enemy has died
            Destroy(gameObject); // Destroy the enemy
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
            }
            else
            {
                // Move towards the player's location when hit
                enemyAgent.SetDestination(gameManager.instance.player.transform.position);
                faceTarget();
            }
        }

<<<<<<< HEAD
        // Trigger detection for player entering range
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                // Player is in range
                playerInRange = true;

        }

        // Trigger detection for player leaving range
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                // Player is out of range
                playerInRange = false;
        }

        // Enemy faces the player
=======
    // Called when another collider enters the trigger collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true; // Set the player in range flag to true
    }

    // Called when another collider exits the trigger collider
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false; // Set the player in range flag to false
    }

    // Faces the target (player)
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
=======
    // Takes damage and updates the enemy's state
    public void takeDamage(int amount)
    {
        HP -= amount; // Reduce health points
        enemyAgent.SetDestination(gameManager.instance.player.transform.position); // Set the destination to the player's position
        StartCoroutine(flashDamage()); // Flash the damage color
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1); // Notify the game manager that an enemy has died
            Destroy(gameObject); // Destroy the enemy
        }
    }

    // Called when another collider enters the trigger collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true; // Set the player in range flag to true
    }

    // Called when another collider exits the trigger collider
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false; // Set the player in range flag to false
    }

    // Faces the target (player)
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
        void faceTarget()
        {
            Quaternion rot = Quaternion.LookRotation(playerDir); // Calculate the rotation to face the player
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed); // Rotate towards the player
        }

<<<<<<< HEAD

        // Flash damage effect
=======
    // Coroutine to flash the damage color
<<<<<<< HEAD
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
=======
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
        IEnumerator flashDamage()
        {
            model.material.color = colorDamage; // Set the damage color
            yield return new WaitForSeconds(0.1f); // Wait for a short duration
            model.material.color = colorigin; // Reset to the original color
        }

<<<<<<< HEAD
<<<<<<< HEAD
        // Handles shooting
=======
    // Coroutine to shoot bullets
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
=======
    // Coroutine to shoot bullets
>>>>>>> 6bc20e692de2727e2a1dcf7e274e5795bf2008fb
        IEnumerator shoot()
        {
            isShooting = true; // Set shooting flag to true
            Instantiate(bullet, ShootPos.position, transform.rotation); // Instantiate the bullet
            yield return new WaitForSeconds(shootRate); // Wait for the shoot rate duration
            isShooting = false; // Set shooting flag to false
        }
    }