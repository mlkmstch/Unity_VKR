using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public static Sword Instance { get; private set; }
    public Weapons weaponData;
    [SerializeField] public int damageAmount = 10;
    public int acidAmount = 5; 
    public int acidCount = 0;

    // Флаг, который запрещает новую атаку, пока не закончится анимация
    private bool isAttacking = false;

    public event EventHandler SwordSwing;

    private PolygonCollider2D polygonCollider2D;

    private void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();
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

        polygonCollider2D.enabled = false;
    }

    public void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        polygonCollider2D.enabled = true;
        SwordSwing?.Invoke(this, EventArgs.Empty);

    }

    public void EndAttack()
    {
        polygonCollider2D.enabled = false;
        isAttacking = false;
    }
}