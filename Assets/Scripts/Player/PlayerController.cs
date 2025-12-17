using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("Santé Joueur")]
    [SerializeField] private int maxHealth = 20;
    private int currentHealth;
    [Header("Energy")]
    [SerializeField] private int maxEnergy = 10;
    private int currentEnergy;
    
    // Timer pour l'invincibilité
    private bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 1.0f; 
    private float invincibilityTimer;

    [Header("References")]
    [SerializeField] private SwordController weaponController;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private GameObject gameOverCanvas;
    
    private Animator animator; 

    [Header("Inputs")]
    public InputAction moveAction;
    public InputAction attackAction;
    
    [Header("Getters")]
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentEnergy => currentEnergy;
    public int MaxEnergy => maxEnergy;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 movementInput;
    private Vector2 mousePos;

    void Start()
    {
        gameOverCanvas.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;

        animator = GetComponentInChildren<Animator>(); 
        if (characterSprite == null) 
            characterSprite = GetComponentInChildren<SpriteRenderer>();

        if (weaponController == null)
            weaponController = GetComponentInChildren<SwordController>();

        if (moveAction.bindings.Count == 0)
        {
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/z")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/q")
                .With("Right", "<Keyboard>/d");
        }
        if (attackAction.bindings.Count == 0)
            attackAction.AddBinding("<Mouse>/leftButton");
    }

    void OnEnable() { moveAction.Enable(); attackAction.Enable(); }
    void OnDisable() { moveAction.Disable(); attackAction.Disable(); }

    void Update()
    {
        movementInput = moveAction.ReadValue<Vector2>();

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            // Effet Clignotement Rouge / Blanc rapide
            if (characterSprite != null)
            {
                float flashSpeed = 20f; // Vitesse du flash
                if (Mathf.Sin(Time.time * flashSpeed) > 0)
                    characterSprite.color = Color.red;
                else
                    characterSprite.color = Color.white;
            }

            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                if (characterSprite != null) 
                    characterSprite.color = Color.white;
            }
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", movementInput.magnitude);
        }

        if (Mouse.current != null)
            mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (characterSprite != null)
        {
            if (mousePos.x < rb.position.x) characterSprite.flipX = true;
            else characterSprite.flipX = false;
        }
        
        if (weaponController != null)
        {
            // 1. On dit à l'arme de viser la souris
            weaponController.HandleRotation(mousePos, rb.position);

            // 2. Si on clique, on dit à l'arme d'attaquer
            if (attackAction.WasPressedThisFrame())
            {
                weaponController.TryAttack();
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage taken : " + damage);
        if (isInvincible) return;

        currentHealth -= damage;
        // Debug.Log($"Joueur touché. PV restants : {currentHealth}");

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("💀 GAME OVER");

        gameOverCanvas.SetActive(true);
        gameObject.SetActive(false); 
        Time.timeScale = 0;
    }
}