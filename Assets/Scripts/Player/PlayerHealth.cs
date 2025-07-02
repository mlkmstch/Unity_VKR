using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }
    public bool isDead {  get; private set; }
    public GameObject fadeTransition;
    [SerializeField] private Transform targetPosition;
    [SerializeField] public int maxHealth = 500;

    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;

    readonly int DEATH_HASH = Animator.StringToHash("Death");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        fadeTransition.SetActive(false);
        isDead = false;
        currentHealth = maxHealth;
        StopAllCoroutines();
        UpdateHealthSlider();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamage(enemy.enemyDamage, collision.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }
        currentHealth -= damageAmount;
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            fadeTransition.SetActive(true);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(DeathLoadSceneRoutine(Player.Instance));
        }
    }
    
    private IEnumerator DeathLoadSceneRoutine(Player player)
    {
        fadeTransition.SetActive(true);
        isDead = false;
        yield return UIFade.Instance.FadeToBlack();

        player.transform.position = targetPosition.position;

        if (Pickup.coinCount <= 50)
        {
            Pickup.coinCount = 0;
        }
        else
        {
            Pickup.coinCount = Pickup.coinCount - 50;
        }
        currentHealth = maxHealth;
        UpdateHealthSlider();

        yield return new WaitForSeconds(0.1f);

        yield return UIFade.Instance.FadeToClear();

        fadeTransition.SetActive(false);
    }
    public void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
