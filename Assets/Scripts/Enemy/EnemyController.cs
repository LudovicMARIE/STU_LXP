using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float currentHealth;
    public bool isDead;
    public float damage;


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
    
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        Debug.Log($"Aie ! L'ennemi a pris {amount} dégâts. Reste : {currentHealth}");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
