using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
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

    // Start is called before the first frame update
    void Start()
    {
        colorigin = model.material.color; // Save the original color of the enemy
        gameManager.instance.updateGameGoal(1); // Notify the game manager that a new enemy has spawned
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is in range and the enemy can see the player, engage the player
        if (playerInRange && canSeePlayer())
        {
            // Engage the player (e.g., shoot at the player)
        }
    }

    // Checks if the enemy can see the player
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position; // Direction to the player
        angToPlayer = Vector3.Angle(playerDir, transform.forward); // Angle to the player
        Debug.DrawRay(transform.position, playerDir); // Draw a debug ray to the player

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
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

                return true;
            }
        }

        return false;
    }

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
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir); // Calculate the rotation to face the player
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed); // Rotate towards the player
    }

    // Coroutine to flash the damage color
    IEnumerator flashDamage()
    {
        model.material.color = colorDamage; // Set the damage color
        yield return new WaitForSeconds(0.1f); // Wait for a short duration
        model.material.color = colorigin; // Reset to the original color
    }

    // Coroutine to shoot bullets
    IEnumerator shoot()
    {
        isShooting = true; // Set shooting flag to true
        Instantiate(bullet, ShootPos.position, transform.rotation); // Instantiate the bullet
        yield return new WaitForSeconds(shootRate); // Wait for the shoot rate duration
        isShooting = false; // Set shooting flag to false
    }
}