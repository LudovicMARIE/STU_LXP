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
    }
    
    void Update()
    {
        if (player == null) return;
        Vector3 direction = (player.transform.position - transform.position).normalized;

        transform.position += direction * speed * Time.deltaTime;
        
        if (spriteRenderer != null && Mathf.Abs(direction.x) > 0.01f)
            spriteRenderer.flipX = direction.x < 0f;

        if (currentHealth <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
