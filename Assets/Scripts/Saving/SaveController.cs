using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance { get; private set; }
    public bool IsReady { get; private set; }
    private string saveLocation;
    private InventoryController inventoryController;
    private DigController[] spots;
    private SaveData loadedData;

    public List<string> destroyedWorldItemIDs;

    // Константы дефолтных значений
    private const int DefaultScore = 0;
    private const string DefaultWeapon = "steel";
    private const int DefaultDamage = 10;
    private const int DefaultCrossbowDamage = 10;
    private const int DefaultAcid = 5;
    private const int DefaultDashRecharge = 10;
    private const int DefaultHealth = 500;

    private const int DefaultDamageIncrease = 0;
    private const int DefaultDamageCrossbowIncrease = 0;
    private const int DefaultAcidNumberIncrease = 0;
    private const int DefaultDashNumberIncrease = 0;
    private const int DefaultHpNumberIncrease = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(this.gameObject);
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        IsReady = false;
        StartCoroutine(InitializeAndLoadGame());
    }

    private IEnumerator InitializeAndLoadGame()
    {
        yield return null;

        if (File.Exists(saveLocation))
        {
            loadedData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

        }

        InitializeComponents();
        LoadGameData();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Обнуляем старые ссылки, чтобы они не указывали на уничтоженные объекты
        inventoryController = null;
        spots = null;

        StartCoroutine(DelayedLoadAfterScene());
    }

    private IEnumerator DelayedLoadAfterScene()
    {
        yield return null;
        yield return null;

        // Ждём до появления всех нужных объектов
        yield return new WaitUntil(() =>
            FindObjectOfType<InventoryController>() != null &&
            Player.Instance != null &&
            Sword.Instance != null &&
            ActiveWeapon.Instance != null &&
            PlayerHealth.Instance != null &&
            UpgradeSystem.Instance != null
        );

        InitializeComponents();
        LoadGameData();
    }

    private void InitializeComponents()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        spots = FindObjectsOfType<DigController>();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventoryController.GetInventoryItems(),
            spotSaveData = GetSpotsState(),
            questProgressData = QuestController.Instance.activateQuests,
            handinQuestIDs = QuestController.Instance.handinQuestIDs,

            // Состояние апгрейд меню
            damageIncrease = UpgradeSystem.Instance.damageIncrease,
            damageCrossbowIncrease = UpgradeSystem.Instance.damageCrossbowIncrease,
            acidNumberIncrease = UpgradeSystem.Instance.acidNumberIncrease,
            dashNumberIncrease = UpgradeSystem.Instance.dashNumberIncrease,
            hpNumberIncrease = UpgradeSystem.Instance.hpNumberIncrease,

            // Характеристики игрока
            scoreNumber = Pickup.coinCount,
            activeWeaponName = ActiveWeapon.Instance.activeWeapon,
            damageAmount = Sword.Instance.damageAmount,
            crossbowDamage = Projectile.crossbowDamage,
            acidAmount = Sword.Instance.acidAmount,
            dashRegargeTime = Player.Instance.dashRegargeTime,
            playerHealth = PlayerHealth.Instance.maxHealth
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private List<SpotSaveData> GetSpotsState()
    {
        List<SpotSaveData> spotStates = new List<SpotSaveData>();

        foreach (DigController spot in spots)
        {
            SpotSaveData spotSaveData = new SpotSaveData
            {
                spotID = spot.SpotID,
                isOpened = spot.IsOpened
            };
            spotStates.Add(spotSaveData);
        }

        return spotStates;
    }

    private void LoadGameData()
    {
        if (loadedData == null && File.Exists(saveLocation))
        {
            loadedData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));
        }

        if (loadedData != null)
        {
            // Характеристики игрока
            Pickup.coinCount = loadedData.scoreNumber;
            ActiveWeapon.Instance.activeWeapon = loadedData.activeWeaponName;
            Sword.Instance.damageAmount = loadedData.damageAmount;
            Projectile.crossbowDamage = loadedData.crossbowDamage;
            Sword.Instance.acidAmount = loadedData.acidAmount;
            Player.Instance.dashRegargeTime = loadedData.dashRegargeTime;
            PlayerHealth.Instance.maxHealth = loadedData.playerHealth;
            PlayerHealth.Instance.currentHealth = loadedData.playerHealth;

            // Состояние апгрейд меню
            UpgradeSystem.Instance.damageIncrease = loadedData.damageIncrease;
            UpgradeSystem.Instance.damageCrossbowIncrease = loadedData.damageCrossbowIncrease;
            UpgradeSystem.Instance.acidNumberIncrease = loadedData.acidNumberIncrease;
            UpgradeSystem.Instance.dashNumberIncrease = loadedData.dashNumberIncrease;
            UpgradeSystem.Instance.hpNumberIncrease = loadedData.hpNumberIncrease;
            UpgradeSystem.Instance.UpdateAllSliders(loadedData.damageIncrease, loadedData.damageCrossbowIncrease,
                loadedData.acidNumberIncrease, loadedData.dashNumberIncrease, loadedData.hpNumberIncrease);

            // Позиция игрока
            GameObject.FindGameObjectWithTag("Player").transform.position = loadedData.playerPosition;

            // Остальные системы
            LoadSpotStates(loadedData.spotSaveData);
            QuestController.Instance.LoadQuestProgress(loadedData.questProgressData);
            QuestController.Instance.handinQuestIDs = loadedData.handinQuestIDs;
        }
        else
        {
            // Первый запуск — дефолтные значения
            Pickup.coinCount = DefaultScore;
            ActiveWeapon.Instance.activeWeapon = DefaultWeapon;
            Sword.Instance.damageAmount = DefaultDamage;
            Projectile.crossbowDamage = DefaultCrossbowDamage;
            Sword.Instance.acidAmount = DefaultAcid;
            Player.Instance.dashRegargeTime = DefaultDashRecharge;
            PlayerHealth.Instance.maxHealth = DefaultHealth;
            PlayerHealth.Instance.currentHealth = DefaultHealth;
            UpgradeSystem.Instance.damageIncrease = DefaultDamageIncrease;
            UpgradeSystem.Instance.damageCrossbowIncrease = DefaultDamageCrossbowIncrease;
            UpgradeSystem.Instance.acidNumberIncrease = DefaultAcidNumberIncrease;
            UpgradeSystem.Instance.dashNumberIncrease = DefaultDashNumberIncrease;
            UpgradeSystem.Instance.hpNumberIncrease = DefaultHpNumberIncrease;

            SaveGame(); // Создаём начальное сохранение
        }

        // ЕДИНСТВЕННОЕ место, где вызываем SetInventoryItems — с защитой!
        if (inventoryController != null)
        {
            inventoryController.SetInventoryItems(
                loadedData != null ? loadedData.inventorySaveData : new List<InventorySaveData>()
            );
        }
        else
        {
            Debug.LogWarning("InventoryController не найден при загрузке.");
        }
        IsReady = true;
    }


    private void LoadSpotStates(List<SpotSaveData> spotStates)
    {
        foreach (DigController spot in spots)
        {
            SpotSaveData spotSaveData = spotStates.FirstOrDefault(c => c.spotID == spot.SpotID);

            if (spotSaveData != null)
            {
                spot.DugUp(spotSaveData.isOpened);
            }
        }
    }
}
