using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Player : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip dashClip;
    public AudioClip attackClip;

    private AudioSource audioSource;

    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    Vector2 inputVector;
    public PlayerInputActions playerInputActions;

    private Rigidbody2D rb;

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;
    private bool isDashing = false;
    private int dashNumber = 3;
    private bool stopCharging = false;

    public UnityEngine.UI.Image weaponChooseImage;
    public Sprite goldenSprite;

    public GameObject dash1;
    public GameObject dash2;
    public GameObject dash3;
    public int dashRegargeTime = 10;
    private bool dualCount = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }

    private void Start()
    {
        playerInputActions.Combat.Dash.performed += _ => Dash();
        playerInputActions.Player.Attack.performed += _ => Attack();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            vcam.Follow = this.transform;
            vcam.LookAt = this.transform;
        }
        else
        {
            Debug.LogWarning("Virtual Camera не найдена в сцене " + scene.name);
        }
    }
    
    private void Attack()
    {
        if (ActiveWeapon.Instance.activeWeapon != "crossbow")
        {
            if (ActiveWeapon.Instance.activeWeapon == "acid")
            {
                if (Sword.Instance.acidCount >= Sword.Instance.acidAmount)
                {
                    ActiveWeapon.Instance.activeWeapon = "golden";
                    weaponChooseImage.sprite = goldenSprite;
                    Sword.Instance.acidCount = 0;
                }
                audioSource.PlayOneShot(attackClip);
                Sword.Instance.Attack();
                Sword.Instance.acidCount++;
            }
            else
            {
                if (ActiveWeapon.Instance.activeWeapon == "dual")
                {
                    if(dualCount == false)
                    {
                        audioSource.PlayOneShot(attackClip);
                        Sword.Instance.Attack();
                        dualCount = true;
                    }
                    else
                    {
                        dualCount = false;
                        Sword.Instance.damageAmount = Sword.Instance.damageAmount + 5;
                        audioSource.PlayOneShot(attackClip);
                        Sword.Instance.Attack();
                        Sword.Instance.damageAmount = Sword.Instance.damageAmount - 5;
                    }
                }
                audioSource.PlayOneShot(attackClip);
                Sword.Instance.Attack();
            }
        }
        else if (ActiveWeapon.Instance.activeWeapon == "crossbow")
        {
            audioSource.PlayOneShot(attackClip);
            Crossbow.Instance.Attack();
        }
    }

    private void Update()
    {
        if (dashNumber < 3 && !stopCharging)
        {
            stopCharging = true;
            Invoke("DashRecharge", dashRegargeTime);
        }

        inputVector = GetMovementVector();

    }

    public void DisabeInput()
    {
        playerInputActions.Disable();
    }

    public void EnableInput()
    {
        playerInputActions.Enable();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void DashRecharge()
    {
        dashNumber++;
        stopCharging = false;
        switch (dashNumber)
        {
            case 1:
                dash1.SetActive(true);
                break;
            case 2:
                dash2.SetActive(true);
                break;
            case 3:
                dash3.SetActive(true);
                break;
        }
    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedUnscaledDeltaTime));
        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    public void Dash()
    {
        switch (dashNumber)
        {
            case 3:
                dash3.SetActive(false);
                break;
            case 2:
                dash2.SetActive(false);
                break;
            case 1:
                dash1.SetActive(false);
                break;
        }
        if (!isDashing && (dashNumber > 0))
        {
            isDashing = true;
            movingSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            audioSource.PlayOneShot(dashClip);
            StartCoroutine(EndDashRoutine());
        }
    }

    public IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        movingSpeed /= dashSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
        dashNumber--;
    }

}