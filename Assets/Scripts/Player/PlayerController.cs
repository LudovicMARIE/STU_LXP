using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")] [SerializeField] private float moveSpeed = 5f;
    private int enemyKill;

    [Header("Santé Joueur")] [SerializeField]
    private int maxHealth = 20;

    private int currentHealth;
    [Header("Energy")] [SerializeField] private int maxEnergy = 10;
    private int currentEnergy;

    // Timer pour l'invincibilité
    private bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = 1.0f;
    private float invincibilityTimer;

    [Header("References")] [SerializeField]
    private SwordController swordController;
    [SerializeField] private KillCounterUI killCounterUI;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private GameObject gameOverCanvas;
    private PlayerAudio _playerAudio;

    private Animator animator;

    [Header("Inputs")] public InputAction moveAction;
    public InputAction attackAction;

    [Header("Getters/Setters")]
    public int MaxHealth => maxHealth;

    private int _bonusMaxHealth = 0;
    private int _bonusDamage = 0;
    private float _bonusMoveSpeedPct = 0f;
    private float _bonusAttackSpeedPct = 0f;
    public int CurrentHealth => currentHealth;
    public int BonusDamage => _bonusDamage;
    public float BonusMoveSpeedPct => _bonusMoveSpeedPct;
    public float BonusAttackSpeedPct => _bonusAttackSpeedPct;


    public int CurrentEnergy => currentEnergy;
    public int MaxEnergy => maxEnergy;

    public int EnemyKill
    {
        get => enemyKill;
        set => enemyKill = value;
    }

    private Rigidbody2D rb;
    private Camera mainCam;
    private Vector2 movementInput;
    private Vector2 mousePos;

    void Start()
    {
        gameOverCanvas.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        _playerAudio = GetComponent<PlayerAudio>();
        mainCam = Camera.main;
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        enemyKill = 0;

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

    void OnEnable()
    {
        moveAction.Enable();
        attackAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
        attackAction.Disable();
    }

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
        if (_playerAudio != null) _playerAudio.PlayHurt();
        DamagePopupManager.Instance.CreatePopup(this.transform.position, damage);

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (_playerAudio != null) _playerAudio.PlayHurt();
        }
    }
    
    public void AddKill()
    {
        enemyKill++;
        if (killCounterUI != null) killCounterUI.SetKills(enemyKill);
    }


    private void Die()
    {
        // 1. Jouer le son (maintenant l'AudioSource restera actif)
        if (_playerAudio != null) _playerAudio.PlayDeath();

        // 2. Afficher le Game Over
        gameOverCanvas.SetActive(true);

        // 3. Cacher le joueur visuellement
        if (characterSprite != null) characterSprite.enabled = false;

        // 4. Désactiver les collisions (pour ne plus se faire toucher)
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 5. Désactiver ce script (pour arrêter les Inputs et l'Update)
        // Cela va appeler OnDisable() qui coupe vos Inputs Actions proprement
        this.enabled = false;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    public void UpgradeMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        _bonusMaxHealth += amount;
    }

    public void UpgradeMoveSpeed(float multiplier)
    {
        moveSpeed *= multiplier;
        _bonusMoveSpeedPct += (multiplier - 1f) * 100f;
    }

    public SwordController GetSword()
    {
        return swordController;
    }

    public void UpgradeSwordDamage(int amount)
    {
        if (swordController != null)
        {
            swordController.UpgradeDamage(amount);
            _bonusDamage += amount;
        }
    }

    public void UpgradeSwordAttackSpeed(float multiplier)
    {
        if (swordController != null)
        {
            swordController.UpgradeAttackSpeed(multiplier);
            
            _bonusAttackSpeedPct += (1f - multiplier) * 100f;
        }
    }
}