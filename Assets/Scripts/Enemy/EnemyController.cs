using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float maxHealth;
    public float currentHealth;
    public float damage;
    [Header("Loot")]
    [SerializeField] private GameObject itemToDrop;
    [Range(0f, 1f)] [SerializeField] private float dropChance = 0.05f;
    
    public GameObject player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }
    }
    
    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        
        if (spriteRenderer != null && Mathf.Abs(direction.x) > 0.01f)
            spriteRenderer.flipX = direction.x < 0f;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            
            if (playerScript != null)
            {
                playerScript.TakeDamage((int)damage);
            }
        }
    }

    private void Die()
    {
        GetComponent<EnemyXpDrop>().DropExperience();
        PlayerController _playerController = player.GetComponent<PlayerController>();
        if (_playerController != null)
            _playerController.AddKill();
        EnemySpawner _enemySpawner = FindObjectOfType<EnemySpawner>();
        if (_enemySpawner != null)
            _enemySpawner.OnEnemyKilled();
        if (itemToDrop != null && Random.value <= dropChance)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
