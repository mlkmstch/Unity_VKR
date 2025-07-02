using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.WSA;


public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance { get; private set; }

    [SerializeField] List<Weapons> weaponImages = new List<Weapons>();
    [SerializeField] private Vector3 rightOffset = new Vector3(0.58f, 4.5f, 0f);
    [SerializeField] private Vector3 leftOffset = new Vector3(-0.58f, 4.5f, 0f);
    [SerializeField] private float baseScale = 0.36f;

    public PlayerInputActions playerControls;
    public event EventHandler OnPlayerAttack;
    public string activeWeapon;
    public Image weaponImage;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
        playerControls.Enable();
        playerControls.Player.Attack.started += PlayerAttack_started;
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
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        activeWeapon = "steel";
        playerControls.Player.Switchweapon.performed += _ => SwitchWeapon();
    }
    private void Update()
    {
        FollowMousePosition();
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    private void FollowMousePosition()
    {
        if (activeWeapon == "crossbow")
        {
            Vector3 rightOffset = new Vector3(1.9f, 3.3f, 0f);
            Vector3 leftOffset = new Vector3(-1.9f, 3.3f, 0f);
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.parent.position);

            bool isMouseLeft = mouseScreenPosition.x < playerScreenPosition.x;

            // Задаём смещение от игрока
            Crossbow.Instance.transform.localPosition = isMouseLeft ? leftOffset : rightOffset;

            // Меняем масштаб по Y для зеркального отображения
            Crossbow.Instance.transform.localScale = isMouseLeft
                ? new Vector3(baseScale, -baseScale, baseScale)
                : new Vector3(baseScale, baseScale, baseScale);

            // Поворачиваем в сторону курсора
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mouseWorldPosition - Crossbow.Instance.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Crossbow.Instance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            Vector3 swordLeftOffset = new Vector3(-4.5f, 3f, 0f);
            Vector3 swordRightOffset = new Vector3(4.5f, 3f, 0f);
            Vector3 mouseScreenPos = Player.Instance.GetMousePosition();
            Vector3 playerScreenPos = Player.Instance.GetPlayerScreenPosition();
            Vector3 playerWorldPos = Player.Instance.transform.position;

            if (mouseScreenPos.x < playerScreenPos.x)
            {
                // Влево
                Sword.Instance.transform.position = playerWorldPos + swordLeftOffset;
                Sword.Instance.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                // Вправо
                Sword.Instance.transform.position = playerWorldPos + swordRightOffset;
                Sword.Instance.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
    public void SwitchWeapon()
    {
        switch (activeWeapon)
        {
            case "steel":
                weaponImage.sprite = weaponImages[6].weaponSprite;
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                activeWeapon = "crossbow";
                break;
            case "golden":
                weaponImage.sprite = weaponImages[6].weaponSprite;
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                activeWeapon = "crossbow";
                break;
            case "acid":
                weaponImage.sprite = weaponImages[6].weaponSprite;
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                activeWeapon = "crossbow";
                break;
            case "jade":
                weaponImage.sprite = weaponImages[6].weaponSprite;
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                activeWeapon = "crossbow";
                break;
            case "dual":
                weaponImage.sprite = weaponImages[6].weaponSprite;
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                activeWeapon = "crossbow";
                break;
            case "diamond":
                weaponImage.sprite = weaponImages[6].weaponSprite;
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                activeWeapon = "crossbow";
                break;
            case "crossbow":
                weaponImage.sprite = weaponImages[SwitchInteraction.weaponIndex].weaponSprite;
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(false);
                activeWeapon = weaponImages[SwitchInteraction.weaponIndex].weaponType; 
                break;
        }
    }

}
