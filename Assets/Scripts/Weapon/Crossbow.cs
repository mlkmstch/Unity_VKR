using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Crossbow : MonoBehaviour
{
    public static Crossbow Instance { get; private set; }
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    public Weapons weaponData;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        //Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Attack()
    {
        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Crossbow.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>();
    }

}
