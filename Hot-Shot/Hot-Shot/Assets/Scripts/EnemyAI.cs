using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Parts of the Enemy -----")]
    // Reference to the NavMeshAgent component
    [SerializeField] NavMeshAgent enemyAgent;

    // Reference to Renderer component
    [SerializeField] Renderer model;

    // Color to flash when damaged
    [SerializeField] Color colorDamage;

    // Position to instantiate bullets from
    [SerializeField] Transform ShootPos;

    // Sets the head position of the enemy
    [SerializeField] Transform headPos;

    [Header("----- Enemy Stats -----")]
    
    // Enemy health 
    [SerializeField] int HP;

    // Speed the enemy turns to face the player
    [SerializeField] int faceTargetSpeed;

    // Field of view angle for detecting player
    [SerializeField] int viewAngle;

    // Roaming distance
    [SerializeField] int roamDist;

    // Time to roam
    [SerializeField] int roamTimer;

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

    // Bullet prefab
    [SerializeField] GameObject bullet;

    [Header("----- Type of Enemy -----")]
    //Bool for basic ranged or melee
    [SerializeField] bool isMelee;

    //Bool for tank
    [SerializeField] bool isTank;

    [Header("----- Teleportation Stats -----")]
    // For teleportation
    [SerializeField] Vector3 teleportArea;
    [SerializeField] Vector3 teleportAreaSize;
    [SerializeField] float teleportCooldown = 10f;

    // Enemies original color
    Color colorigin;

    // Is the enemy attacking
    bool isAttacking;

    // Is the player within in range of the enemy
    bool playerInRange;

    // Is the enemy shooting
    bool isShooting;

    // Is the enemy roaming
    bool isRoaming;

    // Angle to player
    float angToPlayer;

    // Stopping distance
    float stoppingDistOrigin;

    // Direction vector to the player
    Vector3 playerDir;

    // Getting the start position
    Vector3 startPos;

    // Tracks time for teleportation cool down
    float nextTeleport;
    // Start is called before the first frame update
    void Start()
    {
        // Stores the original color of the enemy
        colorigin = model.material.color;

        // Update the game goal when the enemy spawns
        gameManager.instance.updateGameGoal(1);

        // Stores the original starting Position
        startPos = transform.position;

        // Stores the original stopping distance
        stoppingDistOrigin = enemyAgent.stoppingDistance;

        // Initializes the teleportation time
        //https://docs.unity3d.com/ScriptReference/Time-time.html
        nextTeleport = Time.time + teleportCooldown;
    }

    // Update is called once per frame
    void Update()
    {

        float agentSpeed = enemyAgent.velocity.normalized.magnitude;

        if (playerInRange && !canSeePlayer())
        {
            // Checking if the player is in range
            if (!isRoaming && enemyAgent.remainingDistance < 0.05f)
                StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            // Checking if the player is not in range
            if (!isRoaming && enemyAgent.remainingDistance < 0.05f)
                StartCoroutine(roam());
        }

        // Check if it's time to teleport
        if (Time.time >= nextTeleport)
        {
            nextTeleport = Time.time + teleportCooldown;
        }
    }

    IEnumerator roam()
    {

        isRoaming = true;
        yield return new WaitForSeconds(roamTimer);
        enemyAgent.stoppingDistance = 0;
        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        enemyAgent.SetDestination(hit.position);

        isRoaming = false;
    }

    // Check if the enemy can see the player
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.Log(angToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            // If the raycast hits the player and the player is within view angle
            if (hit.collider.CompareTag("Player") && angToPlayer <= viewAngle)
            {
                enemyAgent.SetDestination(gameManager.instance.player.transform.position);
                if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
                    faceTarget();

                if (!isShooting)
                    StartCoroutine(shoot());
                enemyAgent.stoppingDistance = stoppingDistOrigin;
                return true;
            }
        }
        enemyAgent.stoppingDistance = 0;
        return false;
    }

    // Take damage method
    public void takeDamage(int amount)
    {
        // Subtracts from HP
        Debug.Log("Enemy took damage: " + amount);
        HP -= amount;
        
        // Setting destination for roaming
        enemyAgent.SetDestination(gameManager.instance.player.transform.position);

        // Flashing damage
        StartCoroutine(flashDamage());
        
        // Checking if the enemy has no HP
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    // Trigger detection for player entering range
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    // Trigger detection for player leaving range
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is out of range
            enemyAgent.stoppingDistance = 0;
            playerInRange = false;
        }
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

    void Teleport()
    {
        float randomX = Random.Range(teleportArea.x - teleportAreaSize.x / 2, teleportArea.x + teleportAreaSize.x / 2);
        float randomY = Random.Range(teleportArea.y - teleportAreaSize.y / 2, teleportArea.y + teleportAreaSize.y / 2);
        float randomZ = Random.Range(teleportArea.z - teleportAreaSize.z / 2, teleportArea.z + teleportAreaSize.z / 2);

        Vector3 position = new Vector3(randomX, randomY, randomZ);

        NavMeshHit hit;
        if(NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }
}
