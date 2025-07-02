using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableRange = null;
    public GameObject interactionIcon;
    public TMP_Text text;
    public PlayerInputActions playerControls;
    SwitchInteraction switchInteraction;
    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //playerControls.Player.Interact.Disable();
            //playerControls.Combat.StopInteract.Enable();
            if (interactableRange != null)
            {
                interactableRange.Interact();
            }
            else
            {
                text.text = "Ничего не найдено!";
                Invoke("DestroyText", 2);
            }
        }
    }
    private void DestroyText()
    {
        text.text = "";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableRange = interactable;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableRange)
        {
            interactableRange = null;
            interactionIcon.SetActive(false);
        }
    }
}
