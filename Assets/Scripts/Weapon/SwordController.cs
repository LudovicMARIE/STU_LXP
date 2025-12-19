using UnityEngine;
using System.Collections;

public class SwordController : MonoBehaviour
{
    [Header("Stats Arme")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackDuration = 0.3f; // Durée du coup
    [SerializeField] private float attackCooldown = 0.5f; // Temps entre deux coups
    
    [Header("References")]
    [SerializeField] private Transform swordContainer; // Glisse l'objet "SwordContainer"
    [SerializeField] private Collider2D swordCollider; // Glisse l'objet "SwordSprite" (qui a le collider)
    [SerializeField] private SpriteRenderer swordRenderer;
    public PlayerAudio playerAudio;

    private bool isAttacking = false;
    private float lastAttackTime = -999f;
    private Quaternion targetRotation;

    void Start()
    {
        // Sécurité : on s'assure que le collider est éteint au début
        if(swordCollider != null) swordCollider.enabled = false;
        if(swordRenderer != null) swordRenderer.enabled = false;
    }

    // Appelée par le PlayerController à chaque frame
    public void HandleRotation(Vector2 mousePos, Vector2 playerPos)
    {
        if (isAttacking) return; // On ne tourne pas l'arme pendant qu'elle frappe

        Vector2 lookDir = mousePos - playerPos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        
        // On stocke la rotation idéale
        targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = targetRotation;

        // Gestion du flip vertical de l'arme (pour ne pas qu'elle soit à l'envers à gauche)
        Vector3 scale = transform.localScale;
        if (angle > 90 || angle < -90) scale.y = -1f;
        else scale.y = 1f;
        transform.localScale = scale;
    }

    // Appelée par le PlayerController quand on clique
    public void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        if (playerAudio != null) 
        {
            float pitchBonus = 0.3f / attackDuration; 

            playerAudio.source.pitch = pitchBonus; 
            playerAudio.PlaySwing();
        }

        // 1. Activer la hitbox
        if(swordCollider != null) swordCollider.enabled = true;
        if(swordRenderer != null) swordRenderer.enabled = true;

        float startAngle = 70f;
        float endAngle = -70f;
        float timer = 0f;

        Quaternion baseRot = transform.rotation;

        while (timer < attackDuration)
        {
            timer += Time.deltaTime;
            float t = timer / attackDuration;
            
            // Courbe d'animation (Lerp)
            float currentAddedAngle = Mathf.Lerp(startAngle, endAngle, t);
            
            // On applique la rotation sur le conteneur interne pour faire l'effet de balancier
            swordContainer.localRotation = Quaternion.Euler(0, 0, currentAddedAngle);
            
            yield return null;
        }

        // 3. Reset
        if(swordCollider != null) swordCollider.enabled = false;
        if(swordRenderer != null) swordRenderer.enabled = false;
        swordContainer.localRotation = Quaternion.identity;
        isAttacking = false;
    }

    public void UpgradeDamage(int amount)
    {
        damage += amount;
    }

    public void UpgradeAttackSpeed(float multiplier)
    {
        attackCooldown *= multiplier;

        attackDuration *= multiplier;

        if (attackCooldown < 0.05f) attackCooldown = 0.05f;
        if (attackDuration < 0.05f) attackDuration = 0.05f;
    }

    // Détection des coups
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore Player
        if (other.CompareTag("Player")) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage); 
                DamagePopupManager.Instance.CreatePopup(other.transform.position, damage);
                if (playerAudio != null) playerAudio.PlayHitImpulse();
            }
        }
    }
}