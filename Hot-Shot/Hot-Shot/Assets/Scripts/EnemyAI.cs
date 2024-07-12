using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent enemyAgent;
    [SerializeField] Renderer model;
    [SerializeField] Color colorDamage;
    [SerializeField] Transform ShootPos;


    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int viewAngle;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    Color colorigin;

    bool isShooting;
    bool playerInRange;

    float angToPlayer;

    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        colorigin = model.material.color;
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && canSeePlayer())
        {

        }
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;
        angToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(transform.position, playerDir);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angToPlayer <= viewAngle)
            {
                enemyAgent.SetDestination(gameManager.instance.player.transform.position);
                if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance)
                {
                    faceTarget();
                }
                if (!isShooting)
                {
                    StartCoroutine(Shoot());
                }
                return true;
            }
        }
        return false;
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        enemyAgent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    IEnumerator flashDamage()
    {
        model.material.color = colorDamage;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorigin;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, ShootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
