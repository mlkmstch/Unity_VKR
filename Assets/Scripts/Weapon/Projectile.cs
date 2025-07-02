using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    public static int crossbowDamage = 10;
    private Vector3 startPosition;
    GameObject vfxInstance;
    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        hasTriggered = true;
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            vfxInstance = Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
            Invoke("DeleteVFX", 3);
            enemyEntity.TakeDamage(crossbowDamage);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        hasTriggered = false;
    }
    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > 20)
        {
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
            Destroy(gameObject);
            DestroyObjectWithParam();
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
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
