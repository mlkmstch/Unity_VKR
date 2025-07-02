using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DigDetector : MonoBehaviour
{
    private IDigging interactableTarget;

    public PlayerInputActions playerControls;
    public TMP_Text textComponent;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Dig.performed += OnDigging;
    }

    private void OnDisable()
    {
        playerControls.Player.Dig.performed -= OnDigging;
        playerControls.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDigging diggable) && diggable.CanInteract())
        {
            Debug.Log("Found IDigging on: " + collision.name);
            interactableTarget = diggable;
            Debug.Log(interactableTarget);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDigging diggable) && diggable == interactableTarget)
        {
            interactableTarget = null;
            Debug.Log(interactableTarget);
        }
    }

    public void OnDigging(InputAction.CallbackContext context)
    {
        if (interactableTarget != null && context.performed)
        {
            interactableTarget.Dig();
        }
    }

}
