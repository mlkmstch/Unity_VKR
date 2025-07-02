using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance { get; private set; }

    public GameObject upgradeMenu;
    public bool OpenUpgradeWindow = false;
    public int damageIncrease = 0;
    public int damageCrossbowIncrease = 0;
    public int acidNumberIncrease = 0;
    public int dashNumberIncrease = 0;
    public int hpNumberIncrease = 0;

    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;
    public Slider slider5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (OpenUpgradeWindow)
            {
                QuitWindow();
            }
            else
            {
                OpenWindow();
            }
        }
    }
    
    public void QuitWindow()
    {
        upgradeMenu.SetActive(false);
        //Time.timeScale = 1f;
        OpenUpgradeWindow = false;
    }

    public void OpenWindow()
    {
        //Time.timeScale = 0f;
        upgradeMenu.SetActive(true);
        OpenUpgradeWindow = true;
    }

    public void SwordUpgrade()
    {
        if (Pickup.coinCount != 0 && damageIncrease != 20)
        {
            Pickup.coinCount--;
            damageIncrease++;
            Sword.Instance.damageAmount++;
            DamageSource.damageDestructibleObjects++;
            UpdateUpgradeSlider("SwordsUpgradeBar", damageIncrease, 20);
        }
    }

    public void CrossbowUpgrade()
    {
        if (Pickup.coinCount != 0 && damageCrossbowIncrease != 20)
        {
            Pickup.coinCount--;
            damageCrossbowIncrease++;
            Projectile.crossbowDamage++;
            DamageSource.damageDestructibleObjects++;
            UpdateUpgradeSlider("CrossbowUpgradeBar", damageCrossbowIncrease, 20);
        }

    }

    public void AcidUpgrade()
    {
        if (Pickup.coinCount != 0 && acidNumberIncrease != 10)
        {
            Pickup.coinCount--;
            acidNumberIncrease++;
            Sword.Instance.acidAmount = Sword.Instance.acidAmount + 5;
            UpdateUpgradeSlider("AcidUpgradeBar", acidNumberIncrease, 10);
        }
    }

    public void DashUpgrade()
    {
        if (Pickup.coinCount != 0 && dashNumberIncrease != 9)
        {
            Pickup.coinCount--;
            dashNumberIncrease++;
            Player.Instance.dashRegargeTime--;
            UpdateUpgradeSlider("DashUpgradeBar", dashNumberIncrease, 9);
        }
    }

    public void HPUpgrade()
    {
        if (Pickup.coinCount != 0 && hpNumberIncrease != 10)
        {
            Pickup.coinCount--;
            hpNumberIncrease++;
            PlayerHealth.Instance.maxHealth = PlayerHealth.Instance.maxHealth + 10;
            UpdateUpgradeSlider("HPUpgradeBar", hpNumberIncrease, 10);
        }
    }

    public void UpdateUpgradeSlider(string sliderName, int increaseValue, int maxValue)
    {
        Slider upgradeSlider;
        upgradeSlider = GameObject.Find(sliderName).GetComponent<Slider>();
        upgradeSlider.maxValue = 10;
        upgradeSlider.value = increaseValue;
    }

    public void UpdateAllSliders(int increaseValue1, int increaseValue2, int increaseValue3, int increaseValue4, int increaseValue5)
    {
        slider1.value = increaseValue1;
        slider2.value = increaseValue2;
        slider3.value = increaseValue3;
        slider4.value = increaseValue4;
        slider5.value = increaseValue5;
    }
}
