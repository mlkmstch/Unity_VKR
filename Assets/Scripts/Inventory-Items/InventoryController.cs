using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.Rendering.DebugUI;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }
    Dictionary<int, int> itemsCountCache = new();
    public event Action OnInventoryChanged;
    private ItemDictionary itemDictionary;

    [SerializeField] private Quest mainQuest;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefab;
    public Image gemImage;
    public GameObject gemImageObject;
    public static bool itemPlaced = false;
    Item currentItem;
    public GameObject transImageObject;

    public List<GameObject> vfxList = new List<GameObject>();
    public RectTransform VFXspawnPoint;
    GameObject gameobjectVFX;

    public TMP_InputField inputField;
    public Toggle toggle;
    public TMP_Dropdown dropdown1;
    public TMP_Dropdown dropdown2;

    public Animator animator;
    public GameObject cube;
    public GameObject gex;
    public GameObject mono;
    public GameObject trig;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

    }

    public void RebuildItemCounts()
    {
        itemsCountCache.Clear();

        foreach (Transform slotTransform  in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if(item != null)
                {
                    itemsCountCache[item.ID] = itemsCountCache.GetValueOrDefault(item.ID, 0) + item.quantity;
                }
            }
        }
        OnInventoryChanged?.Invoke();
    }
    
    public Dictionary<int, int> GetItemCounts() => itemsCountCache;

    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null) return false;

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if(slotItem != null && slotItem.ID == itemToAdd.ID)
                {

                    slotItem.AddToStack();
                    RebuildItemCounts();
                    return true;
                }
            }
        }

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform, false);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                newItem.transform.localScale = Vector3.one;
                slot.currentItem = newItem;
                RebuildItemCounts();
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData
                { 
                    itemID = item.ID,
                    slotIndex = slotTransform.GetSiblingIndex(),
                    quantity = item.quantity
                });
            }
        }
        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    Item itemComponent = item.GetComponent<Item>();
                    if(itemComponent != null && data.quantity > 1)
                    {
                        itemComponent.quantity = data.quantity;
                        itemComponent.UpdateQuantityDisplay();
                    }
                    slot.currentItem = item;
                }
            }
        }
        RebuildItemCounts();
    }

    public void RemoveItemsFromInventory(int itemID, int amountToRemove)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            if (amountToRemove <= 0) break;

            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot?.currentItem?.GetComponent<Item>() is Item item && item.ID == itemID)
            {
                int removed = Mathf.Min(amountToRemove, item.quantity);
                item.RemoveFromStack(removed);
                amountToRemove -= removed;
                if(item.quantity == 0)
                {
                    Destroy(slot.currentItem);
                    slot.currentItem = null;
                }
            }
        }
        RebuildItemCounts();
    }

    public void PlaceItem()
    {
        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null && itemPlaced == false)
            {
                itemPlaced = true;
                currentItem = slot.currentItem.GetComponent<Item>();
                gemImageObject.SetActive(true);
                gemImage.sprite = currentItem.gemData.gemSprite;
                Destroy(slot.currentItem);
                slot.currentItem = null;
                break;
            }
        }
    }
    void SetGoldReward(Quest quest, int newGoldAmount)
    {
        foreach (var reward in quest.questRewards)
        {
            if (reward.type == RewardType.Gold)
            {
                reward.amount = newGoldAmount;
                Debug.Log($"Новая награда золотом: {newGoldAmount}");
                return;
            }
        } 
    }

    public void SaveData()
    {
        if (string.IsNullOrWhiteSpace(inputField.text) && currentItem.gemData.analyzed == true)
        {
            return;
        }
        else
        {
            currentItem.gemData.analyzed = true;
            if(currentItem.gemData.hardness.ToString() == inputField.text && currentItem.gemData.syngony.ToString() == dropdown1.value.ToString()
                && currentItem.gemData.gloss.ToString() == dropdown2.value.ToString() && (currentItem.gemData.transparency == toggle.isOn))
            {
                currentItem.gemData.cost = 50;
                SetGoldReward(mainQuest, currentItem.gemData.cost);
            }


            gemImageObject.SetActive(false);
            gemImage.sprite = null;
            transImageObject.SetActive(false);
            itemPlaced = false;
        }
    }

    private void DisableCube() => cube.SetActive(false);
    private void DisableGex() => gex.SetActive(false);
    private void DisableMono() => mono.SetActive(false);
    private void DisableTrig() => trig.SetActive(false);

    public void Loupe()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);

            switch (currentItem.gemData.syngony)
            {
                case "cube":
                    cube.SetActive(true);
                    animator = cube.GetComponent<Animator>();
                    animator.SetTrigger("Play");
                    Invoke(nameof(DisableCube), 1f);
                    break;
                case "gex":
                    gex.SetActive(true);
                    animator = gex.GetComponent<Animator>();
                    animator.SetTrigger("Play");
                    Invoke(nameof(DisableGex), 1f);
                    break;
                case "mono":
                    mono.SetActive(true);
                    animator = mono.GetComponent<Animator>();
                    animator.SetTrigger("Play");
                    Invoke(nameof(DisableMono), 1f);
                    break;
                case "trig":
                    trig.SetActive(true);
                    animator = trig.GetComponent<Animator>();
                    animator.SetTrigger("Play");
                    Invoke(nameof(DisableTrig), 1f);
                    break;
            }
        }
    }
    private IEnumerator PlayAnimationWithDelay(GameObject obj, string triggerName, float delay)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(delay);
        Animator anim = obj.GetComponent<Animator>();
        anim.SetTrigger(triggerName);
    }

    [SerializeField] private Transform vfxParent;
    public void Flashlight()
    {
        if (itemPlaced)
        {
            switch (currentItem.gemData.gloss)
            {
                case "diamond":
                    gameobjectVFX = Instantiate(vfxList[0], VFXspawnPoint.position, Quaternion.identity, vfxParent);
                    Invoke("DestroyObjectWithParam", 5);
                    break;
                case "glass":
                    gameobjectVFX = Instantiate(vfxList[1], VFXspawnPoint.position, Quaternion.identity, vfxParent);
                    Invoke("DestroyObjectWithParam", 5);
                    break;
                case "metal":
                    gameobjectVFX = Instantiate(vfxList[2], VFXspawnPoint.position, Quaternion.identity, vfxParent); ;
                    Invoke("DestroyObjectWithParam", 5);
                    break;
                case "pearl":
                    gameobjectVFX = Instantiate(vfxList[3], VFXspawnPoint.position, Quaternion.identity, vfxParent);
                    Invoke("DestroyObjectWithParam", 5);
                    break;
                case "resinous":
                    gameobjectVFX = Instantiate(vfxList[4], VFXspawnPoint.position, Quaternion.identity, vfxParent);
                    Invoke("DestroyObjectWithParam", 5);
                    break;
            }
            if (currentItem.gemData.transparency)
            {
                transImageObject.SetActive(true);
            }
        }
        
    }
    void DestroyObject(GameObject obj)
    {
        Destroy(obj);
    }
    void DestroyObjectWithParam()
    {
        DestroyObject(gameobjectVFX);
    }

    public void HardnessTest1()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 1)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest2()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 2)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest3()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 3)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest4()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 4)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest5()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 5)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest6()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 6)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest7()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 7)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest8()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 8)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest9()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 9)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }
    public void HardnessTest10()
    {
        if (itemPlaced)
        {
            transImageObject.SetActive(false);
            if (currentItem.gemData.hardness == 10)
            {
                gameobjectVFX = Instantiate(vfxList[5], VFXspawnPoint.position, Quaternion.Euler(1, 1, 1));
                Invoke("DestroyObjectWithParam", 1);
            }
        }
    }

}
