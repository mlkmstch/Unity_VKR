using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SwitchInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] List<Weapons> weaponInfo = new List<Weapons>();
    public bool IsOpened { get; private set; }
    public static string currentWeapon;
    public GameObject openingPanel;
    public GameObject[] weaponPrefab;
    public UnityEngine.UI.Image weaponImage;
    public UnityEngine.UI.Image weaponChooseImage;
    public TMP_Text textComponent;
    public GameObject nextButton;
    public GameObject prevButton;

    public static int weaponIndex = 0;

    void Start()
    {
        weaponImage.sprite = weaponInfo[weaponIndex].weaponSprite;
        prevButton.SetActive(false);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        TMP_Text gameText = GameObject.Find("GameText").GetComponent<TMP_Text>();
        gameText.text = "Здоровье восстановлено";
        Invoke("ClearGameText", 3);
        Time.timeScale = 0f; //Ограничить передвижение персонажа (пофиксить паузу)
        openingPanel.SetActive(true);
        PlayerHealth.Instance.currentHealth = PlayerHealth.Instance.maxHealth;
        PlayerHealth.Instance.UpdateHealthSlider();
    }
    private void ClearGameText()
    {
        TMP_Text gameText = GameObject.Find("GameText").GetComponent<TMP_Text>();
        gameText.text = "";
    }

    public void StopInteract()
    {
        //if (!CanInteract()) return;

        Time.timeScale = 1f; //Ограничить передвижение персонажа (пофиксить паузу)
        openingPanel.SetActive(false);
    }

    public void SwitchWeapon()
    {
        ActiveWeapon.Instance.activeWeapon = weaponInfo[weaponIndex].weaponType;
        Sword.Instance.weaponData = weaponInfo[weaponIndex];
        weaponChooseImage.sprite = weaponInfo[weaponIndex].weaponSprite;

    }

    public void NextButton()
    {
        prevButton.SetActive(true);
        if (weaponIndex < weaponInfo.Count-1)
        {
            weaponIndex++;
            textComponent.text = weaponInfo[weaponIndex].weaponName;
            weaponImage.sprite = weaponInfo[weaponIndex].weaponSprite;
            if (weaponIndex == weaponInfo.Count-1)
            {
                nextButton.SetActive(false);
            }
        }
    }

    public void PrevButton()
    {
        nextButton.SetActive(true);
        if (weaponIndex > 0)
        {
            weaponIndex--;
            textComponent.text = weaponInfo[weaponIndex].weaponName;
            weaponImage.sprite = weaponInfo[weaponIndex].weaponSprite;
            if (weaponIndex == 0)
            {
                prevButton.SetActive(false);
            }
        }
        
    }

}



