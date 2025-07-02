using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CraftingTableController : MonoBehaviour, IInteractable
{
    public bool IsOpened { get; private set; }
    public GameObject craftingPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefab;
    

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        Time.timeScale = 0f; //Ограничить передвижение персонажа (пофиксить паузу)
        craftingPanel.SetActive(true);
        Player.Instance.playerInputActions.Disable();
    }

    public void StopInteract()
    {
        craftingPanel.SetActive(false);
        Time.timeScale = 1f;
        Player.Instance.playerInputActions.Enable();
        //PauseGame = false;
    }
}
