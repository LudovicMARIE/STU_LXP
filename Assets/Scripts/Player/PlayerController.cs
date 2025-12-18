using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("Santé Joueur")]
    [SerializeField] private int maxHealth = 20;
    private int currentHealth;
    
    // Timer pour l'invincibilité
    private bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 1.0f; 
    private float invincibilityTimer;

    [Header("References")]
    [SerializeField] private SwordController swordController;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private SpriteRenderer characterSprite;
    
    private Animator animator; 

    [Header("Inputs")]
    public InputAction moveAction;
    public InputAction attackAction;

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 movementInput;
    private Vector2 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        currentHealth = maxHealth;

        animator = GetComponentInChildren<Animator>(); 
        if (characterSprite == null) 
            characterSprite = GetComponentInChildren<SpriteRenderer>();

        if (swordController == null)
            swordController = GetComponentInChildren<SwordController>();

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
        
        if (swordController != null)
        {
            // 1. On dit à l'arme de viser la souris
            swordController.HandleRotation(mousePos, rb.position);

            // 2. Si on clique, on dit à l'arme d'attaquer
            if (attackAction.WasPressedThisFrame())
            {
                swordController.TryAttack();
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        DamagePopupManager.Instance.CreatePopup(this.transform.position, damage);
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

        // temp game stop
        gameObject.SetActive(false); 
        Time.timeScale = 0;
    }
    
}