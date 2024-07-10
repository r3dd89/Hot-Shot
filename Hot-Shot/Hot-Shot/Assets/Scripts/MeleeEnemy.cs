using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Script for a melee style enemy
public class MeleeEnemy : MonoBehaviour, IDamage
{
    // References the NavMeshAgent component for navigation
    [SerializeField] NavMeshAgent enemyAgent;

    // References the Renderer component for visual effects
    [SerializeField] Renderer model;

    // Color to flash when damaged
    [SerializeField] Color colorDamage;

    // Health points of the enemy
    [SerializeField] int HP;

    // The range the enemy will begin its assault
    [SerializeField] float attackRange;

    // The amount of damage the enemy will cause
    [SerializeField] int damageAmount;

    // Original color of the enemy
    Color colorOrigin;

    // Indicates if the player is within range
    bool targetInRange;

    // Direction vector towards the player
    Vector3 targetDirection;


    // Start is called before the first frame update
    void Start()
    {
        // Stores the original color of the model
        colorOrigin = model.material.color;

        // Updates the game goal
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount)
    {
        // Reduces HP by a certain amount
        HP -= amount;

        // Flashes a visual to indicate damage
        StartCoroutine(flashDamage());
        
    }


    // Handles damage taken by the enemy
    public void TakeDamage(int amount)
    {
        // Reduce HP by the specified amount
        HP -= amount;

        // Flash damage visual effect
        StartCoroutine(flashDamage());

        // Checks to see if the enemy is defeated
        if (HP <= 0)
        {
            // Update game goal
            gameManager.instance.updateGameGoal(-1);

            // Destroys the enemy GameObject
            Destroy(gameObject);
        }
    }

    // Coroutine for flashing damage effect
    IEnumerator flashDamage()
    {
        // Change the model's material color to indicate damage
        model.material.color = colorDamage;

        // Wait for a short period
        yield return new WaitForSeconds(0.1f);

        // Restore the original color of the model
        model.material.color = colorOrigin;
    }

    // Handles trigger collider
    void OnTriggerEnter(Collider other)
    {
        // Checks to see if the collider belongs to the player
        if(other.CompareTag("Player"))
        {
            // Player is in range
            targetInRange = true;
        }
    }

    // Handles trigger collider exit
    void OnTriggerExit(Collider other)
    {
        // Checks to see if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            // Player is out of range
            targetInRange = false;
        }
    }    

    // Rotates towards player
    void FaceTarget()
    {
        // Calculates rotation facing the player
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // The enemy will smoothly rotate towards the player
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
    }

    // Enemy will attack the player
    //void Attack()
    //{
    //    gameManager.instance.player.takeDamage(damageAmount);
    //}
}
