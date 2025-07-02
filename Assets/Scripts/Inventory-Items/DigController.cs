using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DigController : MonoBehaviour, IDigging
{
    public bool IsOpened { get; private set; }
    public string SpotID { get; private set; }

    public GameObject itemPrefab;
    public Sprite openedSprite;
    void Start()
    {
        SpotID ??= GlobalHelper.GenerateUniqueID(gameObject);
        IsOpened = false; // сброс состо€ни€
    }
    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Dig()
    {
        if (!CanInteract()) return;
        Digging();
    }

    private void Digging()
    {
        //if (IsOpened) return; // предотвращает повторный дроп
        DugUp(true);

        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
        }
    }

    public void DugUp(bool opened)
    {
        IsOpened = true;
        if (opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}

