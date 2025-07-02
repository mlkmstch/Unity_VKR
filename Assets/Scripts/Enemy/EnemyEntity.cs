using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyEntity : MonoBehaviour
{
    public Enemies enemyData;
    public event EventHandler OnTakeHit;
    public event EventHandler OnDeath;
    
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    public PolygonCollider2D _polygonCollider2D;
    public BoxCollider2D _boxCollider2D;
    public EnemyAI _enemyAI;


    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth; 
        PickUpSpawner pickUpSpawner = GetComponent<PickUpSpawner>();
        if (pickUpSpawner != null)
        {
            pickUpSpawner.enemyData = enemyData;
        }
    }

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        hasTriggered = true;

        if (collision.CompareTag("Sword"))
        {
            TakeDamage(Sword.Instance.damageAmount);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        hasTriggered = false;
    }

    public void TakeDamage(int damage)
    {
        if (enemyData.enemyType == "acid")
        {
            if (ActiveWeapon.Instance.activeWeapon == "diamond")
            {
                _currentHealth -= damage;
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "golden")
            {
                _currentHealth -= (damage + damage);
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "dual" || ActiveWeapon.Instance.activeWeapon == "jade")
            {
                _currentHealth -= (damage - (damage /2));
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
        }
        else if (enemyData.enemyType == "diamond")
        {
            if (ActiveWeapon.Instance.activeWeapon == "diamond")
            {
                _currentHealth -= Sword.Instance.damageAmount;
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "jade")
            {
                _currentHealth -= (Sword.Instance.damageAmount + Sword.Instance.damageAmount);
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
        }
        else if (enemyData.enemyType == "golden")
        {
            if (ActiveWeapon.Instance.activeWeapon == "golden")
            {
                _currentHealth -= Sword.Instance.damageAmount;
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "jade" || ActiveWeapon.Instance.activeWeapon == "diamond"
                || ActiveWeapon.Instance.activeWeapon == "steel" || ActiveWeapon.Instance.activeWeapon == "crossbow" 
                || ActiveWeapon.Instance.activeWeapon == "dual")
            {
                _currentHealth -= (Sword.Instance.damageAmount + Sword.Instance.damageAmount);
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
        }
        else if (enemyData.enemyType == "jade")
        {
            if (ActiveWeapon.Instance.activeWeapon == "jade")
            {
                _currentHealth -= Sword.Instance.damageAmount;
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "acid" || ActiveWeapon.Instance.activeWeapon == "diamond"
                || ActiveWeapon.Instance.activeWeapon == "dual")
            {
                _currentHealth -= (Sword.Instance.damageAmount + Sword.Instance.damageAmount);
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "steel" || ActiveWeapon.Instance.activeWeapon == "crossbow"
                || ActiveWeapon.Instance.activeWeapon == "golden")
            {
                _currentHealth -= (Sword.Instance.damageAmount - (Sword.Instance.damageAmount / 2));
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
        }
        else if (enemyData.enemyType == "ruby" || enemyData.enemyType == "sapphire")
        {
            if (ActiveWeapon.Instance.activeWeapon == "dual")
            {
                _currentHealth -= Sword.Instance.damageAmount;
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "acid" || ActiveWeapon.Instance.activeWeapon == "diamond")
            {
                _currentHealth -= (Sword.Instance.damageAmount + Sword.Instance.damageAmount);
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            else if (ActiveWeapon.Instance.activeWeapon == "steel" || ActiveWeapon.Instance.activeWeapon == "crossbow"
                || ActiveWeapon.Instance.activeWeapon == "golden" || ActiveWeapon.Instance.activeWeapon == "jade")
            {
                _currentHealth -= (Sword.Instance.damageAmount - (Sword.Instance.damageAmount / 2));
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
        }
        else if (enemyData.enemyType == "steel")
        {
            if (ActiveWeapon.Instance.activeWeapon == "diamond")
            {
                _currentHealth -= (Sword.Instance.damageAmount + Sword.Instance.damageAmount);
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
            /*else if (ActiveWeapon.Instance.activeWeapon == "steel" || ActiveWeapon.Instance.activeWeapon == "crossbow")
            {
                _currentHealth -= (Sword.Instance.damageAmount - (Sword.Instance.damageAmount / 2));
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }*/
            else
            {
                _currentHealth -= Sword.Instance.damageAmount;
                OnTakeHit?.Invoke(this, EventArgs.Empty);
                DetectDeath();
            }
        }
    }

    private void DetectDeath()
    {
        PickUpSpawner pickUpSpawner = GetComponent<PickUpSpawner>();
        if (_currentHealth <= 0)
        {
            pickUpSpawner.DeathDrop();
            if (enemyData.itemDrop)
            {
                pickUpSpawner.DropItem();
            }
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            _enemyAI.SetDeathState();
            OnDeath?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject, 1f);

        }
    }
}

