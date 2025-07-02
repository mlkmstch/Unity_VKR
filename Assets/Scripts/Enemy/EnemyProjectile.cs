using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private int damage;
    public GameObject particleOnHitPrefabVFX;
    GameObject vfxInstance;
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerTrigger"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                vfxInstance = Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                playerHealth.TakeDamage(damage, transform);
                Destroy(gameObject);
                Invoke("DestroyObjectWithParam", 3);
            }

        }
    }

    void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
    void DestroyObjectWithParam()
    {
        DestroyObject(vfxInstance);
    }
}