using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float projectileLifetime = 5f;
    [SerializeField] private int damage = 1;

    [SerializeField] private float aimOffsetY = 5f; //  на сколько выше целиться

    public void FireAtPlayer()
    {
        if (Player.Instance == null) return;

        Vector3 targetPosition = Player.Instance.transform.position + new Vector3(0f, aimOffsetY, 0f);
        Vector3 direction = (targetPosition - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);

        EnemyProjectile ep = projectile.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            ep.SetDamage(damage);
        }

        Destroy(projectile, projectileLifetime);
    }
}