using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    // Reference to the NavMeshAgent component
    [SerializeField] NavMeshAgent enemyAgent;

    // Reference to Renderer component
    [SerializeField] Renderer model;

    // Color to flash when damaged
    [SerializeField] Color colorDamage;

    // Position to instantiate bullets from
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

    //Tank enemy burst shooting
    [SerializeField] int burstCount;

    //Tank enemy burst delay
    [SerializeField] float burstDelay;

    //Melee damage
    [SerializeField] int meleeDmg;

    //Attack rate (melee)
    [SerializeField] float atkRate;

    //Bool for basic ranged or melee
    [SerializeField] bool isMelee;

    //Bool for tank
    [SerializeField] bool isTank;

    // Enemies original color
    Color colorigin;

    // Is the enemy attacking
    bool isAttacking;

    // Is the player within in range of the enemy
    bool playerInRange;

    // Angle to player
    float angToPlayer;

    // Direction vector to the player
    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        // Stores the original color of the enemy
        colorigin = model.material.color;

        // Update the game goal when the enemy spawns
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {
            // Sets the enemy's destination to the player's position
            enemyAgent.SetDestination(gameManager.instance.player.transform.position);

                faceTarget();
            

            // If the enemy is not shooting, start shooting
            if (!isAttacking)
            {
                if (!isMelee)
                {
                    StartCoroutine(shoot());
                }
                else
                {
                    StartCoroutine(meleeAttack());
                }
            }
        }
    }

    // Check if the enemy can see the player
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;
        angToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            // If the raycast hits the player and the player is within view angle
            if (hit.collider.CompareTag("Player") && angToPlayer <= viewAngle)
            {
                return true;
            }
        }
        return false;
    }

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
        }
        else
        {
            // Move towards the player's location when hit
            enemyAgent.SetDestination(gameManager.instance.player.transform.position);
            faceTarget();
        }
    }

    // Trigger detection for player entering range
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                // Player is in range
                playerInRange = true;
        }
    }

    // Trigger detection for player leaving range
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            // Player is out of range
            playerInRange = false;
    }

    // Enemy faces the player
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    // Flash damage effect
    IEnumerator flashDamage()
    {
        model.material.color = colorDamage;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorigin;
    }

    // Handles shooting
    IEnumerator shoot()
    {
        isAttacking = true;
        //Check if tank enemy
        if (isTank)
        {

            for (int i = 0; i < burstCount; i++)
            {
                Instantiate(bullet, ShootPos.position, transform.rotation);
                yield return new WaitForSeconds(shootRate);
            }
            yield return new WaitForSeconds(burstDelay);
        }
        else
        {
            Instantiate(bullet, ShootPos.position, transform.rotation);
            yield return new WaitForSeconds(shootRate);
        }
        isAttacking = false;
    }

    // Handles melee
    IEnumerator meleeAttack()
    {
        isAttacking = true;
        gameManager.instance.player.GetComponent<PlayerHealth>().takeDamage(meleeDmg);
        yield return new WaitForSeconds(atkRate);
        isAttacking = false;
    }
}
