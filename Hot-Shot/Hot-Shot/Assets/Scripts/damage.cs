using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    [SerializeField] enum damageType { bullet, stationary, stab }
    [SerializeField] damageType type;

    [SerializeField] Rigidbody rb;
    [SerializeField] int damageAmount;
    [SerializeField] int bulletSpeed;
    [SerializeField] int destroyTime;

    bool hasDamaged;

    // Start is called before the first frame update
    void Start()
    {
        if (type == damageType.bullet)
        {
            rb.velocity = transform.forward * bulletSpeed;
            Destroy(gameObject, destroyTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null && !hasDamaged)
            dmg.takeDamage(damageAmount);

        if (type == damageType.bullet)
        {
            Destroy(gameObject);
            hasDamaged = true;
        }
    }

}
