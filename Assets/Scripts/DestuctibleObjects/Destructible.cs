using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    private int startingHealth = 10;
    private int currentHealth;
    GameObject vfxInstance;

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        vfxInstance = Instantiate(destroyVFX, transform.position, Quaternion.identity);
        Invoke("DestroyObjectWithParam", 5);
        currentHealth -= damage;
        DetectedDeath();
    }

    private void DetectedDeath()
    {
        if (currentHealth <= 0)
        {
            GetComponent<PickUpSpawner>().DeathDrop();
            Destroy(gameObject);
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
