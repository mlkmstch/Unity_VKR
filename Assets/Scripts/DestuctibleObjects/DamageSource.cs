using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public static int damageDestructibleObjects = 1;
    public GameObject particleOnHitPrefabVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var destructible = other.GetComponent<Destructible>();
        if (destructible != null)
        {
            destructible.TakeDamage(damageDestructibleObjects);
            if (gameObject.tag == "Projectile")
            {
                Invoke("DestroyAnimation", 0.02f);
                
            }
        }
    }

    private void DestroyAnimation()
    {
        Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
